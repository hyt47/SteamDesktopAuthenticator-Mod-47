namespace Steam_Desktop_Authenticator
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.chkConfirmationsPeriodicChecking = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.numPeriodicInterval = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chkCheckAll = new System.Windows.Forms.CheckBox();
            this.GroupPopupNewConf = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.chkDisplayPopupConfirmation = new System.Windows.Forms.CheckBox();
            this.groupBoxAutoConfirm = new System.Windows.Forms.GroupBox();
            this.groupBoxStartupAutoConfirmDelay = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numDelayAutoConfirmAtStartup = new System.Windows.Forms.NumericUpDown();
            this.chkDelayAutoConfirmAtStartup = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkAutoConfirmMarket = new System.Windows.Forms.CheckBox();
            this.labelAutoConfirmWarning = new System.Windows.Forms.Label();
            this.chkAutoConfirmTrades = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioBtnBorderConfPopup2 = new System.Windows.Forms.RadioButton();
            this.radioBtnBorderConfPopup1 = new System.Windows.Forms.RadioButton();
            this.groupSystemTray = new System.Windows.Forms.GroupBox();
            this.chkStartMinimizedToSystemTray = new System.Windows.Forms.CheckBox();
            this.chkHideTaskbarIcon = new System.Windows.Forms.CheckBox();
            this.radioButton3_SystemTray = new System.Windows.Forms.RadioButton();
            this.radioButton2_SystemTray = new System.Windows.Forms.RadioButton();
            this.radioButton1_SystemTray = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkAppCanRunMultipleTimes = new System.Windows.Forms.CheckBox();
            this.groupBoxRun = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.numSendAppStatusInterval = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxSendAppStatusToAddress = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numAppNo = new System.Windows.Forms.NumericUpDown();
            this.chkSendAppStatus = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numPeriodicInterval)).BeginInit();
            this.GroupPopupNewConf.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxAutoConfirm.SuspendLayout();
            this.groupBoxStartupAutoConfirmDelay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelayAutoConfirmAtStartup)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupSystemTray.SuspendLayout();
            this.groupBoxRun.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSendAppStatusInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAppNo)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkConfirmationsPeriodicChecking
            // 
            this.chkConfirmationsPeriodicChecking.AutoSize = true;
            this.chkConfirmationsPeriodicChecking.Location = new System.Drawing.Point(12, 22);
            this.chkConfirmationsPeriodicChecking.Name = "chkConfirmationsPeriodicChecking";
            this.chkConfirmationsPeriodicChecking.Size = new System.Drawing.Size(593, 17);
            this.chkConfirmationsPeriodicChecking.TabIndex = 0;
            this.chkConfirmationsPeriodicChecking.Text = "Periodically check for new confirmations ( this will enable Popup Confirmations i" +
    "f Auto-Confirm is Unchecked )";
            this.chkConfirmationsPeriodicChecking.UseVisualStyleBackColor = true;
            this.chkConfirmationsPeriodicChecking.CheckedChanged += new System.EventHandler(this.checkBoxConfirmationsPeriodicChecking_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.btnOk.Location = new System.Drawing.Point(437, 601);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(107, 30);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Save";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.SettingsOk_Click);
            // 
            // numPeriodicInterval
            // 
            this.numPeriodicInterval.Enabled = false;
            this.numPeriodicInterval.Location = new System.Drawing.Point(33, 47);
            this.numPeriodicInterval.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numPeriodicInterval.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numPeriodicInterval.Name = "numPeriodicInterval";
            this.numPeriodicInterval.Size = new System.Drawing.Size(80, 22);
            this.numPeriodicInterval.TabIndex = 2;
            this.numPeriodicInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(324, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Seconds between checking for confirmations ( Range 5 - 300 )";
            // 
            // chkCheckAll
            // 
            this.chkCheckAll.AutoSize = true;
            this.chkCheckAll.Enabled = false;
            this.chkCheckAll.Location = new System.Drawing.Point(34, 79);
            this.chkCheckAll.Name = "chkCheckAll";
            this.chkCheckAll.Size = new System.Drawing.Size(213, 17);
            this.chkCheckAll.TabIndex = 4;
            this.chkCheckAll.Text = "Check all accounts for confirmations";
            this.chkCheckAll.UseVisualStyleBackColor = true;
            // 
            // GroupPopupNewConf
            // 
            this.GroupPopupNewConf.Controls.Add(this.groupBox3);
            this.GroupPopupNewConf.Controls.Add(this.groupBoxAutoConfirm);
            this.GroupPopupNewConf.Controls.Add(this.groupBox2);
            this.GroupPopupNewConf.Controls.Add(this.chkCheckAll);
            this.GroupPopupNewConf.Controls.Add(this.chkConfirmationsPeriodicChecking);
            this.GroupPopupNewConf.Controls.Add(this.numPeriodicInterval);
            this.GroupPopupNewConf.Controls.Add(this.label1);
            this.GroupPopupNewConf.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupPopupNewConf.Location = new System.Drawing.Point(5, 8);
            this.GroupPopupNewConf.Name = "GroupPopupNewConf";
            this.GroupPopupNewConf.Size = new System.Drawing.Size(631, 392);
            this.GroupPopupNewConf.TabIndex = 4;
            this.GroupPopupNewConf.TabStop = false;
            this.GroupPopupNewConf.Text = "Confirmations Popup / Auto-Confirm";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.chkDisplayPopupConfirmation);
            this.groupBox3.Location = new System.Drawing.Point(32, 340);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(593, 45);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Popup Confirmation ( Is visible only if one or both of the Auto-confirm options a" +
    "re unchecked )";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(234, 20);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(309, 13);
            this.label18.TabIndex = 11;
            this.label18.Text = "( If Popup notification is opened, Auto-confirm is paused. )";
            // 
            // chkDisplayPopupConfirmation
            // 
            this.chkDisplayPopupConfirmation.AutoSize = true;
            this.chkDisplayPopupConfirmation.Enabled = false;
            this.chkDisplayPopupConfirmation.Location = new System.Drawing.Point(11, 19);
            this.chkDisplayPopupConfirmation.Name = "chkDisplayPopupConfirmation";
            this.chkDisplayPopupConfirmation.Size = new System.Drawing.Size(171, 17);
            this.chkDisplayPopupConfirmation.TabIndex = 0;
            this.chkDisplayPopupConfirmation.Text = "Display Popup Confirmation";
            this.chkDisplayPopupConfirmation.UseVisualStyleBackColor = true;
            // 
            // groupBoxAutoConfirm
            // 
            this.groupBoxAutoConfirm.Controls.Add(this.groupBoxStartupAutoConfirmDelay);
            this.groupBoxAutoConfirm.Controls.Add(this.label7);
            this.groupBoxAutoConfirm.Controls.Add(this.label6);
            this.groupBoxAutoConfirm.Controls.Add(this.label5);
            this.groupBoxAutoConfirm.Controls.Add(this.label4);
            this.groupBoxAutoConfirm.Controls.Add(this.label3);
            this.groupBoxAutoConfirm.Controls.Add(this.chkAutoConfirmMarket);
            this.groupBoxAutoConfirm.Controls.Add(this.labelAutoConfirmWarning);
            this.groupBoxAutoConfirm.Controls.Add(this.chkAutoConfirmTrades);
            this.groupBoxAutoConfirm.Location = new System.Drawing.Point(32, 156);
            this.groupBoxAutoConfirm.Name = "groupBoxAutoConfirm";
            this.groupBoxAutoConfirm.Size = new System.Drawing.Size(593, 178);
            this.groupBoxAutoConfirm.TabIndex = 8;
            this.groupBoxAutoConfirm.TabStop = false;
            this.groupBoxAutoConfirm.Text = "Auto Confirm ( Checking both options will disable Confirmation Popup )";
            // 
            // groupBoxStartupAutoConfirmDelay
            // 
            this.groupBoxStartupAutoConfirmDelay.Controls.Add(this.label10);
            this.groupBoxStartupAutoConfirmDelay.Controls.Add(this.label2);
            this.groupBoxStartupAutoConfirmDelay.Controls.Add(this.numDelayAutoConfirmAtStartup);
            this.groupBoxStartupAutoConfirmDelay.Controls.Add(this.chkDelayAutoConfirmAtStartup);
            this.groupBoxStartupAutoConfirmDelay.Enabled = false;
            this.groupBoxStartupAutoConfirmDelay.Location = new System.Drawing.Point(6, 127);
            this.groupBoxStartupAutoConfirmDelay.Name = "groupBoxStartupAutoConfirmDelay";
            this.groupBoxStartupAutoConfirmDelay.Size = new System.Drawing.Size(581, 45);
            this.groupBoxStartupAutoConfirmDelay.TabIndex = 7;
            this.groupBoxStartupAutoConfirmDelay.TabStop = false;
            this.groupBoxStartupAutoConfirmDelay.Text = "Delay at startup";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(392, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "sec ( Range 1 - 60 )";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(262, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Delay";
            // 
            // numDelayAutoConfirmAtStartup
            // 
            this.numDelayAutoConfirmAtStartup.Location = new System.Drawing.Point(304, 16);
            this.numDelayAutoConfirmAtStartup.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numDelayAutoConfirmAtStartup.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDelayAutoConfirmAtStartup.Name = "numDelayAutoConfirmAtStartup";
            this.numDelayAutoConfirmAtStartup.Size = new System.Drawing.Size(80, 22);
            this.numDelayAutoConfirmAtStartup.TabIndex = 11;
            this.numDelayAutoConfirmAtStartup.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // chkDelayAutoConfirmAtStartup
            // 
            this.chkDelayAutoConfirmAtStartup.AutoSize = true;
            this.chkDelayAutoConfirmAtStartup.Location = new System.Drawing.Point(11, 19);
            this.chkDelayAutoConfirmAtStartup.Name = "chkDelayAutoConfirmAtStartup";
            this.chkDelayAutoConfirmAtStartup.Size = new System.Drawing.Size(178, 17);
            this.chkDelayAutoConfirmAtStartup.TabIndex = 0;
            this.chkDelayAutoConfirmAtStartup.Text = "Delay Auto-confirm at startup";
            this.chkDelayAutoConfirmAtStartup.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(85, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(337, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "- To avoid problems keep your pc clean or encrypt your account";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(85, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(364, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "- You can lose Items or get scammed, and you will not get them back";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(85, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(402, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "- You will not see any popup when a trade / market transaction is confirmed";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(85, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "- Use this at your own risk! ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(538, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "It is recommended to use \"Popup Confirmations\" instead of \"Auto-Confirm\", It\'s fa" +
    "st and more secure!";
            // 
            // chkAutoConfirmMarket
            // 
            this.chkAutoConfirmMarket.AutoSize = true;
            this.chkAutoConfirmMarket.Enabled = false;
            this.chkAutoConfirmMarket.Location = new System.Drawing.Point(271, 107);
            this.chkAutoConfirmMarket.Name = "chkAutoConfirmMarket";
            this.chkAutoConfirmMarket.Size = new System.Drawing.Size(289, 17);
            this.chkAutoConfirmMarket.TabIndex = 0;
            this.chkAutoConfirmMarket.Text = "Enable Auto-confirm market transactions at startup";
            this.chkAutoConfirmMarket.UseVisualStyleBackColor = true;
            this.chkAutoConfirmMarket.CheckedChanged += new System.EventHandler(this.chkBoxAutoConfirmMarket_CheckedChanged);
            // 
            // labelAutoConfirmWarning
            // 
            this.labelAutoConfirmWarning.AutoSize = true;
            this.labelAutoConfirmWarning.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAutoConfirmWarning.Location = new System.Drawing.Point(7, 18);
            this.labelAutoConfirmWarning.Name = "labelAutoConfirmWarning";
            this.labelAutoConfirmWarning.Size = new System.Drawing.Size(72, 17);
            this.labelAutoConfirmWarning.TabIndex = 2;
            this.labelAutoConfirmWarning.Text = "Warning!!!";
            // 
            // chkAutoConfirmTrades
            // 
            this.chkAutoConfirmTrades.AutoSize = true;
            this.chkAutoConfirmTrades.Enabled = false;
            this.chkAutoConfirmTrades.Location = new System.Drawing.Point(12, 107);
            this.chkAutoConfirmTrades.Name = "chkAutoConfirmTrades";
            this.chkAutoConfirmTrades.Size = new System.Drawing.Size(220, 17);
            this.chkAutoConfirmTrades.TabIndex = 1;
            this.chkAutoConfirmTrades.Text = "Enable Auto-confirm trades at startup";
            this.chkAutoConfirmTrades.UseVisualStyleBackColor = true;
            this.chkAutoConfirmTrades.CheckedChanged += new System.EventHandler(this.chkBoxAutoConfirmTrades_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioBtnBorderConfPopup2);
            this.groupBox2.Controls.Add(this.radioBtnBorderConfPopup1);
            this.groupBox2.Location = new System.Drawing.Point(33, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(592, 45);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Confirmation Popup - Border ( it will take effect after you restart the app )";
            // 
            // radioBtnBorderConfPopup2
            // 
            this.radioBtnBorderConfPopup2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radioBtnBorderConfPopup2.AutoSize = true;
            this.radioBtnBorderConfPopup2.Enabled = false;
            this.radioBtnBorderConfPopup2.Location = new System.Drawing.Point(270, 19);
            this.radioBtnBorderConfPopup2.Name = "radioBtnBorderConfPopup2";
            this.radioBtnBorderConfPopup2.Size = new System.Drawing.Size(110, 17);
            this.radioBtnBorderConfPopup2.TabIndex = 7;
            this.radioBtnBorderConfPopup2.TabStop = true;
            this.radioBtnBorderConfPopup2.Text = "windows border";
            this.radioBtnBorderConfPopup2.UseVisualStyleBackColor = true;
            // 
            // radioBtnBorderConfPopup1
            // 
            this.radioBtnBorderConfPopup1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radioBtnBorderConfPopup1.AutoSize = true;
            this.radioBtnBorderConfPopup1.Enabled = false;
            this.radioBtnBorderConfPopup1.Location = new System.Drawing.Point(12, 19);
            this.radioBtnBorderConfPopup1.Name = "radioBtnBorderConfPopup1";
            this.radioBtnBorderConfPopup1.Size = new System.Drawing.Size(89, 17);
            this.radioBtnBorderConfPopup1.TabIndex = 6;
            this.radioBtnBorderConfPopup1.TabStop = true;
            this.radioBtnBorderConfPopup1.Text = "small border";
            this.radioBtnBorderConfPopup1.UseVisualStyleBackColor = true;
            // 
            // groupSystemTray
            // 
            this.groupSystemTray.Controls.Add(this.chkStartMinimizedToSystemTray);
            this.groupSystemTray.Controls.Add(this.chkHideTaskbarIcon);
            this.groupSystemTray.Controls.Add(this.radioButton3_SystemTray);
            this.groupSystemTray.Controls.Add(this.radioButton2_SystemTray);
            this.groupSystemTray.Controls.Add(this.radioButton1_SystemTray);
            this.groupSystemTray.Location = new System.Drawing.Point(5, 406);
            this.groupSystemTray.Name = "groupSystemTray";
            this.groupSystemTray.Size = new System.Drawing.Size(312, 149);
            this.groupSystemTray.TabIndex = 5;
            this.groupSystemTray.TabStop = false;
            this.groupSystemTray.Text = "System Tray";
            // 
            // chkStartMinimizedToSystemTray
            // 
            this.chkStartMinimizedToSystemTray.AutoSize = true;
            this.chkStartMinimizedToSystemTray.Location = new System.Drawing.Point(12, 123);
            this.chkStartMinimizedToSystemTray.Name = "chkStartMinimizedToSystemTray";
            this.chkStartMinimizedToSystemTray.Size = new System.Drawing.Size(180, 17);
            this.chkStartMinimizedToSystemTray.TabIndex = 13;
            this.chkStartMinimizedToSystemTray.Text = "Start miminized to System Tray";
            this.chkStartMinimizedToSystemTray.UseVisualStyleBackColor = true;
            // 
            // chkHideTaskbarIcon
            // 
            this.chkHideTaskbarIcon.AutoSize = true;
            this.chkHideTaskbarIcon.Location = new System.Drawing.Point(12, 100);
            this.chkHideTaskbarIcon.Name = "chkHideTaskbarIcon";
            this.chkHideTaskbarIcon.Size = new System.Drawing.Size(295, 17);
            this.chkHideTaskbarIcon.TabIndex = 12;
            this.chkHideTaskbarIcon.Text = "Hide Taskbar icon, show the icon only in System Tray";
            this.chkHideTaskbarIcon.UseVisualStyleBackColor = true;
            // 
            // radioButton3_SystemTray
            // 
            this.radioButton3_SystemTray.AutoSize = true;
            this.radioButton3_SystemTray.Location = new System.Drawing.Point(12, 65);
            this.radioButton3_SystemTray.Name = "radioButton3_SystemTray";
            this.radioButton3_SystemTray.Size = new System.Drawing.Size(293, 17);
            this.radioButton3_SystemTray.TabIndex = 8;
            this.radioButton3_SystemTray.TabStop = true;
            this.radioButton3_SystemTray.Text = "none (default action for button Close and Minimize)";
            this.radioButton3_SystemTray.UseVisualStyleBackColor = true;
            // 
            // radioButton2_SystemTray
            // 
            this.radioButton2_SystemTray.AutoSize = true;
            this.radioButton2_SystemTray.Location = new System.Drawing.Point(12, 42);
            this.radioButton2_SystemTray.Name = "radioButton2_SystemTray";
            this.radioButton2_SystemTray.Size = new System.Drawing.Size(242, 17);
            this.radioButton2_SystemTray.TabIndex = 7;
            this.radioButton2_SystemTray.TabStop = true;
            this.radioButton2_SystemTray.Text = "Minimise button minimizes the app to tray";
            this.radioButton2_SystemTray.UseVisualStyleBackColor = true;
            // 
            // radioButton1_SystemTray
            // 
            this.radioButton1_SystemTray.AutoSize = true;
            this.radioButton1_SystemTray.Location = new System.Drawing.Point(12, 19);
            this.radioButton1_SystemTray.Name = "radioButton1_SystemTray";
            this.radioButton1_SystemTray.Size = new System.Drawing.Size(224, 17);
            this.radioButton1_SystemTray.TabIndex = 6;
            this.radioButton1_SystemTray.TabStop = true;
            this.radioButton1_SystemTray.Text = "Close button minimizes the app to tray";
            this.radioButton1_SystemTray.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(550, 601);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(106, 30);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.SettingsCancel_Click);
            // 
            // chkAppCanRunMultipleTimes
            // 
            this.chkAppCanRunMultipleTimes.AutoSize = true;
            this.chkAppCanRunMultipleTimes.Location = new System.Drawing.Point(12, 19);
            this.chkAppCanRunMultipleTimes.Name = "chkAppCanRunMultipleTimes";
            this.chkAppCanRunMultipleTimes.Size = new System.Drawing.Size(218, 17);
            this.chkAppCanRunMultipleTimes.TabIndex = 12;
            this.chkAppCanRunMultipleTimes.Text = "The app can be started multiple times";
            this.chkAppCanRunMultipleTimes.UseVisualStyleBackColor = true;
            // 
            // groupBoxRun
            // 
            this.groupBoxRun.Controls.Add(this.label19);
            this.groupBoxRun.Controls.Add(this.chkAppCanRunMultipleTimes);
            this.groupBoxRun.Location = new System.Drawing.Point(323, 406);
            this.groupBoxRun.Name = "groupBoxRun";
            this.groupBoxRun.Size = new System.Drawing.Size(313, 82);
            this.groupBoxRun.TabIndex = 8;
            this.groupBoxRun.TabStop = false;
            this.groupBoxRun.Text = "Run";
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(9, 43);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(298, 26);
            this.label19.TabIndex = 13;
            this.label19.Text = "( Use it only if you are running multiple SDA 47 apps at the same time. )";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.numSendAppStatusInterval);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBoxSendAppStatusToAddress);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.numAppNo);
            this.groupBox1.Controls.Add(this.chkSendAppStatus);
            this.groupBox1.Location = new System.Drawing.Point(5, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(631, 105);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Send app status";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(598, 18);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(23, 13);
            this.label16.TabIndex = 22;
            this.label16.Text = "sec";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(265, 83);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(155, 13);
            this.label15.TabIndex = 21;
            this.label15.Text = "No data sent = status offline";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(265, 68);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(191, 13);
            this.label14.TabIndex = 20;
            this.label14.Text = "Data format: ?AppNo=1&&Status=ok";
            // 
            // numSendAppStatusInterval
            // 
            this.numSendAppStatusInterval.Location = new System.Drawing.Point(530, 14);
            this.numSendAppStatusInterval.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numSendAppStatusInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSendAppStatusInterval.Name = "numSendAppStatusInterval";
            this.numSendAppStatusInterval.Size = new System.Drawing.Size(65, 22);
            this.numSendAppStatusInterval.TabIndex = 13;
            this.numSendAppStatusInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(426, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Send data interval:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(136, 67);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Data sent by the app:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(119, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Send data to address:";
            // 
            // textBoxSendAppStatusToAddress
            // 
            this.textBoxSendAppStatusToAddress.Location = new System.Drawing.Point(138, 42);
            this.textBoxSendAppStatusToAddress.Name = "textBoxSendAppStatusToAddress";
            this.textBoxSendAppStatusToAddress.Size = new System.Drawing.Size(483, 22);
            this.textBoxSendAppStatusToAddress.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(154, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(175, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "App no. (number your SDA apps)";
            // 
            // numAppNo
            // 
            this.numAppNo.Location = new System.Drawing.Point(336, 15);
            this.numAppNo.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numAppNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAppNo.Name = "numAppNo";
            this.numAppNo.Size = new System.Drawing.Size(65, 22);
            this.numAppNo.TabIndex = 15;
            this.numAppNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkSendAppStatus
            // 
            this.chkSendAppStatus.AutoSize = true;
            this.chkSendAppStatus.Location = new System.Drawing.Point(12, 21);
            this.chkSendAppStatus.Name = "chkSendAppStatus";
            this.chkSendAppStatus.Size = new System.Drawing.Size(109, 17);
            this.chkSendAppStatus.TabIndex = 12;
            this.chkSendAppStatus.Text = "Send app status";
            this.chkSendAppStatus.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(7, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(650, 587);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.GroupPopupNewConf);
            this.tabPage1.Controls.Add(this.groupSystemTray);
            this.tabPage1.Controls.Add(this.groupBoxRun);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(642, 561);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(642, 561);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Send Status to Server";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBox3);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Location = new System.Drawing.Point(5, 119);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(631, 67);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Info";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(432, 16);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(71, 22);
            this.textBox3.TabIndex = 12;
            this.textBox3.Text = "1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 44);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(564, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "The app \"Server for Multiple SDA 47 apps\" can be used to easily Run / Stop / Mana" +
    "ge multiple \"SDA 47\" apps.";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 20);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(414, 13);
            this.label17.TabIndex = 1;
            this.label17.Text = "This app is compatible with the app \"Server for Multiple SDA 47 apps\" Server ID:";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 638);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numPeriodicInterval)).EndInit();
            this.GroupPopupNewConf.ResumeLayout(false);
            this.GroupPopupNewConf.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBoxAutoConfirm.ResumeLayout(false);
            this.groupBoxAutoConfirm.PerformLayout();
            this.groupBoxStartupAutoConfirmDelay.ResumeLayout(false);
            this.groupBoxStartupAutoConfirmDelay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelayAutoConfirmAtStartup)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupSystemTray.ResumeLayout(false);
            this.groupSystemTray.PerformLayout();
            this.groupBoxRun.ResumeLayout(false);
            this.groupBoxRun.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSendAppStatusInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAppNo)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkConfirmationsPeriodicChecking;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.NumericUpDown numPeriodicInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GroupPopupNewConf;
        private System.Windows.Forms.GroupBox groupSystemTray;
        private System.Windows.Forms.RadioButton radioButton3_SystemTray;
        private System.Windows.Forms.RadioButton radioButton2_SystemTray;
        private System.Windows.Forms.RadioButton radioButton1_SystemTray;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkCheckAll;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioBtnBorderConfPopup2;
        private System.Windows.Forms.RadioButton radioBtnBorderConfPopup1;
        private System.Windows.Forms.GroupBox groupBoxAutoConfirm;
        private System.Windows.Forms.CheckBox chkAutoConfirmMarket;
        private System.Windows.Forms.CheckBox chkAutoConfirmTrades;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelAutoConfirmWarning;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkAppCanRunMultipleTimes;
        private System.Windows.Forms.GroupBox groupBoxRun;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numAppNo;
        private System.Windows.Forms.NumericUpDown numSendAppStatusInterval;
        private System.Windows.Forms.CheckBox chkSendAppStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxSendAppStatusToAddress;
        private System.Windows.Forms.CheckBox chkHideTaskbarIcon;
        private System.Windows.Forms.CheckBox chkStartMinimizedToSystemTray;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkDisplayPopupConfirmation;
        private System.Windows.Forms.CheckBox chkDelayAutoConfirmAtStartup;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numDelayAutoConfirmAtStartup;
        private System.Windows.Forms.GroupBox groupBoxStartupAutoConfirmDelay;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
    }
}