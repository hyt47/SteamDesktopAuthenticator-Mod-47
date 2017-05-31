using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Threading;

namespace Steam_Desktop_Authenticator
{
    static class Program
    {
        // Run app only once - Part 1
        //////////////////////////////////
        #region Run app only once - Part 1
        // Activate Old Process Window
        [DllImportAttribute("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion // Run app only once - Part 1



        public static ConsoleForm ConsoleForm_Update = new ConsoleForm(); // Create Gui Class public // So u can update gui elements from other cs classes



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // Read Settings
            /////////////////////
            Manifest manifest_app = Manifest.GetManifest();
            bool Settings_SendAppStatus = manifest_app.SendAppStatus;
            bool Settings_AppCanBeStartedMultipleTimes = manifest_app.AppCanBeStartedMultipleTimes;
            int Settings_SendAppNo = manifest_app.SendAppNo;
            string Settings_SendAppStatusToAddress = manifest_app.SendAppStatusToAddress;
            int Settings_SendAppStatusInterval = manifest_app.SendAppStatusInterval;

            bool Settings_AutoConfirmTrades = manifest_app.AutoConfirmTrades;
            bool Settings_AutoConfirmMarketTransactions = manifest_app.AutoConfirmMarketTransactions;
            int Settings_ConfirmationCheckingInterval = manifest_app.ConfirmationCheckingInterval;

            bool Settings_ConfirmationsPeriodicChecking = manifest_app.ConfirmationsPeriodicChecking;
            bool Settings_ConfirmationCheckAllAccounts = manifest_app.ConfirmationCheckAllAccounts;

            bool Settings_HideTaskbarIcon = manifest_app.HideTaskbarIcon;
            bool Settings_StartMinimizedToSystemTray = manifest_app.StartMinimizedToSystemTray;
            


            // Command Line Args
            ////////////////////// - used by the Server
            #region Command Line Args
            // to test create a shortcut and set the target: "...\Steam Desktop Authenticator 47.exe" arg
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                // check if server version is compatible with the app
                ////////////////////////////////////
                if (arg.IndexOf("-IsCompatibleWithServerId") != -1) {
                    string ServerCompatibleId = arg.Replace("-IsCompatibleWithServerId", "");

                    if (ServerCompatibleId != "") {
                        if (ServerCompatibleId != "1")
                        {
                            MessageBox.Show("This App Version is not compatible with the Server! Server Id: " + ServerCompatibleId);
                            Environment.Exit(0);
                        }
                    }
                }

                // check SendDataToServer On Off
                ////////////////////////////////////
                if (arg.IndexOf("-SendDataToServer") != -1) {
                    string SendDataToServer_New = arg.Replace("-SendDataToServer", "");

                    if (SendDataToServer_New == "1") {
                        manifest_app.SendAppStatus = true;
                        manifest_app.Save();
                    }
                    if (SendDataToServer_New == "0")
                    {
                        manifest_app.SendAppStatus = false;
                        manifest_app.Save();
                    }
                }

                // check AppCanBeStartedMultipleTimes On Off
                ////////////////////////////////////
                if (arg.IndexOf("-AppCanBeStartedMultipleTimes") != -1)
                {
                    string AppCanBeStartedMultipleTimes_New = arg.Replace("-AppCanBeStartedMultipleTimes", "");

                    if (AppCanBeStartedMultipleTimes_New == "1" && Settings_AppCanBeStartedMultipleTimes == false)
                    {
                        manifest_app.AppCanBeStartedMultipleTimes = true;
                        manifest_app.Save();
                    }
                    if (AppCanBeStartedMultipleTimes_New == "0" && Settings_AppCanBeStartedMultipleTimes == true)
                    {
                        manifest_app.AppCanBeStartedMultipleTimes = false;
                        manifest_app.Save();
                    }
                }

                // check AppNo
                ////////////////////////////////////
                if (arg.IndexOf("-AppNo") != -1)
                {
                    string ServerAppNo = arg.Replace("-AppNo", "");

                    if (ServerAppNo != "" && Settings_SendAppNo.ToString() != ServerAppNo)
                    {
                        // convert string to int
                        int SendAppNo_Int = 0;
                        bool isNumeric_ServerAppNo = int.TryParse(ServerAppNo, out SendAppNo_Int);
                        if (isNumeric_ServerAppNo) {
                            // set the app no
                            manifest_app.SendAppNo = SendAppNo_Int;
                            manifest_app.Save();
                        }
                    }
                }

                // check Server Address
                ////////////////////////////////////
                if (arg.IndexOf("-ServerAddress") != -1)
                {
                    string ServerAddress = arg.Replace("-ServerAddress", "");
                    if (ServerAddress != "") {
                        string ServerAddress_ = ServerAddress.Replace("_empty_-_space_", " ");
                        if (ServerAddress_ != "") {
                            if (Settings_SendAppStatusToAddress != ServerAddress_)
                            {
                                // set
                                manifest_app.SendAppStatusToAddress = ServerAddress_;
                                manifest_app.Save();
                            }
                        }
                    }
                }

                // check Send Data Interval
                ////////////////////////////////////
                if (arg.IndexOf("-SendDataInterval") != -1)
                {
                    string SendDataInterval = arg.Replace("-SendDataInterval", "");

                    if (SendDataInterval != "" && Settings_SendAppStatusInterval.ToString() != SendDataInterval)
                    {
                        // convert string to int
                        int SendDataInterval_int = 0;
                        bool isNumeric_SendDataInterval = int.TryParse(SendDataInterval, out SendDataInterval_int);
                        if (isNumeric_SendDataInterval && SendDataInterval_int >= 1 && 9999 >= SendDataInterval_int)
                        {
                            // set the app no
                            manifest_app.SendAppStatusInterval = SendDataInterval_int;
                            manifest_app.Save();
                        }
                    }
                }


                // check AutoConfirmTrades On Off
                ////////////////////////////////////
                if (arg.IndexOf("-AutoConfirmTrades") != -1)
                {
                    string AutoConfirmTrades_New = arg.Replace("-AutoConfirmTrades", "");

                    if (AutoConfirmTrades_New == "1" && Settings_AutoConfirmTrades == false)
                    {
                        manifest_app.AutoConfirmTrades = true;
                        manifest_app.Save();
                    }
                    if (AutoConfirmTrades_New == "0" && Settings_AutoConfirmTrades == true)
                    {
                        manifest_app.AutoConfirmTrades = false;
                        manifest_app.Save();
                    }
                }

                // check AutoConfirmMarket On Off
                ////////////////////////////////////
                if (arg.IndexOf("-AutoConfirmMarket") != -1)
                {
                    string AutoConfirmMarket_New = arg.Replace("-AutoConfirmMarket", "");

                    if (AutoConfirmMarket_New == "1" && Settings_AutoConfirmMarketTransactions == false)
                    {
                        manifest_app.AutoConfirmMarketTransactions = true;
                        manifest_app.Save();
                    }
                    if (AutoConfirmMarket_New == "0" && Settings_AutoConfirmMarketTransactions == true)
                    {
                        manifest_app.AutoConfirmMarketTransactions = false;
                        manifest_app.Save();
                    }
                }

                // check CheckForConfirmationsInterval On Off
                ////////////////////////////////////
                if (arg.IndexOf("-CheckForConfirmationsInterval") != -1)
                {
                    string CheckForConfirmationsInterval_New = arg.Replace("-CheckForConfirmationsInterval", "");

                    // convert string to int
                    int CheckForConfirmationsInterval_New_int = 0;
                    bool isNumeric_CheckForConfirmationsInterval_New = int.TryParse(CheckForConfirmationsInterval_New, out CheckForConfirmationsInterval_New_int);
                    if (isNumeric_CheckForConfirmationsInterval_New && CheckForConfirmationsInterval_New_int >= 5 && 300 >= CheckForConfirmationsInterval_New_int && Settings_ConfirmationCheckingInterval != CheckForConfirmationsInterval_New_int)
                    {
                        manifest_app.ConfirmationCheckingInterval = CheckForConfirmationsInterval_New_int;
                        manifest_app.Save();
                    }
                }


                // check PeriodicallyCheckConfirmations On Off
                ////////////////////////////////////
                if (arg.IndexOf("-PeriodicallyCheckConfirmations") != -1)
                {
                    string PeriodicallyCheckConfirmations_New = arg.Replace("-PeriodicallyCheckConfirmations", "");

                    if (PeriodicallyCheckConfirmations_New == "1" && Settings_ConfirmationsPeriodicChecking == false)
                    {
                        manifest_app.ConfirmationsPeriodicChecking = true;
                        manifest_app.Save();
                    }
                    if (PeriodicallyCheckConfirmations_New == "0" && Settings_ConfirmationsPeriodicChecking == true)
                    {
                        manifest_app.ConfirmationsPeriodicChecking = false;
                        manifest_app.Save();
                    }
                }

                // check CheckAllAccForConfirmations On Off
                ////////////////////////////////////
                if (arg.IndexOf("-CheckAllAccForConfirmations") != -1)
                {
                    string CheckAllAccConfirmations_New = arg.Replace("-CheckAllAccForConfirmations", "");

                    if (CheckAllAccConfirmations_New == "1" && Settings_ConfirmationCheckAllAccounts == false)
                    {
                        manifest_app.ConfirmationCheckAllAccounts = true;
                        manifest_app.Save();
                    }
                    if (CheckAllAccConfirmations_New == "0" && Settings_ConfirmationCheckAllAccounts == true)
                    {
                        manifest_app.ConfirmationCheckAllAccounts = false;
                        manifest_app.Save();
                    }
                }

                // check HideTaskBarIcon On Off
                //////////////////////////////////// HideTaskBarIcon
                if (arg.IndexOf("-HideTaskbarIcon") != -1)
                {
                    string HideTaskbarIcon_New = arg.Replace("-HideTaskbarIcon", "");

                    if (HideTaskbarIcon_New == "1" && Settings_HideTaskbarIcon == false)
                    {
                        manifest_app.HideTaskbarIcon = true;
                        manifest_app.Save();
                    }
                    if (HideTaskbarIcon_New == "0" && Settings_HideTaskbarIcon == true)
                    {
                        manifest_app.HideTaskbarIcon = false;
                        manifest_app.Save();
                    }
                }

                // check StartMinimizedToSystemTray On Off
                ////////////////////////////////////
                if (arg.IndexOf("-StartMinimizedToSystemTray") != -1)
                {
                    string StartMinimizedToSystemTray_New = arg.Replace("-StartMinimizedToSystemTray", "");

                    if (StartMinimizedToSystemTray_New == "1" && Settings_StartMinimizedToSystemTray == false)
                    {
                        manifest_app.StartMinimizedToSystemTray = true;
                        manifest_app.Save();
                    }
                    if (StartMinimizedToSystemTray_New == "0" && Settings_StartMinimizedToSystemTray == true)
                    {
                        manifest_app.StartMinimizedToSystemTray = false;
                        manifest_app.Save();
                    }
                }

                // check Encryption key
                ////////////////////////////////////
                if (arg.IndexOf("-EncryptionKey") != -1){
                    string Encryption_key = arg.Replace("-EncryptionKey", "");

                    MainForm mf = new MainForm();
                    mf.SetEncryptionKey(Encryption_key);
                }
            }
            #endregion // Command Line Args



            // Modify Settings
            /////////////////////
            if (Settings_AppCanBeStartedMultipleTimes == false && Settings_SendAppStatus == true) { Settings_AppCanBeStartedMultipleTimes = true; }



            // Catching Unhandled Exceptions
            ////////////////////////////////// - used by the Server
            #region Catching Unhandled Exceptions
            if (Settings_SendAppStatus)
            {
                // Add the event handler for handling UI thread exceptions to the event.
                //Application.ThreadException = Excep();

                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

                // Set the unhandled exception mode to force all Windows Forms errors
                // to go through our handler.
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                // Add the event handler for handling non-UI thread exceptions to the event. 
                AppDomain.CurrentDomain.UnhandledException += new
                UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }
            #endregion // Catching Unhandled Exceptions


            // Run app only once - Part 2
            //////////////////////////////////
            #region Run app only once - Part 2
            if (Settings_AppCanBeStartedMultipleTimes == false)
            {
                // Activate Old Process Window
                // If another instance is already running, activate it and exit - Part 2
                try
                {
                    Process currentProc = Process.GetCurrentProcess();
                    foreach (Process proc in Process.GetProcessesByName(currentProc.ProcessName))
                    {
                        if (proc.Id != currentProc.Id)
                        {
                            IntPtr firstInstance = FindWindow(null, Manifest.MainAppName);
                            ShowWindow(firstInstance, 1);
                            SetForegroundWindow(firstInstance);
                            return;   // Exit application
                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
            #endregion // Run app only once - Part 2



            //////////////////////////
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

           

            // verify accounts
            //////////////////////////
            #region Verify Accounts
            string ManifestRetun = "ok";
            string ContinueScript = null;

            try { ManifestRetun = manifest_app.VeryfyIfAccountsAreOk(); } catch (Exception) {
                MessageBox.Show("Error verifying accounts!");
            }

            if (ManifestRetun == "ok")
            {
                ContinueScript = "1";
            }
            else if (ManifestRetun == "OpenFolder")
            {
                DialogResult res = MessageBox.Show("You have a few accounts to import back in to the app\n\nDo you want to open the folder app ?", "Accounts to Import", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", Manifest.GetExecutableDir());
                }
                ContinueScript = "1";
            }
            else
            {
                MessageBox.Show("Failed to verify accounts! an error occurred.\nQuit");
                return;
            }
            #endregion // Verify Accounts > end




            // Run >> Install Visual C++ // Form
            /////////////////////////////////////////////
            #region Run >> Install Visual C++ // Form
            if (ContinueScript == "1")
            {
                if (manifest_app.FirstRun)
                {
                    // Install VC++ Redist and wait
                    DialogResult res = MessageBox.Show("Install Visual C++ Redistributable 2013\nvcredist_x86.exe", "SDA Setup", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        new InstallRedistribForm().ShowDialog();
                    }

                    // Already has accounts, just run
                    if (manifest_app.Entries.Count > 0)
                    {
                        Application.Run(new MainForm());
                    }
                    else
                    {
                        // No accounts, run welcome form
                        Application.Run(new WelcomeForm());
                    }
                }
                else
                {
                    Application.Run(new MainForm());
                }
            }
            #endregion // Run >> Install Visual C++ // Form
        }





        // Catching Unhandled Exceptions >>> if the app is sending data to a server
        /////////////////////////////////////////////////////////////////////////////////
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            //MessageBox.Show(e.Exception.Message, "Unhandled Thread Exception");
            // here you can log the exception ...
            Application.Exit();
            // Server will get nothing from the app & it will reopen the app
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //MessageBox.Show((e.ExceptionObject as Exception).Message, "Unhandled UI Exception");
            // here you can log the exception ...
            Application.Exit();
            // Server will get nothing from the app & it will reopen the app
        }




    }
}
