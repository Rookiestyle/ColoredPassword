using KeePass.App.Configuration;
using KeePass.Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ColoredPassword
{
	internal static class ColorConfig
	{
		private static AceCustomConfig m_Config = KeePass.Program.Config.CustomConfig;
		public static bool FirstRun = true;
		public static bool Active = true;
		public static bool ColorEntryView = true;
		public static bool SinglePwDisplayActive = true;
		public static bool ColorPwGen = true;

		public static Version KP_2_51 = new Version(2, 51);

		public static bool SyncColorsWithPrintForm
        {
			get { return m_Config.GetBool("ColoredPassword.SyncColorsWithPrintForm", true); }
			set { m_Config.SetBool("ColoredPassword.SyncColorsWithPrintForm", value); }
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

		public static int ColorDifferenceThreshold
		{
			get { return (int)m_Config.GetLong("ColoredPassword.ColorDifferenceThreshold", 26); }
		}

		public static void Read()
		{
			bool test = Testmode;
			Testmode = false;
			const string ConfigPrefix = "ColoredPassword.";
			FirstRun = m_Config.GetBool(ConfigPrefix + "FirstRun", true);
			Active = m_Config.GetBool(ConfigPrefix + "Active", true);
			ColorEntryView = m_Config.GetBool(ConfigPrefix + "ColorEntryView", true);
			ListViewKeepBackgroundColor = m_Config.GetBool(ConfigPrefix + "ListViewKeepBackgroundColor", true);
			string help = m_Config.GetString(ConfigPrefix + "ForeColorDefault", "WindowText");
			ForeColorDefault = NameToColor(help);
			help = m_Config.GetString(ConfigPrefix + "BackColorDefault", "Window");
			BackColorDefault = NameToColor(help);
			help = m_Config.GetString(ConfigPrefix + "ForeColorDigit", "Red");
			ForeColorDigit = NameToColor(help);
			help = m_Config.GetString(ConfigPrefix + "BackColorDigit", "White");
			BackColorDigit = NameToColor(help);
			help = m_Config.GetString(ConfigPrefix + "ForeColorSpecial", "Green");
			ForeColorSpecial = NameToColor(help);
			help = m_Config.GetString(ConfigPrefix + "BackColorSpecial", "White");
			BackColorSpecial = NameToColor(help);
			LowercaseDifferent = m_Config.GetBool(ConfigPrefix + "LowercaseDifferent", false);
			help = m_Config.GetString(ConfigPrefix + "ForeColorLower", ColorToName(ForeColorDefault));
			ForeColorLower = NameToColor(help);
			help = m_Config.GetString(ConfigPrefix + "BackColorLower", ColorToName(BackColorDefault));
			BackColorLower = NameToColor(help);
			SinglePwDisplayActive = m_Config.GetBool(ConfigPrefix + "SinglePwDisplay", SinglePwDisplayActive);
			ColorPwGen = m_Config.GetBool(ConfigPrefix + "ColorPwGen", ColorPwGen);
			Testmode = test;
			Write();
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
			m_Config.SetBool(ConfigPrefix + "ColorPwGen", ColorPwGen);
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
}
