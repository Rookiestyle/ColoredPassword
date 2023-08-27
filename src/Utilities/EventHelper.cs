using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace PluginTools
{
  public static class EventHelper
  {
    public static object DoInvoke(object sender, bool bUnwrapMonoWorkaround, List<Delegate> handlers, params object[] parameters)
    {
      if (handlers == null) return null;
      List<Delegate> lDelegates = bUnwrapMonoWorkaround ? UnwrapMonoWorkaround(sender, handlers) : handlers;
      if (lDelegates.Count == 1)
      {
        return lDelegates[0].DynamicInvoke(parameters) as object;
      }
      foreach (Delegate d in lDelegates)
        d.DynamicInvoke(parameters);
      return true;
    }

    public static List<Delegate> GetEventHandlers(this object obj, string EventName)
    {
      List<Delegate> result = new List<Delegate>();
      if (obj == null) return result;

      Type t = obj.GetType();
      List<FieldInfo> event_fields = GetTypeEventFields(t);
      EventHandlerList static_event_handlers = null;

      foreach (FieldInfo fi in event_fields)
      {
        if (!CheckEvent(fi, EventName)) continue;

        if (fi.IsStatic)
        {

          if (static_event_handlers == null)
            static_event_handlers = GetStaticEventHandlerList(t, obj);

          object idx = fi.GetValue(obj);


          Delegate eh = static_event_handlers[idx];
          if (eh == null)
          {
            var head = GetHead(static_event_handlers);
            List<Delegate> lDel = new List<Delegate>();
            CollectEventHandler(head, lDel);
            if (lDel.Count == 0) continue;
            result.AddRange(lDel);
          }
          else
          {
            Delegate[] dels = eh.GetInvocationList();
            if (dels == null) continue;
            result.AddRange(dels);
          }
        }
        else
        {
          EventInfo ei = t.GetEvent(fi.Name, AllBindings);
          if (ei == null)
            ei = t.GetEvent(EventName, AllBindings);
          if (ei == null)
            ei = fi.DeclaringType.GetEvent(fi.Name, AllBindings);
          if (ei == null)
            ei = fi.DeclaringType.GetEvent(EventName, AllBindings);
          if (ei != null)
          {
            object val = fi.GetValue(obj);
            Delegate mdel = (val as Delegate);
            if (mdel != null)
              result.AddRange(mdel.GetInvocationList());
          }
        }
      }
      return result;
    }

    public static void RemoveEventHandlers(this object obj, string EventName, List<Delegate> handlers)
    {
      if (obj == null) return;
      if (handlers == null) return;

      Type t = obj.GetType();
      List<FieldInfo> event_fields = GetTypeEventFields(t);

      foreach (FieldInfo fi in event_fields)
      {
        if (!CheckEvent(fi, EventName)) continue;

        if (fi.IsStatic)
        {
          EventInfo ei = t.GetEvent(fi.Name, AllBindings);
          if (ei == null)
            ei = t.GetEvent(EventName, AllBindings);
          if (ei == null)
            ei = fi.DeclaringType.GetEvent(fi.Name, AllBindings);
          if (ei == null)
            ei = fi.DeclaringType.GetEvent(EventName, AllBindings);
          if (ei == null) continue;

          foreach (Delegate del in handlers)
            ei.RemoveEventHandler(obj, del);
        }
        else
        {
          EventInfo ei = t.GetEvent(fi.Name, AllBindings);
          if (ei == null)
            ei = t.GetEvent(EventName, AllBindings);
          if (ei == null)
            ei = fi.DeclaringType.GetEvent(fi.Name, AllBindings);
          if (ei == null)
            ei = fi.DeclaringType.GetEvent(EventName, AllBindings);
          if (ei != null)
          {
            foreach (Delegate del in handlers)
              ei.RemoveEventHandler(obj, del);
          }
        }
      }
    }

    public static bool AddEventHandlers(this object obj, string EventName, List<Delegate> handlers)
    {
      if (obj == null) return false;
      if (handlers == null) return false;

      Type t = obj.GetType();
      List<FieldInfo> event_fields = GetTypeEventFields(t);

      bool added = false;
      foreach (FieldInfo fi in event_fields)
      {
        if (!CheckEvent(fi, EventName)) continue;

        if (fi.IsStatic)
        {
          EventInfo ei = t.GetEvent(fi.Name, AllBindings);
          if (ei == null)
            ei = t.GetEvent(EventName, AllBindings);
          if (ei == null)
            ei = fi.DeclaringType.GetEvent(fi.Name, AllBindings);
          if (ei == null)
            ei = fi.DeclaringType.GetEvent(EventName, AllBindings);
          if (ei == null) continue;

          foreach (var del in handlers)
            ei.AddEventHandler(obj, del);
          added = true;
        }
        else
        {
          EventInfo ei = t.GetEvent(fi.Name, AllBindings);
          if (ei == null)
            ei = t.GetEvent(EventName, AllBindings);
          if (ei == null)
            ei = fi.DeclaringType.GetEvent(fi.Name, AllBindings);
          if (ei == null)
            ei = fi.DeclaringType.GetEvent(EventName, AllBindings);
          if (ei != null)
          {
            foreach (var del in handlers)
              ei.AddEventHandler(obj, del);
            added = true;
          }
        }
      }
      return added;
    }

    private static Dictionary<Type, List<FieldInfo>> m_dicEventFieldInfos = new Dictionary<Type, List<FieldInfo>>();

    private static BindingFlags AllBindings
    {
      get { return BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static; }
    }

    private static List<FieldInfo> GetTypeEventFields(Type t)
    {
      if (m_dicEventFieldInfos.ContainsKey(t)) return m_dicEventFieldInfos[t];

      List<FieldInfo> lst = new List<FieldInfo>();
      BuildEventFields(t, lst);
      m_dicEventFieldInfos[t] = lst;
      return lst;
    }

    private static void BuildEventFields(Type t, List<FieldInfo> lst)
    {
      // Type.GetEvent(s) gets all Events for the type AND it's ancestors
      // Type.GetField(s) gets only Fields for the exact type.
      //  (BindingFlags.FlattenHierarchy only works on PROTECTED & PUBLIC
      //   doesn't work because Fieds are PRIVATE)
      var eil = t.GetEvents(AllBindings);
      lst.Clear();
      Dictionary<Type, List<FieldInfo>> dBuffer = new Dictionary<Type, List<FieldInfo>>();
      foreach (EventInfo ei in eil)
      {
        Type dt = ei.DeclaringType;
        if (!dBuffer.ContainsKey(dt))
        {
          dBuffer[dt] = new List<FieldInfo>();
          dBuffer[dt].AddRange(dt.GetFields(AllBindings));
        }
        FieldInfo fi = t.GetField(ei.Name, AllBindings);
        if (fi == null)
          fi = dt.GetField(ei.Name, AllBindings);
        if (fi == null)
          fi = t.GetField("Event" + ei.Name, AllBindings);
        if (fi == null)
          fi = dt.GetField("Event" + ei.Name, AllBindings);
        if (fi == null)
          fi = t.GetField(ei.Name + "Event", AllBindings);
        if (fi == null)
          fi = dt.GetField(ei.Name + "Event", AllBindings);
        if ((fi == null)) // && (dt.Name == "ListView"))
        {
          fi = dBuffer[dt].Find(x => x.Name.ToLowerInvariant() == "event_" + ei.Name.ToLowerInvariant());
          if (fi == null) fi = dBuffer[dt].Find(x => x.Name.ToLowerInvariant() == ei.Name.ToLowerInvariant() + "_event");
        }
        if (fi != null)
          lst.Add(fi);
      }
    }

    private static EventHandlerList GetStaticEventHandlerList(Type t, object obj)
    {
      MethodInfo mi = t.GetMethod("get_Events", AllBindings);
      while ((mi == null) & (t.BaseType != null))
      {
        t = t.BaseType;
        mi = t.GetMethod("get_Events", AllBindings);
      }
      if (mi == null) return null;
      return (EventHandlerList)mi.Invoke(obj, new object[] { });
    }

    private static bool CheckEvent(FieldInfo fi, string sEventName)
    {
      if (string.IsNullOrEmpty(sEventName))
        return false;
      if (string.Compare(sEventName, fi.Name, true) == 0)
        return true;
      if (string.Compare("Event" + sEventName, fi.Name, true) == 0)
        return true;
      if (string.Compare(sEventName + "Event", fi.Name, true) == 0)
        return true;
      if (string.Compare("Event_" + sEventName, fi.Name, true) == 0)
        return true;
      if (string.Compare(sEventName + "_Event", fi.Name, true) == 0)
        return true;
      return false;
    }

    private static void CollectEventHandler(object obj, List<Delegate> lDel)
    {
      try
      {
        if (obj == null) return;
        FieldInfo fHandler = obj.GetType().GetField("handler", AllBindings);
        if (fHandler == null) fHandler = obj.GetType().GetField("_handler", AllBindings);
        if (fHandler == null) return;
        Delegate d = (Delegate)fHandler.GetValue(obj);
        Delegate[] d2 = d.GetInvocationList();
        lDel.AddRange(d2);
      }
      catch { }
    }

    private static object GetHead(EventHandlerList eh)
    {
      try
      {
        FieldInfo fHead = eh.GetType().GetField("head", AllBindings);
        if (fHead == null) fHead = eh.GetType().GetField("_head", AllBindings);
        return fHead.GetValue(eh);
      }
      catch { }
      return null;
    }

    private static List<Delegate> UnwrapMonoWorkaround(object sender, List<Delegate> handlers)
    {
      if (!handlers[0].Method.DeclaringType.Name.Contains("MonoWorkaround"))
      {
        Tools.ShowInfo("No unwrapping required");
        return handlers;
      }
      List<Delegate> lHandlers = new List<Delegate>();
      FieldInfo fiHandlers = typeof(KeePassLib.Utility.MonoWorkarounds).GetField("m_dictHandlers", BindingFlags.Static | BindingFlags.NonPublic);
      if (fiHandlers == null)
      {
        Tools.ShowError("No unwrapping possible - fiHandlers null");
        return handlers;
      }
      var dictHandler = fiHandlers.GetValue(null);
      MethodInfo miTryGetValue = dictHandler.GetType().GetMethod("TryGetValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      object[] o = new object[] { sender, null };
      miTryGetValue.Invoke(dictHandler, o);
      if ((o == null) || (o.Length < 1))
      {
        Tools.ShowError("No unwrapping possible - object not found");
        return handlers;
      }
      Delegate dUnwrapped = Tools.GetField("m_fnOrg", o[1]) as Delegate;
      Tools.ShowInfo("Wrapped\n\nOld: " + handlers[0].Method.Name + " - " + handlers[0].Method.DeclaringType.Name + "\n" + dUnwrapped.Method.Name + " - " + dUnwrapped.Method.DeclaringType.Name);
      lHandlers.Add(dUnwrapped);
      return lHandlers;
    }
  }

  public class MonoWorkaroundDialogResult : IDisposable
  {
    private bool m_bRequired = false;
    private DialogResult m_dr = DialogResult.None;

    private object m_oMwaHandlerInfo = null;
    private FieldInfo m_fiDialogResult = null;

    public MonoWorkaroundDialogResult(object sender)
    {
      if (sender == null) return;
      if (!KeePassLib.Native.NativeLib.IsUnix()) return;

      GetDialogResultObject(sender);

      if (m_oMwaHandlerInfo == null) return;

      SetDialogResult(DialogResult.None);
    }

    public void Dispose()
    {
      if (!m_bRequired) return;
      SetDialogResult(m_dr);
    }

    private void GetDialogResultObject(object sender)
    {
      if (sender == null) return;

      FieldInfo fiHandlers = typeof(KeePassLib.Utility.MonoWorkarounds).GetField("m_dictHandlers", BindingFlags.Static | BindingFlags.NonPublic);
      object dictHandler = null;
      if (fiHandlers != null) dictHandler = fiHandlers.GetValue(null);

      //Do NOT set bRequired, spmething went wrong and we will stick to KeePass standard behaviour
      if (dictHandler == null) return;

      MethodInfo miTryGetValue = dictHandler.GetType().GetMethod("TryGetValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      //Do NOT set bRequired, spmething went wrong and we will stick to KeePass standard behaviour
      if (miTryGetValue == null) return;

      object[] o = new object[] { sender, null };
      miTryGetValue.Invoke(dictHandler, o);
      //Do NOT set bRequired, spmething went wrong and we will stick to KeePass standard behaviour
      if ((o == null) || (o.Length < 1)) return;

      m_oMwaHandlerInfo = o[1];
      m_fiDialogResult = m_oMwaHandlerInfo.GetType().GetField("m_dr", BindingFlags.Instance | BindingFlags.NonPublic);

      m_bRequired = true;
      m_dr = (DialogResult)m_fiDialogResult.GetValue(m_oMwaHandlerInfo);
    }

    private void SetDialogResult(DialogResult dr)
    {
      m_fiDialogResult.SetValue(m_oMwaHandlerInfo, dr);
    }
  }
}
