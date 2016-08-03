using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteamAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace Steam_Desktop_Authenticator
{
    public partial class ImportAccountForm : Form
    {
        private Manifest mManifest;

        public ImportAccountForm()
        {
            InitializeComponent();
            this.mManifest = Manifest.GetManifest();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

            // check if data already added is encrypted
            if (mManifest.Encrypted == true)
            {
                MessageBox.Show("You can't import an " + Manifest.SteamFileExtension + " because the existing account in the app is encrypted.\nDecrypt it and try again.");
            }
            else {
                string ImportUsingEncryptionPasskey = txtBox.Text; // read passkey

                // Open file browser > to select the file
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                // Set filter options and filter index.
                openFileDialog1.Filter = Manifest.FolderNameSteamFiles + " (" + Manifest.SteamFileExtension + ")|*" + Manifest.SteamFileExtension + "|All Files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.Multiselect = false;

                // Call the ShowDialog method to show the dialog box.
                DialogResult userClickedOK = openFileDialog1.ShowDialog();



                // Process input if the user clicked OK.
                //----------------------------------------
                if (userClickedOK == DialogResult.OK)
                {
                    // Open the selected file to read.
                    System.IO.Stream fileStream = openFileDialog1.OpenFile();
                    string fileContents = null;

                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream)) { fileContents = reader.ReadToEnd(); }
                    fileStream.Close();

                    string fullFilePath = openFileDialog1.FileName;

                    ImportVerifyAndDecideNextStep(fileContents, ImportUsingEncryptionPasskey, fullFilePath);

                }
            }
            this.Close();
        }

        private void btnImportMultipleAcc_Click(object sender, EventArgs e)
        {
            ImportMultipleAccounts();
        }



        // Import Verify & Decide next Step
        //###################################
        private string ImportVerifyAndDecideNextStep(string fileContents, string ImportUsingEncryptionPasskey, string fullFilePath, bool ShowInfoMsg = true) {
            // declare
            bool fileImported_IsEncrypted = false;
            string fileImported_EncryptionVersion = null;


            // check if file is encrypted // search in string
            //################################################
            // check for new encryption
            if (fileContents.Contains("<next_string_of_data>"))
            {
                fileImported_IsEncrypted = true;

                // extract data
                string[] fileTextPart = Regex.Split(fileContents, "<next_string_of_data>");
                try
                {
                    fileImported_EncryptionVersion = fileTextPart[0];
                }
                catch (Exception)
                {
                    if (ShowInfoMsg == true) { MessageBox.Show("Invalid File encryption version!"); }
                    return "error";
                }
            }

            // check if file is not encrypted
            int SearchFoundMaches = 0;
            if (fileImported_IsEncrypted == false)
            {
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
                if (SearchFoundMaches > 4) { } else { fileImported_IsEncrypted = true; fileImported_EncryptionVersion = "v1"; } // else Old Encryption
            }


            // Next
            //#######
            if (fileImported_IsEncrypted == false) { return ImportAccount(fileContents, false, ShowInfoMsg); }  // IMPORT >>>// Import non encrypted file
            else if (fileImported_IsEncrypted == true)
            {
                if (ImportUsingEncryptionPasskey == null || ImportUsingEncryptionPasskey == "")
                {
                    if (ShowInfoMsg == true) { MessageBox.Show("Import cancelled!\n\nThis file is encrypted and you didn't provide encryption passkey"); }
                    return "error";
                }
                else {
                    if (fileImported_EncryptionVersion == null || fileImported_EncryptionVersion == "")
                    {
                        if (ShowInfoMsg == true) { MessageBox.Show("Import cancelled!"); }
                        return "error";
                    }
                    else if (fileImported_EncryptionVersion == "v1")
                    { // old encryption

                        // extract folder path
                        string fileName = Path.GetFileName(fullFilePath);
                        string FolderPath = fullFilePath.Replace(fileName, "");

                        return ImportAccountEncrypted_v1(fileContents, FolderPath, ImportUsingEncryptionPasskey, fileName, ShowInfoMsg); // IMPORT >>> encrypted file v1
                    }
                    else if (fileImported_EncryptionVersion == "v2")
                    {
                        return ImportAccountEncrypted_v2(fileContents, ImportUsingEncryptionPasskey, ShowInfoMsg); // IMPORT >>> encrypted file v2
                    }
                    else {
                        if (ShowInfoMsg == true) { MessageBox.Show("I can't import this file!\n Because the file encryption version is not compatible  with this app!"); }
                        return "error";
                    }
                }
            }

            return "error";
        }



        // Import non encrypted file
        //################################
        private string ImportAccount(string AccountData, bool AccountData_FromEncrypted = false, bool ShowInfoMsg = true)
        {
            // Copy the contents of the config dir to the new config dir
            string currentPath = Manifest.GetExecutableDir();

            // Create config dir if we don't have it
            if (!Directory.Exists(currentPath + "/" + Manifest.FolderNameSteamFiles))
            {
                Directory.CreateDirectory(currentPath + "/" + Manifest.FolderNameSteamFiles);
            }


            // declare
            ulong maFile_Session_SteamID = 0;
            SteamGuardAccount mCurrentAccount = null;

            try
            {
                mCurrentAccount = JsonConvert.DeserializeObject<SteamGuardAccount>(AccountData);
            }
            catch (Exception ex)
            {
                if (AccountData_FromEncrypted == false)
                {
                    if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n(Failed to Deserialize data)"); }
                }
                else {
                    if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\nYour encryption passkey might be incorrect!\n\n(Failed to Deserialize data)"); }
                }
                return "error";
            }


            try
            {
                maFile_Session_SteamID = mCurrentAccount.Session.SteamID;
            }
            catch (Exception ex)
            {
                if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n(Failed to extract SteamID)"); }
                return "error";
            }


            if (maFile_Session_SteamID != 0)
            {
                bool SaveStatus = false;

                try
                {
                    SaveStatus = mManifest.SaveAccount(mCurrentAccount, false);
                }
                catch (Exception ex)
                {
                    if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n(Failed to Save, an error occurred)"); }
                    return "error";
                }

                if (SaveStatus == true)
                {
                    string MsgImportedOk = "Account Imported!\n";
                    if (AccountData_FromEncrypted == true) { MsgImportedOk += "Your account is now Decrypted."; }
                    if (ShowInfoMsg == true) { MessageBox.Show(MsgImportedOk); }

                    return "ok";
                }
                else {
                    if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n(Failed to save)"); }
                    return "error";
                }

            }
            else
            {
                if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n(Failed to read SteamID)"); }
                return "error";
            }

            return "error";
        }






        // Import encrypted file v1
        //################################
        #region Import encrypted file v1
        private string ImportAccountEncrypted_v1(string AccountData, string FolderPath, string Passkey, string ImportFileName, bool ShowInfoMsg = true)
        {
            // manifest.json
            //-----------------------------------
            #region manifest.json

            ImportManifest_maFile_Encrypted_v1 newImportManifest_maFile_Encrypted_v1 = new ImportManifest_maFile_Encrypted_v1();
            newImportManifest_maFile_Encrypted_v1.Encrypted = false;
            newImportManifest_maFile_Encrypted_v1.Entries = new List<ImportManifest_maFile_Encrypted_v1_entry>();

            string ImportManifest_maFile_Encrypted_v1_File = FolderPath + Manifest.ManifestFileNameWithExt;


            // declare
            ImportManifest_maFile_Encrypted_v1 account = null;
            bool Import_encrypted = false;
            string ImportManifest_maFile_Encrypted_v1_Contents = "";


            if (File.Exists(ImportManifest_maFile_Encrypted_v1_File))
            {
                try
                {
                    ImportManifest_maFile_Encrypted_v1_Contents = File.ReadAllText(ImportManifest_maFile_Encrypted_v1_File);
                }
                catch (Exception ex)
                {
                    if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n(Failed to read file)"); }
                    return "error";
                }
            }
            else
            {
                if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n\nYou or file has encryption v1, you need to have the " + Manifest.ManifestFileNameWithExt + " next to it.\n\nI can't find " + Manifest.ManifestFileNameWithExt + "!"); }
                return "error";
            }


            try
            {
                account = JsonConvert.DeserializeObject<ImportManifest_maFile_Encrypted_v1>(ImportManifest_maFile_Encrypted_v1_Contents);
            }
            catch (Exception)
            {
                if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n(Invalid content inside imported " + Manifest.ManifestFileNameWithExt + ")"); }
                return "error";
            }


            /* I don't think this is needed anymore

            bool Import_encrypted = false;
            try
            {
                Import_encrypted = account.Encrypted;
            }
            catch (Exception)
            {
                if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\nFailed to read Encryption status from imported " + Manifest.ManifestFileNameWithExt + "!"); }
                return "error";
            }

            if(bool Import_encrypted == false){
                if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\nImported " + Manifest.ManifestFileNameWithExt + " in not encrypted!"); }
                return "error";
            }
            */


            string FileName = "";
            string IV_Found = "";
            string Salt_Found = "";
            try
            {
                List<ImportManifest_maFile_Encrypted_v1> newEntries = new List<ImportManifest_maFile_Encrypted_v1>();
            }
            catch (Exception)
            {
                if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n(Failed to create manifest data list)"); }
                return "error";
            }


            try
            {
                foreach (var entry in account.Entries)
                {
                    try { FileName = entry.Filename; } catch (Exception) { }

                    if (ImportFileName == FileName)
                    {
                        try { IV_Found = entry.IV; } catch (Exception) { }
                        try { Salt_Found = entry.Salt; } catch (Exception) { }
                    }
                }
            }
            catch (Exception)
            {
                if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n(Failed to read data from imported " + Manifest.ManifestFileNameWithExt + ")"); }
                return "error";
            }




            bool FailedToReadDataFromManifest = false;

            string ErrorMsgManifest = "Import Failed!\n\n";
            ErrorMsgManifest += "Failed to read data from imported " + Manifest.ManifestFileNameWithExt + "\n";

            if (FileName == null || FileName == "") { FailedToReadDataFromManifest = true; ErrorMsgManifest += "- account filename not found\n"; }
            if (Salt_Found == null || Salt_Found == "") { FailedToReadDataFromManifest = true; ErrorMsgManifest += "- 'salt' data not found\n"; }
            if (IV_Found == null || IV_Found == "") { FailedToReadDataFromManifest = true; ErrorMsgManifest += "- 'iv' data not found\n"; }

            // verify data extracted
            if (FailedToReadDataFromManifest == false)
            {
                if (FileName == null || FileName == "") { ErrorMsgManifest += "- invalid mainifest data 'account filename' is empty\n"; FailedToReadDataFromManifest = true; }
                if (Salt_Found == null || Salt_Found == "") { ErrorMsgManifest += "- invalid 'Salt' is empty\n"; FailedToReadDataFromManifest = true; }
                if (IV_Found == null || IV_Found == "") { ErrorMsgManifest += "- invalid 'IV' is empty\n"; FailedToReadDataFromManifest = true; }

            }

            if (FailedToReadDataFromManifest == true)
            {
                if (ShowInfoMsg == true) { MessageBox.Show(ErrorMsgManifest); }
                return "error";
            }

            #endregion // manifest.json - END




            // Decrypt & Import
            //-----------------------------------
            #region Decrypt & Import

            string DecryptedAccount = null;
            try
            {
                DecryptedAccount = FileEncryptor.DecryptData(Passkey, Salt_Found, IV_Found, AccountData);
            }
            catch (Exception ex)
            {
                if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n\nYour encryption passkey might be incorrect!\n\n(Decryption Failed)"); }
                return "error";
            }

            // import
            return ImportAccount(DecryptedAccount, true, ShowInfoMsg);

            #endregion // Decrypt & Import - END


            return "error";
        }

        public class ImportManifest_maFile_Encrypted_v1
        {
            [JsonProperty("encrypted")]
            public bool Encrypted { get; set; }

            [JsonProperty("entries")]
            public List<ImportManifest_maFile_Encrypted_v1_entry> Entries { get; set; }
        }

        public class ImportManifest_maFile_Encrypted_v1_entry
        {
            [JsonProperty("encryption_iv")]
            public string IV { get; set; }

            [JsonProperty("encryption_salt")]
            public string Salt { get; set; }

            [JsonProperty("filename")]
            public string Filename { get; set; }

            [JsonProperty("steamid")]
            public ulong SteamID { get; set; }
        }
        #endregion
        //################################ Import encrypted file v1 - END






        // Import encrypted file v2
        //################################
        private string ImportAccountEncrypted_v2(string AccountData, string Passkey, bool ShowInfoMsg = true)
        {
            string Salt = null;
            string IV = null;
            string AccountString = null;

            // extract data
            //----------------
            string[] StringPart = Regex.Split(AccountData, "<next_string_of_data>");
            //FileEncryptionVersion = StringPart[0];

            bool ExtractStatusError = false;
            string ExtractErrorMessage = "Import Failed!\n\n";
            ExtractErrorMessage += "Failed to read data from your file\n";

            try { Salt = StringPart[1]; } catch (Exception) { ExtractErrorMessage += "- failed to read 'Salt'\n"; ExtractStatusError = true; }
            try { IV = StringPart[2]; } catch (Exception) { ExtractErrorMessage += "- failed to read 'iv'\n"; ExtractStatusError = true; }
            try { AccountString = StringPart[3]; } catch (Exception) { ExtractErrorMessage += "Failed to read 'account string'\n"; ExtractStatusError = true; }

            // verify data extracted
            if (ExtractStatusError == false)
            {
                if (Salt == null || Salt == "") { ExtractErrorMessage += "- invalid 'Salt' is empty\n"; ExtractStatusError = true; }
                if (IV == null || IV == "") { ExtractErrorMessage += "- invalid 'IV' is empty\n"; ExtractStatusError = true; }
                if (AccountString == null || AccountString == "") { ExtractErrorMessage += "- invalid 'account string' is empty\n"; ExtractStatusError = true; }
            }

            if (ExtractStatusError == true)
            {
                if (ShowInfoMsg == true) { MessageBox.Show(ExtractErrorMessage); }
                return "error";
            }

            // Decrypt
            //-----------
            string AccountDataDecrypted = null;
            try
            {
                AccountDataDecrypted = FileEncryptor.DecryptData(Passkey, Salt, IV, AccountString);
            }
            catch (Exception ex)
            {
                if (ShowInfoMsg == true) { MessageBox.Show("Import Failed!\n\nYour encryption passkey might be incorrect!\n\n(Decryption Failed)"); }
                return "error";
            }

            // import
            return ImportAccount(AccountDataDecrypted, true, ShowInfoMsg);



            return "error";
        }







        // Import multiple accounts
        //################################
        public void ImportMultipleAccounts()
        {

            // check if data already added is encrypted
            if (mManifest.Encrypted == true)
            {
                MessageBox.Show("You can't import because the existing account in the app is encrypted.\nDecrypt it and try again.");
            }
            else {

                // Let the user select the config dir
                FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
                folderBrowser.Description = "Select the folder of your old " + Manifest.MainAppName + " app";
                DialogResult userClickedOK = folderBrowser.ShowDialog();

                if (userClickedOK == DialogResult.OK)
                {

                    string ImportUsingEncryptionPasskey = txtBox.Text; // read passkey

                    string[] files;


                    string path = folderBrowser.SelectedPath;
                    string pathToImport = null;
                    if (Directory.Exists(path + "/" + Manifest.FolderNameSteamFiles))
                    {
                        path += "/" + Manifest.FolderNameSteamFiles;

                        int FilesFound = 0;
                        files = Directory.GetFiles(path, "*" + Manifest.SteamFileExtension);
                        foreach (String file in files) { FilesFound++; }


                        if (FilesFound == 0)
                        {
                            MessageBox.Show("The " + Manifest.FolderNameSteamFiles + " folder does not contain " + Manifest.SteamFileExtension + "'s.\n\nPlease select the location where you had\n" + Manifest.MainAppName + " installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else {
                            pathToImport = path;
                        }

                    }
                    else
                    {
                        int FilesFound = 0;
                        files = Directory.GetFiles(path, "*" + Manifest.SteamFileExtension);
                        foreach (string file in files) { FilesFound++; }

                        if (FilesFound == 0)
                        {
                            MessageBox.Show("This folder does not contain " + Manifest.FolderNameSteamFiles + " folder or " + Manifest.SteamFileExtension + "'s.\n\nPlease select the location where you had\n" + Manifest.MainAppName + " installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else {
                            pathToImport = path;
                        }
                    }


                    // Import all files from the old dir to the new one
                    if (pathToImport != null)
                    {
                        MessageBox.Show("Import Started!\n\nClick OK and wait a few seconds...");

                        string MsgImportStatus = "Import Ended!\n\nStatus:\n";

                        int CountFailedImports = 0;
                        int CountImportStatusUnknown = 0;

                        foreach (string file in files)
                        {
                            string fileContents = File.ReadAllText(file);

                            string fileName = Path.GetFileName(file);
                            MsgImportStatus += "- " + fileName;

                            string StatusImport = ImportVerifyAndDecideNextStep(fileContents, ImportUsingEncryptionPasskey, file, false);
                            if (StatusImport == "ok") {
                                MsgImportStatus += " - Ok\n";
                            }
                            else if (StatusImport == "error")
                            {
                                MsgImportStatus += " - Failed\n";
                                CountFailedImports++;
                            }
                            else {
                                MsgImportStatus += " - Unknown\n";
                                CountImportStatusUnknown++;
                            }
                        }

                        if (CountFailedImports > 0) {
                            MsgImportStatus += " \nFailed imports: " + CountFailedImports.ToString() + " \nYou can try and import these files manually by selecting the " + Manifest.SteamFileExtension;
                        }

                        if (CountImportStatusUnknown > 0)
                        {
                            MsgImportStatus += " \nImports status unknown: " + CountImportStatusUnknown.ToString() + " \n";
                            MsgImportStatus += "Check if these accounts were added to your app or in the " + Manifest.FolderNameSteamFiles + " folder.\n";
                            MsgImportStatus += "If you can't find them there you can try and import these files manually by selecting the " + Manifest.SteamFileExtension;
                        }

                        MessageBox.Show(MsgImportStatus);
                    }


                }
            }
            this.Close();
        }





        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Import_maFile_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

    }
}
