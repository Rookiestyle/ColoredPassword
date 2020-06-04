using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using KeePass.App.Configuration;
using KeePass.Plugins;
using KeePass.UI;
using KeePassLib.Utility;

using PluginTools;
using PluginTranslation;

namespace ColoredPassword
{
	public sealed class ColoredPasswordExt : Plugin
	{
		private IPluginHost m_host = null;
		private ToolStripMenuItem m_menu = null;

		private ListView m_lvEntries = null;

		public override bool Initialize(IPluginHost host)
		{
			m_host = host;
			m_OtherHandlers["DrawItem"] = new List<Delegate>();
			m_OtherHandlers["DrawSubItem"] = new List<Delegate>();
			PluginTranslate.Init(this, KeePass.Program.Translation.Properties.Iso6391Code);
			Tools.DefaultCaption = PluginTranslate.PluginName;
			Tools.PluginURL = "https://github.com/rookiestyle/coloredpassword/";

			m_menu = new ToolStripMenuItem(Tools.DefaultCaption + "...");
			m_menu.Image = SmallIcon;
			m_menu.Click += (o, e) => Tools.ShowOptions();
			m_host.MainWindow.ToolsMenu.DropDownItems.Add(m_menu);
			Tools.OptionsFormShown += OptionsShown;
			Tools.OptionsFormClosed += OptionsClosed;

			ColorConfig.Read(m_host);
			m_lvEntries = (ListView)Tools.GetControl("m_lvEntries");
			if (m_lvEntries != null)
			{
				PluginDebug.AddSuccess("m_lvEntries found", 0);
				m_host.MainWindow.FormLoadPost += MainWindow_FormLoadPost;
			}
			else
				PluginDebug.AddError("m_lvEntries not found", 0);
			ColorPasswords(ColorConfig.Active);

			if (!OverridePossible && ColorConfig.FirstRun && ColorConfig.Active)
				GlobalWindowManager.WindowAdded += OnKeyPromptFormLoad;

			return true;
		}

		private Dictionary<string, List<Delegate>> m_OtherHandlers = new Dictionary<string, List<Delegate>>();
		private void MainWindow_FormLoadPost(object sender, EventArgs e)
		{
			m_host.MainWindow.FormLoadPost -= MainWindow_FormLoadPost;

			if (ColorConfig.ColorEntryView)
			{
				m_lvEntries = (ListView)Tools.GetControl("m_lvEntries");
				if (m_lvEntries == null) return;

				#region Add important eventhandlers to debug info
				m_OtherHandlers["DrawItem"] = m_lvEntries.GetEventHandlers("DrawItem").FindAll(x => !x.Method.DeclaringType.FullName.Contains("ColoredPassword"));
				m_OtherHandlers["DrawSubItem"] = m_lvEntries.GetEventHandlers("DrawSubItem").FindAll(x => !x.Method.DeclaringType.FullName.Contains("ColoredPassword"));
				if (PluginDebug.DebugMode)
				{
					List<string> lHandlers = new List<string>();
					foreach (var d in m_OtherHandlers["DrawItem"])
					{
						lHandlers.Add(d.Method.Name + " (" + d.Method.DeclaringType.FullName + ")");
					}
					PluginDebug.AddInfo("m_lvEntries.DrawItem handlers: " + lHandlers.Count, 0, lHandlers.ToArray());
					lHandlers.Clear();
					foreach (var d in m_OtherHandlers["DrawSubItem"])
					{
						lHandlers.Add(d.Method.Name + " (" + d.Method.DeclaringType.FullName + ")");
					}
					PluginDebug.AddInfo("m_lvEntries.DrawSubItem handlers: " + lHandlers.Count, 0, lHandlers.ToArray());
				}
				#endregion

				#region Handle OwnerDraw functionality of QualityHighlighter
				Version vActual = null;
				Version vRef = new Version(1, 3, 0, 1);
				bool qhFound = Tools.GetLoadedPluginsName().TryGetValue("QualityHighlighter.QualityHighlighterExt", out vActual);
				if (!qhFound) vActual = new Version();
				PluginDebug.AddInfo("QualityHighlighter plugin status", 0,
					"Found: " + qhFound.ToString(),
					"Version: " + (qhFound ? vActual.ToString() : "N/A"),
					"Special handling required: " + (qhFound ? (vActual <= vRef).ToString() : "N/A"));
				if (qhFound && (vActual <= vRef))
				{
					m_OtherHandlers["DrawItem"] = m_lvEntries.GetEventHandlers("DrawItem").FindAll(x => x.Method.DeclaringType.FullName.Contains("QualityHighlighter"));
					m_OtherHandlers["DrawSubItem"] = m_lvEntries.GetEventHandlers("DrawSubItem").FindAll(x => x.Method.DeclaringType.FullName.Contains("QualityHighlighter"));
					m_lvEntries.RemoveEventHandlers("DrawSubItem", m_OtherHandlers["DrawSubItem"]);
				}
				else
				{
					m_OtherHandlers["DrawItem"].Clear();
					m_OtherHandlers["DrawSubItem"].Clear();
				}
				#endregion
			}

			ColorPasswords(ColorConfig.Active);
		}

