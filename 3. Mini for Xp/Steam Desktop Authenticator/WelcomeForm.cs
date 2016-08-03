using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steam_Desktop_Authenticator
{
    public partial class WelcomeForm : Form
    {
        private Manifest man;

        public WelcomeForm()
        {
            InitializeComponent();
            man = Manifest.GetManifest();
        }

        private void btnJustStart_Click(object sender, EventArgs e)
        {
            // Mark as not first run anymore
            man.FirstRun = false;
            man.Save();

            showMainForm();
        }

        private void btnImportConfig_Click(object sender, EventArgs e)
        {
            ImportAccountForm Import_Account_Form = new ImportAccountForm();
            Import_Account_Form.ShowDialog();
        }

        private void showMainForm()
        {
            this.Hide();
            new MainForm().Show();
        }

        private void btnAndroidImport_Click(object sender, EventArgs e)
        {
            int oldEntries = man.Entries.Count;

            new PhoneExtractForm().ShowDialog();

            if (man.Entries.Count > oldEntries)
            {
                // Mark as not first run anymore
                man.FirstRun = false;
                man.Save();
                showMainForm();
            }
        }
    }
}
