using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using KeePass.App.Configuration;
using KeePass.Plugins;
using KeePassLib;

namespace ColoredPassword
{
  internal static class ColorConfig
  {
    private static AceCustomConfig m_Config = KeePass.Program.Config.CustomConfig;
    public static bool FirstRun = true;
    public static bool Active = true;
    public static bool ColorEntryView = true;
    public static bool SinglePwDisplayActive = true;

    public static Version KP_2_51 = new Version(2, 51);

    public static bool ActiveKeyPromptForm
    {
      get { return m_Config.GetBool("ColoredPassword.ActiveKeyPromptForm", true); }
      set { m_Config.SetBool("ColoredPassword.ActiveKeyPromptForm", value); }
    }
    public static bool ActivePasswordGeneratorForm
    {
      get { return m_Config.GetBool("ColoredPassword.ActivePasswordGeneratorForm", true); }
      set { m_Config.SetBool("ColoredPassword.ActivePasswordGeneratorForm", value); }
    }

    public static bool ActiveKeyChangeForm
    {
      get { return m_Config.GetBool("ColoredPassword.ActiveKeyChangeForm", true); }
      set { m_Config.SetBool("ColoredPassword.ActiveKeyChangeForm", value); }
    }

    public static bool ActivePwEntryForm
    {
      get { return m_Config.GetBool("ColoredPassword.ActivePwEntryForm", true); }
      set { m_Config.SetBool("ColoredPassword.ActivePwEntryForm", value); }
    }

    public static bool SyncColorsWithPrintForm
    {
      get { return m_Config.GetBool("ColoredPassword.SyncColorsWithPrintForm", true); }
      set { m_Config.SetBool("ColoredPassword.SyncColorsWithPrintForm", value); }
    }

    public static bool DontShowAsteriskForEmptyFields
    {
      get { return m_Config.GetBool("ColoredPassword.DontShowAsteriskForEmptyFields", false); }
      set { m_Config.SetBool("ColoredPassword.DontShowAsteriskForEmptyFields", value); }
    }

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
    public static ListViewDrawMode DrawMode
    {
      get
      {
        if (KeePassLib.Native.NativeLib.IsUnix()) return ListViewDrawMode.DrawAllColumns;
        string m = KeePass.Program.Config.CustomConfig.GetString("ColoredPassword.DrawMode", ListViewDrawMode.DrawPasswordOnly.ToString());
        ListViewDrawMode r = ListViewDrawMode.DrawPasswordOnly;
        try
        {
          r = (ListViewDrawMode)Enum.Parse(typeof(ListViewDrawMode), m, true);
          return r;
        }
        catch
        {
          KeePass.Program.Config.CustomConfig.SetString("ColoredPassword.DrawMode", ListViewDrawMode.DrawPasswordOnly.ToString());
          return ListViewDrawMode.DrawPasswordOnly;
        }
      }
    }

    //Some users manually change the config file
    //This might result in ugly error messages / plugin behaviour otherwise
    //cf. https://github.com/Rookiestyle/ColoredPassword/issues/17
    private static Color GetConfigColor(string strID, string strDefault)
    {
      var s = m_Config.GetString(strID, strDefault);
      if (s == null)
      {
        PluginTools.PluginDebug.AddError("Error reading color value", 0, new string[] { "ID: " + strID });
        s = strDefault;
      }
      try
      {
        return NameToColor(s);
      }
      catch
      {
        PluginTools.PluginDebug.AddError("Error reading color value", 0, new string[] { "ID: " + strID, "Value: " + s });
        if (strID.ToLowerInvariant().Contains("back")) s = "Window";
        else s = "WindowText";
      }
      return NameToColor(s);
    }

