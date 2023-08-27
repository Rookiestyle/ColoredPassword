using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeePass.App.Configuration;
using KeePass.UI;
using PluginTools;

namespace ColoredPassword
{

  public static class SinglePwDisplay
  {
    private static ListView EntriesListView = null;

    private class SingleItem
    {
      public KeePassLib.PwEntry Entry { get; private set; }
      private DateTime m_TimeToHide = DateTime.Now;

      public SingleItem(KeePassLib.PwEntry pe)
      {
        Entry = pe;
        ExtendDisplay();
      }

      public bool Hidden { get { return DateTime.Now > m_TimeToHide; } }

      private void ExtendDisplay()
      {
        int i = Math.Min(60, Math.Max(10, KeePass.Program.Config.Security.ClipboardClearAfterSeconds));
        m_TimeToHide = DateTime.Now + new TimeSpan(0, 0, i);
      }
    }

    private static List<SingleItem> m_lItems = new List<SingleItem>();
    private static Timer m_Timer = new Timer();

    static SinglePwDisplay()
    {
      EntriesListView = Tools.GetControl("m_lvEntries") as ListView;

      m_Timer.Interval = 1000;
      m_Timer.Tick += OnTimerTick;
    }

    private static bool m_Enabled = false;
    public static bool Enabled
    {
      get { return m_Enabled && EntriesListView != null; }
      set { SetEnabled(value); }
    }

    private static void SetEnabled(bool value)
    {
      if (EntriesListView == null || !KeePass.App.AppPolicy.Current.UnhidePasswords)
      {
        m_Enabled = false;
        Clear(true);
        return;
      }
      if (Enabled == value) return;
      if (value)
      {
        if (EntriesListView != null) EntriesListView.MouseClick += OnMouseClick; ;
        if (EntriesListView != null) EntriesListView.DoubleClick += OnLVEntriesDoubleClick;
        KeePass.Program.MainForm.UIStateUpdated += MainForm_UIStateUpdated;
      }
      else
      {
        Clear(true);
        if (EntriesListView != null) EntriesListView.MouseClick -= OnMouseClick;
        if (EntriesListView != null) EntriesListView.DoubleClick -= OnLVEntriesDoubleClick;
        KeePass.Program.MainForm.UIStateUpdated -= MainForm_UIStateUpdated;
      }
      m_Enabled = m_Timer.Enabled = value;
    }

    private static void MainForm_UIStateUpdated(object sender, EventArgs e)
    {
      if (!Enabled) return;
      int iPwColIndex = GetPwColumnIndex();
      if (iPwColIndex < 0)
      {
        m_lItems.Clear();
        return;
      }
      foreach (ListViewItem lvi in EntriesListView.Items)
      {
        if (!PasswordShown(lvi)) continue;
        if (lvi.SubItems[iPwColIndex].Text != KeePassLib.PwDefs.HiddenPassword) continue;
        lvi.SubItems[iPwColIndex].Text = ((PwListItem)lvi.Tag).Entry.Strings.ReadSafe(KeePassLib.PwDefs.PasswordField);
      }
    }

    private static bool AddOrRemove(KeePassLib.PwEntry pe)
    {
      if (!Enabled) return false;
      int idx = IndexOf(pe);
      int iPwColIndex = GetPwColumnIndex();
      SingleItem si = m_lItems.Find(x => x.Entry.Uuid.Equals(pe.Uuid));
      bool bAdd = si == null;
      if (bAdd)
      {
        si = new SingleItem(pe);
        m_lItems.Add(si);
      }
      else
        m_lItems.Remove(si);
      if (idx >= 0 && iPwColIndex >= 0)
      {
        if (bAdd) EntriesListView.Items[idx].SubItems[iPwColIndex].Text = pe.Strings.ReadSafe(KeePassLib.PwDefs.PasswordField);
        else EntriesListView.Items[idx].SubItems[iPwColIndex].Text = KeePassLib.PwDefs.HiddenPassword;
      }
      return true;
    }

    private static bool Remove(KeePassLib.PwEntry pe)
    {
      if (!Enabled) return false;
      int idx = IndexOf(pe);
      int iPwColIndex = GetPwColumnIndex();
      SingleItem si = m_lItems.Find(x => x.Entry.Uuid.Equals(pe.Uuid));
      if (si == null) return true;
      m_lItems.Remove(si);
      if (idx >= 0 && iPwColIndex >= 0)
      {
        EntriesListView.Items[idx].SubItems[iPwColIndex].Text = KeePassLib.PwDefs.HiddenPassword;
      }
      return true;
    }

