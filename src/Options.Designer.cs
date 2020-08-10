namespace ColoredPassword
{
	partial class Options
	{
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Komponenten-Designer generierter Code

		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.cdSelect = new System.Windows.Forms.ColorDialog();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.gExample = new System.Windows.Forms.GroupBox();
			this.ctbExample = new ColoredPassword.ColorTextBox();
			this.pError = new System.Windows.Forms.Panel();
			this.lError2 = new System.Windows.Forms.Label();
			this.lError = new System.Windows.Forms.Label();
			this.cgActive = new RookieUI.CheckedGroupBox();
			this.pColorEntryView = new System.Windows.Forms.Panel();
			this.cbColorEntryViewKeepBackgroundColor = new System.Windows.Forms.CheckBox();
			this.cbColorEntryView = new System.Windows.Forms.CheckBox();
			this.tlp = new System.Windows.Forms.TableLayoutPanel();
			this.lBack = new System.Windows.Forms.Label();
			this.lText = new System.Windows.Forms.Label();
			this.lDefault = new System.Windows.Forms.Label();
			this.bForeColorDefault = new System.Windows.Forms.Button();
			this.bBackColorDefault = new System.Windows.Forms.Button();
			this.cbLowercase = new System.Windows.Forms.CheckBox();
			this.bForeColorLower = new System.Windows.Forms.Button();
			this.bBackColorLower = new System.Windows.Forms.Button();
			this.lDigits = new System.Windows.Forms.Label();
			this.bForeColorDigit = new System.Windows.Forms.Button();
			this.bBackColorDigit = new System.Windows.Forms.Button();
			this.lSpecial = new System.Windows.Forms.Label();
			this.bForeColorSpecial = new System.Windows.Forms.Button();
			this.bBackColorSpecial = new System.Windows.Forms.Button();
			this.gExample.SuspendLayout();
			this.pError.SuspendLayout();
			this.cgActive.SuspendLayout();
			this.pColorEntryView.SuspendLayout();
			this.tlp.SuspendLayout();
			this.SuspendLayout();
			// 
			// gExample
			// 
			this.gExample.Controls.Add(this.ctbExample);
			this.gExample.Dock = System.Windows.Forms.DockStyle.Top;
			this.gExample.Location = new System.Drawing.Point(0, 277);
			this.gExample.Name = "gExample";
			this.gExample.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
			this.gExample.Size = new System.Drawing.Size(843, 63);
			this.gExample.TabIndex = 4;
			this.gExample.TabStop = false;
			this.gExample.Text = "gExample";
			// 
			// ctbExample
			// 
			this.ctbExample.Dock = System.Windows.Forms.DockStyle.Top;
			this.ctbExample.Location = new System.Drawing.Point(10, 28);
			this.ctbExample.Multiline = false;
			this.ctbExample.Name = "ctbExample";
			this.ctbExample.Size = new System.Drawing.Size(823, 26);
			this.ctbExample.TabIndex = 1;
			this.ctbExample.Text = "ABC123!\"@abc";
			// 
			// pInfo
			// 
			this.pError.BackColor = System.Drawing.SystemColors.Control;
			this.pError.Controls.Add(this.lError2);
			this.pError.Controls.Add(this.lError);
			this.pError.Dock = System.Windows.Forms.DockStyle.Top;
			this.pError.Location = new System.Drawing.Point(0, 340);
			this.pError.Name = "pInfo";
			this.pError.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this.pError.Size = new System.Drawing.Size(843, 49);
			this.pError.TabIndex = 5;
			// 
			// lError2
			// 
			this.lError2.AutoEllipsis = true;
			this.lError2.AutoSize = true;
			this.lError2.Dock = System.Windows.Forms.DockStyle.Top;
			this.lError2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lError2.ForeColor = System.Drawing.Color.Red;
			this.lError2.Location = new System.Drawing.Point(10, 25);
			this.lError2.Name = "lError2";
			this.lError2.Size = new System.Drawing.Size(59, 20);
			this.lError2.TabIndex = 9;
			this.lError2.Text = "Error2";
			// 
			// lError
			// 
			this.lError.AutoEllipsis = true;
			this.lError.AutoSize = true;
			this.lError.Dock = System.Windows.Forms.DockStyle.Top;
			this.lError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lError.ForeColor = System.Drawing.Color.Red;
			this.lError.Location = new System.Drawing.Point(10, 5);
			this.lError.Name = "lError";
			this.lError.Size = new System.Drawing.Size(49, 20);
			this.lError.TabIndex = 8;
			this.lError.Text = "Error";
			// 
			// cgActive
			// 
			this.cgActive.AutoSize = true;
			this.cgActive.CheckboxOffset = new System.Drawing.Point(6, 0);
			this.cgActive.Checked = true;
			this.cgActive.Controls.Add(this.pColorEntryView);
			this.cgActive.Controls.Add(this.tlp);
			this.cgActive.Dock = System.Windows.Forms.DockStyle.Top;
			this.cgActive.Location = new System.Drawing.Point(0, 0);
			this.cgActive.Name = "cgActive";
			this.cgActive.Size = new System.Drawing.Size(843, 277);
			this.cgActive.TabIndex = 0;
			this.cgActive.Text = "cgActive";
			// 
			// pColorEntryView
			// 
			this.pColorEntryView.Controls.Add(this.cbColorEntryViewKeepBackgroundColor);
			this.pColorEntryView.Controls.Add(this.cbColorEntryView);
			this.pColorEntryView.Dock = System.Windows.Forms.DockStyle.Top;
			this.pColorEntryView.Location = new System.Drawing.Point(3, 211);
			this.pColorEntryView.Name = "pColorEntryView";
			this.pColorEntryView.Padding = new System.Windows.Forms.Padding(10, 9, 10, 3);
			this.pColorEntryView.Size = new System.Drawing.Size(837, 63);
			this.pColorEntryView.TabIndex = 4;
			// 
			// cbColorEntryViewKeepBackgroundColor
			// 
			this.cbColorEntryViewKeepBackgroundColor.AutoSize = true;
			this.cbColorEntryViewKeepBackgroundColor.Dock = System.Windows.Forms.DockStyle.Top;
			this.cbColorEntryViewKeepBackgroundColor.Location = new System.Drawing.Point(10, 33);
			this.cbColorEntryViewKeepBackgroundColor.Name = "cbColorEntryViewKeepBackgroundColor";
			this.cbColorEntryViewKeepBackgroundColor.Size = new System.Drawing.Size(817, 24);
			this.cbColorEntryViewKeepBackgroundColor.TabIndex = 2;
			this.cbColorEntryViewKeepBackgroundColor.Text = "Keep entry view background color";
			this.cbColorEntryViewKeepBackgroundColor.UseVisualStyleBackColor = true;
			// 
			// cbColorEntryView
			// 
			this.cbColorEntryView.AutoSize = true;
			this.cbColorEntryView.Dock = System.Windows.Forms.DockStyle.Top;
			this.cbColorEntryView.Location = new System.Drawing.Point(10, 9);
			this.cbColorEntryView.Name = "cbColorEntryView";
			this.cbColorEntryView.Size = new System.Drawing.Size(817, 24);
			this.cbColorEntryView.TabIndex = 2;
			this.cbColorEntryView.Text = "Color entry view";
			this.cbColorEntryView.UseVisualStyleBackColor = true;
			// 
			// tlp
			// 
			this.tlp.AutoSize = true;
			this.tlp.ColumnCount = 3;
			this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tlp.Controls.Add(this.lBack, 2, 0);
			this.tlp.Controls.Add(this.lText, 1, 0);
			this.tlp.Controls.Add(this.lDefault, 0, 1);
			this.tlp.Controls.Add(this.bForeColorDefault, 1, 1);
			this.tlp.Controls.Add(this.bBackColorDefault, 2, 1);
			this.tlp.Controls.Add(this.cbLowercase, 0, 2);
			this.tlp.Controls.Add(this.bForeColorLower, 1, 2);
			this.tlp.Controls.Add(this.bBackColorLower, 2, 2);
			this.tlp.Controls.Add(this.lDigits, 0, 3);
			this.tlp.Controls.Add(this.bForeColorDigit, 1, 3);
			this.tlp.Controls.Add(this.bBackColorDigit, 2, 3);
			this.tlp.Controls.Add(this.lSpecial, 0, 4);
			this.tlp.Controls.Add(this.bForeColorSpecial, 1, 4);
			this.tlp.Controls.Add(this.bBackColorSpecial, 2, 4);
			this.tlp.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlp.Location = new System.Drawing.Point(3, 22);
			this.tlp.Name = "tlp";
			this.tlp.RowCount = 4;
			this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
			this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tlp.Size = new System.Drawing.Size(837, 189);
			this.tlp.TabIndex = 1;
			// 
			// lBack
			// 
			this.lBack.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lBack.AutoSize = true;
			this.lBack.Location = new System.Drawing.Point(792, 4);
			this.lBack.Name = "lBack";
			this.lBack.Size = new System.Drawing.Size(39, 20);
			this.lBack.TabIndex = 13;
			this.lBack.Text = "Text";
			// 
			// lText
			// 
			this.lText.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.lText.AutoSize = true;
			this.lText.Location = new System.Drawing.Point(742, 4);
			this.lText.Name = "lText";
			this.lText.Size = new System.Drawing.Size(39, 20);
			this.lText.TabIndex = 0;
			this.lText.Text = "Text";
			// 
			// lDefault
			// 
			this.lDefault.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lDefault.AutoSize = true;
			this.lDefault.Location = new System.Drawing.Point(3, 39);
			this.lDefault.Name = "lDefault";
			this.lDefault.Size = new System.Drawing.Size(61, 20);
			this.lDefault.TabIndex = 12;
			this.lDefault.Text = "Default";
			// 
			// bForeColorDefault
			// 
			this.bForeColorDefault.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.bForeColorDefault.Location = new System.Drawing.Point(743, 32);
			this.bForeColorDefault.Name = "bForeColorDefault";
			this.bForeColorDefault.Size = new System.Drawing.Size(38, 34);
			this.bForeColorDefault.TabIndex = 6;
			this.bForeColorDefault.Text = "button1";
			this.bForeColorDefault.UseVisualStyleBackColor = true;
			this.bForeColorDefault.Click += new System.EventHandler(this.OnColorSelect);
			// 
			// bBackColorDefault
			// 
			this.bBackColorDefault.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.bBackColorDefault.Location = new System.Drawing.Point(793, 32);
			this.bBackColorDefault.Name = "bBackColorDefault";
			this.bBackColorDefault.Size = new System.Drawing.Size(38, 34);
			this.bBackColorDefault.TabIndex = 7;
			this.bBackColorDefault.Text = "button2";
			this.bBackColorDefault.UseVisualStyleBackColor = true;
			this.bBackColorDefault.Click += new System.EventHandler(this.OnColorSelect);
			// 
			// cbLowercase
			// 
			this.cbLowercase.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.cbLowercase.AutoSize = true;
			this.cbLowercase.Location = new System.Drawing.Point(30, 77);
			this.cbLowercase.Margin = new System.Windows.Forms.Padding(30, 3, 3, 3);
			this.cbLowercase.Name = "cbLowercase";
			this.cbLowercase.Size = new System.Drawing.Size(112, 24);
			this.cbLowercase.TabIndex = 12;
			this.cbLowercase.Text = "Lowercase";
			this.cbLowercase.CheckedChanged += new System.EventHandler(this.cbLowercase_CheckedChanged);
			// 
			// bForeColorLower
			// 
			this.bForeColorLower.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.bForeColorLower.Location = new System.Drawing.Point(743, 72);
			this.bForeColorLower.Name = "bForeColorLower";
			this.bForeColorLower.Size = new System.Drawing.Size(38, 34);
			this.bForeColorLower.TabIndex = 6;
			this.bForeColorLower.Text = "button1";
			this.bForeColorLower.UseVisualStyleBackColor = true;
			this.bForeColorLower.Click += new System.EventHandler(this.OnColorSelect);
			// 
			// bBackColorLower
			// 
			this.bBackColorLower.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.bBackColorLower.Location = new System.Drawing.Point(793, 72);
			this.bBackColorLower.Name = "bBackColorLower";
			this.bBackColorLower.Size = new System.Drawing.Size(38, 34);
			this.bBackColorLower.TabIndex = 7;
			this.bBackColorLower.Text = "button2";
			this.bBackColorLower.UseVisualStyleBackColor = true;
			this.bBackColorLower.Click += new System.EventHandler(this.OnColorSelect);
			// 
			// lDigits
			// 
			this.lDigits.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lDigits.AutoSize = true;
			this.lDigits.Location = new System.Drawing.Point(3, 119);
			this.lDigits.Name = "lDigits";
			this.lDigits.Size = new System.Drawing.Size(49, 20);
			this.lDigits.TabIndex = 3;
			this.lDigits.Text = "Digits";
			// 
			// bForeColorDigit
			// 
			this.bForeColorDigit.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.bForeColorDigit.Location = new System.Drawing.Point(743, 112);
			this.bForeColorDigit.Name = "bForeColorDigit";
			this.bForeColorDigit.Size = new System.Drawing.Size(38, 34);
			this.bForeColorDigit.TabIndex = 8;
			this.bForeColorDigit.Text = "button1";
			this.bForeColorDigit.UseVisualStyleBackColor = true;
			this.bForeColorDigit.Click += new System.EventHandler(this.OnColorSelect);
			// 
			// bBackColorDigit
			// 
			this.bBackColorDigit.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.bBackColorDigit.Location = new System.Drawing.Point(793, 112);
			this.bBackColorDigit.Name = "bBackColorDigit";
			this.bBackColorDigit.Size = new System.Drawing.Size(38, 34);
			this.bBackColorDigit.TabIndex = 9;
			this.bBackColorDigit.Text = "button2";
			this.bBackColorDigit.UseVisualStyleBackColor = true;
			this.bBackColorDigit.Click += new System.EventHandler(this.OnColorSelect);
			// 
			// lSpecial
			// 
			this.lSpecial.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lSpecial.AutoSize = true;
			this.lSpecial.Location = new System.Drawing.Point(3, 159);
			this.lSpecial.Name = "lSpecial";
			this.lSpecial.Size = new System.Drawing.Size(61, 20);
			this.lSpecial.TabIndex = 4;
			this.lSpecial.Text = "Special";
			// 
			// bForeColorSpecial
			// 
			this.bForeColorSpecial.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.bForeColorSpecial.Location = new System.Drawing.Point(743, 152);
			this.bForeColorSpecial.Name = "bForeColorSpecial";
			this.bForeColorSpecial.Size = new System.Drawing.Size(38, 34);
			this.bForeColorSpecial.TabIndex = 10;
			this.bForeColorSpecial.Text = "button3";
			this.bForeColorSpecial.UseVisualStyleBackColor = true;
			this.bForeColorSpecial.Click += new System.EventHandler(this.OnColorSelect);
			// 
			// bBackColorSpecial
			// 
			this.bBackColorSpecial.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.bBackColorSpecial.Location = new System.Drawing.Point(793, 152);
			this.bBackColorSpecial.Name = "bBackColorSpecial";
			this.bBackColorSpecial.Size = new System.Drawing.Size(38, 34);
			this.bBackColorSpecial.TabIndex = 11;
			this.bBackColorSpecial.Text = "button4";
			this.bBackColorSpecial.UseVisualStyleBackColor = true;
			this.bBackColorSpecial.Click += new System.EventHandler(this.OnColorSelect);
			// 
			// Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pError);
			this.Controls.Add(this.gExample);
			this.Controls.Add(this.cgActive);
			this.Name = "Options";
			this.Size = new System.Drawing.Size(843, 399);
			this.Resize += new System.EventHandler(this.Options_Resize);
			this.gExample.ResumeLayout(false);
			this.pError.ResumeLayout(false);
			this.pError.PerformLayout();
			this.cgActive.ResumeLayout(false);
			this.cgActive.PerformLayout();
			this.pColorEntryView.ResumeLayout(false);
			this.pColorEntryView.PerformLayout();
			this.tlp.ResumeLayout(false);
			this.tlp.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		internal RookieUI.CheckedGroupBox cgActive;
		private System.Windows.Forms.TableLayoutPanel tlp;
		private System.Windows.Forms.Label lText;
		private System.Windows.Forms.Label lDigits;
		private System.Windows.Forms.Label lSpecial;
		internal System.Windows.Forms.Button bForeColorDefault;
		internal System.Windows.Forms.Button bBackColorDefault;
		internal System.Windows.Forms.Button bForeColorLower;
		internal System.Windows.Forms.Button bBackColorLower;
		internal System.Windows.Forms.Button bForeColorDigit;
		internal System.Windows.Forms.Button bBackColorDigit;
		internal System.Windows.Forms.Button bForeColorSpecial;
		private System.Windows.Forms.Label lDefault;
		internal System.Windows.Forms.CheckBox cbLowercase;
		internal System.Windows.Forms.Button bBackColorSpecial;
		private System.Windows.Forms.ColorDialog cdSelect;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.Label lBack;
		private System.Windows.Forms.GroupBox gExample;
		internal ColorTextBox ctbExample;
		private System.Windows.Forms.Panel pError;
		private System.Windows.Forms.Label lError2;
		internal System.Windows.Forms.Label lError;
		private System.Windows.Forms.Panel pColorEntryView;
		internal System.Windows.Forms.CheckBox cbColorEntryViewKeepBackgroundColor;
		internal System.Windows.Forms.CheckBox cbColorEntryView;
	}
}
