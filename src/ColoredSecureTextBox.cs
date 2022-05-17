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

		private Form m_form = null;

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
			//Focus change is no longer triggered since KeePass 2.49
			Enter += OnFocusChangeRequired;
		}

		private void M_text_ParentChanged(object sender, EventArgs e)
		{
			m_bKeeTheme = (m_text.Parent != null) && m_text.Parent.GetType().FullName.Contains("KeeTheme");

			//https://github.com/Rookiestyle/ColoredPassword/issues/11
			//
			//If passwords are shon in plaintext, ColoredPassword
			//replaces the password text box with a RichTextBox
			//
			//KeePass itself checks m_tbPassword.CanFocus which is false
			//in that case and by that, KeePass does not focuses the password field
			if (m_form != null) m_form.Shown -= CorrectFocus;
			m_form = FindForm() as KeePass.Forms.KeyPromptForm;
			if (m_form == null) m_form = FindForm() as KeePass.Forms.KeyCreationForm;
			if (m_form != null) m_form.Shown += CorrectFocus;
		}

		private void CorrectFocus(object sender, EventArgs e)
		{
			if (!Enabled) return;
			if (m_form == null) return;

			UIUtil.ResetFocus(this, m_form);
			if (Name.Contains("Repeat"))
			{
				var c = Tools.GetControl(Name.Replace("Repeat", string.Empty), m_form);
				if (c != null) UIUtil.ResetFocus(c, m_form, true); 
			} 
		}

		private void ColoredSecureTextBox_SizeChanged(object sender, EventArgs e)
		{
			m_text.Size = Size;
			if (m_bKeeTheme) m_text.Parent.Size = Size;
		}

		private void ColoredSecureTextBox_LocationChanged(object sender, EventArgs e)
		{
			AdjustLocation();
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
			m_text.TabStop = true;
			m_text.TabIndex = TabIndex;
			Parent.ResumeLayout();
			Parent.PerformLayout();
		}

		//Don't show ColorTextBox if
		//  - m_form is set (KeyPromptForm or KeyCreationForm)
		//  AND
		//  - form.Shown event has not been raised yet
		//
		//This can result in a wrong CAPS LOCK warning tooltip - cf. https://github.com/Rookiestyle/ColoredPassword/issues/15
		private bool m_bKeyFormShown = false;
		private bool? m_bRememberedProtectionState = null;
		public override void EnableProtection(bool bEnable)
		{
			PluginDebug.AddInfo(Name + " Protect password display: " + bEnable.ToString());
			m_text.TextChanged -= ColorTextChanged;
			m_text.Name = Name + "_RTB";
			base.EnableProtection(bEnable);
			if (WaitForFormShown())
			{
				RememberProtectionState(bEnable);
				return;
			}
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
				m_text.TabStop = false;
				if (m_bKeeTheme) m_text.Parent.Visible = false;
			}
			else
			{
				//Was required until default colors could be configured as well
				//ColorConfig.ForeColorDefault = ForeColor;
				//ColorConfig.BackColorDefault = BackColor;
				m_text.RightToLeft = RightToLeft;
				m_text.Size = Size;
				AdjustLocation();
				m_text.Font = Font;
				//If password repeat is off and KeeTheme is active => Hide RichTextBox, will not be shown properly otherwise
				bool bVisible = Enabled || !(Name.Contains("Repeat") && m_bKeeTheme);
				if (m_bKeeTheme) m_text.Parent.Visible = bVisible;
				m_text.Visible = bVisible;
				m_text.TabStop = bVisible;
				m_text.Text = Text;
				m_text.ReadOnly = ReadOnly;
				Visible = false;
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

        private void RememberProtectionState(bool bEnable)
        {
			//Subscribe to event 'Shown' only once
			if (!m_bRememberedProtectionState.HasValue)
			{
				m_form.Shown += (o, e) =>
				{
					m_bKeyFormShown = true;
					EnableProtection(m_bRememberedProtectionState.Value);
					CorrectFocus(null, null);
				};
			}
			m_bRememberedProtectionState = bEnable;
		}

		private bool WaitForFormShown()
        {
			if (m_bKeyFormShown) return false;
			if (m_form == null) return false;
			return true;
        }

        private void AdjustLocation()
		{
			//If KeeTheme is active, the ColoredTextBox is contained in 
			//a RichTextBoxDecorator

			//Change the location of the RichTextBoxDecorator if KeeTheme is active
			//Change the location of the ColoredTextBox otherwise
			if (m_bKeeTheme) m_text.Parent.Location = Location;
			else m_text.Location = Location;
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

	public class ColorTextBox : CustomRichTextBoxEx
	{
		private bool m_bColorBackground = true;
		private bool? m_bKeeTheme = null;

		private RichTextBoxContextMenu m_ctx = new RichTextBoxContextMenu();

		public bool ColorBackground
		{
			get { return GetColorBackground(); }
			set { m_bColorBackground = value; }
		}

		private bool GetColorBackground()
		{
			if (!m_bKeeTheme.HasValue && Parent != null) m_bKeeTheme = Parent.GetType().FullName.Contains("KeeTheme");
			if (!m_bKeeTheme.HasValue) return m_bColorBackground;
			return !m_bKeeTheme.Value && m_bColorBackground;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			m_ctx.Detach();
		}

		public ColorTextBox() : base()
		{
			Multiline = false;
			SimpleTextOnly = true;
			m_ctx.Attach(this, null);
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			if (!DesignMode) ColorText();
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
	}
}