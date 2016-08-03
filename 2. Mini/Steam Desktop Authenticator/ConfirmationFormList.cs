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
using System.Diagnostics;
namespace Steam_Desktop_Authenticator
{
    public partial class ConfirmationForm : Form
    {
        private SteamGuardAccount mCurrentAccount;

        private Confirmation[] Confirmations;
        private Confirmation mCurrentConfirmation = null;

        public ConfirmationForm(SteamGuardAccount account)
        {
            InitializeComponent();
            this.mCurrentAccount = account;
            btnDenyConfirmation.Enabled = btnAcceptConfirmation.Enabled = false;
            loadConfirmations();
        }

        private void listAccounts_SelectedValueChanged(object sender, EventArgs e)
        {
            mCurrentConfirmation = null;

            if (listConfirmations.SelectedIndex == -1) return;

            // Triggered when list item is clicked
            for (int i = 0; i < Confirmations.Length; i++)
            {
                if (Confirmations[i].Description == (string)listConfirmations.Items[listConfirmations.SelectedIndex])
                {
                    mCurrentConfirmation = Confirmations[i];
                }
            }
            btnDenyConfirmation.Enabled = btnAcceptConfirmation.Enabled = mCurrentConfirmation != null;
        }

        private async void loadConfirmations(bool retry = false)
        {
            listConfirmations.Items.Clear();
            listConfirmations.SelectedIndex = -1;

            try
            {
                Confirmations = await mCurrentAccount.FetchConfirmationsAsync();
            }
            catch (SteamGuardAccount.WGTokenInvalidException e)
            {

                if (retry)
                {
                    this.Close();
                    return;
                }

                //TODO: catch only the relevant exceptions
                try
                {
                    if (mCurrentAccount.RefreshSession())
                    {
                        loadConfirmations(true);
                    }
                }
                catch (Exception) { }
            }

            try
            {
                if (Confirmations.Length > 0)
                {
                    for (int i = 0; i < Confirmations.Length; i++)
                    {
                        listConfirmations.Items.Add(Confirmations[i].Description);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private async void btnAcceptConfirmation_Click(object sender, EventArgs e)
        {
            this.btnAcceptConfirmation.Enabled = false;
            this.btnDenyConfirmation.Enabled = false;
            this.btnRefreshConfirmation.Enabled = false;

            if (mCurrentConfirmation == null) return;

            bool ConfirmClickStatus = await Task_AcceptConfirmation(mCurrentAccount, mCurrentConfirmation);

            if (ConfirmClickStatus){ MessageBox.Show("Confirmation successfully accepted.");
            } else { MessageBox.Show("Unable to accept confirmation."); }

            this.btnRefreshConfirmation.Enabled = true;
            this.loadConfirmations();
        }

        private Task<bool> Task_AcceptConfirmation(SteamGuardAccount mCurrentAccount, Confirmation mCurrentConfirmation){
            return Task.Run(() => {
                bool ConfirmStatus = mCurrentAccount.AcceptConfirmation(mCurrentConfirmation);
                return ConfirmStatus;
            });
        }

 


        private async void btnDenyConfirmation_Click(object sender, EventArgs e)
        {
            this.btnAcceptConfirmation.Enabled = false;
            this.btnDenyConfirmation.Enabled = false;
            this.btnRefreshConfirmation.Enabled = false;

            if (mCurrentConfirmation == null) return;

            bool DenyConfirmationClickStatus = await Task_DenyConfirmation(mCurrentAccount, mCurrentConfirmation);

            if (DenyConfirmationClickStatus) { MessageBox.Show("Confirmation successfully denied.");
            } else { MessageBox.Show("Unable to deny confirmation."); }

            this.btnRefreshConfirmation.Enabled = true;
            this.loadConfirmations();
        }
        private Task<bool> Task_DenyConfirmation(SteamGuardAccount mCurrentAccount, Confirmation mCurrentConfirmation){
            return Task.Run(() => {
                bool DenyConfirmStatus = mCurrentAccount.DenyConfirmation(mCurrentConfirmation);
                return false;
            });
        }


        private void btnRefreshConfirmation_Click(object sender, EventArgs e)
        {
            this.btnAcceptConfirmation.Enabled = false;
            this.btnDenyConfirmation.Enabled = false;
            this.loadConfirmations();
        }
        private void ConfirmationForm_Load(object sender, EventArgs e)
        {

        }
    }
}
