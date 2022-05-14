using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
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
			PluginTranslate.Init(this, KeePass.Program.Translation.Properties.Iso6391Code);
			Tools.DefaultCaption = PluginTranslate.PluginName;
			Tools.PluginURL = "https://github.com/rookiestyle/coloredpassword/";

			m_menu = new ToolStripMenuItem(Tools.DefaultCaption + "...");
			m_menu.Image = SmallIcon;
			m_menu.Click += (o, e) => Tools.ShowOptions();
			m_host.MainWindow.ToolsMenu.DropDownItems.Add(m_menu);
			Tools.OptionsFormShown += OptionsShown;
			Tools.OptionsFormClosed += OptionsClosed;

			ColorConfig.Read();
			m_lvEntries = (ListView)Tools.GetControl("m_lvEntries");
			if (m_lvEntries != null)
			{
				PluginDebug.AddSuccess("m_lvEntries found", 0);
				m_host.MainWindow.FormLoadPost += MainWindow_FormLoadPost;
			}
			else
				PluginDebug.AddError("m_lvEntries not found", 0);
			ColorPasswords(ColorConfig.Active);

			GlobalWindowManager.WindowAdded += OnWindowAdded;

			SinglePwDisplay.Enabled = ColorConfig.SinglePwDisplayActive;

			return true;
		}

		private void MainWindow_FormLoadPost(object sender, EventArgs e)
		{
			m_host.MainWindow.FormLoadPost -= MainWindow_FormLoadPost;

			ColorPasswords(ColorConfig.Active);
		}

		private Color m_cBackgroundColor = Color.White;
		private void Lv_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			e.DrawDefault = true;
			m_cBackgroundColor = UIUtil.GetAlternateColorEx(e.Header.ListView.BackColor);
		}

		private void Lv_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			AceColumn colPw = KeePass.Program.Config.MainWindow.FindColumn(AceColumnType.Password);
			string m = string.Empty;
			if (((colPw == null) || colPw.HideWithAsterisks) && !SinglePwDisplay.PasswordShown(e.Item))
			{
				//Let the OS draw in case no other handlers exist
				//If other handlers exist, we pass their value for DrawDefault
				e.DrawDefault = true;
				if (colPw == null) m = "Password column not found";
				else m = "Password column found, password hidden";
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
			/* Do not handle first column
			 * First column references the ListView item and can/has to be drawn by the OS
			*/
			if (e.ColumnIndex == 0)
			{
				e.DrawDefault = true;
				return;
			}

			Color cItemForeground = e.Item.UseItemStyleForSubItems ? e.Item.ForeColor : e.SubItem.ForeColor;
			Color cItemBackground = e.Item.UseItemStyleForSubItems ? e.Item.BackColor : e.SubItem.BackColor;
			if (KeePass.Program.Config.MainWindow.EntryListAlternatingBgColors && ((e.ItemIndex & 1) == 1))
			{
				//Only set defined alternate background color, if the default background color was not changed yet
				//This way, explicitly set background colors do have higher priority
				if (cItemBackground == e.Item.ListView.BackColor) cItemBackground = m_cBackgroundColor;
			}
			Font fFont = e.Item.UseItemStyleForSubItems ? e.Item.Font : e.SubItem.Font;

			//Differentiate between different drawing modes
			if ((ColorConfig.DrawMode == ColorConfig.ListViewDrawMode.DrawPasswordOnly) && (e.Header.Text != KeePass.Resources.KPRes.Password))
			{
				Lv_DrawSubItem_Standard(e, cItemBackground);
				return;
			}

			if (!e.Item.Selected) e.Graphics.FillRectangle(new SolidBrush(cItemBackground), new RectangleF(e.Bounds.X + 1, e.Bounds.Y, e.Bounds.Width - 2, e.Bounds.Height));
			int iPaddingX = 3;
			int iPaddingY = 2;

			if (e.Header.Text != KeePass.Resources.KPRes.Password)
			{
				Lv_DrawSubItem_NoPassword(e, cItemForeground, cItemBackground, fFont, iPaddingX, iPaddingY);
				return;
			}
			else Lv_DrawSubItem_Password(e, cItemForeground, cItemBackground, fFont, iPaddingX, iPaddingY);
		}

		private void Lv_DrawSubItem_Password(DrawListViewSubItemEventArgs e, Color cItemForeground, Color cItemBackground, Font fFont, int iPaddingX, int iPaddingY)
		{
			const string MEASUREHELP = "Air";
			e.DrawDefault = false; //Other plugins might have set this to true
			string msg = "m_lvEntries: Handle password column '" + e.Header.Text + "' - Start";
			PluginDebug.AddInfo(msg, 0);
			//SinglePwDisplay sdp = m_lSinglePwDisplay.Find(xPred => xPred.Index == e.ItemIndex && !xPred.Hidden);
			//if (sdp != null) e.SubItem.Text = sdp.Entry.Strings.ReadSafe(KeePassLib.PwDefs.PasswordField);
			char[] s = e.SubItem.Text.ToCharArray();
			float x = e.Bounds.X + iPaddingX;
			for (int i = 0; i < s.Length; i++)
			{
				StringFormat sf = StringFormat.GenericTypographic;
				//Measuring only the to be drawn character produces wrong results
				//Place to be measured character inbetween something else
				//Trailing spaces are skipped and nothing would be shown otherwise
				float fWidth = e.Graphics.MeasureString(MEASUREHELP + s[i] + MEASUREHELP, fFont, int.MaxValue, sf).Width;
				fWidth -= e.Graphics.MeasureString(MEASUREHELP + MEASUREHELP, fFont, int.MaxValue, sf).Width;
				if ((e.Bounds.X + e.Bounds.Width) <= (x + fWidth)) break;
				Color col;
				Color colb;
				msg = GetPasswordCharColor(s[i], out col, out colb, cItemBackground);
				List<string> lMsg = new List<string>();
				if (ColorUtils.BackgroundColorRequiresAdjustment(col, colb))
				{
					lMsg.Add("Foreground and background color identical, adjusting background color");
					colb = ColorUtils.ContrastColor(colb);
				}
				lMsg.Add("Foreground: " + col.ToString());
				lMsg.Add("Background: " + colb.ToString());
				PluginDebug.AddInfo(msg, 0, lMsg.ToArray());
				RectangleF r = new RectangleF(x, e.Bounds.Y + iPaddingY, fWidth, e.Bounds.Height - (2 * iPaddingY));
				e.Graphics.FillRectangle(new SolidBrush(colb), r);
				//Win 7 calculates width of certain characters wrong and they won't be drawn because they're larger than the rectangle
				//=> Provide starting PointF instead of RectangleF
				PointF p = new PointF(r.X, r.Y);
				//e.Graphics.DrawString(e.SubItem.Text.Substring(i, 1), fFont, new SolidBrush(col), p, sf);
				e.Graphics.DrawString(s[i].ToString(), fFont, new SolidBrush(col), p, sf);
				x += fWidth;
			}
			msg = "m_lvEntries: Handle password column '" + e.Header.Text + "' - Finish";
			PluginDebug.AddInfo(msg, 0);
		}

		private string GetPasswordCharColor(char c, out Color cItemForeground, out Color cItemBackground, Color cBackcolorLV)
		{
			string msg = "Color letter";
			if (char.IsDigit(c))
			{
				cItemForeground = ColorConfig.ForeColorDigit;
				cItemBackground = ColorConfig.ListViewKeepBackgroundColor ? cBackcolorLV : ColorConfig.BackColorDigit;
				msg = "Color digit";
			}
			else if (!char.IsLetter(c))
			{
				cItemForeground = ColorConfig.ForeColorSpecial;
				cItemBackground = ColorConfig.ListViewKeepBackgroundColor ? cBackcolorLV : ColorConfig.BackColorSpecial;
				msg = "Color special char";
			}
			else if (ColorConfig.LowercaseDifferent && char.IsLower(c))
			{
				cItemForeground = ColorConfig.ForeColorLower;
				cItemBackground = ColorConfig.ListViewKeepBackgroundColor ? cBackcolorLV : ColorConfig.BackColorLower;
				msg = "Color lowercase char";
			}
			else
			{
				cItemForeground = ColorConfig.ForeColorDefault;
				cItemBackground = ColorConfig.ListViewKeepBackgroundColor ? cBackcolorLV : ColorConfig.BackColorDefault;
				msg = "Color letter";
			}
			return msg;
		}

		private void Lv_DrawSubItem_NoPassword(DrawListViewSubItemEventArgs e, Color cItemForeground, Color cItemBackground, Font fFont, int iPaddingX, int iPaddingY)
		{
			e.DrawDefault = false; //Other plugins might have set this to true
			string m = "m_lvEntries: Draw text for column '" + e.Header.Text + "'";
			if (!PluginDebug.HasMessage(PluginDebug.LogLevelFlags.Info, m)) PluginDebug.AddInfo(m, 0);
			StringFormat sf = StringFormat.GenericDefault;
			sf.FormatFlags = StringFormatFlags.FitBlackBox | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
			sf.Trimming = StringTrimming.EllipsisCharacter;
			Rectangle r = new Rectangle(e.Bounds.X + iPaddingX, e.Bounds.Y + iPaddingY, e.Bounds.Width - (2 * iPaddingX), e.Bounds.Height - (2 * iPaddingY));
			e.Graphics.DrawString(e.SubItem.Text, fFont, new SolidBrush(cItemForeground), r, sf);
		}

		private void Lv_DrawSubItem_Standard(DrawListViewSubItemEventArgs e, Color cBackcolor)
		{
			e.Item.BackColor = e.SubItem.BackColor = cBackcolor;
			e.DrawDefault = true;
			string m = "m_lvEntries: Standard display for column '" + e.Header.Text + "'";
			if (!PluginDebug.HasMessage(PluginDebug.LogLevelFlags.Info, m)) PluginDebug.AddInfo(m, 0);
		}

		#region Hook forms
		private void OnWindowAdded(object sender, GwmWindowEventArgs e)
		{
			if (e.Form is KeePass.Forms.KeyPromptForm) e.Form.Shown += OnKeyPromptFormShown;
			else if (e.Form is KeePass.Forms.PwGeneratorForm) e.Form.Shown += OnPwGeneratorFormShown;
			else if (e.Form is KeePass.Forms.PrintForm && Tools.KeePassVersion >= ColorConfig.KP_2_51) e.Form.Shown += OnPrintFormShown;
		}

        private void OnPrintFormShown(object sender, EventArgs e)
        {
			var fPrint = sender as KeePass.Forms.PrintForm;
			if (fPrint == null) return;
			fPrint.Shown -= OnPrintFormShown;
			if (!ColorConfig.Active) return;
			if (!ColorConfig.SyncColorsWithPrintForm) return;
			fPrint.FormClosed += OnPrintFormClosed;
			//Adjust ColorButtonEx
			Button cbe = Tools.GetControl("m_btnColorPU", fPrint) as Button;
			if (cbe != null) SetColorButtonColor(cbe, ColorConfig.ForeColorDefault);
			cbe = Tools.GetControl("m_btnColorPL", sender as Form) as Button;
			if (cbe != null)
			{
				if (ColorConfig.LowercaseDifferent) SetColorButtonColor(cbe, ColorConfig.ForeColorLower);
				else SetColorButtonColor(cbe, ColorConfig.ForeColorDefault);
			}
			cbe = Tools.GetControl("m_btnColorPD", sender as Form) as Button;
			if (cbe != null) SetColorButtonColor(cbe, ColorConfig.ForeColorDigit);
			cbe = Tools.GetControl("m_btnColorPO", sender as Form) as Button;
			if (cbe != null) SetColorButtonColor(cbe, ColorConfig.ForeColorSpecial);

			var mUpdateWebBrowser = fPrint.GetType().GetMethod("UpdateWebBrowser", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
			if (mUpdateWebBrowser != null) mUpdateWebBrowser.Invoke(fPrint, new object[] { false });
		}

        private void OnPrintFormClosed(object sender, FormClosedEventArgs e)
        {
			var fPrint = sender as KeePass.Forms.PrintForm;
			if (fPrint == null) return;
			fPrint.FormClosed -= OnPrintFormClosed;
			if (fPrint.DialogResult != DialogResult.OK) return;

			Button cbe = Tools.GetControl("m_btnColorPU", fPrint) as Button;
			if (cbe != null) ColorConfig.ForeColorDefault = GetColorButtonColor(cbe, ColorConfig.ForeColorDefault);
			cbe = Tools.GetControl("m_btnColorPL", sender as Form) as Button;
			if (cbe != null) ColorConfig.ForeColorLower = GetColorButtonColor(cbe, ColorConfig.ForeColorLower);
			cbe = Tools.GetControl("m_btnColorPD", sender as Form) as Button;
			if (cbe != null) ColorConfig.ForeColorDigit = GetColorButtonColor(cbe, ColorConfig.ForeColorDigit);
			cbe = Tools.GetControl("m_btnColorPO", sender as Form) as Button;
			if (cbe != null) ColorConfig.ForeColorSpecial = GetColorButtonColor(cbe, ColorConfig.ForeColorSpecial);
		}

        private Color GetColorButtonColor(Button cbe, Color cDefault)
        {
			var pColor = cbe.GetType().GetProperty("SelectedColor");
			if (pColor == null) return cDefault;
			object o = pColor.GetValue(cbe, null);
			if (!(o is Color)) return cDefault;
			return (Color)o;
		}

		private void SetColorButtonColor(Button bPU, Color cTextcolor)
        {
			var pColor = bPU.GetType().GetProperty("SelectedColor");
			if (pColor == null) return;
			pColor.SetValue(bPU, cTextcolor, null);
		}

        //Color passwords in password generator
        private void OnPwGeneratorFormShown(object sender, EventArgs e)
		{
			(sender as Form).Shown -= OnPwGeneratorFormShown;
			if (!ColorConfig.Active) return;
			if (!ColorConfig.ColorPwGen) return;
			KeePass.Forms.PwGeneratorForm pg = sender as KeePass.Forms.PwGeneratorForm;
			TextBox tb = Tools.GetControl("m_tbPreview", pg) as TextBox;
			if (tb == null)
			{
				PluginDebug.AddError("Could not locate m_tbPreview", 0);
				return;
			}
			ColorTextBox rtb = new ColorTextBox();
			rtb.Name = "ColoredPassword_" + tb.Name;
			rtb.Left = tb.Left;
			rtb.Top = tb.Top;
			rtb.Width = tb.Width;
			rtb.Height = tb.Height;
			rtb.ColorBackground = false;
			rtb.Multiline = tb.Multiline;
			rtb.ReadOnly = tb.ReadOnly;
			rtb.WordWrap = tb.WordWrap;
			rtb.Font = tb.Font;
			rtb.ScrollBars = (RichTextBoxScrollBars)tb.ScrollBars;
			tb.Tag = rtb;
			tb.Visible = false;
			tb.Parent.Controls.Add(rtb);
			tb.TextChanged += Tb_TextChanged;
		}

		private void Tb_TextChanged(object sender, EventArgs e)
		{
			TextBox tb = sender as TextBox;
			ColorTextBox rtb = tb.Tag as ColorTextBox;
			if (rtb == null)
			{
				tb.Visible = true;
				return;
			}
			rtb.Lines = tb.Lines;
		}

		//Show error message if TypeOverride is not possible
		private void OnKeyPromptFormShown(object sender, EventArgs e)
		{
			if (OverridePossible || !ColorConfig.FirstRun || !ColorConfig.Active) return;
			(sender as Form).Shown -= OnKeyPromptFormShown;
			Tools.ShowError(string.Format(PluginTranslate.Error, typeof(SecureTextBoxEx).BaseType.Name));
			ColorConfig.FirstRun = false;
			ColorConfig.Write();
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
			o.cbSyncColorsWithPrintForm.Checked = ColorConfig.SyncColorsWithPrintForm;
			o.cbSinglePwDisplay.Checked = ColorConfig.SinglePwDisplayActive;
			o.cbColorPwGen.Checked = ColorConfig.ColorPwGen;
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
			ColorConfig.SyncColorsWithPrintForm = o.cbSyncColorsWithPrintForm.Checked;
			SinglePwDisplay.Enabled = ColorConfig.SinglePwDisplayActive = o.cbSinglePwDisplay.Checked;
			ColorConfig.ColorPwGen = o.cbColorPwGen.Checked;
			ColorConfig.Write();
			if (ColorConfig.Active) ColorPasswords(ColorConfig.Active);
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
					m_lvEntries.OwnerDraw = active;
					if (active)
					{
						m_lvEntries.DrawColumnHeader += Lv_DrawColumnHeader;
						m_lvEntries.DrawItem += Lv_DrawItem;
						m_lvEntries.DrawSubItem += Lv_DrawSubItem;
					}
					else
					{
						m_lvEntries.DrawColumnHeader -= Lv_DrawColumnHeader;
						m_lvEntries.DrawItem -= Lv_DrawItem;
						m_lvEntries.DrawSubItem -= Lv_DrawSubItem;
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
			GlobalWindowManager.WindowAdded -= OnWindowAdded;

			ColorPasswords(false);
			PluginDebug.SaveOrShow();
			m_host = null;
		}
	}
}