    private static void Clear(bool bAll)
    {
      if (!Enabled) return;
      int iPwColIndex = GetPwColumnIndex();
      if (iPwColIndex < 0) m_lItems.Clear();
      for (int i = m_lItems.Count - 1; i >= 0; i--)
      {
        SingleItem si = m_lItems[i];
        int idx = IndexOf(si.Entry);
        if (idx < 0)
        {
          m_lItems.Remove(si);
          continue;
        }
        if (!bAll && !si.Hidden) continue;
        EntriesListView.Items[idx].SubItems[iPwColIndex].Text = KeePassLib.PwDefs.HiddenPassword;
        m_lItems.Remove(si);
      }
    }

    private static int GetPwColumnIndex()
    {
      AceColumn c = KeePass.Program.Config.MainWindow.FindColumn(AceColumnType.Password);
      if (c == null || !c.HideWithAsterisks) return -1;
      return KeePass.Program.Config.MainWindow.EntryListColumns.IndexOf(c);
    }

    private static int IndexOf(KeePassLib.PwEntry pe)
    {
      if (!Enabled) return -1;
      foreach (ListViewItem lvi in EntriesListView.Items)
      {
        if (((PwListItem)lvi.Tag).Entry.Uuid.Equals(pe.Uuid))
          return lvi.Index;
      }
      return -1;
    }

    public static bool PasswordShown(ListViewItem lvi)
    {
      if (!Enabled) return false;
      if (lvi == null) return false;
      if (!(lvi.Tag is PwListItem)) return false;
      KeePassLib.PwEntry pe = (lvi.Tag as PwListItem).Entry;

      if (pe == null) return false;
      SingleItem si = m_lItems.Find(x => x.Entry.Uuid.Equals(pe.Uuid));
      return si != null && !si.Hidden;
    }

    private static void OnTimerTick(object sender, EventArgs e)
    {
      if (!Enabled) return;
      AceColumn c = KeePass.Program.Config.MainWindow.FindColumn(AceColumnType.Password);
      bool bAll = (c == null || c.Type != AceColumnType.Password || !c.HideWithAsterisks);
      Clear(bAll);
    }

    private static Dictionary<KeePassLib.PwUuid, Timer> m_CheckDoubleClick = new Dictionary<KeePassLib.PwUuid, Timer>();
    private static void OnMouseClick(object sender, MouseEventArgs e)
    {
      if (!Enabled) return;
      if (e.Button != MouseButtons.Left) return;
      KeePassLib.PwEntry pe = GetEntryFromClick();
      if (pe == null) return;
      Timer t;
      if (m_CheckDoubleClick.TryGetValue(pe.Uuid, out t) && t != null)
      {
        t.Stop();
        t.Dispose();
        m_CheckDoubleClick.Remove(pe.Uuid);
      }
      t = new Timer();
      t.Interval = SystemInformation.DoubleClickTime;
      t.Tick += (o, e1) =>
      {
        AddOrRemove(pe);
        t.Dispose();
        m_CheckDoubleClick.Remove(pe.Uuid);
      };
      m_CheckDoubleClick[pe.Uuid] = t;
      t.Start();
    }

    private static void OnLVEntriesDoubleClick(object sender, EventArgs e)
    {
      if (!Enabled) return;
      KeePassLib.PwEntry pe = GetEntryFromClick();
      if (pe == null) return;
      Timer t;
      if (m_CheckDoubleClick.TryGetValue(pe.Uuid, out t) && t != null)
      {
        t.Stop();
        t.Dispose();
        m_CheckDoubleClick.Remove(pe.Uuid);
      }
      Remove(pe);
    }

    private static KeePassLib.PwEntry GetEntryFromClick()
    {
      Point mousePos = EntriesListView.PointToClient(Control.MousePosition);
      try
      {
        ListViewHitTestInfo hitTest = EntriesListView.HitTest(mousePos);
        int columnIndex = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
        var c = KeePass.Program.Config.MainWindow.EntryListColumns[columnIndex];
        if (c == null || c.Type != AceColumnType.Password || !c.HideWithAsterisks) return null;
        return (hitTest.Item.Tag as PwListItem).Entry;
      }
      catch { return null; }
    }
  }
}
