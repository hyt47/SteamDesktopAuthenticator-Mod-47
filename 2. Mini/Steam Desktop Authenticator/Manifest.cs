using Newtonsoft.Json;
using SteamAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using JR.Utils.GUI.Forms;

namespace Steam_Desktop_Authenticator
{
    public class Manifest
    {
        public static string MainAppName = "Steam Desktop Authenticator 47";
        public static string FolderNameSteamFiles = "maFiles";
        public static string SteamFileExtension = ".maFile";
        public static string ManifestFileNameWithExt = "manifest.json";


        [JsonProperty("app_can_be_started_multiple_times")]
        public bool AppCanBeStartedMultipleTimes { get; set; } = false;

        [JsonProperty("RunAtStartup")]
        public bool RunAtStartup { get; set; } = false;

        [JsonProperty("encrypted")]
        public bool Encrypted { get; set; }

        [JsonProperty("first_run")]
        public bool FirstRun { get; set; } = true;

        [JsonProperty("minimise_to_system_tray")]
        public string MinimiseToSystemTray { get; set; } = "CloseBtnMinimizeToTray";

        [JsonProperty("hide_taskbar_icon")]
        public bool HideTaskbarIcon { get; set; } = false;

        [JsonProperty("start_minimized_to_system_tray")]
        public bool StartMinimizedToSystemTray { get; set; } = false;

        [JsonProperty("confirmations_periodic_checking")]
        public bool ConfirmationsPeriodicChecking { get; set; } = false;

        [JsonProperty("confirmation_checking_interval")]
        public int ConfirmationCheckingInterval { get; set; } = 30;

        [JsonProperty("confirmation_checkallaccounts")]
        public bool ConfirmationCheckAllAccounts { get; set; } = false;

        [JsonProperty("display_popup_confirmation")]
        public bool DisplayPopupConfirmation { get; set; } = true;

        [JsonProperty("popup_confirmation_border")]
        public string PopupConfirmationBorder { get; set; } = "1";

        [JsonProperty("delay_auto_confirm_at_startup")]
        public bool DelayAutoConfirmAtStartup { get; set; } = true;

        [JsonProperty("delay_auto_confirm_at_startup_interval")]
        public int DelayAutoConfirmAtStartupInterval { get; set; } = 10;
        
        [JsonProperty("auto_confirm_market_transactions")]
        public bool AutoConfirmMarketTransactions { get; set; } = false;

        [JsonProperty("auto_confirm_trades")]
        public bool AutoConfirmTrades { get; set; } = false;

        [JsonProperty("send_app_status")]
        public bool SendAppStatus { get; set; } = false;

        [JsonProperty("send_app_status_interval")]
        public int SendAppStatusInterval { get; set; } = 1;

        [JsonProperty("send_app_status_to_address")]
        public string SendAppStatusToAddress { get; set; } = "http://localhost:4141/";

        [JsonProperty("send_app_no")]
        public int SendAppNo { get; set; } = 1;

        [JsonProperty("entries")]
        public List<ManifestEntry> Entries { get; set; }

        private static Manifest _manifest { get; set; }

        public static string Get_FileEncryption_Version()
        {
            return "v2";
        }

        public static string GetExecutableDir()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        }

        public static Manifest GetManifest(bool forceLoad = false)
        {
            // Check if already staticly loaded
            if (_manifest != null && !forceLoad)
            {
                return _manifest;
            }

            // Find config dir and manifest file
            string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/";
            string maFile = maDir + ManifestFileNameWithExt;

            // If there's no config dir, create it
            if (!Directory.Exists(maDir))
            {
                _manifest = _generateNewManifest();
                return _manifest;
            }

            // If there's no manifest, create it
            if (!File.Exists(maFile))
            {
                _manifest = _generateNewManifest(true);
                return _manifest;
            }

            try
            {
                string manifestContents = File.ReadAllText(maFile);
                _manifest = JsonConvert.DeserializeObject<Manifest>(manifestContents);

                if (_manifest.Encrypted && _manifest.Entries.Count == 0)
                {
                    _manifest.Encrypted = false;
                    _manifest.Save();
                }

                _manifest.RecomputeExistingEntries();

                return _manifest;
            }
            catch (Exception)
            {
                return null;
            }
        }


