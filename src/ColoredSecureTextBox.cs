using KeePass.UI;
using PluginTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ColoredPassword
{
	public enum CharType
	{
		Unkown,
		Letter,
		Digit,
		Special,
		Lowercase
	}

	public class CharRange
	{
		public CharacterRange Range;
		public CharType CharType;

		public CharRange()
		{
			Range = new CharacterRange();
		}

#if DEBUG
		public override string ToString()
		{
			return CharType.ToString() + ": " + Range.First.ToString() + " - " + Range.Length.ToString();
		}
#endif
	}

	internal sealed class ColoredSecureTextBox : SecureTextBoxEx
	{
		private ColorTextBox m_text = null;

		private bool m_bReadOnlySaved = false;
		private bool m_bReadOnly = false;

		private bool m_bKeeTheme = false;

		public ColoredSecureTextBox()
		{
			if (DesignMode) return;
			m_text = new ColorTextBox();
			m_text.ParentChanged += M_text_ParentChanged;
			TextChanged += ColoredSecureTextBoxChanged;
			GotFocus += OnFocusChangeRequired;
			EnabledChanged += UpdateEnabledState;
			m_text.Size = Size;
			m_text.Location = Location;
			LocationChanged += ColoredSecureTextBox_LocationChanged;
			SizeChanged += ColoredSecureTextBox_SizeChanged;
		}

		private void M_text_ParentChanged(object sender, EventArgs e)
		{
			m_bKeeTheme = (m_text.Parent != null) && m_text.Parent.GetType().FullName.Contains("KeeTheme");
		}

		private void ColoredSecureTextBox_SizeChanged(object sender, EventArgs e)
		{
			if (!m_bKeeTheme) m_text.Size = Size;
		}

		private void ColoredSecureTextBox_LocationChanged(object sender, EventArgs e)
		{
			if (!m_bKeeTheme) m_text.Location = Location;
		}

		private void UpdateEnabledState(object sender, EventArgs e)
		{
			if (m_text == null) return;
			if (m_text.IsDisposed) return;
			m_text.Enabled = Enabled;
		}

		/*
		* Eventhandler is invoked e. g. in case of generating a new password
		* in PwEntryForm with password being visible
		*/
		private void ColoredSecureTextBoxChanged(object sender, EventArgs e)
		{
			if (UseSystemPasswordChar) return;
			if (m_text == null) return;
			//password is shown in plaintext already => no need to protect anything
			string pw = TextEx.ReadString();
			if (pw != m_text.Text)
			{
				PluginDebug.AddInfo(Name + " ColoredSecureTextBoxChanged - Text changed");
				m_text.Text = pw;
			}
			else
				PluginDebug.AddInfo(Name + " ColoredSecureTextBoxChanged - Text not changed");
		}

		private void OnFocusChangeRequired(object sender, EventArgs e)
		{
			PluginDebug.AddInfo(Name + " Check for focus change");
			if (UseSystemPasswordChar) return;
			if (!ReadOnly) return;
			if (TabStop) return;
			if (m_text == null) return;
			//UIUtil.SetFocus(m_text, GetForm(m_text), true); //Only availabe as of KeePass 2.42
			UIUtil.SetFocus(m_text, GetForm(m_text));
			if (m_text.CanFocus) m_text.Focus();
			PluginDebug.AddInfo(Name + " Focus changed");
		}

		/* React on text changes in internal RichTextBox
		* and update the text in SecureTextBoxEx
		*/
		private void ColorTextChanged(object sender, EventArgs e)
		{
			TextChanged -= ColoredSecureTextBoxChanged;
			if (Text != m_text.Text)
			{
				PluginDebug.AddInfo(Name + " ColorTextChanged - Text changed");
				Text = m_text.Text;
			}
			else
				PluginDebug.AddInfo(Name + " ColorTextChanged - Text not changed");
			TextChanged += ColoredSecureTextBoxChanged;
		}

		/* Ensure the internal RichTextBox m_text has the same parent
		* as the SecureTextBoxEx
		* Adjust TabIndex as well
		*/
		protected override void OnParentChanged(EventArgs e)
		{
			PluginDebug.AddInfo(Name + " Adjust parent control");
			base.OnParentChanged(e);
			if (m_text == null) return;
			if (m_text.Parent != null)
				m_text.Parent.Controls.Remove(m_text);
			if (Parent == null) return;
			Parent.SuspendLayout();
			Parent.Controls.Add(m_text);
			m_text.Parent = Parent;
			foreach (Control c in Parent.Controls)
			{
				if (c.TabIndex > TabIndex)
					c.TabIndex++;
			}
			m_text.TabStop = true;
			m_text.TabIndex = TabIndex + 1;
			Parent.ResumeLayout();
			Parent.PerformLayout();
		}

		public override void EnableProtection(bool bEnable)
		{
			PluginDebug.AddInfo(Name + " Protect password display: " + bEnable.ToString());
			m_text.TextChanged -= ColorTextChanged;
			base.EnableProtection(bEnable);
			if (bEnable)
			{
				Visible = true;
				TabStop = true;
				if (m_bReadOnlySaved)
				{
					ReadOnly = m_bReadOnly;
					m_bReadOnlySaved = false;
				}
				Focus();
				Select(m_text.SelectionStart, m_text.SelectionLength);
				m_text.Visible = false;
				if (m_bKeeTheme) m_text.Parent.Visible = false;
			}
			else
			{
				//Was required until default colors could be configured as well
				//ColorConfig.ForeColorDefault = ForeColor;
				//ColorConfig.BackColorDefault = BackColor;
				m_text.RightToLeft = RightToLeft;
				m_text.Size = Size;
				if (!m_bKeeTheme) m_text.Location = Location;
				m_text.Font = Font;
				m_text.Visible = true;
				if (m_bKeeTheme) m_text.Parent.Visible = true;
				m_text.Text = Text;
				m_text.ReadOnly = ReadOnly;
				TabStop = false;
				if (!m_bReadOnlySaved)
				{
					m_bReadOnly = ReadOnly;
					m_bReadOnlySaved = true;
				}
				ReadOnly = true;
				m_text.Select(SelectionStart, SelectionLength);
				if (m_text.Enabled && !m_text.ReadOnly)
				{
					//UIUtil.SetFocus(m_text, GetForm(m_text), true); //Only availabe as of KeePass 2.42
					UIUtil.SetFocus(m_text, GetForm(m_text));
					if (m_text.CanFocus) m_text.Focus();
				}
				m_text.TextChanged += ColorTextChanged;
				if (m_bKeeTheme) m_text.Parent.BringToFront();
				else m_text.BringToFront();
				if (KeePassLib.Native.NativeLib.IsUnix()) m_text.ColorText();
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				m_text.Dispose();
			}
		}

		private Form GetForm(Control c)
		{
			if (c == null) return null;
			var f = c.Parent;
			while ((f != null) && !(f is Form))
				f = f.Parent;
			return f as Form;
		}
	}

	public class ColorTextBox : RichTextBox
	{
		public bool ColorBackground = true;
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing && (ContextMenuStrip != null) && !ContextMenuStrip.IsDisposed)
			{
				ContextMenuStrip.Opening -= ContextMenuStrip_Opening;
				ContextMenuStrip.Dispose();
				ContextMenuStrip = null;
			}
		}

		public ColorTextBox() : base()
		{
			Multiline = false;

			//Keep ContextMenuStrip_Opening in sync with this
			ContextMenuStrip = new ContextMenuStrip();
			ToolStripMenuItem m = new ToolStripMenuItem() { Text = KeePass.Resources.KPRes.Cut, Name = "CM_Cut" };
			m.Click += (o, e) => Cut();
			ContextMenuStrip.Items.Add(m);
			m = new ToolStripMenuItem() { Text = KeePass.Resources.KPRes.Copy, Name = "CM_Copy" };
			m.Click += (o, e) => Copy();
			ContextMenuStrip.Items.Add(m);
			m = new ToolStripMenuItem() { Text = KeePass.Resources.KPRes.Paste, Name = "CM_Paste" };
			m.Click += (o, e) => Paste(Clipboard.GetText());
			ContextMenuStrip.Items.Add(m);
			m = new ToolStripMenuItem() { Text = KeePass.Resources.KPRes.Delete, Name = "CM_Delete" };
			m.Click += (o, e) =>
			{ SelectedText = string.Empty; };
			ContextMenuStrip.Items.Add(m);
			ContextMenuStrip.Items.Add(new ToolStripSeparator());
			m = new ToolStripMenuItem() { Text = KeePass.Resources.KPRes.SelectAll, Name = "CM_SelectAll" };
			m.Click += (o, e) => { Select(0, Text.Length); };
			ContextMenuStrip.Items.Add(m);
			ContextMenuStrip.Opening += ContextMenuStrip_Opening;
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			if (!DesignMode) ColorText();
		}

		protected void Paste(string strData)
		{
			if (this.SelectionLength > 0)
			{
				this.SelectedText = strData;
				return;
			}
			int nCursorPos = this.SelectionStart;
			string strPre = this.Text.Substring(0, nCursorPos);
			string strPost = this.Text.Substring(nCursorPos);
			this.Text = strPre + strData + strPost;
		}

		public static List<CharRange> GetRanges(string s)
		{
			List<CharRange> lCR = new List<CharRange>();
			if (string.IsNullOrEmpty(s)) return lCR;
			CharType ctPrev = CharType.Unkown;
			CharRange cr = new CharRange();
			int i = 0;
			while (i < s.Length)
			{
				CharType ctCur = CharType.Letter;
				if (char.IsDigit(s, i)) ctCur = CharType.Digit;
				else if (!char.IsLetter(s, i)) ctCur = CharType.Special;
				if (ColorConfig.LowercaseDifferent && (ctCur == CharType.Letter) && char.IsLower(s, i)) ctCur = CharType.Lowercase;
				bool bNewCR = (ctPrev == CharType.Unkown) || ctPrev != ctCur;
				if (bNewCR)
				{
					if (ctPrev != CharType.Unkown)
						lCR.Add(cr);
					cr = new CharRange();
					cr.CharType = ctCur;
					cr.Range.First = i;
				}
				cr.Range.Length++;
				ctPrev = ctCur;
				i++;
			}
			lCR.Add(cr);
			return lCR;
		}

		public void ColorText()
		{
			int nCursorPos = SelectionStart; //save cursor position
			SelectAll();

			List<CharRange> lCR = GetRanges(this.Text);
			List<string> lMsg = new List<string>();
			bool bError = false;
			foreach (var cr in lCR)
			{
				lMsg.Add(cr.ToString());
				Select(cr.Range.First, cr.Range.Length);
				switch (cr.CharType)
				{
					case CharType.Digit:
						SelectionColor = ColorConfig.ForeColorDigit;
						if (ColorBackground) SelectionBackColor = ColorConfig.BackColorDigit;
						break;
					case CharType.Letter:
						SelectionColor = ColorConfig.ForeColorDefault;
						if (ColorBackground) SelectionBackColor = ColorConfig.BackColorDefault;
						break;
					case CharType.Lowercase:
						SelectionColor = ColorConfig.ForeColorLower;
						if (ColorBackground) SelectionBackColor = ColorConfig.BackColorLower;
						break;
					case CharType.Special:
						SelectionColor = ColorConfig.ForeColorSpecial;
						if (ColorBackground) SelectionBackColor = ColorConfig.BackColorSpecial;
						break;
					default:
						lMsg[lMsg.Count - 1] += " unknown character type";
						bError = true;
						continue;
				}
			}

			if (bError) PluginDebug.AddError(Name + " Color password", 0, lMsg.ToArray());
			else PluginDebug.AddInfo(Name + " Color password", 0, lMsg.ToArray());

			Select(nCursorPos, 0); //restore cursor position
		}

		private void ContextMenuStrip_Opening(object sender, EventArgs e)
		{
			//Mono does not suppport 'searchAllChildren' for ToolStripMenuItem
			ContextMenuStrip.Items[0].Enabled = !string.IsNullOrEmpty(SelectedText); //Cut
			ContextMenuStrip.Items[1].Enabled = !string.IsNullOrEmpty(SelectedText); //Copy
			ContextMenuStrip.Items[2].Enabled = Clipboard.ContainsText(); //Paste
			ContextMenuStrip.Items[3].Enabled = !string.IsNullOrEmpty(SelectedText); //Delete

			//ContextMenuStrip.Items[4] = Separator

			ContextMenuStrip.Items[5].Enabled = !string.IsNullOrEmpty(SelectedText); //Select All
		}
	}
}
