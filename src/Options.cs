using System;
using System.Windows.Forms;

using PluginTranslation;

namespace ColoredPassword
{
	public partial class Options : UserControl
	{
		public Options()
		{
			InitializeComponent();
			bForeColorDefault.Text = string.Empty;
			bBackColorDefault.Text = string.Empty;
			bForeColorLower.Text = string.Empty;
			bBackColorLower.Text = string.Empty;
			bForeColorDigit.Text = string.Empty;
			bBackColorDigit.Text = string.Empty;
			bForeColorSpecial.Text = string.Empty;
			bBackColorSpecial.Text = string.Empty;

			tpSettings.Text = KeePass.Resources.KPRes.Options;
			tpAdvanced.Text = KeePass.Resources.KPRes.Advanced;
			
			cgActive.Text = PluginTranslate.Active;
			lDefault.Text = KeePass.Resources.KPRes.Default;
			cbLowercase.Text = PluginTranslate.LowercaseDifferentColor;
			lDigits.Text = PluginTranslate.CharDigit;
			lSpecial.Text = PluginTranslate.CharSpecial;

			gExample.Text = PluginTranslate.Example;

			lError.Text = string.Format(PluginTranslate.Error, typeof(KeePass.UI.SecureTextBoxEx).BaseType.Name);
			lError2.Text = KeePass.Resources.KPRes.PolicyRequiredFlag + ": " + KeePass.Resources.KPRes.UnhidePasswords;

			gEntryView.Text = KeePass.Resources.KPRes.EntryList;

			cbColorEntryView.Text = PluginTranslate.ColorEntryView;
			cbColorEntryViewKeepBackgroundColor.Text = PluginTranslate.ColorEntryViewKeepBackgroundColor;
			cbSinglePwDisplay.Text = PluginTranslate.SinglePwDisplay;
		}

		private void OnColorSelect(object sender, EventArgs e)
		{
			cdSelect.Color = (sender as Button).BackColor;
			if (cdSelect.ShowDialog() == DialogResult.OK)
				(sender as Button).BackColor = cdSelect.Color;
			ColorConfig.ForeColorDefault = bForeColorDefault.BackColor;
			ColorConfig.BackColorDefault = bBackColorDefault.BackColor;
			ColorConfig.ForeColorLower = bForeColorLower.BackColor;
			ColorConfig.BackColorLower = bBackColorLower.BackColor;
			ColorConfig.ForeColorDigit = bForeColorDigit.BackColor;
			ColorConfig.BackColorDigit = bBackColorDigit.BackColor;
			ColorConfig.ForeColorSpecial = bForeColorSpecial.BackColor;
			ColorConfig.BackColorSpecial = bBackColorSpecial.BackColor;
			ctbExample.ColorText();
		}

		public void Options_Resize(object sender, EventArgs e)
		{
			pError.Visible = !ColoredPasswordExt.OverridePossible || !KeePass.App.AppPolicy.Current.UnhidePasswords;
			lError2.Visible = !KeePass.App.AppPolicy.Current.UnhidePasswords;
			lError.Visible = !ColoredPasswordExt.OverridePossible;
			if (!lError.Visible && lError2.Visible) lError2.Top = lError.Top;
		}

		private void cbLowercase_CheckedChanged(object sender, EventArgs e)
		{
			ColorConfig.LowercaseDifferent = cbLowercase.Checked;
			ctbExample.ColorText();
		}
	}
}
