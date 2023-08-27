using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using KeePass.Plugins;
using KeePass.Util;
using KeePassLib.Utility;
using PluginTools;

namespace PluginTranslation
{
  public class TranslationChangedEventArgs : EventArgs
  {
    public string OldLanguageIso6391 = string.Empty;
    public string NewLanguageIso6391 = string.Empty;

    public TranslationChangedEventArgs(string OldLanguageIso6391, string NewLanguageIso6391)
    {
      this.OldLanguageIso6391 = OldLanguageIso6391;
      this.NewLanguageIso6391 = NewLanguageIso6391;
    }
  }

  public static class PluginTranslate
  {
    public static long TranslationVersion = 0;
    public static event EventHandler<TranslationChangedEventArgs> TranslationChanged = null;
    private static string LanguageIso6391 = string.Empty;
    #region Definitions of translated texts go here
    public const string PluginName = "Colored Password";
    /// <summary>
    /// Active
    /// </summary>
    public static readonly string Active = @"Active";
    /// <summary>
    /// Different color for lowercase letters
    /// </summary>
    public static readonly string LowercaseDifferentColor = @"Different color for lowercase letters";
    /// <summary>
    /// Colors for digits
    /// </summary>
    public static readonly string CharDigit = @"Colors for digits";
    /// <summary>
    /// Colors for special chars
    /// </summary>
    public static readonly string CharSpecial = @"Colors for special chars";
    /// <summary>
    /// Example text / test text
    /// </summary>
    public static readonly string Example = @"Example text / test text";
    /// <summary>
    /// Overriding SecureTextBoxEx not possible, wrong basetype: {0}
    /// </summary>
    public static readonly string Error = @"Overriding SecureTextBoxEx not possible, wrong basetype: {0}";
    /// <summary>
    /// Color passwords in entry list
    /// </summary>
    public static readonly string ColorEntryView = @"Color passwords in entry list";
    /// <summary>
    /// Do NOT adjust background color in entry list
    /// </summary>
    public static readonly string ColorEntryViewKeepBackgroundColor = @"Do NOT adjust background color in entry list";
    /// <summary>
    /// Single click password to unhide
    /// </summary>
    public static readonly string SinglePwDisplay = @"Single click password to unhide";
    /// <summary>
    /// Color passwords in password generator
    /// </summary>
    public static readonly string ColorPwGenDisplay = @"Color passwords in password generator";
    /// <summary>
    /// Snychronize font colors
    /// </summary>
    public static readonly string SyncColors = @"Snychronize font colors";
    #endregion

    #region NO changes in this area
    private static StringDictionary m_translation = new StringDictionary();

    public static void Init(Plugin plugin, string LanguageCodeIso6391)
    {
      List<string> lDebugStrings = new List<string>();
      m_translation.Clear();
      bool bError = true;
      LanguageCodeIso6391 = InitTranslation(plugin, lDebugStrings, LanguageCodeIso6391, out bError);
      if (bError && (LanguageCodeIso6391.Length > 2))
      {
        LanguageCodeIso6391 = LanguageCodeIso6391.Substring(0, 2);
        lDebugStrings.Add("Trying fallback: " + LanguageCodeIso6391);
        LanguageCodeIso6391 = InitTranslation(plugin, lDebugStrings, LanguageCodeIso6391, out bError);
      }
      if (bError)
      {
        PluginDebug.AddError("Reading translation failed", 0, lDebugStrings.ToArray());
        LanguageCodeIso6391 = "en";
      }
      else
      {
        List<FieldInfo> lTranslatable = new List<FieldInfo>(
          typeof(PluginTranslate).GetFields(BindingFlags.Static | BindingFlags.Public)
          ).FindAll(x => x.IsInitOnly);
        lDebugStrings.Add("Parsing complete");
        lDebugStrings.Add("Translated texts read: " + m_translation.Count.ToString());
        lDebugStrings.Add("Translatable texts: " + lTranslatable.Count.ToString());
        foreach (FieldInfo f in lTranslatable)
        {
          if (m_translation.ContainsKey(f.Name))
          {
            lDebugStrings.Add("Key found: " + f.Name);
            f.SetValue(null, m_translation[f.Name]);
          }
          else
            lDebugStrings.Add("Key not found: " + f.Name);
        }
        PluginDebug.AddInfo("Reading translations finished", 0, lDebugStrings.ToArray());
      }
      if (TranslationChanged != null)
      {
        TranslationChanged(null, new TranslationChangedEventArgs(LanguageIso6391, LanguageCodeIso6391));
      }
      LanguageIso6391 = LanguageCodeIso6391;
      lDebugStrings.Clear();
    }

