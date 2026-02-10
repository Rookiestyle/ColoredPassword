using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using KeePass;
using KeePass.Ecas;
using KeePass.Plugins;
using KeePassLib;
using PluginTools;

namespace PluginTools
{
  internal partial class KeeThemeStub
  {
    //Taken from EcasDefaultEventprovider.cs
    private static readonly PwUuid AppInitPost = new PwUuid(new byte[] {
      0xD4, 0xCE, 0xCD, 0xB5, 0x4B, 0x98, 0x4F, 0xF2,
      0xA6, 0xA9, 0xE2, 0x55, 0x26, 0x1E, 0xC8, 0xE8
    });
    private static Plugin _kt = null;
    private static object _ktTheme = null;
    private static PropertyInfo _pEnabled = null;
    private static object _themeListView = null;
    private static PropertyInfo _pOddRowColor = null;
    private static object _controlVisitor = null;
    private static MethodInfo _visit = null;
    private static bool _initialized = false;

    internal static bool Installed { get { return InitInternal(); } }

    private static List<string> lErrors = new List<string>();
    internal static bool Enabled
    {
      get
      {
        if (!Installed || _ktTheme == null || _pEnabled == null) return false;
        return (bool)_pEnabled.GetValue(_ktTheme, null);
      }
    }

    internal static Color GetOddRowColor(Color colDefault)
    {
      if (!Installed || _themeListView == null || _pOddRowColor == null) return colDefault;
      try
      {
        return (Color)_pOddRowColor.GetValue(_themeListView, null);
      }
      catch { return colDefault; }
    }

    static KeeThemeStub() 
    { 
      InitInternal(true);
      if (Installed) return;
      Init();
    }

    public static void Init()
    {
      //Trigger AppInitPost is triggered BEFORE the last used database is opened
      Program.TriggerSystem.RaisingEvent -= TriggerSystem_RaisingEvent;
      Program.MainForm.FormLoadPost -= MainForm_FormLoadPost;
      if (Program.TriggerSystem.Enabled) Program.TriggerSystem.RaisingEvent += TriggerSystem_RaisingEvent;
      else Program.MainForm.FormLoadPost += MainForm_FormLoadPost;
    }

    private static bool InitInternal(bool bForce = false)
    {
      if (_initialized && !bForce) return _kt != null;
      _initialized = true;
      
      if (_kt != null) return true;
      _kt = Tools.GetPluginInstance("KeeTheme") as Plugin;

      if (_kt != null)
      {
        GetFields();
        var f = _kt.GetType().GetField("_controlVisitor", BindingFlags.Instance | BindingFlags.NonPublic);
        _controlVisitor = f.GetValue(_kt);
        _visit = _controlVisitor.GetType().GetMethod("Visit");
      }
      return _kt != null;
    }

    private static void MainForm_FormLoadPost(object sender, EventArgs e)
    {
      Program.MainForm.FormLoadPost -= MainForm_FormLoadPost;
      InitInternal(true);
    }

    private static void TriggerSystem_RaisingEvent(object sender, KeePass.Ecas.EcasRaisingEventArgs e)
    {
      if (!e.Event.Type.Equals(AppInitPost)) return;
      Program.TriggerSystem.RaisingEvent -= TriggerSystem_RaisingEvent;
      InitInternal(true);
    }

    private static void GetFields()
    {
      var propKeeTheme = _kt.GetType().GetField("_theme", BindingFlags.Instance | BindingFlags.NonPublic);
      if (propKeeTheme == null)
      {
        lErrors.Add("Could not locate field KeeTheme._theme");
        return;
      }
      _ktTheme = propKeeTheme.GetValue(_kt);
      if (_ktTheme == null)
      {
        lErrors.Add("Could not get KeeTheme._theme");
        return;
      }

      _pEnabled = _ktTheme.GetType().GetProperty("Enabled");
      if (_pEnabled == null)
      {
        lErrors.Add("Could not locate property KeeTheme._theme.Enabled");
        return;
      }

      var f = _ktTheme.GetType().GetField("_theme", BindingFlags.Instance | BindingFlags.NonPublic);
      if (f == null)
      {
        lErrors.Add("Could not locate field KeeTheme._theme._theme");
        return;
      }
      var _theme = f.GetValue(_ktTheme);
      if (_theme == null)
      {
        lErrors.Add("Could not get KeeTheme._theme._theme");
        return;
      }
      var pListView = _theme.GetType().GetProperty("ListView", BindingFlags.Instance | BindingFlags.Public);
      if (pListView == null)
      {
        lErrors.Add("Could not locate property KeeTheme._theme._theme.ListViw");
        return;
      }
      _themeListView = pListView.GetValue(_theme, null);
      if (_themeListView == null)
      {
        lErrors.Add("Could not get property KeeTheme._theme._theme.ListView");
        return;
      }

      _pOddRowColor = _themeListView.GetType().GetProperty("OddRowColor", BindingFlags.Instance | BindingFlags.Public);
      if (_pOddRowColor == null)
      {
        lErrors.Add("Could not locate property KeeTheme._theme._theme.ListView.OddRowColor");
        return;
      }
    }

    internal static void Visit(params Control[] aControls)
    {
      if (!Enabled) return;
      if (_visit == null) return;
      foreach (Control c in aControls)
      {
        try
        {
          _visit.Invoke(_controlVisitor, new object[] { c });
        }
        catch { PluginDebug.AddError("Could not call KeeTheme to decorate the new control"); }
      }
    }
  }
}