    public static void Read()
    {
      bool test = Testmode;
      Testmode = false;
      const string ConfigPrefix = "ColoredPassword.";
      Dictionary<string, string> dPrintColors = new Dictionary<string, string>();
      dPrintColors["ColorPU"] = "WindowText";
      dPrintColors["ColorPL"] = "WindowText";
      dPrintColors["ColorPD"] = "Red";
      dPrintColors["ColorPO"] = "Green";
      var lColorKeys = new List<string>( dPrintColors.Keys );
      if (SyncColorsWithPrintForm && PluginTools.Tools.KeePassVersion >= ColorConfig.KP_2_51)
      {
        //values are hardcode in early versions
        dPrintColors["ColorPU"] = "#0000ff"; // Color.FromArgb(0, 0, 255);
        dPrintColors["ColorPL"] = "#000000"; // Color.FromArgb(0, 0, 0);
        dPrintColors["ColorPD"] = "#008000"; // Color.FromArgb(0, 128, 0);
        dPrintColors["ColorPO"] = "#c00000"; // Color.FromArgb(192, 0, 0);
        var y = KeePass.Program.MainForm.GetType().Assembly.GetTypes().Where(x => x.FullName == "KeePass.App.Configuration.AcePrint").FirstOrDefault();
        try
        {
          var c = y.GetConstructors()[0];
          var o = c.Invoke(new object[] { });
          foreach (var sKey in lColorKeys)
          {
            var p = o.GetType().GetProperty(sKey);
            dPrintColors[sKey] = (string)p.GetValue(o, null);
          }
        }
        catch { }
      }
      FirstRun = m_Config.GetBool(ConfigPrefix + "FirstRun", true);
      Active = m_Config.GetBool(ConfigPrefix + "Active", true);
      ColorEntryView = m_Config.GetBool(ConfigPrefix + "ColorEntryView", true);
      ListViewKeepBackgroundColor = m_Config.GetBool(ConfigPrefix + "ListViewKeepBackgroundColor", true);
      ForeColorDefault = GetConfigColor(ConfigPrefix + "ForeColorDefault", dPrintColors["ColorPU"]);
      BackColorDefault = GetConfigColor(ConfigPrefix + "BackColorDefault", "Window");
      ForeColorDigit = GetConfigColor(ConfigPrefix + "ForeColorDigit", dPrintColors["ColorPD"]);
      BackColorDigit = GetConfigColor(ConfigPrefix + "BackColorDigit", "White");
      ForeColorSpecial = GetConfigColor(ConfigPrefix + "ForeColorSpecial", dPrintColors["ColorPO"]);
      BackColorSpecial = GetConfigColor(ConfigPrefix + "BackColorSpecial", "White");
      LowercaseDifferent = m_Config.GetBool(ConfigPrefix + "LowercaseDifferent", false);
      ForeColorLower = GetConfigColor(ConfigPrefix + "ForeColorLower", dPrintColors["ColorPL"]); // ColorToName(ForeColorDefault));
      BackColorLower = GetConfigColor(ConfigPrefix + "BackColorLower", ColorToName(BackColorDefault));
      SinglePwDisplayActive = m_Config.GetBool(ConfigPrefix + "SinglePwDisplay", SinglePwDisplayActive);
      Testmode = test;
      Write();
    }

    public static void Reset()
    {
      bool test = Testmode;
      Testmode = false;
      const string ConfigPrefix = "ColoredPassword.";
      m_Config.SetString(ConfigPrefix + "ForeColorDefault", null);
      m_Config.SetString(ConfigPrefix + "BackColorDefault", null);
      m_Config.SetString(ConfigPrefix + "ForeColorDigit", null);
      m_Config.SetString(ConfigPrefix + "BackColorDigit", null);
      m_Config.SetString(ConfigPrefix + "ForeColorSpecial", null);
      m_Config.SetString(ConfigPrefix + "BackColorSpecial", null);
      m_Config.SetString(ConfigPrefix + "ForeColorLower", null);
      m_Config.SetString(ConfigPrefix + "BackColorLower", null);
      Read();
      Testmode = test;
    }