		private void Lv_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawDefault = true;
		}

		private void Lv_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			if (m_OtherHandlers["DrawItem"].Count > 0)
			{
				PluginDebug.AddInfo("Invoking other eventhandlers", 0);
				foreach (Delegate d in m_OtherHandlers["DrawItem"])
				{
					d.Method.Invoke(d.Target, new object[] { sender, e });
				}
			}
			if (e.Item.ListView.View != View.Details)
			{
				if (m_OtherHandlers["DrawItem"].Count == 0)
					e.DrawDefault = true;
				PluginDebug.AddInfo("Entrylist viewstyle != View.Details", 0);
				return;
			}
			AceColumn colPw = KeePass.Program.Config.MainWindow.FindColumn(AceColumnType.Password);
			string m = string.Empty;
			if ((colPw == null) || colPw.HideWithAsterisks)
			{
				//Let the OS draw in case no other handlers exist
				//If other handlers exist, we pass their value for DrawDefault
				if (m_OtherHandlers["DrawItem"].Count == 0)
					e.DrawDefault = true;
				if (colPw == null)
				{
					m = "Password column not found";
				}
				else
					m = "Password column found, password hidden";
				List<string> lCol = new List<string>();
				foreach (var c in KeePass.Program.Config.MainWindow.EntryListColumns)
					lCol.Add(c.GetDisplayName());
				PluginDebug.AddInfo(m, 0, lCol.ToArray());
				return;
			}
			e.DrawDefault = false;
			m = "Password column found, password shown";
			PluginDebug.AddSuccess(m, 0);

