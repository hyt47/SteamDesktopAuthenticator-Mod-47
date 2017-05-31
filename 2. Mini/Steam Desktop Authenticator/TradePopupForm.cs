using SteamAuth;
using System;
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
    public partial class TradePopupForm : Form
    {
        private Manifest manifest;
        private SteamGuardAccount acc;
        private List<Confirmation> confirms = new List<Confirmation>();
        private bool deny2, accept2;

        public int TotalConfirmations = 0;
        public int CurrentConfirmationNo = -1;
        public int DisplayFirstSetOfConfirmations = 0;

        //Settings-declare
        public string Settings_PopupConfirmationBorder;

        public TradePopupForm()
        {
            InitializeComponent();
            lblStatus.Text = "";

            // read settings
            // Get latest manifest
            manifest = Manifest.GetManifest(true);
            Settings_PopupConfirmationBorder = manifest.PopupConfirmationBorder;

            if (Settings_PopupConfirmationBorder == "1" || Settings_PopupConfirmationBorder == "2") { } else { Settings_PopupConfirmationBorder = "1"; }


            if (Settings_PopupConfirmationBorder == "1")
            {
                this.Text = string.Empty;
                this.ControlBox = false;
            }
            else if (Settings_PopupConfirmationBorder == "2")
            {
                this.Text = string.Empty;
                this.ControlBox = false;
                this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            }
        }


        // hide resize mouse
        protected override void WndProc(ref Message message)
        {
            const int WM_NCHITTEST = 0x0084;

            if (message.Msg == WM_NCHITTEST)
                return;

            base.WndProc(ref message);
        }

        // Receive Data From Main Form
        public SteamGuardAccount ConfirmationsPopup_ForAcc {
            get { return acc; }
            set { acc = value; lblAccount.Text = acc.AccountName; }
        } 
        public Confirmation[] ConfirmationsPopup {
            get { return confirms.ToArray(); }
            set { confirms = new List<Confirmation>(value); }
        }

        private void TradePopupForm_Load(object sender, EventArgs e)
        {
            this.Location = (Point)Size.Subtract(Screen.GetWorkingArea(this).Size, this.Size);
        }

        private async void btnAccept_Click(object sender, EventArgs e)
        {
            if (deny2){
                // restore Deny btn if it was Activated
                deny2 = false;
                btnDeny.FlatAppearance.BorderColor = Color.BurlyWood;
                BtnPopupConfBack.Enabled = true;
                BtnPopupConfNext.Enabled = true;
            }

            if (!accept2)
            {
                // Allow user to confirm first
                lblStatus.Text = "Press Accept again to confirm";
                btnAccept.FlatAppearance.BorderColor = Color.ForestGreen;
                accept2 = true;
            }
            else
            {
                BtnPopupConfBack.Enabled = false;
                BtnPopupConfNext.Enabled = false;

                Program.ConsoleForm_Update.SetConsoleText("POPUP Confirmation: Accepting... " + confirms[CurrentConfirmationNo].Description, "ConsoleStatus_Confirmed");
                lblStatus.Text = "Accepting...";
                btnAccept.Enabled = false;
                btnDeny.Enabled = false;
                bool ConfirmClickStatus = await Task_AcceptConfirmation(acc, confirms[CurrentConfirmationNo]);

                if (ConfirmClickStatus == true) { confirms.RemoveAt(CurrentConfirmationNo); }
                CurrentConfirmationNo = -1; // show again first confirmation
                Reset();
            }
        }
        private Task<bool> Task_AcceptConfirmation(SteamGuardAccount acc, Confirmation CurrentConfirmation)
        {
            return Task.Run(() => {
                bool ConfirmStatus = acc.AcceptConfirmation(CurrentConfirmation);
                return ConfirmStatus;
            });
        }

        private async void btnDeny_Click(object sender, EventArgs e)
        {
            if (accept2){
                // restore Accept btn if it was Activated
                accept2 = false;
                btnAccept.FlatAppearance.BorderColor = Color.DarkSeaGreen;

                BtnPopupConfBack.Enabled = true;
                BtnPopupConfNext.Enabled = true;
            }

            if (!deny2)
            {
                lblStatus.Text = "Press Deny again to confirm";
                btnDeny.FlatAppearance.BorderColor = Color.Orange;
                deny2 = true;
            }
            else
            {
                BtnPopupConfBack.Enabled = false;
                BtnPopupConfNext.Enabled = false;

                Program.ConsoleForm_Update.SetConsoleText("POPUP Confirmation: Denying... " + confirms[CurrentConfirmationNo].Description, "ConsoleStatus_Confirmed");
                lblStatus.Text = "Denying...";
                btnAccept.Enabled = false;
                btnDeny.Enabled = false;
                bool DenyConfirmationClickStatus = await Task_DenyConfirmation(acc, confirms[CurrentConfirmationNo]);

                if (DenyConfirmationClickStatus == true) { confirms.RemoveAt(CurrentConfirmationNo); }
                CurrentConfirmationNo = -1; // show again first confirmation
                Reset();
            }
        }
        private Task<bool> Task_DenyConfirmation(SteamGuardAccount acc, Confirmation CurrentConfirmation){
            return Task.Run(() => {
                bool DenyConfirmStatus = acc.DenyConfirmation(CurrentConfirmation);
                return false;
            });
        }


        private void Reset(string MoveDirection = "front")
        {
            btnAccept.Enabled = true;
            btnDeny.Enabled = true;

            TotalConfirmations = confirms.Count;

            // this will trigger the GUI to show next confirmation
            if (MoveDirection == "front")
            {
                if (TotalConfirmations > 1 && (TotalConfirmations - 1) > CurrentConfirmationNo)
                {
                    CurrentConfirmationNo++;
                }
                else {
                    CurrentConfirmationNo = 0; // first confirmation
                }
            }
            else if (MoveDirection == "back")
            {
                if (TotalConfirmations > 1 && CurrentConfirmationNo > 1)
                {
                    CurrentConfirmationNo--;
                }
                else {
                    CurrentConfirmationNo = 0; // first confirmation
                }
            }
            else if (MoveDirection == "reset_to_first_confirmation") {
                CurrentConfirmationNo = 0; // first confirmation
            }

            deny2 = false;
            accept2 = false;
            btnDeny.FlatAppearance.BorderColor = Color.BurlyWood;
            btnAccept.FlatAppearance.BorderColor = Color.DarkSeaGreen;

            btnAccept.Text = "Accept";
            btnDeny.Text = "Deny";
            lblStatus.Text = "";

            if (TotalConfirmations == 0)
            {
                this.Hide();
            }
            else
            {

                string Trade_with_user = confirms[CurrentConfirmationNo].Description.Replace("Trade with ", "");
                lblDesc.Text = Trade_with_user;
            }
            if (TotalConfirmations == 1)
            {
                BtnPopupConfBack.Visible = false;
                BtnPopupConfNext.Visible = false;
            }
            else if (TotalConfirmations > 1)
            {
                if (CurrentConfirmationNo == 0) { BtnPopupConfBack.Visible = false; } else { BtnPopupConfBack.Visible = true; }
                if (CurrentConfirmationNo == (TotalConfirmations - 1)) { BtnPopupConfNext.Visible = false; } else { BtnPopupConfNext.Visible = true; }
            }


            labelConfirmationNo.Text = "no. " + (1 + CurrentConfirmationNo).ToString();
            label_Info.Text = "Total " + TotalConfirmations.ToString() + " confirmations waiting...";

            BtnPopupConfBack.Enabled = true;
            BtnPopupConfNext.Enabled = true;
        }


        private void TradePopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // show next confirmation
                Reset();
                DisplayFirstSetOfConfirmations = 0; // reset
                this.Hide();
                e.Cancel = true;
            }
            else
            {
                this.Close();
            }

        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayFirstSetOfConfirmations = 0; // reset
            this.Hide();
        }


        private void PopupConfNext_Click(object sender, EventArgs e)
        {
            // restore Deny btn if it was Activated
            deny2 = false;
            btnDeny.FlatAppearance.BorderColor = Color.BurlyWood;
            BtnPopupConfBack.Enabled = true;
            BtnPopupConfNext.Enabled = true;

            // restore Accept btn if it was Activated
            accept2 = false;
            btnAccept.FlatAppearance.BorderColor = Color.DarkSeaGreen;
            BtnPopupConfBack.Enabled = true;
            BtnPopupConfNext.Enabled = true;

            // show next confirmation
            Reset();
        }

        private void PopupConfBack_Click(object sender, EventArgs e)
        {
            // restore Deny btn if it was Activated
            deny2 = false;
            btnDeny.FlatAppearance.BorderColor = Color.BurlyWood;
            BtnPopupConfBack.Enabled = true;
            BtnPopupConfNext.Enabled = true;

            // restore Accept btn if it was Activated
            accept2 = false;
            btnAccept.FlatAppearance.BorderColor = Color.DarkSeaGreen;
            BtnPopupConfBack.Enabled = true;
            BtnPopupConfNext.Enabled = true;

            // show next confirmation
            Reset("back");
        }

        public void Popup()
        {
            Reset("reset_to_first_confirmation");
            this.Show();
        }
    }
}
