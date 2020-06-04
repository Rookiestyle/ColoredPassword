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

			cgActive.Text = PluginTranslate.Active;
			lText.Text = KeePass.Resources.KPRes.TextColor;
			lBack.Text = KeePass.Resources.KPRes.BackgroundColor;
			lDefault.Text = KeePass.Resources.KPRes.Default;
			cbLowercase.Text = PluginTranslate.LowercaseDifferentColor;
			lDigits.Text = PluginTranslate.CharDigit;
			lSpecial.Text = PluginTranslate.CharSpecial;
			cbColorEntryView.Text = PluginTranslate.ColorEntryView;
			cbColorEntryViewKeepBackgroundColor.Text = PluginTranslate.ColorEntryViewKeepBackgroundColor;
			gExample.Text = PluginTranslate.Example;

			lError.Text = string.Format(PluginTranslate.Error, typeof(KeePass.UI.SecureTextBoxEx).BaseType.Name);

			lError2.Visible = !KeePass.App.AppPolicy.Current.UnhidePasswords;
			lError2.Text = KeePass.Resources.KPRes.PolicyRequiredFlag + ": " + KeePass.Resources.KPRes.UnhidePasswords; ;
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
			tlp.ColumnStyles[1].SizeType = tlp.ColumnStyles[2].SizeType = SizeType.AutoSize;
			int w = (int)Math.Max(lText.Width, lBack.Width) + 10;
			w = Math.Min(w, tlp.ClientSize.Width / 3);
			tlp.ColumnStyles[1].SizeType = tlp.ColumnStyles[2].SizeType = SizeType.Absolute;
			tlp.ColumnStyles[1].Width = tlp.ColumnStyles[2].Width = w;
		}

		private void cbLowercase_CheckedChanged(object sender, EventArgs e)
		{
			ColorConfig.LowercaseDifferent = cbLowercase.Checked;
			ctbExample.ColorText();
		}
	}
}
