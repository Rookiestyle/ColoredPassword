using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeePass;

namespace PluginTools
{
  internal partial class KeeThemeStub
  {
    private static bool IsKeeThemeEvent(Delegate del)
    {
      if (!Installed) return false;
      if (del == null) return false;
      if (_kt == null) return false;
      return del.Target == _kt;
    }

    internal static void HookMenu(EventHandler OnClickEvent)
    {
      if (!Installed)
      {
        PluginDebug.AddInfo("Check for KeeTheme during startup", 0, "KeeTheme is not installed");
        return;
      }
      var lMsg = new List<string>();
      lMsg.Add("KeeTheme is installed");
      lMsg.AddRange(lErrors);
      if (Enabled) lMsg.Add("KeeTheme is enabled, KeeTheme menu does not need to be hooked");
      else
      {
        lMsg.Add("KeeTheme is not enabled, KeeTheme menu must to be hooked");
        bool bFound = false;
        foreach (var i in Program.MainForm.ToolsMenu.DropDownItems)
        {
          if (!(i is ToolStripMenuItem)) continue;
          var m = i as ToolStripMenuItem;
          var lEvents = m.GetEventHandlers("Click");
          var ktClick = lEvents.Find(x => IsKeeThemeEvent(x));
          if (ktClick != null)
          {
            //if KeeTheme is not active at startup, we need to hook it
            //ListView.OwnerDraw will be set by us otherwise and thus KeeTheme's handlers won't be added
            m.RemoveEventHandlers("Click", lEvents);
            m.Tag = lEvents;
            m.Click += OnClickEvent;
            bFound = true;
            break;
          }
        }
        if (bFound) lMsg.Add("KeeTheme menu found and replaced");
        else lMsg.Add("KeeTheme menu not found and not replaced");
      }
      PluginDebug.AddInfo("Check for KeeTheme during startup", 0, lMsg.ToArray());
    }
  }
}
