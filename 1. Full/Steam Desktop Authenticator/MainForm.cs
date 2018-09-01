using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteamAuth;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Drawing;
using System.Linq;

namespace Steam_Desktop_Authenticator
{
    public partial class MainForm : Form
    {

        //Settings-declare
        public string Settings_SendAppStatusToAddress = "";
        public int Settings_SendAppStatusInterval = 5;
        public int Settings_AppNo = 1;
        public string Settings_MinimiseToSystemTray;
        public bool Settings_ShowConfirmationListButton;
        public bool Settings_DisplayPopupConfirmation;
        public bool Settings_ConfirmationsPeriodicChecking;
        public int Settings_ConfirmationCheckAllAcc_RefreshSessionDellay;
        public bool Settings_ConfirmationCheckAllAccounts;
        public bool Settings_DelayAutoConfirmAtStartup;
        public static int Settings_DelayAutoConfirmAtStartupInterval;
        public int Settings_PopupNewConf_Interval;

        int Secure_ManuallyEnableWebsitePlannerAtMyOwnRisk = 0; // Secure Settings
        string Secure_ManuallySetWebsitePlannerAddress = ""; // Secure Settings
        public bool Settings_SendToWebsitePlanner = false;
        public string Settings_WebsitePlannerAddress = "";
        public int Settings_WebsitePlannerInterval = 1;
        public bool Settings_WebsitePlannerSendCode = false;
        public int Settings_WebsitePlannerShift1 = 1;
        public int Settings_WebsitePlannerShift2 = 2;
        public int Settings_WebsitePlannerShift3 = 3;
        public int Settings_WebsitePlannerShift4 = 4;
        public int Settings_WebsitePlannerShift5 = 5;

        public int DefaultSettings_PopupNewConf_Interval = 30;
        public int DefaultSettings_PopupNewConf_Planner_Interval = 1000;
        public int Settings_PopupNewConf_Planner_Interval_Tick = 1000;
        public int Code_DellayAtStartup_Sec = 6;


        public int RefreshSession_InfoStatus = 0; // 0 = none // 1 = started...
        public int AllAcc_RefreshSession_InfoStatus = 0; // 0 = none // 1 = started...

        public int AutoConfirm_Trades = 0; // 0 = no // 1 = yes // 2 = yes but dellay at startup
        public int AutoConfirm_Market = 0;

        bool backgroundWorkerSendAppStatus_restart = false;
        int backgroundWorkerSendAppStatus_Timer = 0;

        private SteamGuardAccount TargetAccount = null;

        private SteamGuardAccount currentAccount = null;
        private SteamGuardAccount[] allAccounts;
        private List<string> updatedSessions = new List<string>();
        private Manifest manifest;
        private static SemaphoreSlim confirmationsSemaphore = new SemaphoreSlim(1, 1);

        private long steamTime = 0;
        private long currentSteamChunk = 0;
        private string passKey = null;

        // Forms
        private TradePopupForm popupFrm = new TradePopupForm();