    private static string InitTranslation(Plugin plugin, List<string> lDebugStrings, string LanguageCodeIso6391, out bool bError)
    {
      if (string.IsNullOrEmpty(LanguageCodeIso6391))
      {
        lDebugStrings.Add("No language identifier supplied, using 'en' as fallback");
        LanguageCodeIso6391 = "en";
      }
      string filename = GetFilename(plugin.GetType().Namespace, LanguageCodeIso6391);
      lDebugStrings.Add("Translation file: " + filename);

      if (!File.Exists(filename)) //If e. g. 'plugin.zh-tw.language.xml' does not exist, try 'plugin.zh.language.xml'
      {
        lDebugStrings.Add("File does not exist");
        bError = true;
        return LanguageCodeIso6391;
      }
      else
      {
        string translation = string.Empty;
        try { translation = File.ReadAllText(filename); }
        catch (Exception ex)
        {
          lDebugStrings.Add("Error reading file: " + ex.Message);
          LanguageCodeIso6391 = "en";
          bError = true;
          return LanguageCodeIso6391;
        }
        XmlSerializer xs = new XmlSerializer(m_translation.GetType());
        lDebugStrings.Add("File read, parsing content");
        try
        {
          m_translation = (StringDictionary)xs.Deserialize(new StringReader(translation));
        }
        catch (Exception ex)
        {
          string sException = ex.Message;
          if (ex.InnerException != null) sException += "\n" + ex.InnerException.Message;
          lDebugStrings.Add("Error parsing file: " + sException);
          LanguageCodeIso6391 = "en";
          MessageBox.Show("Error parsing translation file\n\n" + sException, PluginName, MessageBoxButtons.OK, MessageBoxIcon.Error);
          bError = true;
          return LanguageCodeIso6391;
        }
        bError = false;
        return LanguageCodeIso6391;
      }
    }

    private static string GetFilename(string plugin, string lang)
    {
      string filename = UrlUtil.GetFileDirectory(WinUtil.GetExecutable(), true, true);
      filename += KeePass.App.AppDefs.PluginsDir + UrlUtil.LocalDirSepChar + "Translations" + UrlUtil.LocalDirSepChar;
      filename += plugin + "." + lang + ".language.xml";
      return filename;
    }
    #endregion
  }

  #region NO changes in this area
  [XmlRoot("Translation")]
  public class StringDictionary : Dictionary<string, string>, IXmlSerializable
  {
    public System.Xml.Schema.XmlSchema GetSchema()
    {
      return null;
    }

    public void ReadXml(XmlReader reader)
    {
      bool wasEmpty = reader.IsEmptyElement;
      reader.Read();
      if (wasEmpty) return;
      bool bFirst = true;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (bFirst)
        {
          bFirst = false;
          try
          {
            reader.ReadStartElement("TranslationVersion");
            PluginTranslate.TranslationVersion = reader.ReadContentAsLong();
            reader.ReadEndElement();
          }
          catch { }
        }
        reader.ReadStartElement("item");
        reader.ReadStartElement("key");
        string key = reader.ReadContentAsString();
        reader.ReadEndElement();
        reader.ReadStartElement("value");
        string value = reader.ReadContentAsString();
        reader.ReadEndElement();
        this.Add(key, value);
        reader.ReadEndElement();
        reader.MoveToContent();
      }
      reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
      writer.WriteStartElement("TranslationVersion");
      writer.WriteString(PluginTranslate.TranslationVersion.ToString());
      writer.WriteEndElement();
      foreach (string key in this.Keys)
      {
        writer.WriteStartElement("item");
        writer.WriteStartElement("key");
        writer.WriteString(key);
        writer.WriteEndElement();
        writer.WriteStartElement("value");
        writer.WriteString(this[key]);
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
    }
  }
  #endregion
}
