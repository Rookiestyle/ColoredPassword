﻿using System;
using System.Windows.Forms;
using KeePass.Resources;
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
      //lText.Text = KeePass.Resources.KPRes.TextColor;
      //lBack.Text = KeePass.Resources.KPRes.BackgroundColor;
      lDefault.Text = KeePass.Resources.KPRes.Default;
      cbLowercase.Text = PluginTranslate.LowercaseDifferentColor;
      lDigits.Text = PluginTranslate.CharDigit;
      lSpecial.Text = PluginTranslate.CharSpecial;
      bReset.Text = KPRes.Default;

      gExample.Text = PluginTranslate.Example;

      lError.Text = string.Format(PluginTranslate.Error, typeof(KeePass.UI.SecureTextBoxEx).BaseType.Name);
      lError2.Text = KeePass.Resources.KPRes.PolicyRequiredFlag + ": " + KeePass.Resources.KPRes.UnhidePasswords;

      gEntryView.Text = KeePass.Resources.KPRes.EntryList;

      cbColorEntryView.Text = PluginTranslate.ColorEntryView;
      cbColorEntryViewKeepBackgroundColor.Text = PluginTranslate.ColorEntryViewKeepBackgroundColor;
      cbSinglePwDisplay.Text = PluginTranslate.SinglePwDisplay;
      var f = KeePass.Program.Translation.Forms.Find(x => x.FullName == "KeePass.Forms.PwGeneratorForm");
      if (f != null && f.Window != null) cbColorPwGen.Text = f.Window.Text;
      else cbColorPwGen.Text = "Password Generator";

      gSyncColorsWithPrintForm.Text = KeePass.Resources.KPRes.Print;
      cbSyncColorsWithPrintForm.Text = PluginTranslate.SyncColors;

      cbDontShowAsterisk.Text = string.Format(PluginTranslate.DontShowAsterisks, KeePass.Resources.KPRes.HideUsingAsterisks);

      cbKeyPromptForm.Text = KPRes.OpenDatabase;
      cbKeyChangeForm.UseMnemonic = false;
      cbKeyChangeForm.Text = KPRes.CreateMasterKey + " & " + KPRes.ChangeMasterKey;
      cbPwEntryForm.Text = KPRes.EditEntry;
      gActiveForms.Text = PluginTranslate.FormsUsingColoredPassword;
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

    private void cgActive_CheckedChanged(object sender, RookieUI.CheckedGroupCheckEventArgs e)
    {
      foreach (Control c in tpAdvanced.Controls)
      {
        if (c.Tag is string && ((string)c.Tag != "KEEPENABLED")) c.Enabled = cgActive.Checked;
        else
        {
          foreach (Control c2 in c.Controls)
          {
            if (c2.Tag is string && ((string)c.Tag != "KEEPENABLED")) c2.Enabled = cgActive.Checked;
          }
        }
      }
      if (PluginTools.Tools.KeePassVersion < ColorConfig.KP_2_51) gSyncColorsWithPrintForm.Enabled = false;
    }

    private void bReset_Click(object sender, EventArgs e)
    {
      var bSyncColorsWithPrintForm = ColorConfig.SyncColorsWithPrintForm;
      ColorConfig.SyncColorsWithPrintForm = cbSyncColorsWithPrintForm.Checked;
      ColorConfig.Reset();
      ColorConfig.SyncColorsWithPrintForm = bSyncColorsWithPrintForm;
      bForeColorDefault.BackColor = ColorConfig.ForeColorDefault;
      bBackColorDefault.BackColor = ColorConfig.BackColorDefault;
      bForeColorDigit.BackColor = ColorConfig.ForeColorDigit;
      bBackColorDigit.BackColor = ColorConfig.BackColorDigit;
      bForeColorSpecial.BackColor = ColorConfig.ForeColorSpecial;
      bBackColorSpecial.BackColor = ColorConfig.BackColorSpecial;
      bForeColorLower.BackColor = ColorConfig.ForeColorLower;
      bBackColorLower.BackColor = ColorConfig.BackColorLower;
      ctbExample.ColorText();
    }
  }
}