        public MainForm()
        {
            InitializeComponent();
        }
        public void SetEncryptionKey(string key)
        {
            passKey = key;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            trayIcon.Icon = this.Icon;

            try
            {
                this.manifest = Manifest.GetManifest();
                loadSettings();
            }
            catch
            {
                MessageBox.Show("Unable to read your settings. Try restating the App.", Manifest.MainAppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

        }

        // Form event handlers
        private void MainForm_Shown(object sender, EventArgs e)
        {
            // set Btn custom File and Folder name
            this.menuImportmaFile.Text = "From " + Manifest.SteamFileExtension + " file, " + Manifest.FolderNameSteamFiles + " folder, or an older App folder";

            this.labelVersion.Text = String.Format("v{0}", Application.ProductVersion);

            // Make sure we don't show that welcome dialog again
            this.manifest.FirstRun = false;
            this.manifest.Save();

            // Tick first time manually to sync time
            timerSteamGuard_Tick(new object(), EventArgs.Empty);

            if (manifest.Encrypted)
            {
                if (passKey == null)
                {
                    passKey = manifest.PromptForPassKey();
                    if (passKey == null)
                    {
                        Application.Exit();
                    }
                }
            }
            menuManageEncryption.Enabled = manifest.Entries.Count > 0;

            // start minimized
            if (manifest.StartMinimizedToSystemTray) { this.Hide(); }

            loadAccountsList();
        }


        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (Settings_MinimiseToSystemTray == "MinimiseBtnMinimizeToTray")
            {
                if (this.WindowState == FormWindowState.Minimized) { this.Hide(); }
            }
        }




        // Buttons
        /////////////////////
        #region Buttons

        // Quit Buttons
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (Settings_MinimiseToSystemTray == "CloseBtnMinimizeToTray") { this.Hide(); e.Cancel = true; }
            }
            else
            {
                if (backgroundWorkerSendAppStatus.IsBusy) { backgroundWorkerSendAppStatus.CancelAsync(); }
                if (timer_DelayAutoConfirmAtStartup.Enabled) { timer_DelayAutoConfirmAtStartup.Enabled = false; }
                Application.Exit();
            }
        }
        private void menuQuit_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerSendAppStatus.IsBusy) { backgroundWorkerSendAppStatus.CancelAsync(); }
            if (timer_DelayAutoConfirmAtStartup.Enabled) { timer_DelayAutoConfirmAtStartup.Enabled = false; }
            Application.Exit();
        }

        private void trayQuit_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerSendAppStatus.IsBusy) { backgroundWorkerSendAppStatus.CancelAsync(); }
            if (timer_DelayAutoConfirmAtStartup.Enabled) { timer_DelayAutoConfirmAtStartup.Enabled = false; }
            Application.Exit();
        }


        // UI Button handlers
        private void menuSteamLogin_Click(object sender, EventArgs e) { LoginForm mLoginForm = new LoginForm(); mLoginForm.ShowDialog(); this.loadAccountsList(); }

        private void btnTradeConfirmations_Click(object sender, EventArgs e)
        {
            if (currentAccount == null) return;

            try
            {
                ConfirmationFormWeb confirms = new ConfirmationFormWeb(currentAccount);
                confirms.Show();
            }
            catch (Exception)
            {
                DialogResult res = MessageBox.Show("You are missing a dependency required to view your trade confirmations.\nWould you like to install it now?", "Trade confirmations failed to open", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes) { new InstallRedistribForm(true).ShowDialog(); }
            }
        }

        private void btnTradeConfirmationsList_Click(object sender, EventArgs e)
        {
            if (currentAccount == null)
            {
                MessageBox.Show("No account selected"); return;
            }
            else { ConfirmationForm confirmations = new ConfirmationForm(currentAccount); confirmations.ShowDialog(); }
        }


        private void menuManageEncryption_Click(object sender, EventArgs e)
        {
            if (manifest.Encrypted)
            {
                InputForm currentPassKeyForm = new InputForm("Enter current passkey", true);
                currentPassKeyForm.ShowDialog();

                if (currentPassKeyForm.Canceled)
                {
                    return;
                }

                string curPassKey = currentPassKeyForm.txtBox.Text;

                InputForm changePassKeyForm = new InputForm("Enter new passkey, or leave blank to remove encryption.");
                changePassKeyForm.ShowDialog();

                if (changePassKeyForm.Canceled && !string.IsNullOrEmpty(changePassKeyForm.txtBox.Text))
                {
                    return;
                }

                InputForm changePassKeyForm2 = new InputForm("Confirm new passkey, or leave blank to remove encryption.");
                changePassKeyForm2.ShowDialog();

                if (changePassKeyForm2.Canceled && !string.IsNullOrEmpty(changePassKeyForm.txtBox.Text))
                {
                    return;
                }

                string newPassKey = changePassKeyForm.txtBox.Text;
                string confirmPassKey = changePassKeyForm2.txtBox.Text;

                if (newPassKey != confirmPassKey)
                {
                    MessageBox.Show("Passkeys do not match.");
                    return;
                }

                if (newPassKey.Length == 0)
                {
                    newPassKey = null;
                }

                string action = newPassKey == null ? "remove" : "change";
                if (!manifest.ChangeEncryptionKey(curPassKey, newPassKey))
                {
                    MessageBox.Show("Unable to " + action + " passkey.");
                }
                else
                {
                    MessageBox.Show("Passkey successfully " + action + "d.");
                    this.loadAccountsList();
                }
            }
            else
            {
                passKey = manifest.PromptSetupPassKey();
                this.loadAccountsList();
            }
        }

        private void menuCheckForUpdates_Click(object sender, EventArgs e) { checkForUpdates(); }

        private void btnCopy_Click(object sender, EventArgs e) { CopyLoginToken(); }

        private void MainForm_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control) { CopyLoginToken(); } }

        private void CopyLoginToken()
        {
            string text = txtLoginToken.Text;
            if (String.IsNullOrEmpty(text)) { return; }
            Clipboard.SetText(text);
        }

        // Tool strip menu handlers
        private void menuRemoveAccountFromManifest_Click(object sender, EventArgs e)
        {
            string Message = "This will remove the selected account from the manifest file.\n\n";
            Message += "This will NOT delete your " + Manifest.SteamFileExtension + ",\nyour file will be moved to:  'accounts removed from manifest'.\n\n";
            if (manifest.Encrypted)
            {
                Message += "Your " + Manifest.SteamFileExtension + " will remain encrypted, if you want to remove it and be decrypted remove the encryption first.\n\n";
                Message += "" + Manifest.SteamFileExtension + "'s that are encrypted can be easily imported back, using your encryption passkey.\n\n";
            }
            Message += "Use this to move a " + Manifest.SteamFileExtension + " to another computer.";

            DialogResult res = MessageBox.Show(Message, "Remove from manifest", MessageBoxButtons.OKCancel);

            if (res == DialogResult.OK)
            {
                string MoveFileReturn = manifest.MoveAccountToRemovedFromManifest(currentAccount);

                if (MoveFileReturn == "ok")
                {
                    // Remove from manifest
                    manifest.RemoveAccount(currentAccount, false);

                    DialogResult ReturnDialog = MessageBox.Show("Account removed from manifest.\nYou can now move the " + Manifest.SteamFileExtension + " to another computer and import it using the File menu.\n\nDo you want to open the folder: 'accounts removed from manifest' ???", "Remove from Manifest", MessageBoxButtons.YesNo);
                    if (ReturnDialog == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("explorer.exe", Manifest.GetExecutableDir() + @"\accounts removed from manifest");
                    }
                    txtLoginToken.Text = "";
                    pbTimeout.Value = 0;
                    loadAccountsList();
                }
                else
                {
                    // error
                    MessageBox.Show("Failed to move file.\nOperation canceled!");
                }
            }
        }

        private void menuLoginAgain_Click(object sender, EventArgs e)
        {
            RefreshSession_InfoStatus = 1;
            this.PromptRefreshLogin(currentAccount);
            RefreshSession_InfoStatus = 0;
        }

        private void menuImportmaFile_Click(object sender, EventArgs e)
        {
            ImportAccountForm Import_Account_Form = new ImportAccountForm();
            Import_Account_Form.ShowDialog();
            loadAccountsList();
        }

        private void menuImportAndroid_Click(object sender, EventArgs e) { new PhoneExtractForm().ShowDialog(); loadAccountsList(); }

        private void menuSettings_Click(object sender, EventArgs e)
        {
            // stop dellay
            if (timer_DelayAutoConfirmAtStartup.Enabled == true) { timer_DelayAutoConfirmAtStartup.Enabled = false; }
            Settings_PopupNewConf.Stop();
            // stop Auto Confirm
            AutoConfirm_Trades = 0;
            AutoConfirm_Market = 0;
            // show form
            new SettingsForm().ShowDialog();
            // Load settings
            manifest = Manifest.GetManifest(true);
            loadSettings();
        }

        private void btn_forceSessionRefreshForAllAccounts_ToolStripMenuItem_Click(object sender, EventArgs e) { Func_RefreshSession_ForAllAcc(); }


        private Process console;
        private void menuConsole_Click(object sender, EventArgs e) { Program.ConsoleForm_Update.Visible = true; }
        private void lblStatus_Click(object sender, EventArgs e) { Program.ConsoleForm_Update.Visible = true; }

        private void menuDeactivateAuthenticator_Click(object sender, EventArgs e)
        {
            if (currentAccount == null) return;

            DialogResult res = MessageBox.Show("Would you like to remove Steam Guard completely?\nYes - Remove Steam Guard completely.\nNo - Switch back to Email authentication.", "Remove Steam Guard", MessageBoxButtons.YesNoCancel);
            int scheme = 0;
            if (res == DialogResult.Yes)
            {
                scheme = 2;
            }
            else if (res == DialogResult.No)
            {
                scheme = 1;
            }
            else if (res == DialogResult.Cancel) { scheme = 0; }

            if (scheme != 0)
            {
                string confCode = currentAccount.GenerateSteamGuardCode();
                InputForm confirmationDialog = new InputForm(String.Format("Remvoing Steam Guard from {0}. Enter this confirmation code: {1}", currentAccount.AccountName, confCode));
                confirmationDialog.ShowDialog();

                if (confirmationDialog.Canceled) { return; }

                string enteredCode = confirmationDialog.txtBox.Text.ToUpper();
                if (enteredCode != confCode)
                {
                    MessageBox.Show("Confirmation codes do not match. Steam Guard not removed.");
                    return;
                }

                bool success = currentAccount.DeactivateAuthenticator(scheme);
                if (success)
                {
                    MessageBox.Show(String.Format("Steam Guard {0}. " + Manifest.SteamFileExtension + " will be deleted after hitting okay. If you need to make a backup, now's the time.", (scheme == 2 ? "removed completely" : "switched to emails")));
                    this.manifest.RemoveAccount(currentAccount);
                    this.loadAccountsList();
                }
                else
                {
                    MessageBox.Show("Steam Guard failed to deactivate.");
                }
            }
            else { MessageBox.Show("Steam Guard was not removed. No action was taken."); }
        }

        private async void menuRefreshSession_Click(object sender, EventArgs e)
        {
            int RefreshSession_InfoStatus_ = 1;

            bool status = false;
            try { status = await currentAccount.RefreshSessionAsync(); } catch (Exception) { }

            if (status == true)
            {
                MessageBox.Show("Your session has been refreshed.", "Session refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                manifest.SaveAccount(currentAccount, manifest.Encrypted, passKey);
            }
            else
            {
                MessageBox.Show("Failed to refresh your session.\nTry again soon.", "Session refresh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RefreshSession_InfoStatus_ = 0;
            }
            RefreshSession_InfoStatus = RefreshSession_InfoStatus_;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FolderPath = Manifest.GetExecutableDir();
            if (File.Exists(FolderPath + @"\help\Help.html"))
            {
                System.Diagnostics.Process.Start(FolderPath + @"\help\Help.html");
            }
            else { MessageBox.Show("Help file is missing!"); }
        }

        private void checkForUpdates() { System.Diagnostics.Process.Start("https://github.com/hyt47/SteamDesktopAuthenticator-Mod-47"); }

        // Label Btn
        private void btn_labelAutoConfirmTrades_MouseEnter(object sender, EventArgs e) { btn_labelAutoConfirm_Color("Trades", "MouseEnter"); }
        private void btn_labelAutoConfirmTrades_MouseLeave(object sender, EventArgs e) { btn_labelAutoConfirm_Color("Trades", "MouseLeave"); }
        private void btn_labelAutoConfirmMarket_MouseEnter(object sender, EventArgs e) { btn_labelAutoConfirm_Color("Market", "MouseEnter"); }
        private void btn_labelAutoConfirmMarket_MouseLeave(object sender, EventArgs e) { btn_labelAutoConfirm_Color("Market", "MouseLeave"); }

        private void btn_labelAutoConfirmTrades_Click(object sender, EventArgs e)
        {
            if (Settings_ConfirmationsPeriodicChecking == true)
            {

                // Confirm Enable Auto-Confirm Trades
                if (AutoConfirm_Trades == 0)
                {
                    var confirmResult = MessageBox.Show("Warning:\n    - Enabling this will severely reduce the security of your items!\n    - Use of this option is at your own risk.\n\nAre you sure to Enable Auto-confirm Trades ?", "Warning!", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.No) { return; }
                }

                // Enable Auto-Confirm Trades
                if (AutoConfirm_Trades == 0)
                {
                    AutoConfirm_Trades = 1;
                    btn_labelAutoConfirmTrades.Text = "OFF";
                    SetAutoConfirmLabelStatus("Trades", "On");
                    CloseConfirmationPopup();

                    // enable loop > Check for new Confirmations
                    if (Settings_PopupNewConf.Enabled == false) { Settings_PopupNewConf.Enabled = true; }
                }
                else if (AutoConfirm_Trades == 1 || AutoConfirm_Trades == 2)
                {
                    // Disable Auto-Confirm Trades
                    AutoConfirm_Trades = 0;
                    btn_labelAutoConfirmTrades.Text = "ON";
                    SetAutoConfirmLabelStatus("Trades", "Off");

                    // disable loop > Check for new Confirmations
                    if (AutoConfirm_Trades == 0 && AutoConfirm_Market == 0 && Settings_DisplayPopupConfirmation == false) { Settings_PopupNewConf.Enabled = false; }
                }

            }
        }

        private void btn_labelAutoConfirmMarket_Click(object sender, EventArgs e)
        {
            if (Settings_ConfirmationsPeriodicChecking == true)
            {
                // Confirm Enable Auto-Confirm Market
                if (AutoConfirm_Market == 0 && AutoConfirm_Market == 0)
                {
                    var confirmResult = MessageBox.Show("Warning:\n    - Enabling this will severely reduce the security of your items!\n    - Use of this option is at your own risk.\n\nAre you sure to enable Auto-confirm Market Transactions ?", "Warning!", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.No) { return; }
                }

                // Enable Auto-Confirm Market
                if (AutoConfirm_Market == 0)
                {
                    AutoConfirm_Market = 1;
                    btn_labelAutoConfirmMarket.Text = "OFF";
                    SetAutoConfirmLabelStatus("Market", "On");
                    CloseConfirmationPopup();

                    // enable loop > Check for new Confirmations
                    if (Settings_PopupNewConf.Enabled == false) { Settings_PopupNewConf.Enabled = true; }
                }
                else if (AutoConfirm_Market == 1 || AutoConfirm_Market == 2)
                {
                    // Disable Auto-Confirm Market
                    AutoConfirm_Market = 0;
                    btn_labelAutoConfirmMarket.Text = "ON";
                    SetAutoConfirmLabelStatus("Market", "Off");

                    // disable loop > Check for new Confirmations
                    if (AutoConfirm_Trades == 0 && AutoConfirm_Market == 0 && Settings_DisplayPopupConfirmation == false) { Settings_PopupNewConf.Enabled = false; }
                }
            }
        }



        // Tray menu handlers
        //////////////////////////
        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e) { trayRestore_Click(sender, EventArgs.Empty); }
        private void trayRestore_Click(object sender, EventArgs e) { this.Show(); this.WindowState = FormWindowState.Normal; }
        private void trayCopySteamGuard_Click(object sender, EventArgs e) { if (txtLoginToken.Text != "") { Clipboard.SetText(txtLoginToken.Text); } }
        private void trayAccountList_SelectedIndexChanged(object sender, EventArgs e) { listAccounts.SelectedIndex = trayAccountList.SelectedIndex; }

        #endregion // Buttons




        // Misc UI handlers (listAccounts)
        //////////////////////////
        #region Misc UI handlers (listAccounts)
        private async void listAccounts_SelectedValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < allAccounts.Length; i++)
            {
                // Check if index is out of bounds first
                if (i < 0 || listAccounts.SelectedIndex < 0)
                    continue;

                SteamGuardAccount account = allAccounts[i];
                if (account.AccountName == (string)listAccounts.Items[listAccounts.SelectedIndex])
                {
                    trayAccountList.Text = account.AccountName;
                    currentAccount = account;
                    loadAccountInfo();
                    await UpdateCurrentSession();
                    break;
                }
            }
        }
        private void txtAccSearch_TextChanged(object sender, EventArgs e)
        {
            List<string> names = new List<string>(getAllNames());
            names = names.FindAll(new Predicate<string>(IsFilter));

            listAccounts.Items.Clear();
            listAccounts.Items.AddRange(names.ToArray());

            trayAccountList.Items.Clear();
            trayAccountList.Items.AddRange(names.ToArray());
        }
        #endregion // Misc UI handlers (listAccounts)

        // Timer SteamGuard > Check for Confirmations
        //////////////////////////
        #region Timer SteamGuard > Check for Confirmations
        private async void timerSteamGuard_Tick(object sender, EventArgs e)
        {
            //lblStatus.Text = "Aligning time with Steam...";
            Program.ConsoleForm_Update.SetConsoleText("Aligning time with Steam...", "ConsoleStatus_Task");

            try
            {
                steamTime = await TimeAligner.GetSteamTimeAsync();
                //lblStatus.Text = "Aligning time with Steam > Done";
                Program.ConsoleForm_Update.SetConsoleText("Aligning time with Steam > Done", "ConsoleStatus_Return");
            }
            catch (Exception)
            {
                //lblStatus.Text = "Aligning time with Steam > Failed";
                Program.ConsoleForm_Update.SetConsoleText("Aligning time with Steam > Failed", "ConsoleStatus_ReturnWarning");
            }

            currentSteamChunk = steamTime / 30L;
            int secondsUntilChange = (int)(steamTime - (currentSteamChunk * 30L));

            loadAccountInfo();
            if (currentAccount != null) { pbTimeout.Value = 30 - secondsUntilChange; }
        }

        private async void Func_RefreshSession_ForAllAcc()
        {
            Program.ConsoleForm_Update.SetConsoleText("Func_RefreshSession_ForAllAcc", "ConsoleStatus_TaskImportant");

            SteamGuardAccount[] RefreshAccounts = allAccounts;
            int RefreshErr = 0;
            List<SteamAuth.SteamGuardAccount> accounts_refresh_failed = new List<SteamAuth.SteamGuardAccount>();

            foreach (var acc in RefreshAccounts)
            {
                if (acc == null) { return; }
                await AllAcc_UpdateSession(acc);
                if (AllAcc_RefreshSession_InfoStatus == 1) { } else { RefreshErr++; accounts_refresh_failed.Add(acc); }
            }

            // status
            if (RefreshErr == 0)
            {
                MessageBox.Show("Your session has been refreshed for All Accounts.", "Session refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Program.ConsoleForm_Update.SetConsoleText("Refresh All Acc > Your session has been refreshed for All Accounts.", "ConsoleStatus_Return");
            }
            else
            {
                string MsgBox_Text = "";
                SteamGuardAccount[] Failed_RefreshAccounts = accounts_refresh_failed.ToArray();
                foreach (var acc in Failed_RefreshAccounts)
                {
                    Program.ConsoleForm_Update.SetConsoleText("Refresh All Acc > Failed: " + acc.AccountName, "ConsoleStatus_Error");
                    if (MsgBox_Text == "") { } else { MsgBox_Text += "/r/n"; }
                    MsgBox_Text += acc.AccountName;
                }
                MessageBox.Show("Session Refresh failed for:\r\n" + MsgBox_Text + "\r\n\r\nTry again soon.", "Session refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private async Task AllAcc_UpdateSession(SteamGuardAccount account)
        {
            if (account == null) { AllAcc_RefreshSession_InfoStatus = 0; return; };

            Task.Delay(Settings_ConfirmationCheckAllAcc_RefreshSessionDellay).Wait();

            int AllAcc_RefreshSession_InfoStatus_ = 1;
            try { await account.RefreshSessionAsync(); } catch (Exception) { AllAcc_RefreshSession_InfoStatus_ = 0; }

            AllAcc_RefreshSession_InfoStatus = AllAcc_RefreshSession_InfoStatus_;
        }










        // Confirm Trades
        //////////////////////////
        private async void Settings_PopupNewConf_Tick(object sender, EventArgs e)
        {
            if (Code_DellayAtStartup_Sec > 0) { Code_DellayAtStartup_Sec--; return; }

            if (Settings_PopupNewConf_Interval > 0)
            {
                Settings_PopupNewConf_Interval--;
                label_TimerAutoConfirm.Text = Settings_PopupNewConf_Interval.ToString();
                return;
            }
            Settings_PopupNewConf_Interval = DefaultSettings_PopupNewConf_Interval;




            Settings_PopupNewConf.Stop();
            if (currentAccount == null) { return; }

            List<Confirmation> confs = new List<Confirmation>();
            Dictionary<SteamGuardAccount, List<Confirmation>> autoAcceptConfirmations = new Dictionary<SteamGuardAccount, List<Confirmation>>();

            try
            {
                // Check account / acounts
                //////////////////////////
                SteamGuardAccount[] CheckAccounts = null;

                if (Settings_ConfirmationCheckAllAccounts == true || (Settings_SendToWebsitePlanner == true && Secure_ManuallyEnableWebsitePlannerAtMyOwnRisk == 1))
                {
                    // Check all accounts
                    CheckAccounts = allAccounts;
                }
                else
                {
                    // Check only the selected account
                    List<SteamAuth.SteamGuardAccount> accounts_to_check = new List<SteamAuth.SteamGuardAccount>();
                    accounts_to_check.Add(currentAccount);
                    CheckAccounts = accounts_to_check.ToArray();
                }



                // Check Confirmations >>> Using Planner
                //##############################################################
                #region Planner
                if (Settings_SendToWebsitePlanner == true && Secure_ManuallyEnableWebsitePlannerAtMyOwnRisk == 1)
                {

                    lblStatus.Text = "Running Planner...";
                    Program.ConsoleForm_Update.SetConsoleText(" ", "");
                    Program.ConsoleForm_Update.SetConsoleText("Running Planner...", "ConsoleStatus_TaskImportant");


                    #region foreach account check planner
                    foreach (var acc in CheckAccounts)
                    {
                        if (acc == null) { return; }
                        if (RefreshSession_InfoStatus == 1)
                        {
                            lblStatus.Text = "Planner > End > Refresh S = 1";
                            Program.ConsoleForm_Update.SetConsoleText("Planner > End > Refresh S = 1", "ConsoleStatus_TaskImportant");
                        }
                        else
                        {
                            await UpdateCurrentSession_ForBg(acc, "UseDellay"); // refresh session ??
                            if (RefreshSession_InfoStatus == 1)
                            {
                                lblStatus.Text = "Planner > End > Refresh S = 1";
                                Program.ConsoleForm_Update.SetConsoleText("Planner > End > Refresh S = 1", "ConsoleStatus_TaskImportant");
                            }
                        }



                        var CountDetected_Autoconfirm_Market = 0;
                        var CountDetected_Autoconfirm_Trades = 0;
                        var CountDetected_Popup_Trades_Market = 0;

                        int AutoAcceptingBatch_Status = 0;




                        // Planner
                        var User_SteamId64 = acc.Session.SteamID.ToString();
                        string Enc_User_SteamId64 = Post_WebPlanner.EncryptDecrypt_Shift(User_SteamId64, Settings_WebsitePlannerShift1, Settings_WebsitePlannerShift2, Settings_WebsitePlannerShift3, Settings_WebsitePlannerShift4, Settings_WebsitePlannerShift5);


                        var AuthenticatorCode = "none";
                        TargetAccount = acc;
                        if (Settings_WebsitePlannerSendCode == true) { AuthenticatorCode = get_Account_Code(); }
                        if (AuthenticatorCode == null || AuthenticatorCode == "") { AuthenticatorCode = "failed"; }
                        string Enc_AuthenticatorCode = AuthenticatorCode;
                        if (AuthenticatorCode == "none" || AuthenticatorCode == "failed") { }
                        else
                        {
                            Enc_AuthenticatorCode = Post_WebPlanner.EncryptDecrypt_Shift(AuthenticatorCode, Settings_WebsitePlannerShift1, Settings_WebsitePlannerShift2, Settings_WebsitePlannerShift3, Settings_WebsitePlannerShift4, Settings_WebsitePlannerShift5);
                        }


                        Program.ConsoleForm_Update.SetConsoleText("Web Planner: " + acc.AccountName + " " + User_SteamId64 + " " + AuthenticatorCode, "ConsoleStatus_Info");

                        var PlannerReturn = "empty";
                        if (RefreshSession_InfoStatus == 0)
                        {
                            PlannerReturn = Post_WebPlanner.SendPostData_ToWebPlanner(Settings_WebsitePlannerAddress, Enc_User_SteamId64, Enc_AuthenticatorCode);

                            lblStatus.Text = "Running Planner > www";
                            Program.ConsoleForm_Update.SetConsoleText("Running Planner > www", "ConsoleStatus_TaskImportant");
                        }

                        // decrypt response
                        if (PlannerReturn == "empty" || PlannerReturn == "empty 1" || PlannerReturn == "empty 2" || PlannerReturn == "empty 3" || PlannerReturn == "Sending post > Failed") { }
                        else
                        {
                            // make the number negative
                            int Shift1 = Settings_WebsitePlannerShift1 * -1; int Shift2 = Settings_WebsitePlannerShift2 * -1;
                            int Shift3 = Settings_WebsitePlannerShift3 * -1; int Shift4 = Settings_WebsitePlannerShift4 * -1;
                            int Shift5 = Settings_WebsitePlannerShift5 * -1;
                            PlannerReturn = Post_WebPlanner.EncryptDecrypt_Shift(PlannerReturn, Shift1, Shift2, Shift3, Shift4, Shift5);
                        }
                        Program.ConsoleForm_Update.SetConsoleText("Web Planner return: " + PlannerReturn, "ConsoleStatus_Info");

                        if (PlannerReturn == "empty" || PlannerReturn == "empty 1" || PlannerReturn == "empty 2" || PlannerReturn == "empty 3" || PlannerReturn == "Sending post > Failed")
                        {
                        }
                        else
                        {
                            // Declare
                            string ReadReturn_SteamId64 = "NoSteamID64";
                            string ReadReturn_RunMode = "2"; // 1 = normal // 2 = use planner
                            string ReadReturn_CheckForConfirmations = "0";
                            string ReadReturn_AutoConfirmTrades = "0";
                            string ReadReturn_AutoConfirmMarket = "0";

                            // Read String Data
                            // SteamId64=000|ReadReturn_CheckForConfirmations=1|AutoConfirmTrades=0|AutoConfirmMarket=1
                            string[] PlannerReturn_arr1 = PlannerReturn.Split('|');
                            foreach (string PlannerReturn_Pair in PlannerReturn_arr1)
                            {
                                string[] PlannerReturn_arr2 = PlannerReturn_Pair.Split('=');
                                string RKey = PlannerReturn_arr2[0];
                                string RVal = PlannerReturn_arr2[1];

                                if (RKey == "SteamId64") { ReadReturn_SteamId64 = RVal; }
                                else if (RKey == "AutoConfirmTrades") { ReadReturn_AutoConfirmTrades = RVal; }
                                else if (RKey == "AutoConfirmMarket") { ReadReturn_AutoConfirmMarket = RVal; }
                                else if (RKey == "CheckForConfirmations") { ReadReturn_CheckForConfirmations = RVal; }
                                else if (RKey == "RunMode") { ReadReturn_RunMode = RVal; }
                                else if (RKey == "PlannerInterval")
                                {
                                    string ReadReturn_PlannerInterval_ = RVal;
                                    int ReadReturn_PlannerInterval; bool isNumeric = int.TryParse(ReadReturn_PlannerInterval_, out ReadReturn_PlannerInterval);
                                    if (isNumeric)
                                    {
                                        if (ReadReturn_PlannerInterval > 1000) { } else { ReadReturn_PlannerInterval = 1000; }
                                        if (ReadReturn_PlannerInterval != DefaultSettings_PopupNewConf_Planner_Interval)
                                        {
                                            Settings_PopupNewConf_Interval = ReadReturn_PlannerInterval;
                                            DefaultSettings_PopupNewConf_Planner_Interval = ReadReturn_PlannerInterval;
                                        }
                                    }
                                }
                            }

                            // Continue
                            if (User_SteamId64.ToString() == ReadReturn_SteamId64)
                            {

                                // UPDATE GUI
                                if (ReadReturn_AutoConfirmTrades == "1")
                                {
                                    AutoConfirm_Trades = 2;
                                    btn_labelAutoConfirmTrades.Text = "OFF"; btn_labelAutoConfirm_Color("Trades", "MouseLeave");
                                }
                                else
                                {
                                    AutoConfirm_Trades = 0;
                                    btn_labelAutoConfirmTrades.Text = "ON"; btn_labelAutoConfirm_Color("Trades", "MouseLeave");
                                    SetAutoConfirmLabelStatus("Trades", "Off");
                                }

                                if (ReadReturn_AutoConfirmTrades == "1")
                                {
                                    AutoConfirm_Market = 2;
                                    btn_labelAutoConfirmMarket.Text = "OFF"; btn_labelAutoConfirm_Color("Market", "MouseLeave");
                                }
                                else
                                {
                                    AutoConfirm_Market = 0;
                                    btn_labelAutoConfirmMarket.Text = "ON"; btn_labelAutoConfirm_Color("Market", "MouseLeave");
                                    SetAutoConfirmLabelStatus("Market", "Off");
                                }

                                // run mode
                                int GoCheckConfirmations = 0;
                                if (ReadReturn_RunMode == "2")
                                {
                                    GoCheckConfirmations = 1;
                                    Settings_PopupNewConf_Planner_Interval_Tick = DefaultSettings_PopupNewConf_Interval; // restore data in case the timer is not 100%
                                }
                                else
                                {
                                    // use default timer
                                    if (0 >= Settings_PopupNewConf_Planner_Interval_Tick)
                                    {
                                        GoCheckConfirmations = 1; Settings_PopupNewConf_Planner_Interval_Tick = DefaultSettings_PopupNewConf_Interval;
                                    }
                                    else { GoCheckConfirmations = 0; Settings_PopupNewConf_Planner_Interval_Tick = Settings_PopupNewConf_Planner_Interval_Tick - DefaultSettings_PopupNewConf_Planner_Interval; }
                                }

                                // continue
                                if (ReadReturn_CheckForConfirmations == "1" && GoCheckConfirmations == 1 && RefreshSession_InfoStatus == 0)
                                {

                                    lblStatus.Text = "Checking confirmations...";
                                    Program.ConsoleForm_Update.SetConsoleText("Checking confirmations started...", "ConsoleStatus_TaskImportant");

                                    try
                                    {
                                        Confirmation[] tmp = await currentAccount.FetchConfirmationsAsync();
                                        foreach (var conf in tmp)
                                        {
                                            //MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
                                            if (conf.ConfType == Confirmation.ConfirmationType.MarketSellTransaction && ReadReturn_AutoConfirmMarket == "1")
                                            {
                                                AutoAcceptingBatch_Status = 1;
                                                //Program.ConsoleForm_Update.SetConsoleText("+ Add to Auto-confirming Market BATCH: " + acc.AccountName + " Market > " + conf.Description.ToString() + " > ID: " + conf.ID.ToString(), "ConsoleStatus_Task");
                                                CountDetected_Autoconfirm_Market++;
                                                if (!autoAcceptConfirmations.ContainsKey(acc)) { autoAcceptConfirmations[acc] = new List<Confirmation>(); }
                                                autoAcceptConfirmations[acc].Add(conf);
                                            }

                                            //TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT
                                            else if (conf.ConfType == Confirmation.ConfirmationType.Trade && ReadReturn_AutoConfirmTrades == "1")
                                            {
                                                AutoAcceptingBatch_Status = 1;
                                                //Program.ConsoleForm_Update.SetConsoleText("+ Add to Auto-confirming Trade BATCH: " + acc.AccountName + " Trade > " + conf.Description.ToString() + " > ID: " + conf.ID.ToString(), "ConsoleStatus_Task");
                                                CountDetected_Autoconfirm_Trades++;
                                                if (!autoAcceptConfirmations.ContainsKey(acc)) { autoAcceptConfirmations[acc] = new List<Confirmation>(); }
                                                autoAcceptConfirmations[acc].Add(conf);
                                            }
                                        }

                                        // Info // Acc End
                                        //------------------------------------------------------------------------------------------------------
                                        #region Info
                                        if (CountDetected_Autoconfirm_Market > 0 && CountDetected_Autoconfirm_Trades > 0)
                                        {
                                            lblStatus.Text = "Auto-confirming Trade & Market... " + acc.AccountName;
                                            Program.ConsoleForm_Update.SetConsoleText("To Confirm: " + acc.AccountName + ", Trades: " + CountDetected_Autoconfirm_Trades + ", Market: " + CountDetected_Autoconfirm_Market, "ConsoleStatus_Info");
                                        }
                                        else if (CountDetected_Autoconfirm_Market > 0)
                                        {
                                            lblStatus.Text = "Auto-confirming Market... " + acc.AccountName;
                                            Program.ConsoleForm_Update.SetConsoleText("To Confirm: " + acc.AccountName + ", Market: " + CountDetected_Autoconfirm_Market, "ConsoleStatus_Info");
                                        }
                                        else if (CountDetected_Autoconfirm_Trades > 0)
                                        {
                                            lblStatus.Text = "Auto-confirming Trade... " + acc.AccountName;
                                            Program.ConsoleForm_Update.SetConsoleText("To Confirm: " + acc.AccountName + ", Trades: " + CountDetected_Autoconfirm_Trades, "ConsoleStatus_Info");
                                        }
                                        #endregion //Info

                                        // Auto Confirm // Acc End
                                        //+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
                                        if (autoAcceptConfirmations.Count > 0)
                                        {
                                            foreach (var this_acc in autoAcceptConfirmations.Keys)
                                            {
                                                Program.ConsoleForm_Update.SetConsoleText("Auto-confirming BATCH account: " + this_acc.AccountName, "ConsoleStatus_Confirmed");
                                                var confirmations = autoAcceptConfirmations[this_acc].ToArray();
                                                this_acc.AcceptMultipleConfirmations(confirmations);
                                            }
                                            autoAcceptConfirmations.Clear(); // Reset Dictionary after the data has been used
                                        }

                                        Program.ConsoleForm_Update.SetConsoleText(" ", "ConsoleStatus_Info"); // empty line
                                    }
                                    catch (SteamGuardAccount.WGTokenInvalidException)
                                    {
                                        lblStatus.Text = "Failed > Refreshing session";
                                        Program.ConsoleForm_Update.SetConsoleText("Check Confirmations > Failed > Refreshing session", "ConsoleStatus_ReturnFaildFixIt");

                                        await currentAccount.RefreshSessionAsync(); //Don't save it to the HDD, of course. We'd need their encryption passkey again.
                                        lblStatus.Text = "Refreshing session > Done";
                                        Program.ConsoleForm_Update.SetConsoleText("Refreshing session > Done", "ConsoleStatus_Return");
                                    }
                                    catch (SteamGuardAccount.WGTokenExpiredException)
                                    {
                                        //Prompt to relogin
                                        PromptRefreshLogin(currentAccount);
                                        break; //Don't bombard a user with login refresh requests if they have multiple accounts. Give them a few seconds to disable the autocheck option if they want.
                                    }
                                    catch (WebException) { }
                                }
                            }
                        }
                    }
                    #endregion // foreach account check planner
                    lblStatus.Text = "Planner > End";
                }
                #endregion //End Planner
                //##############################################################
                // Check Confirmations >>> Using Planner END



                // Check Confirmations >>> Using The App Timer
                //##############################################################
                #region // Normal Check
                else
                {
                    lblStatus.Text = "Checking confirmations...";
                    Program.ConsoleForm_Update.SetConsoleText(" ", "");
                    Program.ConsoleForm_Update.SetConsoleText("Checking confirmations started...", "ConsoleStatus_TaskImportant");

                    #region foreach

                    // info
                    Program.ConsoleForm_Update.SetConsoleText("Checking confirmations running...", "ConsoleStatus_Info");

                    foreach (var acc in CheckAccounts)
                    {
                        if (acc == null) { return; }
                        if (RefreshSession_InfoStatus == 1)
                        {
                            lblStatus.Text = "Check Conf > End > Refresh S = 1";
                            Program.ConsoleForm_Update.SetConsoleText("Check Conf > End > Refresh S = 1", "ConsoleStatus_TaskImportant");

                        }
                        else
                        {
                            await UpdateCurrentSession_ForBg(acc, "UseDellay"); // refresh session ??
                            if (RefreshSession_InfoStatus == 1)
                            {
                                lblStatus.Text = "Check Conf > End > Refresh S = 1";
                                Program.ConsoleForm_Update.SetConsoleText("Check Conf > End > Refresh S = 1", "ConsoleStatus_TaskImportant");
                            }
                        }

                        var CountDetected_Autoconfirm_Market = 0;
                        var CountDetected_Autoconfirm_Trades = 0;
                        var CountDetected_Popup_Trades_Market = 0;

                        int AutoAcceptingBatch_Status = 0;

                        try
                        {
                            if (RefreshSession_InfoStatus == 0 && (AutoConfirm_Market == 1 || AutoConfirm_Trades == 1 || Settings_DisplayPopupConfirmation))
                            {

                                // info
                                Program.ConsoleForm_Update.SetConsoleText("Checking confirmations account: " + acc.AccountName, "ConsoleStatus_TaskImportant");

                                Confirmation[] tmp = await currentAccount.FetchConfirmationsAsync();
                                #region foreach
                                foreach (var conf in tmp)
                                {

                                    string TradeWith = "";
                                    string ConfirmationType = "Unknown Type";

                                    if (conf.ConfType == Confirmation.ConfirmationType.MarketSellTransaction) { ConfirmationType = "Market"; }
                                    else if (conf.ConfType == Confirmation.ConfirmationType.Trade) { ConfirmationType = "Trade"; }

                                    // OLD CODE > used when I didn't have user name
                                    TradeWith = ConfirmationType;
                                    if (ConfirmationType == "Trade") { TradeWith += " offer ID: " + conf.ID; }
                                    else if (ConfirmationType == "Market") { TradeWith += " ID: " + conf.ID; }

                                    // NEW CODE
                                    if (ConfirmationType == "Trade") { TradeWith = "Trade: " + conf.OtherUserName; }
                                    else if (ConfirmationType == "Market") { TradeWith = "Sell: " + conf.OtherUserName; }

                                    Program.ConsoleForm_Update.SetConsoleText("Confirmation Detected " + acc.AccountName + " > " + TradeWith + " > ID: " + conf.ID.ToString(), "ConsoleStatus_Info");

                                    //MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
                                    if (conf.ConfType == Confirmation.ConfirmationType.MarketSellTransaction && AutoConfirm_Market == 1)
                                    {
                                        AutoAcceptingBatch_Status = 1;
                                        //Program.ConsoleForm_Update.SetConsoleText("+ Add to Auto-confirming Market BATCH: " + acc.AccountName + " Market > " + conf.Description.ToString() + " > ID: " + conf.ID.ToString(), "ConsoleStatus_Task");
                                        CountDetected_Autoconfirm_Market++;
                                        if (!autoAcceptConfirmations.ContainsKey(acc)) { autoAcceptConfirmations[acc] = new List<Confirmation>(); }
                                        autoAcceptConfirmations[acc].Add(conf);
                                    }

                                    //TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT
                                    else if (conf.ConfType == Confirmation.ConfirmationType.Trade && AutoConfirm_Trades == 1)
                                    {
                                        AutoAcceptingBatch_Status = 1;
                                        //Program.ConsoleForm_Update.SetConsoleText("+ Add to Auto-confirming Trade BATCH: " + acc.AccountName + " Trade > " + conf.Description.ToString() + " > ID: " + conf.ID.ToString(), "ConsoleStatus_Task");
                                        CountDetected_Autoconfirm_Trades++;
                                        if (!autoAcceptConfirmations.ContainsKey(acc)) { autoAcceptConfirmations[acc] = new List<Confirmation>(); }
                                        autoAcceptConfirmations[acc].Add(conf);
                                    }

                                    //PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP
                                    else if (Settings_DisplayPopupConfirmation && popupFrm.Visible == false)
                                    {
                                        if ((conf.ConfType == Confirmation.ConfirmationType.MarketSellTransaction && AutoConfirm_Market == 0) || (conf.ConfType == Confirmation.ConfirmationType.Trade && AutoConfirm_Trades == 0))
                                        {
                                            confs.Add(conf);
                                            CountDetected_Popup_Trades_Market++;
                                            //Program.ConsoleForm_Update.SetConsoleText("+ " + acc.AccountName + " > add to POPUP Confirmation: " + conf.Description.ToString(), "ConsoleStatus_Info");
                                            //Program.ConsoleForm_Update.SetConsoleText("POPUP INFO ID: " + conf.ID.ToString(), "ConsoleStatus_Info");
                                            //Program.ConsoleForm_Update.SetConsoleText("POPUP INFO Key: " + conf.Key.ToString(), "ConsoleStatus_Info");
                                        }
                                    }

                                } // foreach (var conf in tmp){ // END
                                #endregion // foreach

                                // Info // Acc End
                                //------------------------------------------------------------------------------------------------------
                                #region Info
                                if (CountDetected_Autoconfirm_Market > 0 && CountDetected_Autoconfirm_Trades > 0)
                                {
                                    lblStatus.Text = "Auto-confirming Trade & Market... " + acc.AccountName;
                                    Program.ConsoleForm_Update.SetConsoleText("To Confirm: " + acc.AccountName + ", Trades: " + CountDetected_Autoconfirm_Trades + ", Market: " + CountDetected_Autoconfirm_Market, "ConsoleStatus_Info");
                                }
                                else if (CountDetected_Autoconfirm_Market > 0)
                                {
                                    lblStatus.Text = "Auto-confirming Market... " + acc.AccountName;
                                    Program.ConsoleForm_Update.SetConsoleText("To Confirm: " + acc.AccountName + ", Market: " + CountDetected_Autoconfirm_Market, "ConsoleStatus_Info");
                                }
                                else if (CountDetected_Autoconfirm_Trades > 0)
                                {
                                    lblStatus.Text = "Auto-confirming Trade... " + acc.AccountName;
                                    Program.ConsoleForm_Update.SetConsoleText("To Confirm: " + acc.AccountName + ", Trades: " + CountDetected_Autoconfirm_Trades, "ConsoleStatus_Info");
                                }
                                if (CountDetected_Popup_Trades_Market > 0) { Program.ConsoleForm_Update.SetConsoleText("To Show Popups: " + acc.AccountName + ", No: " + CountDetected_Popup_Trades_Market, "ConsoleStatus_Info"); }
                                #endregion //Info


                                // Auto Confirm // Acc End
                                //+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
                                if (autoAcceptConfirmations.Count > 0)
                                {
                                    foreach (var this_acc in autoAcceptConfirmations.Keys)
                                    {
                                        Program.ConsoleForm_Update.SetConsoleText("Auto-confirming BATCH account: " + this_acc.AccountName, "ConsoleStatus_Confirmed");
                                        var confirmations = autoAcceptConfirmations[this_acc].ToArray();
                                        this_acc.AcceptMultipleConfirmations(confirmations);
                                    }
                                    autoAcceptConfirmations.Clear(); // Reset Dictionary after the data has been used
                                }


                                // Show Confirmation Popup
                                //=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P=P
                                // if another popup is not visible
                                // if the app is not auto confirming for this account // sending double confirmations at the same time 3can cause the confirmation to fail
                                if (confs.Count > 0)
                                {
                                    if (popupFrm.Visible == false && AutoAcceptingBatch_Status == 0 && acc != null)
                                    {
                                        Program.ConsoleForm_Update.SetConsoleText("Show Popup Confirmation", "ConsoleStatus_TaskImportant");

                                        popupFrm.ConfirmationsPopup = confs.ToArray();
                                        popupFrm.Popup();
                                        popupFrm.ConfirmationsPopup_ForAcc = acc; // added after the form is shown so it will update the text in the GUI
                                    }
                                    confs.Clear(); // Reset Dictionary >> so that this variable can be used by other account
                                }

                                Program.ConsoleForm_Update.SetConsoleText(" ", "ConsoleStatus_Info"); // empty line
                            }
                        }
                        catch (SteamGuardAccount.WGTokenInvalidException)
                        {
                            lblStatus.Text = "Failed > Refreshing session";
                            Program.ConsoleForm_Update.SetConsoleText("Check Confirmations > Failed > Refreshing session", "ConsoleStatus_ReturnFaildFixIt");

                            await currentAccount.RefreshSessionAsync(); //Don't save it to the HDD, of course. We'd need their encryption passkey again.
                            lblStatus.Text = "Refreshing session > Done";
                            Program.ConsoleForm_Update.SetConsoleText("Refreshing session > Done", "ConsoleStatus_Return");
                        }
                        catch (SteamGuardAccount.WGTokenExpiredException)
                        {
                            //Prompt to relogin
                            PromptRefreshLogin(currentAccount);
                            break; //Don't bombard a user with login refresh requests if they have multiple accounts. Give them a few seconds to disable the autocheck option if they want.
                        }
                        catch (WebException) { }
                    } // Foreach account End
                    #endregion // foreach
                    lblStatus.Text = "Checking confirmations > END";
                }
                #endregion // Normal Check
                //##############################################################
                // Check Confirmations >>> Using The App Timer



            }
            catch (SteamGuardAccount.WGTokenInvalidException)
            {
                lblStatus.Text = "Checking confirmations > Failed";
                Program.ConsoleForm_Update.SetConsoleText("Checking confirmations > Failed", "ConsoleStatus_ReturnWarning");
            }

            Settings_PopupNewConf.Start();
        }

        #endregion //Timer SteamGuard > Check for Confirmations

        // Other methods

        /// <summary>
        /// Refresh this account's session data using their OAuth Token
        /// </summary>
        /// <param name="account">The account to refresh</param>
        /// <param name="attemptRefreshLogin">Whether or not to prompt the user to re-login if their OAuth token is expired.</param>
        /// <returns></returns>
        private async Task<bool> RefreshAccountSession(SteamGuardAccount account, bool attemptRefreshLogin = true)
        {
            if (account == null) return false;

            try
            {
                bool refreshed = await account.RefreshSessionAsync();
                return refreshed; //No exception thrown means that we either successfully refreshed the session or there was a different issue preventing us from doing so.
            }
            catch (SteamGuardAccount.WGTokenExpiredException)
            {
                if (!attemptRefreshLogin) return false;

                PromptRefreshLogin(account);

                return await RefreshAccountSession(account, false);
            }
        }

        /// <summary>
        /// Display a login form to the user to refresh their OAuth Token
        /// </summary>
        /// <param name="account">The account to refresh</param>
        private void PromptRefreshLogin(SteamGuardAccount account)
        {
            var loginForm = new LoginForm(LoginForm.LoginType.Refresh, account);
            loginForm.ShowDialog();
        }

        // Other Functions
        //////////////////////////
        #region Other Functions

        private void btn_labelAutoConfirm_Color(string AffectBtn, string Function)
        {
            if (AffectBtn == "Trades")
            {
                if (Function == "MouseEnter") { btn_labelAutoConfirmTrades.BackColor = Color.SlateBlue; btn_labelAutoConfirmTrades.ForeColor = Color.White; }
                if (Function == "MouseLeave") { btn_labelAutoConfirmTrades.BackColor = Color.SlateGray; btn_labelAutoConfirmTrades.ForeColor = Color.White; }
                if (Function == "Disabled") { btn_labelAutoConfirmTrades.BackColor = Color.DimGray; btn_labelAutoConfirmTrades.ForeColor = Color.Black; }
            }
            if (AffectBtn == "Market")
            {
                if (Function == "MouseEnter") { btn_labelAutoConfirmMarket.BackColor = Color.SlateBlue; btn_labelAutoConfirmMarket.ForeColor = Color.White; }
                if (Function == "MouseLeave") { btn_labelAutoConfirmMarket.BackColor = Color.SlateGray; btn_labelAutoConfirmMarket.ForeColor = Color.White; }
                if (Function == "Disabled") { btn_labelAutoConfirmMarket.BackColor = Color.DimGray; btn_labelAutoConfirmMarket.ForeColor = Color.Black; }
            }
        }

        public void SetAutoConfirmLabelStatus(string Target, string Function)
        {
            // Trades
            if (Target == "Trades")
            {
                if (Function == "On")
                {
                    labelAutoConfirmTrades.Text = "Auto-confirm Trades: yes";
                    labelAutoConfirmTrades.ForeColor = Color.Lime; /*Text color*/ labelAutoConfirmTrades.BackColor = Color.Black; /*bg color*/
                }
                if (Function == "Off")
                {
                    labelAutoConfirmTrades.Text = "Auto-confirm Trades: no";
                    labelAutoConfirmTrades.ForeColor = Color.DarkGray; /*Text color*/ labelAutoConfirmTrades.BackColor = Color.Black; /*bg color*/
                }
                if (Function == "Delay") { labelAutoConfirmTrades.ForeColor = Color.Yellow; /*Text color*/ labelAutoConfirmTrades.BackColor = Color.Black; /*bg color*/ }
            }

            // Market
            if (Target == "Market")
            {
                if (Function == "On")
                {
                    labelAutoConfirmMarket.Text = "Auto-confirm Market: yes";
                    labelAutoConfirmMarket.ForeColor = Color.Lime; /*Text color*/ labelAutoConfirmMarket.BackColor = Color.Black; /*bg color*/
                }
                if (Function == "Off")
                {
                    labelAutoConfirmMarket.Text = "Auto-confirm Market: no";
                    labelAutoConfirmMarket.ForeColor = Color.DarkGray; /*Text color*/ labelAutoConfirmMarket.BackColor = Color.Black; /*bg color*/
                }
                if (Function == "Delay") { labelAutoConfirmMarket.ForeColor = Color.Yellow; /*Text color*/ labelAutoConfirmMarket.BackColor = Color.Black; /*bg color*/ }
            }
        }


        private void CloseConfirmationPopup()
        {
            try
            {
                TradePopupForm PopupFormToClose = (TradePopupForm)Application.OpenForms["TradePopupForm"];
                PopupFormToClose.Close();
            }
            catch (NullReferenceException ex) { /*form is not opened*/ }
            catch { }
        }

        /// <summary>
        /// Load UI with the current account info, this is run every second
        /// </summary>
        private void loadAccountInfo()
        {
            if (currentAccount != null && steamTime != 0)
            {
                txtLoginToken.Text = currentAccount.GenerateSteamGuardCodeForTime(steamTime);
                groupAccount.Text = "Account: " + currentAccount.AccountName;
                accountToolStripMenuItem.Text = "Account: " + currentAccount.AccountName;
            }
        }
        private string get_Account_Code()
        {
            if (TargetAccount != null && steamTime != 0) { return TargetAccount.GenerateSteamGuardCodeForTime(steamTime); }
            return null;
        }

        /// <summary>
        /// Decrypts files and populates list UI with accounts
        /// </summary>
        private void loadAccountsList()
        {
            currentAccount = null;

            listAccounts.Items.Clear();
            listAccounts.SelectedIndex = -1;

            trayAccountList.Items.Clear();
            trayAccountList.SelectedIndex = -1;


            if (manifest.Encrypted)
            {
                menuManageEncryption.Text = "Manage Encryption " + Manifest.Get_FileEncryption_Version();

                importAccountToolStripMenuItem.Enabled = false;
                importAccountToolStripMenuItem.Text = "Import Account - Disable the Encryption First";
            }
            else
            {
                menuManageEncryption.Text = "Setup Encryption " + Manifest.Get_FileEncryption_Version();

                importAccountToolStripMenuItem.Enabled = true;
                importAccountToolStripMenuItem.Text = "Import Account";
            }

            menuManageEncryption.Enabled = manifest.Entries.Count > 0;



            allAccounts = manifest.GetAllAccounts(passKey);

            if (allAccounts.Length > 0)
            {
                for (int i = 0; i < allAccounts.Length; i++)
                {
                    SteamGuardAccount account = allAccounts[i];
                    listAccounts.Items.Add(account.AccountName);
                    trayAccountList.Items.Add(account.AccountName);
                }

                listAccounts.SelectedIndex = 0;
                trayAccountList.SelectedIndex = 0;
            }
            menuDeactivateAuthenticator.Enabled = btnTradeConfirmations.Enabled = btnTradeConfirmationsList.Enabled = menuLoginAgain.Enabled = menuRefreshSession.Enabled = btn_forceSessionRefreshForAllAccounts_ToolStripMenuItem.Enabled = menuRemoveAccountFromManifest.Enabled = menuDeactivateAuthenticator.Enabled = allAccounts.Length > 0;

        }

        /// <summary>
        /// Reload the session of the current account
        /// </summary>
        /// <returns></returns>

        // Session Update on account auto check for confirmations
        private async Task UpdateCurrentSession_ForBg(SteamGuardAccount account, string UseDellay = "NoDellay")
        {
            if (account == null) { RefreshSession_InfoStatus = 0; return; };
            if (updatedSessions.Contains(account.AccountName)) { RefreshSession_InfoStatus = 0; return; }

            if (UseDellay == "UseDellay") { Task.Delay(Settings_ConfirmationCheckAllAcc_RefreshSessionDellay).Wait(); }

            int RefreshSession_InfoStatus_ = 1;
            Program.ConsoleForm_Update.SetConsoleText("Auto check for Confirmations > " + account.AccountName + " > Refreshing session...", "ConsoleStatus_Task");
            try
            {
                await account.RefreshSessionAsync();
                if (updatedSessions.Contains(account.AccountName) == false) { updatedSessions.Add(account.AccountName); }
                Program.ConsoleForm_Update.SetConsoleText("Auto check for Confirmations > " + account.AccountName + " > Refreshing session > Done", "ConsoleStatus_Return");
            }
            catch (Exception)
            {
                RefreshSession_InfoStatus_ = 0;
                Program.ConsoleForm_Update.SetConsoleText("Auto check for Confirmations > " + account.AccountName + " > Refreshing session > Failed", "ConsoleStatus_ReturnWarning");
            }

            RefreshSession_InfoStatus = RefreshSession_InfoStatus_;
        }


        // Session Update on account select
        private async Task UpdateCurrentSession() { await UpdateSession(currentAccount); }
        private async Task UpdateSession(SteamGuardAccount account)
        {
            RefreshSession_InfoStatus = 1;

            if (account == null) { RefreshSession_InfoStatus = 0; return; }
            if (updatedSessions.Contains(account.AccountName)) { RefreshSession_InfoStatus = 0; return; }

            lblStatus.Text = "Refreshing session...";
            Program.ConsoleForm_Update.SetConsoleText("Refreshing session...", "ConsoleStatus_Task");
            btnTradeConfirmations.Enabled = false;
            btnTradeConfirmationsList.Enabled = false;
            labelDisableListClick.Visible = true;

            try
            {
                await currentAccount.RefreshSessionAsync();
                if (updatedSessions.Contains(account.AccountName) == false) { updatedSessions.Add(account.AccountName); }
                lblStatus.Text = "Refreshing session > Done";
                Program.ConsoleForm_Update.SetConsoleText("Refreshing session > Done", "ConsoleStatus_Return");
            }
            catch (Exception)
            {
                lblStatus.Text = "Refreshing session > Failed";
                Program.ConsoleForm_Update.SetConsoleText("Refreshing session > Failed", "ConsoleStatus_ReturnWarning");
            }

            btnTradeConfirmations.Enabled = true;
            btnTradeConfirmationsList.Enabled = true;
            labelDisableListClick.Visible = false;

            RefreshSession_InfoStatus = 0;
        }
        private void listAccounts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    int to = listAccounts.SelectedIndex - (e.KeyCode == Keys.Up ? 1 : -1);
                    manifest.MoveEntry(listAccounts.SelectedIndex, to);
                    loadAccountsList();
                }
                return;
            }

            if (!IsKeyAChar(e.KeyCode) && !IsKeyADigit(e.KeyCode)) { return; }

            txtAccSearch.Focus();
            txtAccSearch.Text = e.KeyCode.ToString();
            txtAccSearch.SelectionStart = 1;
        }

        private static bool IsKeyAChar(Keys key) { return key >= Keys.A && key <= Keys.Z; }

        private static bool IsKeyADigit(Keys key) { return (key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9); }

        private bool IsFilter(string f)
        {
            if (txtAccSearch.Text.StartsWith("~"))
            {
                try { return Regex.IsMatch(f, txtAccSearch.Text); } catch (Exception) { return true; }
            }
            else { return f.Contains(txtAccSearch.Text); }
        }

        private string[] getAllNames()
        {
            string[] itemArray = new string[allAccounts.Length];
            for (int i = 0; i < itemArray.Length; i++) { itemArray[i] = allAccounts[i].AccountName; }
            return itemArray;
        }

        #endregion // Other Functions



        // Load Settings
        ////////////////////
        private void loadSettings()
        {
            label_TimerAutoConfirm.Text = "";

            this.Invoke(new Action(() => { Program.ConsoleForm_Update.SetConsoleText(" ", "ConsoleStatus_Task"); }));
            this.Invoke(new Action(() => { Program.ConsoleForm_Update.SetConsoleText(" ", "ConsoleStatus_Task"); }));
            this.Invoke(new Action(() => { Program.ConsoleForm_Update.SetConsoleText(">>> Loading Settings", "ConsoleStatus_Info"); }));
            this.Invoke(new Action(() => { Program.ConsoleForm_Update.SetConsoleText(" ", "ConsoleStatus_Task"); }));
            this.Invoke(new Action(() => { Program.ConsoleForm_Update.SetConsoleText(" ", "ConsoleStatus_Task"); }));


            // Send to Planner
            #region Send App Status
            Secure_ManuallyEnableWebsitePlannerAtMyOwnRisk = Manifest.ManuallyEnableWebsitePlannerAtMyOwnRisk; // Secure Settings
            Secure_ManuallySetWebsitePlannerAddress = Manifest.ManuallySetWebsitePlannerAddress; // Secure Settings

            if (manifest.SendToWebsitePlanner == true && Secure_ManuallyEnableWebsitePlannerAtMyOwnRisk == 1)
            {
                Settings_SendToWebsitePlanner = true;

                if (Secure_ManuallySetWebsitePlannerAddress == "" || Secure_ManuallySetWebsitePlannerAddress == null) { } else { Settings_WebsitePlannerAddress = Secure_ManuallySetWebsitePlannerAddress; }

                Settings_WebsitePlannerInterval = manifest.WebsitePlannerInterval;
                if (Settings_WebsitePlannerInterval >= 1 && 9999 >= Settings_WebsitePlannerInterval) { } else { Settings_WebsitePlannerInterval = 1; }

                Settings_WebsitePlannerSendCode = manifest.WebsitePlannerSendCode;

                Settings_WebsitePlannerShift1 = Manifest.ManuallySetWebsitePlannerShift1;
                if (Settings_WebsitePlannerShift1 >= 0 && 9 >= Settings_WebsitePlannerShift1) { } else { Settings_WebsitePlannerShift1 = 1; }

                Settings_WebsitePlannerShift2 = Manifest.ManuallySetWebsitePlannerShift2;
                if (Settings_WebsitePlannerShift2 >= 0 && 9 >= Settings_WebsitePlannerShift2) { } else { Settings_WebsitePlannerShift2 = 2; }

                Settings_WebsitePlannerShift3 = Manifest.ManuallySetWebsitePlannerShift3;
                if (Settings_WebsitePlannerShift3 >= 0 && 9 >= Settings_WebsitePlannerShift3) { } else { Settings_WebsitePlannerShift3 = 2; }

                Settings_WebsitePlannerShift4 = Manifest.ManuallySetWebsitePlannerShift4;
                if (Settings_WebsitePlannerShift4 >= 0 && 9 >= Settings_WebsitePlannerShift4) { } else { Settings_WebsitePlannerShift4 = 2; }

                Settings_WebsitePlannerShift5 = Manifest.ManuallySetWebsitePlannerShift5;
                if (Settings_WebsitePlannerShift5 >= 0 && 9 >= Settings_WebsitePlannerShift5) { } else { Settings_WebsitePlannerShift5 = 2; }
            }
            else { Settings_SendToWebsitePlanner = false; }
            #endregion // Send to Planner


            Settings_MinimiseToSystemTray = manifest.MinimiseToSystemTray;
            if (manifest.HideTaskbarIcon) { this.ShowInTaskbar = false; } else { this.ShowInTaskbar = true; }

            if (manifest.ConfirmationCheckingInterval >= 5 && 300 >= manifest.ConfirmationCheckingInterval)
            {
                Settings_PopupNewConf_Interval = manifest.ConfirmationCheckingInterval;
            }
            else { Settings_PopupNewConf_Interval = 30; }
            DefaultSettings_PopupNewConf_Interval = Settings_PopupNewConf_Interval;

            // owerwrite interval > for planner
            if (manifest.WebsitePlannerInterval >= 1 && 300 >= manifest.WebsitePlannerInterval)
            {
                DefaultSettings_PopupNewConf_Planner_Interval = manifest.WebsitePlannerInterval * 1000;
            }
            else { DefaultSettings_PopupNewConf_Planner_Interval = 1000; }
            Settings_PopupNewConf_Planner_Interval_Tick = DefaultSettings_PopupNewConf_Planner_Interval;

            if (Settings_SendToWebsitePlanner == true) { Settings_PopupNewConf_Interval = DefaultSettings_PopupNewConf_Planner_Interval; }


            Settings_ShowConfirmationListButton = manifest.ShowConfirmationListButton;
            Settings_DisplayPopupConfirmation = manifest.DisplayPopupConfirmation;
            Settings_ConfirmationCheckAllAccounts = manifest.ConfirmationCheckAllAccounts;

            if (manifest.ConfirmationCheckAllAcc_RefreshSessionDellay >= 100)
            {
                Settings_ConfirmationCheckAllAcc_RefreshSessionDellay = manifest.ConfirmationCheckAllAcc_RefreshSessionDellay;
            }
            else { Settings_ConfirmationCheckAllAcc_RefreshSessionDellay = 500; }


            // show Auto-confirm status
            Settings_DelayAutoConfirmAtStartup = manifest.DelayAutoConfirmAtStartup;

            if (manifest.DelayAutoConfirmAtStartupInterval > 0 || 60 >= manifest.DelayAutoConfirmAtStartupInterval)
            {
                Settings_DelayAutoConfirmAtStartupInterval = manifest.DelayAutoConfirmAtStartupInterval + 1;
            }
            else
            {
                Settings_DelayAutoConfirmAtStartupInterval = 6;
            }

            if (manifest.ConfirmationsPeriodicChecking == true)
            {


                Settings_ConfirmationsPeriodicChecking = true;

                btn_labelAutoConfirmTrades.Enabled = true;
                btn_labelAutoConfirmMarket.Enabled = true;

                //DelayAutoConfirmAtStartup
                if (Settings_DelayAutoConfirmAtStartup)
                {
                    if (manifest.AutoConfirmTrades)
                    {
                        AutoConfirm_Trades = 2;
                        btn_labelAutoConfirmTrades.Text = "OFF"; btn_labelAutoConfirm_Color("Trades", "MouseLeave");
                    }
                    else
                    {
                        AutoConfirm_Trades = 0;
                        btn_labelAutoConfirmTrades.Text = "ON"; btn_labelAutoConfirm_Color("Trades", "MouseLeave");
                        SetAutoConfirmLabelStatus("Trades", "Off");
                    }


                    if (manifest.AutoConfirmMarketTransactions)
                    {
                        AutoConfirm_Market = 2;
                        btn_labelAutoConfirmMarket.Text = "OFF"; btn_labelAutoConfirm_Color("Market", "MouseLeave");
                    }
                    else
                    {
                        AutoConfirm_Market = 0;
                        btn_labelAutoConfirmMarket.Text = "ON"; btn_labelAutoConfirm_Color("Market", "MouseLeave");
                        SetAutoConfirmLabelStatus("Market", "Off");
                    }

                    // Dellay
                    if (Settings_DelayAutoConfirmAtStartup) { timer_DelayAutoConfirmAtStartup.Enabled = true; }

                }
                else
                {
                    //No DelayAutoConfirmAtStartup

                    // Trades
                    if (manifest.AutoConfirmTrades == true)
                    {
                        AutoConfirm_Trades = 1;
                        SetAutoConfirmLabelStatus("Trades", "On");
                        btn_labelAutoConfirmTrades.Text = "OFF"; btn_labelAutoConfirm_Color("Trades", "MouseLeave");
                    }
                    else
                    {
                        AutoConfirm_Trades = 0;
                        btn_labelAutoConfirmTrades.Text = "ON"; btn_labelAutoConfirm_Color("Trades", "MouseLeave");
                        SetAutoConfirmLabelStatus("Trades", "Off");
                    }

                    // Market
                    if (manifest.AutoConfirmMarketTransactions == true)
                    {
                        AutoConfirm_Market = 1;
                        SetAutoConfirmLabelStatus("Market", "On");
                        btn_labelAutoConfirmMarket.Text = "OFF"; btn_labelAutoConfirm_Color("Market", "MouseLeave");
                    }
                    else
                    {
                        AutoConfirm_Market = 0;
                        btn_labelAutoConfirmMarket.Text = "ON"; btn_labelAutoConfirm_Color("Market", "MouseLeave");
                        SetAutoConfirmLabelStatus("Market", "Off");
                    }

                }

                // Check For Confirmations
                //////////////////////
                if (manifest.AutoConfirmTrades || manifest.AutoConfirmMarketTransactions || Settings_DisplayPopupConfirmation) { Settings_PopupNewConf.Enabled = manifest.ConfirmationsPeriodicChecking; }

            }
            else
            {
                // Btn Disabled
                Settings_ConfirmationsPeriodicChecking = false;

                btn_labelAutoConfirmTrades.Enabled = false; btn_labelAutoConfirm_Color("Trades", "Disabled"); btn_labelAutoConfirmTrades.Text = "OFF";
                btn_labelAutoConfirmMarket.Enabled = false; btn_labelAutoConfirm_Color("Market", "Disabled"); btn_labelAutoConfirmMarket.Text = "OFF";

                labelAutoConfirmTrades.Text = "Auto-confirm Trades: no";
                labelAutoConfirmTrades.ForeColor = Color.DarkGray; /*Text color*/ labelAutoConfirmTrades.BackColor = Color.Black; /*bg color*/

                labelAutoConfirmMarket.Text = "Auto-confirm Market: no";
                labelAutoConfirmMarket.ForeColor = Color.DarkGray; /*Text color*/ labelAutoConfirmMarket.BackColor = Color.Black; /*bg color*/
            }


            // apply settings
            #region use settings

            // Quit btn under x
            #region Quit btn under x
            if (Settings_MinimiseToSystemTray == "CloseBtnMinimizeToTray")
            {
                quitUnderXToolStripMenuItem.Visible = true;
                ShowInTaskbar = true;
            }
            else
            {
                quitUnderXToolStripMenuItem.Visible = false;
            }
            if (Settings_MinimiseToSystemTray == "MinimiseBtnMinimizeToTray") { } else { }
            if (Settings_MinimiseToSystemTray == "default") { }
            #endregion // Quit btn under x

            // Show / Hide Confirmations List btn - update GUI
            #region Show / Hide Confirmations List btn - update GUI
            if (Settings_ShowConfirmationListButton == true)
            {
                // 
                // MainForm
                // 
                this.ClientSize = new System.Drawing.Size(309, 433);
                this.MinimumSize = new System.Drawing.Size(325, 433);
                // 
                // groupAccount
                // 
                this.groupAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.groupAccount.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.groupAccount.Location = new System.Drawing.Point(7, 102);
                this.groupAccount.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
                this.groupAccount.Size = new System.Drawing.Size(296, 91);
                // 
                // btnTradeConfirmations
                // 
                this.btnTradeConfirmations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.btnTradeConfirmations.Font = new System.Drawing.Font("Segoe UI", 9.25F);
                this.btnTradeConfirmations.Location = new System.Drawing.Point(6, 15);
                this.btnTradeConfirmations.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
                this.btnTradeConfirmations.Size = new System.Drawing.Size(284, 32);
                this.btnTradeConfirmations.Text = "View Confirmations in Browser";
                // 
                // btnTradeConfirmationsList
                // 
                this.btnTradeConfirmationsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.btnTradeConfirmationsList.Font = new System.Drawing.Font("Segoe UI", 9.25F);
                this.btnTradeConfirmationsList.Location = new System.Drawing.Point(6, 52);
                this.btnTradeConfirmationsList.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
                this.btnTradeConfirmationsList.Size = new System.Drawing.Size(284, 32);
                this.btnTradeConfirmationsList.Visible = true;
                // 
                // listAccounts
                // 
                this.listAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.listAccounts.Location = new System.Drawing.Point(7, 199);
                this.listAccounts.Size = new System.Drawing.Size(296, 147);
                // 
                // labelDisableListClick
                // 
                this.labelDisableListClick.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.labelDisableListClick.Location = new System.Drawing.Point(8, 200);
                this.labelDisableListClick.Size = new System.Drawing.Size(294, 145);
            }
            else
            {
                // 
                // MainForm
                //
                this.ClientSize = new System.Drawing.Size(309, 394);
                this.MinimumSize = new System.Drawing.Size(325, 395);
                // 
                // groupAccount
                // 
                this.groupAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.groupAccount.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.groupAccount.Location = new System.Drawing.Point(7, 102);
                this.groupAccount.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
                this.groupAccount.Size = new System.Drawing.Size(296, 53);
                // 
                // btnTradeConfirmations
                // 
                this.btnTradeConfirmations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.btnTradeConfirmations.Font = new System.Drawing.Font("Segoe UI", 9.25F);
                this.btnTradeConfirmations.Location = new System.Drawing.Point(6, 15);
                this.btnTradeConfirmations.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
                this.btnTradeConfirmations.Size = new System.Drawing.Size(284, 32);
                this.btnTradeConfirmations.Text = "View Confirmations";
                // 
                // btnTradeConfirmationsList
                // 
                this.btnTradeConfirmationsList.Visible = false;
                // 
                // listAccounts
                // 
                this.listAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.listAccounts.Location = new System.Drawing.Point(7, 160);
                this.listAccounts.Size = new System.Drawing.Size(296, 186);
                // 
                // listAccounts
                // 
                this.listAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.listAccounts.Location = new System.Drawing.Point(7, 160);
                this.listAccounts.Size = new System.Drawing.Size(296, 147);
                // 
                // labelDisableListClick
                // 
                this.labelDisableListClick.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                this.labelDisableListClick.Location = new System.Drawing.Point(9, 160);
                this.labelDisableListClick.Size = new System.Drawing.Size(294, 146);
            }
            #endregion // Show / Hide Confirmations List btn - update GUI

            // Send App Status
            #region Send App Status
            if (manifest.SendAppStatus == true)
            {
                // set app name
                string AddToName = manifest.SendAppNo.ToString() + " ";
                this.Text = AddToName + Manifest.MainAppName;
                this.trayIcon.Text = AddToName + Manifest.MainAppName;

                Settings_SendAppStatusToAddress = manifest.SendAppStatusToAddress;

                if (manifest.SendAppStatusInterval >= 1 && 9999 >= manifest.SendAppStatusInterval)
                {
                    Settings_SendAppStatusInterval = manifest.SendAppStatusInterval;
                }
                else { Settings_SendAppStatusInterval = 1; }

                Settings_AppNo = manifest.SendAppNo;

                if (backgroundWorkerSendAppStatus.IsBusy) { backgroundWorkerSendAppStatus_restart = true; backgroundWorkerSendAppStatus_Timer = 0; backgroundWorkerSendAppStatus.CancelAsync(); }
                else { backgroundWorkerSendAppStatus_Timer = 0; backgroundWorkerSendAppStatus.RunWorkerAsync(); }

            }
            else
            {
                if (backgroundWorkerSendAppStatus.IsBusy) { backgroundWorkerSendAppStatus.CancelAsync(); }
                this.Text = Manifest.MainAppName;
                this.trayIcon.Text = Manifest.MainAppName;
            }
            #endregion // Send App Status


            #endregion // use settings


        }



        // Delay Auto-confirm at startup
        //////////////////////////////////////
        private void timer_DelayAutoConfirmAtStartup_Tick(object sender, EventArgs e)
        {
            Settings_DelayAutoConfirmAtStartupInterval--;

            if (manifest.AutoConfirmTrades == false && manifest.AutoConfirmMarketTransactions == false)
            {
                if (AutoConfirm_Trades == 1 || AutoConfirm_Market == 1 || Settings_DisplayPopupConfirmation) { Settings_PopupNewConf.Enabled = manifest.ConfirmationsPeriodicChecking; }
                timer_DelayAutoConfirmAtStartup.Enabled = false;
                return;
            }

            // Trades
            if (manifest.AutoConfirmTrades)
            {
                labelAutoConfirmTrades.Text = "Auto-confirm Trades: delay " + Settings_DelayAutoConfirmAtStartupInterval.ToString();
                SetAutoConfirmLabelStatus("Trades", "Delay");

                if (Settings_DelayAutoConfirmAtStartupInterval == 0) { AutoConfirm_Trades = 1; SetAutoConfirmLabelStatus("Trades", "On"); }
            }

            // Market
            if (manifest.AutoConfirmMarketTransactions)
            {
                labelAutoConfirmMarket.Text = "Auto-confirm Market: delay " + Settings_DelayAutoConfirmAtStartupInterval.ToString();
                SetAutoConfirmLabelStatus("Market", "Delay");

                if (Settings_DelayAutoConfirmAtStartupInterval == 0) { AutoConfirm_Market = 1; SetAutoConfirmLabelStatus("Market", "On"); }
            }

            // Close timer // start loop > Check for new Confirmations
            if (Settings_DelayAutoConfirmAtStartupInterval == 0)
            {
                timer_DelayAutoConfirmAtStartup.Enabled = false;
                Settings_PopupNewConf.Enabled = manifest.ConfirmationsPeriodicChecking;
            }
        }



        // Worker Send App Status
        ///////////////////////////
        #region Worker Send App Status
        private void backgroundWorkerSendAppStatus_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (!backgroundWorkerSendAppStatus.CancellationPending)
            {

                if (backgroundWorkerSendAppStatus_Timer == 0)
                {
                    backgroundWorkerSendAppStatus_Timer = Settings_SendAppStatusInterval; // Interval

                    if (Settings_SendAppStatusToAddress != "" && Settings_SendAppStatusInterval > 0)
                    {

                        //lblStatus.Invoke(new MethodInvoker(delegate { lblStatus.Text = ">>> Sending app status..."; }));
                        this.Invoke(new Action(() => { Program.ConsoleForm_Update.SetConsoleText(">>> Sending app status...", "ConsoleStatus_Task"); }));

                        string SendPost_Status = Post_AppStatus.SendPostData_ToWebAppStatus(Settings_SendAppStatusToAddress, Settings_AppNo.ToString());

                        //lblStatus.Invoke(new MethodInvoker(delegate { lblStatus.Text = SendPost_Status; }));

                        if (SendPost_Status == "Sending app status > Done")
                        {
                            this.Invoke(new Action(() => { Program.ConsoleForm_Update.SetConsoleText(">>> " + SendPost_Status, "ConsoleStatus_Return"); }));
                        }
                        if (SendPost_Status == "Sending app status > Failed" || SendPost_Status == "Sending app status > Invalid Response" || SendPost_Status == "Sending app status > Error")
                        {
                            this.Invoke(new Action(() => { Program.ConsoleForm_Update.SetConsoleText(">>> " + SendPost_Status, "ConsoleStatus_ReturnWarning"); }));
                        }

                    }
                }

                backgroundWorkerSendAppStatus_Timer--;
                Thread.Sleep(1000);
            }
        }
        private void backgroundWorkerSendAppStatus_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (backgroundWorkerSendAppStatus_restart)
            {
                backgroundWorkerSendAppStatus_restart = false;
                backgroundWorkerSendAppStatus.RunWorkerAsync();
            }
        }




        #endregion // Worker Send App Status


    }
}
