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
            this.bReset = new System.Windows.Forms.Button();
            this.ctbExample = new ColoredPassword.ColorTextBox();
            this.pError = new System.Windows.Forms.Panel();
            this.lError2 = new System.Windows.Forms.Label();
            this.lError = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.cgActive = new RookieUI.CheckedGroupBox();
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
            this.tpAdvanced = new System.Windows.Forms.TabPage();
            this.gActiveForms = new System.Windows.Forms.GroupBox();
            this.cbPwEntryForm = new System.Windows.Forms.CheckBox();
            this.cbKeyChangeForm = new System.Windows.Forms.CheckBox();
            this.cbKeyPromptForm = new System.Windows.Forms.CheckBox();
            this.gSyncColorsWithPrintForm = new System.Windows.Forms.GroupBox();
            this.cbSyncColorsWithPrintForm = new System.Windows.Forms.CheckBox();
            this.gEntryView = new System.Windows.Forms.GroupBox();
            this.cbDontShowAsterisk = new System.Windows.Forms.CheckBox();
            this.cbSinglePwDisplay = new System.Windows.Forms.CheckBox();
            this.cbColorEntryViewKeepBackgroundColor = new System.Windows.Forms.CheckBox();
            this.cbColorEntryView = new System.Windows.Forms.CheckBox();
            this.cbColorPwGen = new System.Windows.Forms.CheckBox();
            this.gExample.SuspendLayout();
            this.pError.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.cgActive.SuspendLayout();
            this.tpAdvanced.SuspendLayout();
            this.gActiveForms.SuspendLayout();
            this.gSyncColorsWithPrintForm.SuspendLayout();
            this.gEntryView.SuspendLayout();
            this.SuspendLayout();
            // 
            // gExample
            // 
            this.gExample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gExample.AutoSize = true;
            this.gExample.Controls.Add(this.bReset);
            this.gExample.Controls.Add(this.ctbExample);
            this.gExample.Location = new System.Drawing.Point(2, 163);
            this.gExample.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gExample.Name = "gExample";
            this.gExample.Padding = new System.Windows.Forms.Padding(0);
            this.gExample.Size = new System.Drawing.Size(714, 68);
            this.gExample.TabIndex = 4;
            this.gExample.TabStop = false;
            this.gExample.Text = "gExample";
            // 
            // bReset
            // 
            this.bReset.Location = new System.Drawing.Point(498, 22);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(166, 23);
            this.bReset.TabIndex = 2;
            this.bReset.Text = "Reset";
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // ctbExample
            // 
            this.ctbExample.ColorBackground = true;
            this.ctbExample.Location = new System.Drawing.Point(9, 22);
            this.ctbExample.Margin = new System.Windows.Forms.Padding(0);
            this.ctbExample.Multiline = false;
            this.ctbExample.Name = "ctbExample";
            this.ctbExample.Size = new System.Drawing.Size(267, 22);
            this.ctbExample.TabIndex = 1;
            this.ctbExample.Text = "ABC123!\"@abc";
            // 
            // pError
            // 
            this.pError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pError.AutoSize = true;
            this.pError.BackColor = System.Drawing.Color.Transparent;
            this.pError.Controls.Add(this.lError2);
            this.pError.Controls.Add(this.lError);
            this.pError.Location = new System.Drawing.Point(2, 221);
            this.pError.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pError.Name = "pError";
            this.pError.Padding = new System.Windows.Forms.Padding(9, 4, 9, 4);
            this.pError.Size = new System.Drawing.Size(714, 49);
            this.pError.TabIndex = 5;
            // 
            // lError2
            // 
            this.lError2.AutoEllipsis = true;
            this.lError2.AutoSize = true;
            this.lError2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lError2.ForeColor = System.Drawing.Color.Red;
            this.lError2.Location = new System.Drawing.Point(9, 20);
            this.lError2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lError2.Name = "lError2";
            this.lError2.Size = new System.Drawing.Size(54, 17);
            this.lError2.TabIndex = 9;
            this.lError2.Text = "Error2";
            // 
            // lError
            // 
            this.lError.AutoEllipsis = true;
            this.lError.AutoSize = true;
            this.lError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lError.ForeColor = System.Drawing.Color.Red;
            this.lError.Location = new System.Drawing.Point(9, 4);
            this.lError.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lError.Name = "lError";
            this.lError.Size = new System.Drawing.Size(45, 17);
            this.lError.TabIndex = 8;
            this.lError.Text = "Error";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpSettings);
            this.tabControl1.Controls.Add(this.tpAdvanced);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(726, 320);
            this.tabControl1.TabIndex = 6;
            // 
            // tpSettings
            // 
            this.tpSettings.Controls.Add(this.pError);
            this.tpSettings.Controls.Add(this.gExample);
            this.tpSettings.Controls.Add(this.cgActive);
            this.tpSettings.Location = new System.Drawing.Point(4, 25);
            this.tpSettings.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpSettings.Size = new System.Drawing.Size(718, 291);
            this.tpSettings.TabIndex = 0;
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // cgActive
            // 
            this.cgActive.CheckboxOffset = new System.Drawing.Point(6, 0);
            this.cgActive.Checked = true;
            this.cgActive.Controls.Add(this.lDefault);
            this.cgActive.Controls.Add(this.bForeColorDefault);
            this.cgActive.Controls.Add(this.bBackColorDefault);
            this.cgActive.Controls.Add(this.cbLowercase);
            this.cgActive.Controls.Add(this.bForeColorLower);
            this.cgActive.Controls.Add(this.bBackColorLower);
            this.cgActive.Controls.Add(this.lDigits);
            this.cgActive.Controls.Add(this.bForeColorDigit);
            this.cgActive.Controls.Add(this.bBackColorDigit);
            this.cgActive.Controls.Add(this.lSpecial);
            this.cgActive.Controls.Add(this.bForeColorSpecial);
            this.cgActive.Controls.Add(this.bBackColorSpecial);
            this.cgActive.Location = new System.Drawing.Point(2, 3);
            this.cgActive.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cgActive.Name = "cgActive";
            this.cgActive.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cgActive.Size = new System.Drawing.Size(714, 160);
            this.cgActive.TabIndex = 0;
            this.cgActive.Text = "cgActive";
            this.cgActive.CheckedChanged += new System.EventHandler<RookieUI.CheckedGroupCheckEventArgs>(this.cgActive_CheckedChanged);
            // 
            // lDefault
            // 
            this.lDefault.AutoSize = true;
            this.lDefault.Location = new System.Drawing.Point(9, 23);
            this.lDefault.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lDefault.Name = "lDefault";
            this.lDefault.Size = new System.Drawing.Size(49, 16);
            this.lDefault.TabIndex = 23;
            this.lDefault.Text = "Default";
            // 
            // bForeColorDefault
            // 
            this.bForeColorDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bForeColorDefault.Location = new System.Drawing.Point(498, 18);
            this.bForeColorDefault.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.bForeColorDefault.Name = "bForeColorDefault";
            this.bForeColorDefault.Size = new System.Drawing.Size(34, 27);
            this.bForeColorDefault.TabIndex = 15;
            this.bForeColorDefault.Text = "button1";
            this.bForeColorDefault.UseVisualStyleBackColor = true;
            this.bForeColorDefault.Click += new System.EventHandler(this.OnColorSelect);
            // 
            // bBackColorDefault
            // 
            this.bBackColorDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBackColorDefault.Location = new System.Drawing.Point(630, 18);
            this.bBackColorDefault.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.bBackColorDefault.Name = "bBackColorDefault";
            this.bBackColorDefault.Size = new System.Drawing.Size(34, 27);
            this.bBackColorDefault.TabIndex = 17;
            this.bBackColorDefault.Text = "button2";
            this.bBackColorDefault.UseVisualStyleBackColor = true;
            this.bBackColorDefault.Click += new System.EventHandler(this.OnColorSelect);
            // 
            // cbLowercase
            // 
            this.cbLowercase.AutoSize = true;
            this.cbLowercase.Location = new System.Drawing.Point(33, 54);
            this.cbLowercase.Margin = new System.Windows.Forms.Padding(26, 3, 2, 3);
            this.cbLowercase.Name = "cbLowercase";
            this.cbLowercase.Size = new System.Drawing.Size(95, 20);
            this.cbLowercase.TabIndex = 24;
            this.cbLowercase.Text = "Lowercase";
            this.cbLowercase.CheckedChanged += new System.EventHandler(this.cbLowercase_CheckedChanged);
            // 
            // bForeColorLower
            // 
            this.bForeColorLower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bForeColorLower.Location = new System.Drawing.Point(498, 50);
            this.bForeColorLower.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.bForeColorLower.Name = "bForeColorLower";
            this.bForeColorLower.Size = new System.Drawing.Size(34, 27);
            this.bForeColorLower.TabIndex = 16;
            this.bForeColorLower.Text = "button1";
            this.bForeColorLower.UseVisualStyleBackColor = true;
            this.bForeColorLower.Click += new System.EventHandler(this.OnColorSelect);
            // 
            // bBackColorLower
            // 
            this.bBackColorLower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBackColorLower.Location = new System.Drawing.Point(630, 50);
            this.bBackColorLower.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.bBackColorLower.Name = "bBackColorLower";
            this.bBackColorLower.Size = new System.Drawing.Size(34, 27);
            this.bBackColorLower.TabIndex = 18;
            this.bBackColorLower.Text = "button2";
            this.bBackColorLower.UseVisualStyleBackColor = true;
            this.bBackColorLower.Click += new System.EventHandler(this.OnColorSelect);
            // 
            // lDigits
            // 
            this.lDigits.AutoSize = true;
            this.lDigits.Location = new System.Drawing.Point(9, 87);
            this.lDigits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lDigits.Name = "lDigits";
            this.lDigits.Size = new System.Drawing.Size(41, 16);
            this.lDigits.TabIndex = 13;
            this.lDigits.Text = "Digits";
            // 
            // bForeColorDigit
            // 
            this.bForeColorDigit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bForeColorDigit.Location = new System.Drawing.Point(498, 82);
            this.bForeColorDigit.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.bForeColorDigit.Name = "bForeColorDigit";
            this.bForeColorDigit.Size = new System.Drawing.Size(34, 27);
            this.bForeColorDigit.TabIndex = 19;
            this.bForeColorDigit.Text = "button1";
            this.bForeColorDigit.UseVisualStyleBackColor = true;
            this.bForeColorDigit.Click += new System.EventHandler(this.OnColorSelect);
            // 
            // bBackColorDigit
            // 
            this.bBackColorDigit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBackColorDigit.Location = new System.Drawing.Point(630, 82);
            this.bBackColorDigit.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.bBackColorDigit.Name = "bBackColorDigit";
            this.bBackColorDigit.Size = new System.Drawing.Size(34, 27);
            this.bBackColorDigit.TabIndex = 20;
            this.bBackColorDigit.Text = "button2";
            this.bBackColorDigit.UseVisualStyleBackColor = true;
            this.bBackColorDigit.Click += new System.EventHandler(this.OnColorSelect);
            // 
            // lSpecial
            // 
            this.lSpecial.AutoSize = true;
            this.lSpecial.Location = new System.Drawing.Point(9, 119);
            this.lSpecial.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lSpecial.Name = "lSpecial";
            this.lSpecial.Size = new System.Drawing.Size(53, 16);
            this.lSpecial.TabIndex = 14;
            this.lSpecial.Text = "Special";
            // 
            // bForeColorSpecial
            // 
            this.bForeColorSpecial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bForeColorSpecial.Location = new System.Drawing.Point(498, 114);
            this.bForeColorSpecial.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.bForeColorSpecial.Name = "bForeColorSpecial";
            this.bForeColorSpecial.Size = new System.Drawing.Size(34, 27);
            this.bForeColorSpecial.TabIndex = 21;
            this.bForeColorSpecial.Text = "button3";
            this.bForeColorSpecial.UseVisualStyleBackColor = true;
            this.bForeColorSpecial.Click += new System.EventHandler(this.OnColorSelect);
            // 
            // bBackColorSpecial
            // 
            this.bBackColorSpecial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bBackColorSpecial.Location = new System.Drawing.Point(630, 114);
            this.bBackColorSpecial.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.bBackColorSpecial.Name = "bBackColorSpecial";
            this.bBackColorSpecial.Size = new System.Drawing.Size(34, 27);
            this.bBackColorSpecial.TabIndex = 22;
            this.bBackColorSpecial.Text = "button4";
            this.bBackColorSpecial.UseVisualStyleBackColor = true;
            this.bBackColorSpecial.Click += new System.EventHandler(this.OnColorSelect);
            // 
            // tpAdvanced
            // 
            this.tpAdvanced.Controls.Add(this.gActiveForms);
            this.tpAdvanced.Controls.Add(this.gSyncColorsWithPrintForm);
            this.tpAdvanced.Controls.Add(this.gEntryView);
            this.tpAdvanced.Location = new System.Drawing.Point(4, 25);
            this.tpAdvanced.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpAdvanced.Name = "tpAdvanced";
            this.tpAdvanced.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpAdvanced.Size = new System.Drawing.Size(718, 291);
            this.tpAdvanced.TabIndex = 1;
            this.tpAdvanced.Text = "Advanced";
            this.tpAdvanced.UseVisualStyleBackColor = true;
            // 
            // gActiveForms
            // 
            this.gActiveForms.Controls.Add(this.cbColorPwGen);
            this.gActiveForms.Controls.Add(this.cbPwEntryForm);
            this.gActiveForms.Controls.Add(this.cbKeyChangeForm);
            this.gActiveForms.Controls.Add(this.cbKeyPromptForm);
            this.gActiveForms.Location = new System.Drawing.Point(2, 166);
            this.gActiveForms.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gActiveForms.Name = "gActiveForms";
            this.gActiveForms.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gActiveForms.Size = new System.Drawing.Size(714, 119);
            this.gActiveForms.TabIndex = 8;
            this.gActiveForms.TabStop = false;
            this.gActiveForms.Text = "Forms using ColoredPassword";
            // 
            // cbPwEntryForm
            // 
            this.cbPwEntryForm.AutoSize = true;
            this.cbPwEntryForm.Location = new System.Drawing.Point(9, 62);
            this.cbPwEntryForm.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbPwEntryForm.Name = "cbPwEntryForm";
            this.cbPwEntryForm.Size = new System.Drawing.Size(108, 20);
            this.cbPwEntryForm.TabIndex = 7;
            this.cbPwEntryForm.Text = "PwEntryForm";
            this.cbPwEntryForm.UseVisualStyleBackColor = true;
            // 
            // cbKeyChangeForm
            // 
            this.cbKeyChangeForm.AutoSize = true;
            this.cbKeyChangeForm.Location = new System.Drawing.Point(9, 40);
            this.cbKeyChangeForm.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbKeyChangeForm.Name = "cbKeyChangeForm";
            this.cbKeyChangeForm.Size = new System.Drawing.Size(130, 20);
            this.cbKeyChangeForm.TabIndex = 6;
            this.cbKeyChangeForm.Text = "KeyChangeForm";
            this.cbKeyChangeForm.UseVisualStyleBackColor = true;
            // 
            // cbKeyPromptForm
            // 
            this.cbKeyPromptForm.AutoSize = true;
            this.cbKeyPromptForm.Location = new System.Drawing.Point(9, 18);
            this.cbKeyPromptForm.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbKeyPromptForm.Name = "cbKeyPromptForm";
            this.cbKeyPromptForm.Size = new System.Drawing.Size(126, 20);
            this.cbKeyPromptForm.TabIndex = 5;
            this.cbKeyPromptForm.Text = "KeyPromptForm";
            this.cbKeyPromptForm.UseVisualStyleBackColor = true;
            // 
            // gSyncColorsWithPrintForm
            // 
            this.gSyncColorsWithPrintForm.Controls.Add(this.cbSyncColorsWithPrintForm);
            this.gSyncColorsWithPrintForm.Location = new System.Drawing.Point(2, 112);
            this.gSyncColorsWithPrintForm.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gSyncColorsWithPrintForm.Name = "gSyncColorsWithPrintForm";
            this.gSyncColorsWithPrintForm.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gSyncColorsWithPrintForm.Size = new System.Drawing.Size(714, 44);
            this.gSyncColorsWithPrintForm.TabIndex = 7;
            this.gSyncColorsWithPrintForm.TabStop = false;
            this.gSyncColorsWithPrintForm.Text = "Sync with print form";
            // 
            // cbSyncColorsWithPrintForm
            // 
            this.cbSyncColorsWithPrintForm.AutoSize = true;
            this.cbSyncColorsWithPrintForm.Location = new System.Drawing.Point(9, 18);
            this.cbSyncColorsWithPrintForm.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbSyncColorsWithPrintForm.Name = "cbSyncColorsWithPrintForm";
            this.cbSyncColorsWithPrintForm.Size = new System.Drawing.Size(141, 20);
            this.cbSyncColorsWithPrintForm.TabIndex = 5;
            this.cbSyncColorsWithPrintForm.Text = "Sync with print form";
            this.cbSyncColorsWithPrintForm.UseVisualStyleBackColor = true;
            // 
            // gEntryView
            // 
            this.gEntryView.Controls.Add(this.cbDontShowAsterisk);
            this.gEntryView.Controls.Add(this.cbSinglePwDisplay);
            this.gEntryView.Controls.Add(this.cbColorEntryViewKeepBackgroundColor);
            this.gEntryView.Controls.Add(this.cbColorEntryView);
            this.gEntryView.Location = new System.Drawing.Point(2, 3);
            this.gEntryView.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gEntryView.Name = "gEntryView";
            this.gEntryView.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gEntryView.Size = new System.Drawing.Size(714, 103);
            this.gEntryView.TabIndex = 5;
            this.gEntryView.TabStop = false;
            this.gEntryView.Tag = "KEEPENABLED";
            this.gEntryView.Text = "Entry list";
            // 
            // cbDontShowAsterisk
            // 
            this.cbDontShowAsterisk.AutoSize = true;
            this.cbDontShowAsterisk.Location = new System.Drawing.Point(9, 73);
            this.cbDontShowAsterisk.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbDontShowAsterisk.Name = "cbDontShowAsterisk";
            this.cbDontShowAsterisk.Size = new System.Drawing.Size(244, 20);
            this.cbDontShowAsterisk.TabIndex = 7;
            this.cbDontShowAsterisk.Tag = "KEEPENABLED";
            this.cbDontShowAsterisk.Text = "Don\'t show asterisks for empty fields";
            this.cbDontShowAsterisk.UseVisualStyleBackColor = true;
            // 
            // cbSinglePwDisplay
            // 
            this.cbSinglePwDisplay.AutoSize = true;
            this.cbSinglePwDisplay.Location = new System.Drawing.Point(9, 56);
            this.cbSinglePwDisplay.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbSinglePwDisplay.Name = "cbSinglePwDisplay";
            this.cbSinglePwDisplay.Size = new System.Drawing.Size(207, 20);
            this.cbSinglePwDisplay.TabIndex = 6;
            this.cbSinglePwDisplay.Tag = "KEEPENABLED";
            this.cbSinglePwDisplay.Text = "Single click password to show";
            this.cbSinglePwDisplay.UseVisualStyleBackColor = true;
            // 
            // cbColorEntryViewKeepBackgroundColor
            // 
            this.cbColorEntryViewKeepBackgroundColor.AutoSize = true;
            this.cbColorEntryViewKeepBackgroundColor.Location = new System.Drawing.Point(9, 37);
            this.cbColorEntryViewKeepBackgroundColor.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbColorEntryViewKeepBackgroundColor.Name = "cbColorEntryViewKeepBackgroundColor";
            this.cbColorEntryViewKeepBackgroundColor.Size = new System.Drawing.Size(231, 20);
            this.cbColorEntryViewKeepBackgroundColor.TabIndex = 4;
            this.cbColorEntryViewKeepBackgroundColor.Text = "Keep entry view background color";
            this.cbColorEntryViewKeepBackgroundColor.UseVisualStyleBackColor = true;
            // 
            // cbColorEntryView
            // 
            this.cbColorEntryView.AutoSize = true;
            this.cbColorEntryView.Location = new System.Drawing.Point(9, 18);
            this.cbColorEntryView.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbColorEntryView.Name = "cbColorEntryView";
            this.cbColorEntryView.Size = new System.Drawing.Size(123, 20);
            this.cbColorEntryView.TabIndex = 5;
            this.cbColorEntryView.Text = "Color entry view";
            this.cbColorEntryView.UseVisualStyleBackColor = true;
            // 
            // cbColorPwGen
            // 
            this.cbColorPwGen.AutoSize = true;
            this.cbColorPwGen.Location = new System.Drawing.Point(9, 84);
            this.cbColorPwGen.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cbColorPwGen.Name = "cbColorPwGen";
            this.cbColorPwGen.Size = new System.Drawing.Size(230, 20);
            this.cbColorPwGen.TabIndex = 8;
            this.cbColorPwGen.Text = "Use colors in password generator";
            this.cbColorPwGen.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Options";
            this.Size = new System.Drawing.Size(726, 358);
            this.Resize += new System.EventHandler(this.Options_Resize);
            this.gExample.ResumeLayout(false);
            this.pError.ResumeLayout(false);
            this.pError.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.tpSettings.PerformLayout();
            this.cgActive.ResumeLayout(false);
            this.cgActive.PerformLayout();
            this.tpAdvanced.ResumeLayout(false);
            this.gActiveForms.ResumeLayout(false);
            this.gActiveForms.PerformLayout();
            this.gSyncColorsWithPrintForm.ResumeLayout(false);
            this.gSyncColorsWithPrintForm.PerformLayout();
            this.gEntryView.ResumeLayout(false);
            this.gEntryView.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		internal RookieUI.CheckedGroupBox cgActive;
		private System.Windows.Forms.ColorDialog cdSelect;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.GroupBox gExample;
		internal ColorTextBox ctbExample;
		private System.Windows.Forms.Panel pError;
		private System.Windows.Forms.Label lError2;
		private System.Windows.Forms.Label lError;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tpSettings;
		private System.Windows.Forms.TabPage tpAdvanced;
		private System.Windows.Forms.GroupBox gEntryView;
		internal System.Windows.Forms.CheckBox cbSinglePwDisplay;
		internal System.Windows.Forms.CheckBox cbColorEntryViewKeepBackgroundColor;
		internal System.Windows.Forms.CheckBox cbColorEntryView;
		private System.Windows.Forms.Label lDefault;
		internal System.Windows.Forms.Button bForeColorDefault;
		internal System.Windows.Forms.Button bBackColorDefault;
		internal System.Windows.Forms.CheckBox cbLowercase;
		internal System.Windows.Forms.Button bForeColorLower;
		internal System.Windows.Forms.Button bBackColorLower;
		private System.Windows.Forms.Label lDigits;
		internal System.Windows.Forms.Button bForeColorDigit;
		internal System.Windows.Forms.Button bBackColorDigit;
		private System.Windows.Forms.Label lSpecial;
		internal System.Windows.Forms.Button bForeColorSpecial;
		internal System.Windows.Forms.Button bBackColorSpecial;
        private System.Windows.Forms.GroupBox gSyncColorsWithPrintForm;
        internal System.Windows.Forms.CheckBox cbSyncColorsWithPrintForm;
    internal System.Windows.Forms.CheckBox cbDontShowAsterisk;
    private System.Windows.Forms.Button bReset;
    private System.Windows.Forms.GroupBox gActiveForms;
    internal System.Windows.Forms.CheckBox cbKeyPromptForm;
    internal System.Windows.Forms.CheckBox cbPwEntryForm;
    internal System.Windows.Forms.CheckBox cbKeyChangeForm;
    internal System.Windows.Forms.CheckBox cbColorPwGen;
  }
}