			if ((e.State & ListViewItemStates.Selected) != 0)
				e.DrawFocusRectangle();
		}

		private void Lv_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			//Setting e.DrawDefault = true for all columns beside the password column
			//does not work. Alternating background color is not shown
			//unless e.Graphics.FillRectangle is called
			//
			//Only exception is the title column which shows the entry's icon
			if (e.Header.Text != KeePass.Resources.KPRes.Password)
			{
				if (!KeePass.Program.Config.MainWindow.EntryListAlternatingBgColors ||
				(e.Header.Text == KeePass.Resources.KPRes.Title))
				{
					string m = "m_lvEntries: Skip column '" + e.Header.Text + "'";
					if (!PluginDebug.HasMessage(PluginDebug.LogLevelFlags.Info, m))
						PluginDebug.AddInfo(m, 0);
					Font f = e.Item.UseItemStyleForSubItems ? e.Item.Font : e.SubItem.Font;
					bool DrawDefault = e.Header.Text == KeePass.Resources.KPRes.Title;
					if (!DrawDefault)
					{
						//Use default drawing if entry is not expired, otherwise the strikeout is not drawn
						KeePassLib.PwEntry pe = (e.Item.Tag as PwListItem).Entry;
						if (pe == null)
							DrawDefault = true;
						else
							DrawDefault = !pe.Expires || (pe.ExpiryTime >= DateTime.UtcNow);
					}
					if (DrawDefault)
					{
						e.DrawDefault = true;
						return;
					}
				}
			}
			Color cItemForeground = e.Item.UseItemStyleForSubItems ? e.Item.ForeColor : e.SubItem.ForeColor;
			Color cItemBackground = e.Item.UseItemStyleForSubItems ? e.Item.BackColor : e.SubItem.BackColor;
			if (KeePass.Program.Config.MainWindow.EntryListAlternatingBgColors && ((e.ItemIndex & 1) == 1))
				cItemBackground = UIUtil.GetAlternateColorEx(m_lvEntries.BackColor);
			Font fFont = e.Item.UseItemStyleForSubItems ? e.Item.Font : e.SubItem.Font;
			if (!e.Item.Selected)
				e.Graphics.FillRectangle(new SolidBrush(cItemBackground), e.Bounds);
			if (e.Header.Text != KeePass.Resources.KPRes.Password)
			{
				string m = "m_lvEntries: Draw text for column '" + e.Header.Text + "'";
				if (!PluginDebug.HasMessage(PluginDebug.LogLevelFlags.Info, m))
					PluginDebug.AddInfo(m, 0);
				StringFormat sf = StringFormat.GenericDefault;
				sf.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				Rectangle r = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
				e.Graphics.DrawString(e.SubItem.Text, fFont, new SolidBrush(cItemForeground), r, sf);
				return;
			}
			string msg = "m_lvEntries: Handle password column '" + e.Header.Text + "' - Start";
			PluginDebug.AddInfo(msg);
			string s = e.SubItem.Text;
			float x = e.Bounds.X + 3;
			int i = 0;
			while (i < s.Length)
			{
				StringFormat sf = StringFormat.GenericTypographic;
				float fWidth = e.Graphics.MeasureString("A!r" + s.Substring(i, 1), fFont, int.MaxValue, sf).Width;
				fWidth -= e.Graphics.MeasureString("A!r", fFont, int.MaxValue, sf).Width;
				if ((e.Bounds.X + e.Bounds.Width) <= (x + fWidth)) break;
				Color col = ColorConfig.ForeColorDefault;
				Color colb = ColorConfig.ListViewKeepBackgroundColor ? cItemBackground : ColorConfig.BackColorDefault;
				msg = "Color letter";
				if (char.IsDigit(s, i))
				{
					col = ColorConfig.ForeColorDigit;
					colb = ColorConfig.ListViewKeepBackgroundColor ? cItemBackground : ColorConfig.BackColorDigit;
					msg = "Color digit";
				}
				else if (!char.IsLetter(s, i))
				{
					col = ColorConfig.ForeColorSpecial;
					colb = ColorConfig.ListViewKeepBackgroundColor ? cItemBackground : ColorConfig.BackColorSpecial;
					msg = "Color special char";
				}
				else if (ColorConfig.LowercaseDifferent && char.IsLower(s, i))
				{
					col = ColorConfig.ForeColorLower;
					colb = ColorConfig.ListViewKeepBackgroundColor ? cItemBackground : ColorConfig.BackColorLower;
					msg = "Color lowercase char";
				}
				List<string> lMsg = new List<string>();
				if (col.ToArgb() == colb.ToArgb())
				{
					lMsg.Add("Foreground and background color identical, adjusting background color");
					colb = ContrastColor(colb);
				}
				lMsg.Add("Foreground: " + col.ToString());
				lMsg.Add("Background: " + colb.ToString());
				PluginDebug.AddInfo(msg, 0, lMsg.ToArray());
				RectangleF r = new RectangleF(x, e.Bounds.Y, fWidth, e.Bounds.Height);
				e.Graphics.FillRectangle(new SolidBrush(colb), r);
				e.Graphics.DrawString(s.Substring(i, 1), fFont, new SolidBrush(col), r, sf);
				x += fWidth;
				i++;
			}
			msg = "m_lvEntries: Handle password column '" + e.Header.Text + "' - Finish";
			PluginDebug.AddInfo(msg, 0);
		}

		private Color ContrastColor(Color c)
		{
			//https://en.wikipedia.org/wiki/Luma_%28video%29
			var l = 0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B;
			return l > 0.5 ? Color.White : Color.Black;
		}

		#region Show error message if TypeOverride is not possible
		private void OnKeyPromptFormLoad(object sender, GwmWindowEventArgs e)
		{
			if (e.Form is KeePass.Forms.KeyPromptForm)
			{
				GlobalWindowManager.WindowAdded -= OnKeyPromptFormLoad;
				e.Form.Shown += OnKeyPromptFormShown;
			}
		}

		private void OnKeyPromptFormShown(object sender, EventArgs e)
		{
			(sender as Form).Shown -= OnKeyPromptFormShown;
			Tools.ShowError(string.Format(PluginTranslate.Error, typeof(SecureTextBoxEx).BaseType.Name));
			ColorConfig.FirstRun = false;
			ColorConfig.Write(m_host);
		}
		#endregion

		#region Configuration
		private void OptionsShown(object sender, Tools.OptionsFormsEventArgs e)
		{
			Tools.AddPluginToOptionsForm(this, new Options());
			e.form.Shown += OptionsForm_Shown;
		}

		private void OptionsForm_Shown(object sender, EventArgs e)
		{
			//Set button colors during OnShow
			//to improve compatibility with KeeTheme
			bool shown = false;
			Options o = (Options)Tools.GetPluginFromOptions(this, out shown);
			o.cgActive.Checked = ColorConfig.Active;
			o.bForeColorDefault.BackColor = ColorConfig.ForeColorDefault;
			o.bBackColorDefault.BackColor = ColorConfig.BackColorDefault;
			o.bForeColorDigit.BackColor = ColorConfig.ForeColorDigit;
			o.bBackColorDigit.BackColor = ColorConfig.BackColorDigit;
			o.bForeColorSpecial.BackColor = ColorConfig.ForeColorSpecial;
			o.bBackColorSpecial.BackColor = ColorConfig.BackColorSpecial;
			o.cbLowercase.Checked = ColorConfig.LowercaseDifferent;
			o.bForeColorLower.BackColor = ColorConfig.ForeColorLower;
			o.bBackColorLower.BackColor = ColorConfig.BackColorLower;
			o.cbColorEntryView.Checked = ColorConfig.ColorEntryView;
			o.cbColorEntryViewKeepBackgroundColor.Checked = ColorConfig.ListViewKeepBackgroundColor;
			o.lError.Visible = !ColoredPasswordExt.OverridePossible;
			o.ctbExample.ColorText();
			ColorConfig.Testmode = true;
		}

		private void OptionsClosed(object sender, Tools.OptionsFormsEventArgs e)
		{
			ColorConfig.Testmode = false;
			if (e.form.DialogResult != DialogResult.OK) return;
			bool shown;
			Options o = (Options)Tools.GetPluginFromOptions(this, out shown);
			PluginDebug.AddInfo("Plugin options window closed, plugin options shown: " + shown.ToString(), 0);
			if (!shown) return;
			ColorPasswords(false);
			ColorConfig.Active = o.cgActive.Checked;
			ColorConfig.ForeColorDefault = o.bForeColorDefault.BackColor;
			ColorConfig.BackColorDefault = o.bBackColorDefault.BackColor;
			ColorConfig.ForeColorDigit = o.bForeColorDigit.BackColor;
			ColorConfig.BackColorDigit = o.bBackColorDigit.BackColor;
			ColorConfig.ForeColorSpecial = o.bForeColorSpecial.BackColor;
			ColorConfig.BackColorSpecial = o.bBackColorSpecial.BackColor;
			ColorConfig.LowercaseDifferent = o.cbLowercase.Checked;
			ColorConfig.ForeColorLower = o.bForeColorLower.BackColor;
			ColorConfig.BackColorLower = o.bBackColorLower.BackColor;

			ColorConfig.ColorEntryView = o.cbColorEntryView.Checked;
			ColorConfig.ListViewKeepBackgroundColor = o.cbColorEntryViewKeepBackgroundColor.Checked;
			ColorConfig.Write(m_host);
			if (ColorConfig.Active)
				ColorPasswords(ColorConfig.Active);
		}
		#endregion

		#region Override SecureTextBoxEx with ColoredSecureTextBox
		private void ColorPasswords(bool active)
		{
			if (!OverridePossible)
			{
				PluginDebug.AddError("TypeOverride not possible");
				return;
			}
			if (!KeePass.App.AppPolicy.Current.UnhidePasswords)
			{
				PluginDebug.AddError("Policy 'UnhidePasswords' does not allow unhiding passwords'");
				return;
			}
			if (ColorConfig.ColorEntryView)
			{
				if (m_lvEntries != null)
				{
					m_lvEntries.OwnerDraw = active || m_OtherHandlers["DrawItem"].Count > 0;
					if (active)
					{
						m_lvEntries.DrawColumnHeader += Lv_DrawColumnHeader;
						m_lvEntries.DrawItem += Lv_DrawItem;
						m_lvEntries.DrawSubItem += Lv_DrawSubItem;
						m_lvEntries.RemoveEventHandlers("DrawItem", m_OtherHandlers["DrawItem"]);
					}
					else
					{
						m_lvEntries.DrawColumnHeader -= Lv_DrawColumnHeader;
						m_lvEntries.DrawItem -= Lv_DrawItem;
						m_lvEntries.DrawSubItem -= Lv_DrawSubItem;
						m_lvEntries.AddEventHandlers("DrawItem", m_OtherHandlers["DrawItem"]);
					}
					PluginDebug.AddInfo("m_lvEntries changed", "OwnerDraw: " + m_lvEntries.OwnerDraw.ToString(), "ColorPasswords active: " + active.ToString(), "Viewstyle: " + m_lvEntries.View.ToString());
				}
				else
					PluginDebug.AddError("m_lvEntries cannot be changed");
			}
			else
				PluginDebug.AddInfo("ColorEntryView is disabled, check KeePass config file", 0);
			if (active)
				TypeOverridePool.Register(typeof(SecureTextBoxEx), CreateCustomInstance);
			else
				TypeOverridePool.Unregister(typeof(SecureTextBoxEx));
		}

		public static bool OverridePossible
		{
			get
			{
				bool result = typeof(SecureTextBoxEx).IsSubclassOf(typeof(TextBox));
				if (result)
					PluginDebug.AddInfo("TypeOverride possible", 0);
				else
					PluginDebug.AddError("TypeOverride possible", 0);
				return result;
			}
		}

		private static object CreateCustomInstance()
		{
			return new ColoredSecureTextBox();
		}
		#endregion

		public override string UpdateUrl
		{
			get { return "https://raw.githubusercontent.com/rookiestyle/coloredpassword/master/version.info"; }
		}

		public override Image SmallIcon
		{
			get { return Resources.smallicon; }
		}

		public override void Terminate()
		{
			if (m_host == null) return;

			Tools.OptionsFormShown -= OptionsShown;
			Tools.OptionsFormClosed -= OptionsClosed;
			m_host.MainWindow.ToolsMenu.DropDownItems.Remove(m_menu);
			m_menu.Dispose();

			ColorPasswords(false);
			PluginDebug.SaveOrShow();
			m_host = null;
		}
	}

	internal static class ColorConfig
	{
		public static bool FirstRun = true;
		public static bool Active = true;
		public static bool ColorEntryView = true;
		public static bool Testmode
		{
			get { return m_Testmode; }
			set { m_Testmode = value; if (m_Testmode) InitTestmode(); }
		}
		public static Color ForeColorDefault
		{
			get { return Testmode ? m_ForeColorDefaultTest : m_ForeColorDefault; }
			set
			{
				if (Testmode) m_ForeColorDefaultTest = value;
				else m_ForeColorDefault = value;
			}
		}
		public static Color BackColorDefault
		{
			get { return Testmode ? m_BackColorDefaultTest : m_BackColorDefault; }
			set
			{
				if (Testmode) m_BackColorDefaultTest = value;
				else m_BackColorDefault = value;
			}
		}
		public static bool LowercaseDifferent
		{
			get { return Testmode ? m_bLowercaseDifferentTest : m_bLowercaseDifferent; }
			set
			{
				if (Testmode) m_bLowercaseDifferentTest = value;
				else m_bLowercaseDifferent = value;
			}
		}
		public static Color ForeColorLower
		{
			get { return Testmode ? m_ForeColorLowerTest : m_ForeColorLower; }
			set
			{
				if (Testmode) m_ForeColorLowerTest = value;
				else m_ForeColorLower = value;
			}
		}
		public static Color BackColorLower
		{
			get { return Testmode ? m_BackColorLowerTest : m_BackColorLower; }
			set
			{
				if (Testmode) m_BackColorLowerTest = value;
				else m_BackColorLower = value;
			}
		}
		public static Color ForeColorDigit
		{
			get { return Testmode ? m_ForeColorDigitTest : m_ForeColorDigit; }
			set
			{
				if (Testmode) m_ForeColorDigitTest = value;
				else m_ForeColorDigit = value;
			}
		}
		public static Color BackColorDigit
		{
			get { return Testmode ? m_BackColorDigitTest : m_BackColorDigit; }
			set
			{
				if (Testmode) m_BackColorDigitTest = value;
				else m_BackColorDigit = value;
			}
		}
		public static Color ForeColorSpecial
		{
			get { return Testmode ? m_ForeColorSpecialTest : m_ForeColorSpecial; }
			set
			{
				if (Testmode) m_ForeColorSpecialTest = value;
				else m_ForeColorSpecial = value;
			}
		}
		public static Color BackColorSpecial
		{
			get { return Testmode ? m_BackColorSpecialTest : m_BackColorSpecial; }
			set
			{
				if (Testmode) m_BackColorSpecialTest = value;
				else m_BackColorSpecial = value;
			}
		}
		public static bool ListViewKeepBackgroundColor = true;

		public static void Read(IPluginHost host)
		{
			bool test = Testmode;
			Testmode = false;
			const string ConfigPrefix = "ColoredPassword.";
			FirstRun = host.CustomConfig.GetBool(ConfigPrefix + "FirstRun", true);
			Active = host.CustomConfig.GetBool(ConfigPrefix + "Active", true);
			ColorEntryView = host.CustomConfig.GetBool(ConfigPrefix + "ColorEntryView", true);
			ListViewKeepBackgroundColor = host.CustomConfig.GetBool(ConfigPrefix + "ListViewKeepBackgroundColor", true);
			string help = host.CustomConfig.GetString(ConfigPrefix + "ForeColorDefault", "WindowText");
			ForeColorDefault = NameToColor(help);
			help = host.CustomConfig.GetString(ConfigPrefix + "BackColorDefault", "Window");
			BackColorDefault = NameToColor(help);
			help = host.CustomConfig.GetString(ConfigPrefix + "ForeColorDigit", "Red");
			ForeColorDigit = NameToColor(help);
			help = host.CustomConfig.GetString(ConfigPrefix + "BackColorDigit", "White");
			BackColorDigit = NameToColor(help);
			help = host.CustomConfig.GetString(ConfigPrefix + "ForeColorSpecial", "Green");
			ForeColorSpecial = NameToColor(help);
			help = host.CustomConfig.GetString(ConfigPrefix + "BackColorSpecial", "White");
			BackColorSpecial = NameToColor(help);
			LowercaseDifferent = host.CustomConfig.GetBool(ConfigPrefix + "LowercaseDifferent", false);
			help = host.CustomConfig.GetString(ConfigPrefix + "ForeColorLower", ColorToName(ForeColorDefault));
			ForeColorLower = NameToColor(help);
			help = host.CustomConfig.GetString(ConfigPrefix + "BackColorLower", ColorToName(BackColorDefault));
			BackColorLower = NameToColor(help);

			Testmode = test;
			Write(host);
		}

		public static void Write(IPluginHost host)
		{
			bool test = Testmode;
			Testmode = false;
			const string ConfigPrefix = "ColoredPassword.";
			host.CustomConfig.SetBool(ConfigPrefix + "FirstRun", FirstRun);
			host.CustomConfig.SetBool(ConfigPrefix + "Active", Active);
			host.CustomConfig.SetBool(ConfigPrefix + "ColorEntryView", ColorEntryView);
			host.CustomConfig.SetBool(ConfigPrefix + "ListViewKeepBackgroundColor", ListViewKeepBackgroundColor);
			host.CustomConfig.SetString(ConfigPrefix + "ForeColorDefault", ColorToName(ForeColorDefault));
			host.CustomConfig.SetString(ConfigPrefix + "BackColorDefault", ColorToName(BackColorDefault));
			host.CustomConfig.SetString(ConfigPrefix + "ForeColorDigit", ColorToName(ForeColorDigit));
			host.CustomConfig.SetString(ConfigPrefix + "BackColorDigit", ColorToName(BackColorDigit));
			host.CustomConfig.SetString(ConfigPrefix + "ForeColorSpecial", ColorToName(ForeColorSpecial));
			host.CustomConfig.SetString(ConfigPrefix + "BackColorSpecial", ColorToName(BackColorSpecial));
			host.CustomConfig.SetBool(ConfigPrefix + "LowercaseDifferent", LowercaseDifferent);
			host.CustomConfig.SetString(ConfigPrefix + "ForeColorLower", ColorToName(ForeColorLower));
			host.CustomConfig.SetString(ConfigPrefix + "BackColorLower", ColorToName(BackColorLower));
			Testmode = test;
		}

		private static bool m_Testmode = false;
		private static Color m_ForeColorDefault = SystemColors.WindowText;
		private static Color m_BackColorDefault = SystemColors.Window;
		private static Color m_ForeColorDigit = Color.Red;
		private static Color m_BackColorDigit = Color.White;
		private static Color m_ForeColorSpecial = Color.Green;
		private static Color m_BackColorSpecial = Color.White;
		private static Color m_ForeColorDefaultTest = SystemColors.WindowText;
		private static Color m_BackColorDefaultTest = SystemColors.Window;
		private static Color m_ForeColorDigitTest = Color.Red;
		private static Color m_BackColorDigitTest = Color.White;
		private static Color m_ForeColorSpecialTest = Color.Green;
		private static Color m_BackColorSpecialTest = Color.White;
		private static Color m_ForeColorLower = SystemColors.WindowText;
		private static Color m_BackColorLower = SystemColors.Window;
		private static bool m_bLowercaseDifferent = false;
		private static bool m_bLowercaseDifferentTest = false;
		private static Color m_ForeColorLowerTest = SystemColors.WindowText;
		private static Color m_BackColorLowerTest = SystemColors.Window;

		private static string ColorToName(Color c)
		{
			if (c.IsNamedColor) return c.Name;
			return c.ToArgb().ToString();
		}

		private static Color NameToColor(string c)
		{
			int argb = 0;
			if (int.TryParse(c, out argb)) return Color.FromArgb(argb);
			return Color.FromName(c);
		}

		private static void InitTestmode()
		{
			m_ForeColorDefaultTest = m_ForeColorDefault;
			m_BackColorDefaultTest = m_BackColorDefault;
			m_bLowercaseDifferentTest = m_bLowercaseDifferent;
			m_ForeColorLowerTest = m_ForeColorLower;
			m_BackColorLowerTest = m_BackColorLower;
			m_ForeColorDigitTest = m_ForeColorDigit;
			m_BackColorDigitTest = m_BackColorDigit;
			m_ForeColorSpecialTest = m_ForeColorSpecial;
			m_BackColorSpecialTest = m_BackColorSpecial;
		}
	}
}