        // Verify Accounts
        public string VeryfyIfAccountsAreOk()
        {
            int TotalAccounts = 0;
            int TotalNotEncryptedAccounts = 0;
            int TotalEncryptedAccounts_v1 = 0;
            int TotalEncryptedAccounts_v2 = 0;
            int TotalEncryptedAccounts_VersionUnknown = 0;

            string NotEncryptedAccounts_List = "";
            string EncryptedAccounts_v1_List = "";
            string EncryptedAccounts_v2_List = "";
            string EncryptedAccounts_VersionUnknown_List = "";

            string AtTheEndOpenFolder = "";

            // Accounts Inventory
            //########################
            #region Accounts Inventory

            string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/";
            if (Directory.Exists(maDir))
            {
                DirectoryInfo dir = new DirectoryInfo(maDir);
                var files = dir.GetFiles();

                foreach (var file in files)
                {
                    if (file.Extension == SteamFileExtension)
                    {
                        TotalAccounts++;

                        string fileContents = File.ReadAllText(file.FullName);
                        string FileName = Path.GetFileName(file.FullName);

                        // Check for encryption v2 ************
                        if (fileContents.Contains("<next_string_of_data>"))
                        {
                            string[] fileTextPart = Regex.Split(fileContents, "<next_string_of_data>");
                            string FileEncryptionVersion = null;
                            try { FileEncryptionVersion = fileTextPart[0]; } catch (Exception) { }

                            if (FileEncryptionVersion == Get_FileEncryption_Version())
                            {
                                // encryption version 2
                                TotalEncryptedAccounts_v2++;
                                if (EncryptedAccounts_v2_List == "") { EncryptedAccounts_v2_List = FileName; } else { EncryptedAccounts_v2_List += " " + FileName; }
                            }
                            else {
                                // unknown encryption version
                                TotalEncryptedAccounts_VersionUnknown++;
                                if (EncryptedAccounts_VersionUnknown_List == "") { EncryptedAccounts_VersionUnknown_List = FileName; } else { EncryptedAccounts_VersionUnknown_List += " " + FileName; }
                            }

                        }
                        else {
                            // Check for encryption v2 ************
                            // detect if user moved old encrypted data v1 to this app that is using a newer encryption
                            int SearchFoundMaches = 0;
                            if (fileContents.Contains("shared_secret")) { SearchFoundMaches++; }
                            if (fileContents.Contains("serial_number")) { SearchFoundMaches++; }
                            if (fileContents.Contains("serial_number")) { SearchFoundMaches++; }
                            if (fileContents.Contains("server_time")) { SearchFoundMaches++; }
                            if (fileContents.Contains("account_name")) { SearchFoundMaches++; }
                            if (fileContents.Contains("token_gid")) { SearchFoundMaches++; }
                            if (fileContents.Contains("identity_secret")) { SearchFoundMaches++; }
                            if (fileContents.Contains("secret_1")) { SearchFoundMaches++; }
                            if (fileContents.Contains("status")) { SearchFoundMaches++; }
                            if (fileContents.Contains("device_id")) { SearchFoundMaches++; }
                            if (fileContents.Contains("fully_enrolled")) { SearchFoundMaches++; }
                            if (fileContents.Contains("Session")) { SearchFoundMaches++; }
                            if (fileContents.Contains("SessionID")) { SearchFoundMaches++; }
                            if (fileContents.Contains("SteamLogin")) { SearchFoundMaches++; }
                            if (fileContents.Contains("SteamLoginSecure")) { SearchFoundMaches++; }
                            if (fileContents.Contains("WebCookie")) { SearchFoundMaches++; }
                            if (fileContents.Contains("OAuthToken")) { SearchFoundMaches++; }
                            if (fileContents.Contains("SteamID")) { SearchFoundMaches++; }

                            //if at least 4 matches are found the file is not encrypted // (this should work with just 1 match, I added 4 to be sure)
                            if (SearchFoundMaches > 4)
                            {
                                // is not encrypted ************
                                TotalNotEncryptedAccounts++;
                                if (NotEncryptedAccounts_List == "") { NotEncryptedAccounts_List = FileName; } else { NotEncryptedAccounts_List += " " + FileName; }
                            }
                            else {
                                // is encrypted v2 ************
                                TotalEncryptedAccounts_v1++;
                                if (EncryptedAccounts_v1_List == "") { EncryptedAccounts_v1_List = FileName; } else { EncryptedAccounts_v1_List += " " + FileName; }
                            }
                        }
                    }
                }
            }
            else {
                return "ok";
            }
            #endregion // Accounts Inventory > END



            // Detect problems
            //#########################
            #region Detect problems
            string MsgProblemTitle = "Verify accounts";

            if (TotalNotEncryptedAccounts == 0 && TotalEncryptedAccounts_v1 == 0 && TotalEncryptedAccounts_v2 == 0 && TotalEncryptedAccounts_VersionUnknown == 0) { return "ok"; }

            int AccountTypes = 0;
            if (TotalEncryptedAccounts_v1 > 0) { AccountTypes++; }
            if (TotalEncryptedAccounts_v2 > 0) { AccountTypes++; }
            if (TotalNotEncryptedAccounts > 0) { AccountTypes++; }
            if (TotalEncryptedAccounts_VersionUnknown > 0) { AccountTypes++; }


            if (AccountTypes > 1)
            {
                string MsgInventory = "Accounts inventory:\n------------------------------------\n";

                if (TotalNotEncryptedAccounts > 0)
                {
                    MsgInventory += "Accounts not encrypted:\n";
                    MsgInventory += NotEncryptedAccounts_List.Replace(" ", " \n");
                    MsgInventory += " \n\n";
                }
                if (TotalEncryptedAccounts_v2 > 0)
                {
                    MsgInventory += "Encrypted accounts v2:\n";
                    MsgInventory += EncryptedAccounts_v2_List.Replace(" ", " \n");
                    MsgInventory += " \n\n";
                }
                if (TotalEncryptedAccounts_v1 > 0)
                {
                    MsgInventory += "Encrypted accounts v1:\n";
                    MsgInventory += EncryptedAccounts_v1_List.Replace(" ", " \n");
                    MsgInventory += " \n\n";
                }
                if (TotalEncryptedAccounts_VersionUnknown > 0)
                {
                    MsgInventory += "Accounts encrypted version unknown:\n";
                    MsgInventory += EncryptedAccounts_VersionUnknown_List.Replace(" ", " \n");
                    MsgInventory += " \n\n";
                }
                FlexibleMessageBox.Show(MsgInventory, MsgProblemTitle);
            }
            #endregion // Detect problems > end


            string MoveAccountsEncrypted_v1_to = "accounts encrypted v1 to import again";
            string MoveAccountsEncrypted_v2_to = "accounts encrypted v2 to import again";
            string MoveAccountsEncrypted_VersionUnknown_to = "accounts encrypted version unknown";

            // Fix problems
            //#########################
            #region Fix problems
            if (1 == 1)
            {
                string MsgProblem = "";

                if (AccountTypes > 1)
                {
                    MsgProblem += "Problem detected in your accounts!\n---------------------------------------------\n\nStatus:\n";


                    //** Fix - Accounts Encrypted v1
                    //**************************
                    //##########################
                    #region Fix - Accounts Encrypted v1
                    if (1 == 1)
                    {
                        if (TotalEncryptedAccounts_v1 > 0)
                        {
                            // Action:
                            // - Move encrypted accounts v1
                            // - Copy or move manifest.json (move only if there aren't any other types of accounts)

                            // Move files encrypted v1
                            //##########################
                            #region Move files encrypted v1
                            MsgProblem += "Accounts with encryption v1 found: " + TotalEncryptedAccounts_v1.ToString() + " \n";
                            MsgProblem += "- I will move them to folder: '" + MoveAccountsEncrypted_v1_to + "' \n";

                            string ToImportAgain_Encrypted_v1_FolderPath = Manifest.GetExecutableDir() + @"\" + MoveAccountsEncrypted_v1_to;

                            // Create dir if we don't have it
                            if (!Directory.Exists(ToImportAgain_Encrypted_v1_FolderPath))
                            {
                                Directory.CreateDirectory(ToImportAgain_Encrypted_v1_FolderPath);
                            }

                            string[] Files = EncryptedAccounts_v1_List.Split(' ');
                            foreach (string File_Name in Files)
                            {
                                string ExtractFileNameSteamID64_v1 = File_Name.Replace(SteamFileExtension, "");

                                if (File_Name == "" || File_Name == null || ExtractFileNameSteamID64_v1 == "" || ExtractFileNameSteamID64_v1 == null) { }
                                else {
                                    // move
                                    string ReturnFromMovingFile_v1 = MoveAccountToRemovedFromManifest(null, ExtractFileNameSteamID64_v1, MoveAccountsEncrypted_v1_to);

                                    if (ReturnFromMovingFile_v1 == "ok")
                                    {
                                        MsgProblem += File_Name + " - moved ok\n";
                                    }
                                    else {
                                        MsgProblem += "Failed to move: " + File_Name + "\n";
                                        MsgProblem += "\n\nQuit";

                                        FlexibleMessageBox.Show(MsgProblem, MsgProblemTitle);
                                        return "error";
                                    }
                                }
                            }
                            #endregion // Move files encrypted v1 > end



                            // copy or move manifest
                            //##################
                            #region copy or move manifest
                            string v1_CopyOrMove = "copy";
                            if (TotalEncryptedAccounts_v2 == 0 && TotalNotEncryptedAccounts == 0) { v1_CopyOrMove = "move"; }


                            MsgProblem += " \nI will " + v1_CopyOrMove + " " + ManifestFileNameWithExt + " to folder: '" + MoveAccountsEncrypted_v1_to + "' \n";

                            string copyFrom_manifest_v1 = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/" + ManifestFileNameWithExt;
                            string copy_manifest_v1_toFolder = Manifest.GetExecutableDir() + @"\" + MoveAccountsEncrypted_v1_to;
                            string copy_manifest_v1_to = copy_manifest_v1_toFolder + @"\" + ManifestFileNameWithExt;



                            // set check if file to move exists:
                            if (File.Exists(copyFrom_manifest_v1))
                            {
                                // check if dir exists:
                                if (!Directory.Exists(copy_manifest_v1_toFolder))
                                {
                                    Directory.CreateDirectory(copy_manifest_v1_toFolder);
                                }

                                // if file already exists rename it
                                if (File.Exists(copy_manifest_v1_to))
                                {
                                    string timeStamp1 = DateTime.Now.ToString();
                                    string timeStamp2 = timeStamp1.Replace("/", "-");
                                    string timeStamp3 = timeStamp2.Replace(":", ".");
                                    string NewFileName = copy_manifest_v1_toFolder + @"\" + timeStamp3 + " old " + ManifestFileNameWithExt;

                                    try
                                    {
                                        // rename
                                        System.IO.File.Move(@copy_manifest_v1_to, @NewFileName);
                                        MsgProblem += "- file manifest already exists inside folder '" + MoveAccountsEncrypted_v1_to + "', I renamed it: '" + timeStamp3 + " old " + ManifestFileNameWithExt + "'\n\n";
                                    }
                                    catch (Exception)
                                    {
                                        MsgProblem += "- Failed to rename existent manifest inside '" + MoveAccountsEncrypted_v1_to + "', I can't " + v1_CopyOrMove + " the file";
                                        MsgProblem += " \nQuit";

                                        FlexibleMessageBox.Show(MsgProblem, MsgProblemTitle);
                                        return "error";
                                    }
                                }

                                // copy or move file
                                try
                                {
                                    if (v1_CopyOrMove == "copy")
                                    {
                                        File.Copy(copyFrom_manifest_v1, copy_manifest_v1_to);
                                    }
                                    else { File.Move(copyFrom_manifest_v1, copy_manifest_v1_to); }

                                    MsgProblem += "- file manifest ";
                                    if (v1_CopyOrMove == "copy") { MsgProblem += "copyed"; } else { MsgProblem += "moved"; }
                                    MsgProblem += " ok\n\n";
                                }
                                catch (Exception)
                                {
                                    MsgProblem += "- Failed to " + v1_CopyOrMove + " manifest file!";
                                    MsgProblem += " \nQuit";

                                    FlexibleMessageBox.Show(MsgProblem, MsgProblemTitle);
                                    return "error";
                                }


                                AtTheEndOpenFolder = "OpenFolder";
                            }
                            else {
                                MsgProblem += " \n- " + ManifestFileNameWithExt + " not found for acounts encrypted v1 - if you lost this file you can't import any file with encryption v1\n\n";
                            }
                            #endregion //copy or move manifest > END


                            MsgProblem += " \n- You will have to import these accounts manually.\n File > Import > From " + SteamFileExtension + " > select file\n\n";
                        }
                    }
                    #endregion // Fix - Accounts Encrypted v1 > END



                    //** Fix - Accounts Encrypted Version Unknown
                    //**************************
                    //##########################
                    #region Fix - Accounts Encrypted Version Unknown
                    if (1 == 1)
                    {
                        if (TotalEncryptedAccounts_VersionUnknown > 0)
                        {
                            // Action:
                            // - Move encrypted accounts Version Unknown
                            // - Copy or move manifest.json (move only if there aren't any other types of accounts)

                            //  Move encrypted accounts Version Unknown
                            //##########################
                            #region  Move encrypted accounts Version Unknown
                            MsgProblem += "Accounts encrypted version unknown found: " + TotalEncryptedAccounts_VersionUnknown.ToString() + " \n";
                            MsgProblem += "- I will move them to folder: '" + MoveAccountsEncrypted_VersionUnknown_to + "' \n";

                            string ToImportAgain_Encrypted_vUnknown_FolderPath = Manifest.GetExecutableDir() + @"\" + MoveAccountsEncrypted_VersionUnknown_to;

                            // Create dir if we don't have it
                            if (!Directory.Exists(ToImportAgain_Encrypted_vUnknown_FolderPath))
                            {
                                Directory.CreateDirectory(ToImportAgain_Encrypted_vUnknown_FolderPath);
                            }

                            string[] Files = EncryptedAccounts_VersionUnknown_List.Split(' ');
                            foreach (string File_Name in Files)
                            {
                                string ExtractFileNameSteamID64_vUnknown = File_Name.Replace(SteamFileExtension, "");

                                if (File_Name == "" || File_Name == null || ExtractFileNameSteamID64_vUnknown == "" || ExtractFileNameSteamID64_vUnknown == null) { }
                                else {
                                    // move
                                    string ReturnFromMovingFile_v1 = MoveAccountToRemovedFromManifest(null, ExtractFileNameSteamID64_vUnknown, MoveAccountsEncrypted_VersionUnknown_to);

                                    if (ReturnFromMovingFile_v1 == "ok")
                                    {
                                        MsgProblem += File_Name + " - moved ok\n";
                                    }
                                    else {
                                        MsgProblem += "Failed to move: " + File_Name + "\n";
                                        MsgProblem += "\n\nQuit";

                                        FlexibleMessageBox.Show(MsgProblem, MsgProblemTitle);
                                        return "error";
                                    }
                                }
                            }
                            #endregion //  Move encrypted accounts Version Unknown > end


                            MsgProblem += " \n- You can import these accounts in a newer app version.\n File > Import > From " + SteamFileExtension + " > select file\n\n";
                        }
                    }
                    #endregion // Fix - Accounts Encrypted Version Unknown > END



                    // ** Fix - Detected accounts encrypted v2 & unencrypted inside maFiles folder
                    //**************************
                    //##########################
                    #region Fix - Detected accounts encrypted v2 & unencrypted inside maFiles folder
                    if (1 == 1)
                    {
                        if (TotalNotEncryptedAccounts > 0 && TotalEncryptedAccounts_v2 > 0)
                        {
                            // Action:
                            // - Move encrypted accounts v2
                            // - Unlink encypted accounts v2 from manifest
                            // - Set manifest encrypted=false


                            MsgProblem += "\nDetected accounts encrypted v2 & unencrypted\n";
                            MsgProblem += "\n- I will move encrypted accounts to: '" + MoveAccountsEncrypted_v2_to + "' \n";

                            string ToImportAgain_Encrypted_v2_FolderPath = Manifest.GetExecutableDir() + @"\" + MoveAccountsEncrypted_v2_to;

                            // Create dir if we don't have it
                            if (!Directory.Exists(ToImportAgain_Encrypted_v2_FolderPath))
                            {
                                Directory.CreateDirectory(ToImportAgain_Encrypted_v2_FolderPath);
                            }

                            string[] Files = EncryptedAccounts_v2_List.Split(' ');
                            foreach (string File_Name in Files)
                            {
                                string ExtractFileNameSteamID64_v2 = File_Name.Replace(SteamFileExtension, "");

                                if (File_Name == "" || File_Name == null || ExtractFileNameSteamID64_v2 == "" || ExtractFileNameSteamID64_v2 == null) { }
                                else {
                                    // move
                                    string ReturnFromMovingFile_v2 = MoveAccountToRemovedFromManifest(null, ExtractFileNameSteamID64_v2, MoveAccountsEncrypted_v2_to);

                                    if (ReturnFromMovingFile_v2 == "ok")
                                    {
                                        MsgProblem += File_Name + " - moved ok";
                                    }
                                    else {
                                        MsgProblem += "Failed to move: " + File_Name + "\n";
                                        MsgProblem += "\n\nQuit";

                                        FlexibleMessageBox.Show(MsgProblem, MsgProblemTitle);
                                        return "error";
                                    }

                                    // unlink
                                    UnlinkAccFromManifestStringSteamID64(ExtractFileNameSteamID64_v2);
                                    MsgProblem += " - unlinked from manifest ok\n";

                                    AtTheEndOpenFolder = "OpenFolder";
                                }
                            }
                            MsgProblem += " \n- You will have to import these accounts manually.\n File > Import > From " + SteamFileExtension + " > select file\n\n";

                            // set manifest
                            if (Encrypted)
                            {
                                MsgProblem += "- manifest, encrypted was set to false, now you can use you unencrypted accounts\n\n";
                                Encrypted = false;
                                Save();
                            }

                        }
                    }
                    #endregion // Fix - Detected accounts encrypted v2 & unencrypted  inside maFiles folder > end

                } //  if(AccountTypes > 1) > end



                // ** Detected accounts not encrypted & manifest is encrypted
                //**************************
                //##########################
                #region Detected accounts encrypted v2 & unencrypted
                if (TotalNotEncryptedAccounts > 0 && TotalEncryptedAccounts_v2 == 0)
                {
                    if (Encrypted)
                    {
                        MsgProblem += "Manifest value is encrypted=true and your accounts are unencrypted\n";
                        MsgProblem += "- " + ManifestFileNameWithExt + ", encrypted was set to false, now you can use you unencrypted accounts\n\n";
                        Encrypted = false;
                        Save();
                    }
                }
                #endregion



                // ** Detected accounts encrypted v2 & manifest is not encrypted
                //**************************
                //##########################
                #region Detected accounts encrypted v2 & unencrypted
                if (TotalNotEncryptedAccounts == 0 && TotalEncryptedAccounts_v2 > 0)
                {
                    if (!Encrypted)
                    {
                        MsgProblem += "Manifest value is encrypted=false and your accounts are encrypted\n";
                        MsgProblem += "- " + ManifestFileNameWithExt + ", encrypted was set to true, now you can use you encrypted accounts\n\n";
                        Encrypted = true;
                        Save();
                    }
                }
                #endregion


                if (MsgProblem == "") { } else { FlexibleMessageBox.Show(MsgProblem, MsgProblemTitle); }

            }
            #endregion // Fix problems > end


            if (AtTheEndOpenFolder != "") { return AtTheEndOpenFolder; }
            return "ok";
        }
        // Verify Accounts > end





