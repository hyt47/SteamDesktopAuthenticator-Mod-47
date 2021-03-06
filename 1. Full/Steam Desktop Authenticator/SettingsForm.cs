﻿using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Steam_Desktop_Authenticator
{
    public partial class SettingsForm : Form
    {
        Manifest manifest;
        bool fullyLoaded = false;
        bool Read_AutoConfirmTrades_IsStartedSecurely = false;
        bool Read_AutoConfirmMarket_IsStartedSecurely = false;


        // Run at Startup
        bool startupEnabled;
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private string StartupLine { get { return "\"" + Application.ExecutablePath + "\" -startup"; } }


        public SettingsForm()
        {
            InitializeComponent();

            // Get latest manifest
            manifest = Manifest.GetManifest(true);

            // set
            #region Set

                // set GUI Confirmation Popup
                #region set GUI Confirmation Popup
                    chkConfirmationsPeriodicChecking.Checked = manifest.ConfirmationsPeriodicChecking;
                    numPeriodicInterval.Value = manifest.ConfirmationCheckingInterval;

                    string Settings_PopupConfirmationBorder = manifest.PopupConfirmationBorder;
                    if (Settings_PopupConfirmationBorder == "1") { radioBtnBorderConfPopup1.Checked = true; }
                    else if (Settings_PopupConfirmationBorder == "2") { radioBtnBorderConfPopup2.Checked = true; }
                    else { radioBtnBorderConfPopup2.Checked = true; }
                #endregion


            // set Check all accounts for new Confirmations
            chkCheckAll.Checked = manifest.ConfirmationCheckAllAccounts;
            num_ChackAllAcc_DelaySessionRefresh.Value = manifest.ConfirmationCheckAllAcc_RefreshSessionDellay;

                // set Auto Confirm
                    chkAutoConfirmTrades.Checked = manifest.AutoConfirmTrades;
                    chkAutoConfirmMarket.Checked = manifest.AutoConfirmMarketTransactions;
                    chkDelayAutoConfirmAtStartup.Checked = manifest.DelayAutoConfirmAtStartup;
                    numDelayAutoConfirmAtStartup.Value = manifest.DelayAutoConfirmAtStartupInterval;

                // set Popup confirmation
                    chkDisplayPopupConfirmation.Checked = manifest.DisplayPopupConfirmation;

                // set GUI Confirmation List btn
                    checkBoxConfirmationListBtn.Checked = manifest.ShowConfirmationListButton;


                // set GUI System Tray
                    string Settings_MinimiseToSystemTray = manifest.MinimiseToSystemTray;
                    if (Settings_MinimiseToSystemTray == "CloseBtnMinimizeToTray") { radioButton1_SystemTray.Checked = true; }
                    else if (Settings_MinimiseToSystemTray == "MinimiseBtnMinimizeToTray") { radioButton2_SystemTray.Checked = true; }
                    else if (Settings_MinimiseToSystemTray == "default") { radioButton3_SystemTray.Checked = true; }
                    else { radioButton1_SystemTray.Checked = true; }

                    chkHideTaskbarIcon.Checked = manifest.HideTaskbarIcon;
                    chkStartMinimizedToSystemTray.Checked = manifest.StartMinimizedToSystemTray;

                // set Send App Status
                    chkSendAppStatus.Checked = manifest.SendAppStatus;
                    textBoxSendAppStatusToAddress.Text =  manifest.SendAppStatusToAddress;
                    numAppNo.Value =  manifest.SendAppNo;
                    numSendAppStatusInterval.Value = manifest.SendAppStatusInterval;

                // set Planner
                #region set Planner
                    int Secure_ManuallyEnableWebsitePlannerAtMyOwnRisk = Manifest.ManuallyEnableWebsitePlannerAtMyOwnRisk; // Secure Settings
                    string Secure_ManuallySetWebsitePlannerAddress = Manifest.ManuallySetWebsitePlannerAddress; // Secure Settings
                    if(Secure_ManuallyEnableWebsitePlannerAtMyOwnRisk == 1) {
                        label_DisabledWebPlanner.Visible = false;
                        label_EnabledWebPlanner.Visible = true;

                        checkBoxWPlanner.Checked = manifest.SendToWebsitePlanner;

                        if (Secure_ManuallySetWebsitePlannerAddress == "" || Secure_ManuallySetWebsitePlannerAddress == null) { textBoxWPlannerAddress.Text = ""; }
                        else { textBoxWPlannerAddress.Text = Secure_ManuallySetWebsitePlannerAddress; }
                
                        numericUpDownWPlannerInterval.Value =  manifest.WebsitePlannerInterval;
                        checkBoxWPlannerSendCode.Checked = manifest.WebsitePlannerSendCode;
                        numericUpDownWPlannerShift1.Value = Manifest.ManuallySetWebsitePlannerShift1;
                        numericUpDownWPlannerShift2.Value = Manifest.ManuallySetWebsitePlannerShift2;
                        numericUpDownWPlannerShift3.Value = Manifest.ManuallySetWebsitePlannerShift3;
                        numericUpDownWPlannerShift4.Value = Manifest.ManuallySetWebsitePlannerShift4;
                        numericUpDownWPlannerShift5.Value = Manifest.ManuallySetWebsitePlannerShift5;

                        if(manifest.SendToWebsitePlanner == true) {
                            chkConfirmationsPeriodicChecking.Checked = false; 
                            chkCheckAll.Checked = false;
                        }
                    } else { // Disable All
                        label_DisabledWebPlanner.Visible = true;
                        label_EnabledWebPlanner.Visible = false;

                        checkBoxWPlanner.Enabled = false;
                        textBoxWPlannerAddress.Enabled = false;
                        numericUpDownWPlannerInterval.Enabled = false;
                        checkBoxWPlannerSendCode.Enabled = false;
                        numericUpDownWPlannerShift1.Enabled = false;
                        numericUpDownWPlannerShift2.Enabled = false;
                        numericUpDownWPlannerShift3.Enabled = false;
                        numericUpDownWPlannerShift4.Enabled = false;
                        numericUpDownWPlannerShift5.Enabled = false;
                    }
                #endregion // set Planner End

                // set App Can Run Multiple Times
                    if (manifest.SendAppStatus)
                    {
                        chkAppCanRunMultipleTimes.Checked = true;
                    }
                    else {
                        chkAppCanRunMultipleTimes.Checked = manifest.AppCanBeStartedMultipleTimes;
                    }

                #endregion // Set




            // enable settings form
            SetControlsEnabledState_IfAutoCheckingForConfirmations(chkConfirmationsPeriodicChecking.Checked);

            // manifest settings loaded
                fullyLoaded = true;

            // Run at Startup
            startupEnabled = rkApp.GetValue("SDA")?.ToString() == StartupLine;
            checkBoxRunAtStartup.Checked = startupEnabled;
            checkBoxRunAtStartup.Checked = manifest.RunAtStartup;
        }


        private void SetControlsEnabledState_IfAutoCheckingForConfirmations(bool Enabled)
        {
            numPeriodicInterval.Enabled = chkCheckAll.Enabled = radioBtnBorderConfPopup1.Enabled = radioBtnBorderConfPopup2.Enabled = chkAutoConfirmMarket.Enabled = chkAutoConfirmTrades.Enabled = groupBoxStartupAutoConfirmDelay.Enabled = chkDisplayPopupConfirmation.Enabled = Enabled;
        }

        // Auto Confirm Checked
        private void ShowWarning(CheckBox affectedBox)
        {
            if (!fullyLoaded) return;

            var result = MessageBox.Show("Warning:\n    - Enabling this will severely reduce the security of your items!\n    - Use of this option is at your own risk.\n\nWould you like to continue?", "Warning!", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                affectedBox.Checked = false;
            }
        }

        private void SettingsOk_Click(object sender, EventArgs e)
        {
            // Run at Startup
            manifest.RunAtStartup = checkBoxRunAtStartup.Checked;

            // Confirmation Popup
            manifest.ConfirmationsPeriodicChecking = chkConfirmationsPeriodicChecking.Checked;
            manifest.SendToWebsitePlanner = checkBoxWPlanner.Checked;
            if (checkBoxWPlanner.Checked == false) { manifest.SendToWebsitePlanner = false; } // disable Planner

            manifest.ConfirmationCheckingInterval = (int)numPeriodicInterval.Value;

            string PopupConfirmationBorderValue = "1"; // default value
            if (radioBtnBorderConfPopup1.Checked) { PopupConfirmationBorderValue = "1"; }
            if (radioBtnBorderConfPopup2.Checked) { PopupConfirmationBorderValue = "2"; }
            manifest.PopupConfirmationBorder = PopupConfirmationBorderValue;

            // Check all accounts for new Confirmations
            manifest.ConfirmationCheckAllAccounts = chkCheckAll.Checked;
            manifest.SendToWebsitePlanner = checkBoxWPlanner.Checked;
            if (checkBoxWPlanner.Checked == false) { manifest.SendToWebsitePlanner = false; } // disable Planner
            manifest.ConfirmationCheckAllAcc_RefreshSessionDellay = (int)num_ChackAllAcc_DelaySessionRefresh.Value;

            // Auto Confirm
            manifest.AutoConfirmTrades = chkAutoConfirmTrades.Checked;
            manifest.AutoConfirmMarketTransactions = chkAutoConfirmMarket.Checked;
            manifest.DelayAutoConfirmAtStartup = chkDelayAutoConfirmAtStartup.Checked;
            manifest.DelayAutoConfirmAtStartupInterval = (int)numDelayAutoConfirmAtStartup.Value;

            // Popup confirmation
            manifest.DisplayPopupConfirmation = chkDisplayPopupConfirmation.Checked;

            // Confirmation List btn
            manifest.ShowConfirmationListButton = checkBoxConfirmationListBtn.Checked;

            // System Tray
            string SystemTrayValue = "CloseBtnMinimizeToTray"; // default value
            if (radioButton1_SystemTray.Checked) { SystemTrayValue = "CloseBtnMinimizeToTray"; }
            if (radioButton2_SystemTray.Checked) { SystemTrayValue = "MinimiseBtnMinimizeToTray"; }
            if (radioButton3_SystemTray.Checked) { SystemTrayValue = "default"; }
            manifest.MinimiseToSystemTray = SystemTrayValue;


            manifest.HideTaskbarIcon = chkHideTaskbarIcon.Checked;
            manifest.StartMinimizedToSystemTray = chkStartMinimizedToSystemTray.Checked;

            // Send Status
            #region Send Status
            manifest.SendAppStatus = chkSendAppStatus.Checked;
            manifest.SendAppStatusToAddress = textBoxSendAppStatusToAddress.Text;
            manifest.SendAppNo = (int)numAppNo.Value;
            manifest.SendAppStatusInterval = (int)numSendAppStatusInterval.Value;
            #endregion // Send Status

            // Save Planner
            #region Save Planner
            int Secure_ManuallyEnableWebsitePlannerAtMyOwnRisk = Manifest.ManuallyEnableWebsitePlannerAtMyOwnRisk; // Secure Settings
            string Secure_ManuallySetWebsitePlannerAddress = Manifest.ManuallySetWebsitePlannerAddress; // Secure Settings

            if (Secure_ManuallyEnableWebsitePlannerAtMyOwnRisk == 1)
            {
                manifest.SendToWebsitePlanner = checkBoxWPlanner.Checked;
                if (checkBoxWPlanner.Checked == true)
                {
                    manifest.ConfirmationsPeriodicChecking = true;  // Enable Auto Check
                    manifest.ConfirmationCheckAllAccounts = true; // Enable Auto Check All Accounts
                }
                manifest.WebsitePlannerInterval = (int)numericUpDownWPlannerInterval.Value;
                manifest.WebsitePlannerSendCode = checkBoxWPlannerSendCode.Checked;
            }
            #endregion // Save Planner End

            //App Can Run Multiple Times
            if (chkSendAppStatus.Checked == true) { manifest.AppCanBeStartedMultipleTimes = true; }
                else { manifest.AppCanBeStartedMultipleTimes = chkAppCanRunMultipleTimes.Checked;  }

            //save
                manifest.Save();
            
            // Run at Startup
            if (checkBoxRunAtStartup.Checked && !startupEnabled) { rkApp.SetValue("SDA", StartupLine); }
            else if (!checkBoxRunAtStartup.Checked && startupEnabled) { rkApp.DeleteValue("SDA", false); }
            
            //close form
            this.Close();
        }

        private void SettingsCancel_Click(object sender, EventArgs e) { this.Close(); }

        private void checkBoxConfirmationsPeriodicChecking_CheckedChanged(object sender, EventArgs e) {
            SetControlsEnabledState_IfAutoCheckingForConfirmations(chkConfirmationsPeriodicChecking.Checked);
        }

        private void chkBoxAutoConfirmMarket_CheckedChanged(object sender, EventArgs e) { if (chkAutoConfirmMarket.Checked) { ShowWarning(chkAutoConfirmMarket); } }
        
        private void chkBoxAutoConfirmTrades_CheckedChanged(object sender, EventArgs e){ if (chkAutoConfirmTrades.Checked) { ShowWarning(chkAutoConfirmTrades);  } }

        // SendAppStatus
        private void chkSendAppStatus_CheckedChanged(object sender, EventArgs e) {  }

        // Planner
        private void checkBoxWPlanner_CheckedChanged(object sender, EventArgs e)  { }


    }
}