    public static void Write()
    {
      bool test = Testmode;
      Testmode = false;
      const string ConfigPrefix = "ColoredPassword.";
      m_Config.SetBool(ConfigPrefix + "FirstRun", FirstRun);
      m_Config.SetBool(ConfigPrefix + "Active", Active);
      m_Config.SetBool(ConfigPrefix + "ColorEntryView", ColorEntryView);
      m_Config.SetBool(ConfigPrefix + "ListViewKeepBackgroundColor", ListViewKeepBackgroundColor);
      m_Config.SetString(ConfigPrefix + "ForeColorDefault", ColorToName(ForeColorDefault));
      m_Config.SetString(ConfigPrefix + "BackColorDefault", ColorToName(BackColorDefault));
      m_Config.SetString(ConfigPrefix + "ForeColorDigit", ColorToName(ForeColorDigit));
      m_Config.SetString(ConfigPrefix + "BackColorDigit", ColorToName(BackColorDigit));
      m_Config.SetString(ConfigPrefix + "ForeColorSpecial", ColorToName(ForeColorSpecial));
      m_Config.SetString(ConfigPrefix + "BackColorSpecial", ColorToName(BackColorSpecial));
      m_Config.SetBool(ConfigPrefix + "LowercaseDifferent", LowercaseDifferent);
      m_Config.SetString(ConfigPrefix + "ForeColorLower", ColorToName(ForeColorLower));
      m_Config.SetString(ConfigPrefix + "BackColorLower", ColorToName(BackColorLower));
      m_Config.SetBool(ConfigPrefix + "SinglePwDisplay", SinglePwDisplayActive);
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
      if (c.Length == 7 && c[0] == '#')
      {
        try
        {
          return ColorTranslator.FromHtml(c);
        }
        catch { }
      }
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

    internal enum ListViewDrawMode
    {
      DrawAllColumns,
      DrawPasswordOnly,
    }
  }

  internal static class ColorUtils
  {
    internal static bool BackgroundColorRequiresAdjustment(Color c1, Color c2)
    {
      return c1.ToArgb() == c2.ToArgb();
    }

    internal static Color ContrastColor(Color c)
    {
      //https://en.wikipedia.org/wiki/Luma_%28video%29
      var l = 0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B;
      return l < 0.5 ? Color.White : Color.Black;
    }
  }

  class CP_ColumnType
  {
    private AceColumn m_ace = null;

    private string m_sField = null;

    public CP_ColumnType(AceColumn ace)
    {
      m_ace = ace;
      if (m_ace == null) return;
      switch (ace.Type)
      {
        case AceColumnType.Title: m_sField = PwDefs.TitleField; break;
        case AceColumnType.UserName: m_sField = PwDefs.UserNameField; break;
        case AceColumnType.Password: m_sField = PwDefs.PasswordField; break;
        case AceColumnType.Url: m_sField = PwDefs.UrlField; break;
        case AceColumnType.Notes: m_sField = PwDefs.NotesField; break;
        case AceColumnType.CustomString: m_sField = m_ace.CustomName; break;
        case AceColumnType.PluginExt: m_sField = m_ace.CustomName; break;
      }
      if (!string.IsNullOrEmpty(m_sField)) return;
      
      PluginTools.PluginDebug.AddWarning("Unsupported column for asterisk check: " + m_ace.Type.ToString() + 
          (string.IsNullOrEmpty(m_ace.CustomName) ? string.Empty : " - " + m_ace.CustomName), 0);
    }
    public bool IsEmpty(PwEntry pe)
    {
      if (m_ace == null) return false;

      if (m_sField == null) return false;
      if (pe == null) return false;

      if (m_ace.Type != AceColumnType.PluginExt) return pe.Strings.GetSafe(m_sField).IsEmpty;

      return string.IsNullOrEmpty(KeePass.Program.ColumnProviderPool.GetCellData(m_sField, pe));
    }
  }

}