        private static Manifest _generateNewManifest(bool scanDir = false)
        {
            // No directory means no manifest file anyways.
            Manifest newManifest = new Manifest();
            newManifest.AppCanBeStartedMultipleTimes = false;
            newManifest.Encrypted = false;
            newManifest.ConfirmationCheckingInterval = 30;
            newManifest.ConfirmationsPeriodicChecking = false;
            newManifest.AutoConfirmMarketTransactions = false;
            newManifest.AutoConfirmTrades = false;

            newManifest.SendAppStatus = false;
            newManifest.SendAppStatusInterval = 1;
            newManifest.SendAppStatusToAddress = "http://localhost:4141/";
            newManifest.SendAppNo = 1;

            newManifest.Entries = new List<ManifestEntry>();
            newManifest.FirstRun = true;

            // Take a pre-manifest version and generate a manifest for it.
            if (scanDir)
            {
                string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/";
                if (Directory.Exists(maDir))
                {
                    DirectoryInfo dir = new DirectoryInfo(maDir);
                    var files = dir.GetFiles();


                    // Check if accounts are encrypted or not 
                    #region inventory
                    int TotalAccountsNotEncrypted = 0;
                    int TotalAccountsEncrypted = 0;

                    foreach (var file in files)
                    {
                        if (file.Extension != SteamFileExtension) continue;
                        string contents = File.ReadAllText(file.FullName);

                        if (contents.Contains("<next_string_of_data>"))
                        {
                            string[] fileTextPart = Regex.Split(contents, "<next_string_of_data>");
                            string FileEncryptionVersion = null;
                            try { FileEncryptionVersion = fileTextPart[0]; } catch (Exception) { }

                            if (FileEncryptionVersion == Get_FileEncryption_Version())
                            {
                                TotalAccountsEncrypted++;
                            }
                        }
                        else
                        {
                            TotalAccountsNotEncrypted++;
                        }
                    }
                    #endregion // inventory > end

                    // not encrypted accounts 
                    // or encrypted & not encrypted - add only non encrypted & the encrypted ones will be moved by "VeryfyIfAccountsAreOk"
                    #region not encrypted accounts
                    if (TotalAccountsNotEncrypted > 0 && TotalAccountsEncrypted == 0 || TotalAccountsNotEncrypted > 0 && TotalAccountsEncrypted > 0)
                    {
                        foreach (var file in files)
                        {
                            if (file.Extension != SteamFileExtension) continue;

                            string contents = File.ReadAllText(file.FullName);
                            try
                            {
                                SteamGuardAccount account = JsonConvert.DeserializeObject<SteamGuardAccount>(contents);
                                ManifestEntry newEntry = new ManifestEntry()
                                {
                                    Filename = file.Name,
                                    SteamID = account.Session.SteamID
                                };
                                newManifest.Entries.Add(newEntry);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    #endregion // not encrypted accounts > end

                    // encrypted accounts
                    #region encrypted accounts
                    else if (TotalAccountsNotEncrypted == 0 && TotalAccountsEncrypted > 0)
                    {
                        string passKey = newManifest.PromptForPassKey(true);

                        if (passKey == null )
                        {
                            MessageBox.Show("That passkey is invalid.\nQuit");
                            Application.Exit();
                        }

                        foreach (var file in files)
                        {
                            if (file.Extension != SteamFileExtension) continue;

                            string contents = File.ReadAllText(file.FullName);


                            // extract data
                            string[] fileTextPart = Regex.Split(contents, "<next_string_of_data>");
                            string FileEncryptionVersion = null;
                            string Salt = null;
                            string IV = null;
                            string SteamData = null;


                            try { FileEncryptionVersion = fileTextPart[0]; } catch (Exception) { }

                            if (FileEncryptionVersion == Get_FileEncryption_Version())
                            {
                                try { Salt = fileTextPart[1]; } catch (Exception) { }

                                try { IV = fileTextPart[2]; } catch (Exception) { }

                                try { SteamData = fileTextPart[3]; } catch (Exception) { }

                                if (Salt == null || IV == null || SteamData == null){
                                }
                                else
                                {
                                    string decryptedText = null;
                                    try {  decryptedText = FileEncryptor.DecryptData(passKey, Salt, IV, SteamData); } catch (Exception) { }

                                    if (decryptedText != null)
                                    {
                                        try
                                        {
                                            SteamGuardAccount account = JsonConvert.DeserializeObject<SteamGuardAccount>(decryptedText);
                                            ManifestEntry newEntry = new ManifestEntry()
                                            {
                                                Filename = file.Name,
                                                SteamID = account.Session.SteamID
                                            };
                                            newManifest.Entries.Add(newEntry);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                        newManifest.Encrypted = true;
                    }
                    #endregion // encrypted accounts > end


                    if (newManifest.Entries.Count > 0)
                    {
                        newManifest.Save();
                    }

                }
            }


            if (newManifest.Save())
            {
                return newManifest;
            }

            return null;
        }

        public class IncorrectPassKeyException : Exception { }
        public class ManifestNotEncryptedException : Exception { }

        public string PromptForPassKey(bool CreateManifestList = false)
        {
            if (CreateManifestList == false)
            {
                if (!this.Encrypted)
                {
                    throw new ManifestNotEncryptedException();
                }
            }

            bool passKeyValid = false;
            string passKey = null;

            if (CreateManifestList == false)
            {
                while (!passKeyValid)
                {
                    InputForm passKeyForm = new InputForm("Please enter your encryption passkey.", true);
                    passKeyForm.ShowDialog();
                    if (!passKeyForm.Canceled)
                    {
                        passKey = passKeyForm.txtBox.Text;

                        passKeyValid = this.VerifyPasskey(passKey);
                        if (!passKeyValid)
                        {
                            MessageBox.Show("That passkey is invalid.");
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else {
                InputForm passKeyForm = new InputForm("Please enter your encryption passkey.\nTo recreate the manifest list.", true);
                passKeyForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                passKeyForm.ShowDialog();
                if (!passKeyForm.Canceled) { passKey = passKeyForm.txtBox.Text; } else { return null; }
            }
            return passKey;
        }

        public string PromptSetupPassKey(string initialPrompt = "Enter passkey, or hit cancel to remain unencrypted.\nEncryption v2")
        {
            InputForm newPassKeyForm = new InputForm(initialPrompt);
            newPassKeyForm.ShowDialog();
            if (newPassKeyForm.Canceled || newPassKeyForm.txtBox.Text.Length == 0)
            {
                MessageBox.Show("WARNING: You chose to not encrypt your files. Doing so imposes a security risk for yourself. If an attacker were to gain access to your computer, they could completely lock you out of your account and steal all your items.");
                return null;
            }

            InputForm newPassKeyForm2 = new InputForm("Confirm new passkey.");
            newPassKeyForm2.ShowDialog();
            if (newPassKeyForm2.Canceled)
            {
                MessageBox.Show("WARNING: You chose to not encrypt your files. Doing so imposes a security risk for yourself. If an attacker were to gain access to your computer, they could completely lock you out of your account and steal all your items.");
                return null;
            }

            string newPassKey = newPassKeyForm.txtBox.Text;
            string confirmPassKey = newPassKeyForm2.txtBox.Text;

            if (newPassKey != confirmPassKey)
            {
                MessageBox.Show("Passkeys do not match.");
                return null;
            }

            if (!this.ChangeEncryptionKey(null, newPassKey))
            {
                MessageBox.Show("Unable to set passkey.");
                return null;
            }
            else
            {
                MessageBox.Show("Passkey successfully set.");
            }

            return newPassKey;
        }

        public SteamAuth.SteamGuardAccount[] GetAllAccounts(string passKey = null, int limit = -1)
        {
            if (passKey == null && this.Encrypted) return new SteamGuardAccount[0];
            string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/";

            List<SteamAuth.SteamGuardAccount> accounts = new List<SteamAuth.SteamGuardAccount>();
            foreach (var entry in this.Entries)
            {
                string fileText = File.ReadAllText(maDir + entry.Filename);
                if (this.Encrypted)
                {
                    string[] fileTextPart = Regex.Split(fileText, "<next_string_of_data>");
                    string FileEncryptionVersion = null;
                    string Salt = null;
                    string IV = null;

                    try { FileEncryptionVersion = fileTextPart[0]; } catch (Exception) { }
                    if (FileEncryptionVersion == Get_FileEncryption_Version())
                    {
                        try { Salt = fileTextPart[1]; } catch (Exception) { }
                        try { IV = fileTextPart[2]; } catch (Exception) { }
                        try { fileText = fileTextPart[3]; } catch (Exception) { fileText = null; }

                        string decryptedText = null;
                        if (Salt == null || IV == null || fileText == null)
                        {
                            return new SteamGuardAccount[0];
                        }
                        else {
                            try { decryptedText = FileEncryptor.DecryptData(passKey, Salt, IV, fileText); } catch (Exception) { }
                        }
                        if (decryptedText == null) { return new SteamGuardAccount[0]; } else { fileText = decryptedText; }

                    }
                    else {
                        return new SteamGuardAccount[0];
                    }
                }

                if (fileText != null)
                {
                    try
                    {
                        var account = JsonConvert.DeserializeObject<SteamAuth.SteamGuardAccount>(fileText);

                        if (account == null) continue;
                        accounts.Add(account);

                        if (limit != -1 && limit >= accounts.Count)
                            break;

                    }
                    catch (Exception) { }
                }
            }

            return accounts.ToArray();
        }

        public bool ChangeEncryptionKey(string oldKey, string newKey)
        {
            if (this.Encrypted)
            {
                if (!this.VerifyPasskey(oldKey))
                {
                    return false;
                }
            }
            bool toEncrypt = newKey != null;

            string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/";
            for (int i = 0; i < this.Entries.Count; i++)
            {
                ManifestEntry entry = this.Entries[i];
                string filename = maDir + entry.Filename;
                if (!File.Exists(filename)) continue;

                string fileContents = File.ReadAllText(filename);
                if (this.Encrypted)
                {
                    string[] fileTextPart = Regex.Split(fileContents, "<next_string_of_data>");

                    string FileEncryptionVersion = null;
                    try { FileEncryptionVersion = fileTextPart[0]; } catch (Exception) { }

                    if (FileEncryptionVersion == Get_FileEncryption_Version())
                    {
                        string Salt = null;
                        string IV = null;

                        try { Salt = fileTextPart[1]; } catch (Exception) { }
                        try { IV = fileTextPart[2]; } catch (Exception) { }
                        try { fileContents = fileTextPart[3]; } catch (Exception) { fileContents = null; }

                        if (Salt == null || IV == null || fileContents == null) {
                            return false;
                        }
                        else {
                            try { fileContents = FileEncryptor.DecryptData(oldKey, Salt, IV, fileContents); } catch (Exception) { return false; }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                string newSalt = null;
                string newIV = null;
                string toWriteFileContents = fileContents;

                if (toEncrypt)
                {
                    newSalt = FileEncryptor.GetRandomSalt();
                    newIV = FileEncryptor.GetInitializationVector();
                    toWriteFileContents = Get_FileEncryption_Version() + "<next_string_of_data>" + newSalt + "<next_string_of_data>" + newIV + "<next_string_of_data>" + FileEncryptor.EncryptData(newKey, newSalt, newIV, fileContents);
                }

                File.WriteAllText(filename, toWriteFileContents);
            }

            this.Encrypted = toEncrypt;

            this.Save();
            return true;
        }

        public bool VerifyPasskey(string passkey)
        {
            if (!this.Encrypted || this.Entries.Count == 0) return true;

            var accounts = this.GetAllAccounts(passkey, 1);
            return accounts != null && accounts.Length == 1;
        }


        public string MoveAccountToRemovedFromManifest(SteamGuardAccount account = null, string SteamID64 = null, string Folder = null)
        {
            string EntryFileName = null;
            string FolderName = "accounts removed from manifest";
            if (Folder != null) { FolderName = Folder; }


            // use SteamGuardAccount account
            if (SteamID64 == null)
            {
                ManifestEntry entry = (from e in this.Entries where e.SteamID == account.Session.SteamID select e).FirstOrDefault();

                // If something never existed, did you do what they asked?
                if (entry == null) { }
                else {
                    EntryFileName = entry.Filename;
                }
            }
            // use SteamID64
            else {
                EntryFileName = SteamID64 + SteamFileExtension;
            }


            if (EntryFileName == null)
            {
                return "error";
            }
            else {
                string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles;
                string filenameWithPath = maDir + "/" + EntryFileName;

                string MoveToFolder = Manifest.GetExecutableDir() + "/" + FolderName;
                string MoveToFilenameWithPath = MoveToFolder + "/" + EntryFileName;


                // set check if file to move exists:
                if (File.Exists(filenameWithPath)) {
                    // check if dir exists:
                    if (!Directory.Exists(MoveToFolder)){
                        Directory.CreateDirectory(MoveToFolder);
                    }

                    // if file already exists rename it
                    if (File.Exists(MoveToFilenameWithPath))
                    {
                        string ExtractFileName = Path.GetFileNameWithoutExtension(MoveToFilenameWithPath);
                        string ExtractFolderPath = Path.GetDirectoryName(MoveToFilenameWithPath);
                        string timeStamp1 = DateTime.Now.ToString();
                        string timeStamp2 = timeStamp1.Replace("/", "-");
                        string timeStamp3 = timeStamp2.Replace(":", ".");
                        string NewFileName = ExtractFolderPath + @"\" + timeStamp3 + " old " + ExtractFileName + SteamFileExtension; 

                        try{
                            System.IO.File.Move(MoveToFilenameWithPath, NewFileName);
                        }
                        catch(Exception){ return "error"; }
                    }

                    // move file
                    try{
                        File.Move(filenameWithPath, MoveToFilenameWithPath);
                        return "ok"; // DONE >>
                    } catch (Exception) { return "error"; }
                }
                else {
                    return "error";
                }
            }
            return "error";
        }

        public bool RemoveAccount(SteamGuardAccount account, bool deletemaFile = true)
        {
            ManifestEntry entry = (from e in this.Entries where e.SteamID == account.Session.SteamID select e).FirstOrDefault();
            if (entry == null) return true; // If something never existed, did you do what they asked?

            string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/";
            string filenameWithPath = maDir + entry.Filename;

            this.Entries.Remove(entry);

            if (this.Entries.Count == 0)
            {
                this.Encrypted = false;
            }

            if (this.Save() && deletemaFile)
            {
                try
                {
                    File.Delete(filenameWithPath);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        public bool UnlinkAccFromManifestStringSteamID64(string account)
        {
            ManifestEntry entry = (from e in this.Entries where e.SteamID.ToString() == account select e).FirstOrDefault();
            if (entry == null) return true; // If something never existed, did you do what they asked?

            string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/";
            string filename = maDir + entry.Filename;
            this.Entries.Remove(entry);
            Save();
            return false;
        }



        public bool SaveAccount(SteamGuardAccount account, bool encrypt, string passKey = null)
        {

            if (encrypt && String.IsNullOrEmpty(passKey)) return false;
            if (!encrypt && this.Encrypted) return false;

            string salt = null;
            string iV = null;
            string jsonAccount = "";

            try
            {
                jsonAccount = JsonConvert.SerializeObject(account);
            }
            catch (Exception)
            {
                return false;
            }

            if (encrypt)
            {
                salt = FileEncryptor.GetRandomSalt();
                iV = FileEncryptor.GetInitializationVector();
                string encrypted = FileEncryptor.EncryptData(passKey, salt, iV, jsonAccount);
                if (encrypted == null) return false;
                jsonAccount = Get_FileEncryption_Version() + "<next_string_of_data>" + salt + "<next_string_of_data>" + iV + "<next_string_of_data>" + encrypted;
            }

            string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/";
            string filename = account.Session.SteamID.ToString() + SteamFileExtension;

            ManifestEntry newEntry = new ManifestEntry()
            {
                SteamID = account.Session.SteamID,
                Filename = filename
            };

            bool foundExistingEntry = false;
            for (int i = 0; i < this.Entries.Count; i++)
            {
                if (this.Entries[i].SteamID == account.Session.SteamID)
                {
                    this.Entries[i] = newEntry;
                    foundExistingEntry = true;
                    break;
                }
            }

            if (!foundExistingEntry)
            {
                this.Entries.Add(newEntry);
            }

            bool wasEncrypted = this.Encrypted;
            this.Encrypted = encrypt || this.Encrypted;

            if (!this.Save())
            {
                this.Encrypted = wasEncrypted;
                return false;
            }

            try
            {
                File.WriteAllText(maDir + filename, jsonAccount);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Save()
        {
            string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/";
            string filename = maDir + ManifestFileNameWithExt;
            if (!Directory.Exists(maDir))
            {
                try
                {
                    Directory.CreateDirectory(maDir);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            try
            {
                string contents = JsonConvert.SerializeObject(this);
                File.WriteAllText(filename, contents);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void RecomputeExistingEntries()
        {
            List<ManifestEntry> newEntries = new List<ManifestEntry>();
            string maDir = Manifest.GetExecutableDir() + "/" + FolderNameSteamFiles + "/";

            foreach (var entry in this.Entries)
            {
                string filename = maDir + entry.Filename;
                if (File.Exists(filename))
                {
                    newEntries.Add(entry);
                }
            }

            this.Entries = newEntries;

            if (this.Entries.Count == 0)
            {
                this.Encrypted = false;
            }
        }

        public void MoveEntry(int from, int to)
        {
            if (from < 0 || to < 0 || from > Entries.Count || to > Entries.Count - 1) return;
            ManifestEntry sel = Entries[from];
            Entries.RemoveAt(from);
            Entries.Insert(to, sel);
            Save();
        }

        public class ManifestEntry
        {
            [JsonProperty("filename")]
            public string Filename { get; set; }

            [JsonProperty("steamid")]
            public ulong SteamID { get; set; }
        }
    }
}

