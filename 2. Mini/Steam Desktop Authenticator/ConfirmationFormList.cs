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
using System.Threading;

namespace Steam_Desktop_Authenticator
{
    public partial class ConfirmationForm : Form
    {
        public int ClickAcceptBtn = 0;
        public int ClickDeclineBtn = 0;
        public int ClickBtn_CountChecked = 0;

        public int CountConfirmations = 0;

        private SteamGuardAccount mCurrentAccount;

        private Confirmation[] Confirmations;
        private Confirmation mCurrentConfirmation = null;

        public ConfirmationForm(SteamGuardAccount account) {
            InitializeComponent();
            this.mCurrentAccount = account;
            btnDenyConfirmation.Enabled = btnAcceptConfirmation.Enabled = false;
            loadConfirmations();
        }


        private async void loadConfirmations(bool retry = false)
        {
            // reset btn Deny
            ClickDeclineBtn = 0;
            btnDenyConfirmation.Text = "Cancel Selected";
            btnDenyConfirmation.FlatAppearance.BorderColor = Color.BurlyWood;
            // reset btn Accept
            ClickAcceptBtn = 0;
            btnAcceptConfirmation.Text = "Accept Selected";
            btnAcceptConfirmation.FlatAppearance.BorderColor = Color.DarkSeaGreen;
            // Reset counter
            ClickBtn_CountChecked = 0;

            CountConfirmations = 0; var CountSelectedConfirmations = 0;

            dataGridView1.Rows.Clear();
            label1.Text = "Account: " + mCurrentAccount.AccountName;

            try { Confirmations = await mCurrentAccount.FetchConfirmationsAsync(); }
            catch (SteamGuardAccount.WGTokenInvalidException) {
                //MessageBox.Show("Error WGTokenInvalidException");
                if (retry) { this.Close(); return; }

                //TODO: catch only the relevant exceptions
                try { if (mCurrentAccount.RefreshSession()) { loadConfirmations(true); } } catch (Exception) { }
            }

            try {
                if (Confirmations.Length > 0) {
                    for (int i = 0; i < Confirmations.Length; i++) {
                        CountConfirmations++;

                        string ConfirmationType_ = "unknown";
                        string ConfirmationType = "Unknown Type";
                        if (Confirmations[i].ConfType == Confirmation.ConfirmationType.MarketSellTransaction) { ConfirmationType_ = "market"; ConfirmationType = "Market"; }
                        else if (Confirmations[i].ConfType == Confirmation.ConfirmationType.Trade) { ConfirmationType_ = "trade"; ConfirmationType = "Trade"; }

                        var indexR = dataGridView1.Rows.Add();
                        dataGridView1.Rows[indexR].Cells["ConfirmNo"].Value = (i + 1).ToString();

                        string TradeWith = ConfirmationType;
                        if (ConfirmationType == "Trade") { TradeWith += " offer ID: " + Confirmations[i].ID;  }
                        else if (ConfirmationType == "Market") { TradeWith += " ID: " + Confirmations[i].ID; }

                        // for DEBUG
                        //using (System.IO.TextWriter writer = System.IO.File.CreateText(System.AppDomain.CurrentDomain.BaseDirectory + @"\- debug pg.txt")) { writer.Write(Confirmations[i].AllData);  }

                            dataGridView1.Rows[indexR].Cells["ConfirmInfo"].Value = TradeWith;

                        dataGridView1.Rows[indexR].Cells["ConfirmCheckBox"].Value = false;
                        dataGridView1.Rows[indexR].Cells["ConfirmationType"].Value = ConfirmationType_;
                    }
                }
            } catch (Exception) { }

            this.btnAcceptConfirmation.Enabled = true;
            this.btnDenyConfirmation.Enabled = true;
            this.btnRefreshConfirmation.Enabled = true;

            btnCheckSelected.Enabled = true;
            btnUncheckSelected.Enabled = true;
            btnCheckMarket.Enabled = true;
            btnUncheckMarket.Enabled = true;
            btnCheckTrades.Enabled = true;
            btnUncheckTrades.Enabled = true;
            btnCheckAll.Enabled = true;
            btnUncheckAll.Enabled = true;

            labelCounter.Text = "... / " + CountConfirmations.ToString();
        }

        private async void btnAcceptConfirmation_Click(object sender, EventArgs e)
        {
            int Local_CountChecked = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows){
                if (Convert.ToBoolean(row.Cells["ConfirmCheckBox"].Value)) { Local_CountChecked++; }
            }
            if (ClickBtn_CountChecked == 0) { ClickBtn_CountChecked = Local_CountChecked; }
            else if (ClickBtn_CountChecked != Local_CountChecked) {
                // user changed selected items
                // Full reset
                //-------------------------------------
                // reset btn Deny
                btnDenyConfirmation.Text = "Cancel Selected";
                btnDenyConfirmation.FlatAppearance.BorderColor = Color.BurlyWood;
                // reset btn Accept
                ClickAcceptBtn = 0;
                btnAcceptConfirmation.Text = "Accept Selected";
                btnAcceptConfirmation.FlatAppearance.BorderColor = Color.DarkSeaGreen;
                // Reset counter
                ClickBtn_CountChecked = 0;

                // set
                ClickBtn_CountChecked = Local_CountChecked;
            }

            if (Local_CountChecked > 0){
                if (ClickAcceptBtn == 0) {
                    // reset
                    ClickDeclineBtn = 0;
                    btnDenyConfirmation.Text = "Cancel Selected";
                    btnDenyConfirmation.FlatAppearance.BorderColor = Color.BurlyWood;

                    // set
                    ClickAcceptBtn = 1;
                    btnAcceptConfirmation.Text = "Press Accept again";
                    btnAcceptConfirmation.FlatAppearance.BorderColor = Color.ForestGreen;

                    UpdateSelectedCounter();
                } else {

                    this.btnAcceptConfirmation.Enabled = false;
                    this.btnDenyConfirmation.Enabled = false;
                    this.btnRefreshConfirmation.Enabled = false;

                    btnCheckSelected.Enabled = false;
                    btnUncheckSelected.Enabled = false;
                    btnCheckMarket.Enabled = false;
                    btnUncheckMarket.Enabled = false;
                    btnCheckTrades.Enabled = false;
                    btnUncheckTrades.Enabled = false;
                    btnCheckAll.Enabled = false;
                    btnUncheckAll.Enabled = false;

                    Dictionary<SteamGuardAccount, List<Confirmation>> Do_confirmations_list = new Dictionary<SteamGuardAccount, List<Confirmation>>();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        var oCell = row.Cells["ConfirmCheckBox"] as DataGridViewCheckBoxCell;
                        bool bChecked = (null != oCell && null != oCell.Value && true == (bool)oCell.Value);
                        if (true == bChecked)
                        {
                            string ConfIndex = row.Cells[0].Value.ToString(); // -1
                            int ConfIndex_int = (Int32.Parse(ConfIndex) - 1);
                            //MessageBox.Show("Selected" + ConfIndex_int);

                            if (!Do_confirmations_list.ContainsKey(mCurrentAccount)) { Do_confirmations_list[mCurrentAccount] = new List<Confirmation>(); }
                            Do_confirmations_list[mCurrentAccount].Add(Confirmations[ConfIndex_int]);
                        }
                    }
            
                    if (Do_confirmations_list.Count > 0) {
                        var Do_confirmations = Do_confirmations_list[mCurrentAccount].ToArray();
                        mCurrentAccount.AcceptMultipleConfirmations(Do_confirmations);
                        Do_confirmations_list.Clear(); // Reset Dictionary after the data has been used
                    }

                    this.btnRefreshConfirmation.Enabled = true;
                    this.loadConfirmations();
                }
            }
        }




        private async void btnDenyConfirmation_Click(object sender, EventArgs e)
        {
            int Local_CountChecked = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows){
                if (Convert.ToBoolean(row.Cells["ConfirmCheckBox"].Value)) { Local_CountChecked++; }
            }
            if (ClickBtn_CountChecked == 0) { ClickBtn_CountChecked = Local_CountChecked; }
            else if (ClickBtn_CountChecked != Local_CountChecked) {
                // user changed selected items
                // Full reset
                //-------------------------------------
                // reset btn Deny
                ClickDeclineBtn = 0;
                btnDenyConfirmation.Text = "Cancel Selected";
                btnDenyConfirmation.FlatAppearance.BorderColor = Color.BurlyWood;
                // reset btn Accept
                ClickAcceptBtn = 0;
                btnAcceptConfirmation.Text = "Accept Selected";
                btnAcceptConfirmation.FlatAppearance.BorderColor = Color.DarkSeaGreen;

                // set
                ClickBtn_CountChecked = Local_CountChecked;
            }

            if (Local_CountChecked > 0)
            {
                if (ClickDeclineBtn == 0)
                {
                    // reset
                    ClickAcceptBtn = 0;
                    btnAcceptConfirmation.Text = "Accept Selected";
                    btnAcceptConfirmation.FlatAppearance.BorderColor = Color.DarkSeaGreen;

                    // set
                    ClickDeclineBtn = 1;
                    btnDenyConfirmation.Text = "Press Cancel again";
                    btnDenyConfirmation.FlatAppearance.BorderColor = Color.Orange;

                    UpdateSelectedCounter();
                }
                else
                {

                    this.btnAcceptConfirmation.Enabled = false;
                    this.btnDenyConfirmation.Enabled = false;
                    this.btnRefreshConfirmation.Enabled = false;

                    btnCheckSelected.Enabled = false;
                    btnUncheckSelected.Enabled = false;
                    btnCheckMarket.Enabled = false;
                    btnUncheckMarket.Enabled = false;
                    btnCheckTrades.Enabled = false;
                    btnUncheckTrades.Enabled = false;
                    btnCheckAll.Enabled = false;
                    btnUncheckAll.Enabled = false;
                    

                    Dictionary<SteamGuardAccount, List<Confirmation>> Do_confirmations_list = new Dictionary<SteamGuardAccount, List<Confirmation>>();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        var oCell = row.Cells["ConfirmCheckBox"] as DataGridViewCheckBoxCell;
                        bool bChecked = (null != oCell && null != oCell.Value && true == (bool)oCell.Value);
                        if (true == bChecked)
                        {
                            string ConfIndex = row.Cells[0].Value.ToString(); // -1
                            int ConfIndex_int = (Int32.Parse(ConfIndex) - 1);
                            //MessageBox.Show("Selected" + ConfIndex_int);

                            if (!Do_confirmations_list.ContainsKey(mCurrentAccount)) { Do_confirmations_list[mCurrentAccount] = new List<Confirmation>(); }
                            Do_confirmations_list[mCurrentAccount].Add(Confirmations[ConfIndex_int]);
                        }
                    }

                    if (Do_confirmations_list.Count > 0)
                    {
                        var Do_confirmations = Do_confirmations_list[mCurrentAccount].ToArray();
                        mCurrentAccount.DenyMultipleConfirmations(Do_confirmations);
                        Do_confirmations_list.Clear(); // Reset Dictionary after the data has been used
                    }

                    this.btnRefreshConfirmation.Enabled = true;
                    this.loadConfirmations();
                }
            }
        }


        private void btnRefreshConfirmation_Click(object sender, EventArgs e)
        {
            this.btnAcceptConfirmation.Enabled = false;
            this.btnDenyConfirmation.Enabled = false;
            this.btnRefreshConfirmation.Enabled = false;

            btnCheckSelected.Enabled = false;
            btnUncheckSelected.Enabled = false;
            btnCheckMarket.Enabled = false;
            btnUncheckMarket.Enabled = false;
            btnCheckTrades.Enabled = false;
            btnUncheckTrades.Enabled = false;
            btnCheckAll.Enabled = false;
            btnUncheckAll.Enabled = false;

            this.loadConfirmations();
        }



        // Check Selected
        private void btnCheckSelected_Click(object sender, EventArgs e){
            foreach (DataGridViewRow row in dataGridView1.Rows){
                if (row.Cells[0].Selected) {
                    if (Convert.ToBoolean(row.Cells["ConfirmCheckBox"].Value) == false) { row.Cells["ConfirmCheckBox"].Value = true; }
                }
            }
        }
        // Uncheck Selected
        private void btnUncheckSelected_Click(object sender, EventArgs e){
            foreach (DataGridViewRow row in dataGridView1.Rows){
                if (row.Cells[0].Selected) {
                    if (Convert.ToBoolean(row.Cells["ConfirmCheckBox"].Value)) { row.Cells["ConfirmCheckBox"].Value = false; }
                }
            }
        }
        // Check Market
        private void btnCheckMarket_Click(object sender, EventArgs e){
            foreach (DataGridViewRow row in dataGridView1.Rows){
                if (row.Cells["ConfirmationType"].Value.ToString() == "market") { row.Cells["ConfirmCheckBox"].Value = true; }
            }
        }
        // Uncheck Market
        private void btnUncheckMarket_Click(object sender, EventArgs e){
            foreach (DataGridViewRow row in dataGridView1.Rows){
                if (row.Cells["ConfirmationType"].Value.ToString() == "market") { row.Cells["ConfirmCheckBox"].Value = false; }
            }
        }
        // Check Trades
        private void btnCheckTrades_Click(object sender, EventArgs e){
            foreach (DataGridViewRow row in dataGridView1.Rows){
                if (row.Cells["ConfirmationType"].Value.ToString() == "trade") { row.Cells["ConfirmCheckBox"].Value = true; }
            }
        }
        // Uncheck Trades
        private void btnUncheckTrades_Click(object sender, EventArgs e){
            foreach (DataGridViewRow row in dataGridView1.Rows){
                if (row.Cells["ConfirmationType"].Value.ToString() == "trade") { row.Cells["ConfirmCheckBox"].Value = false; }
            }
        }
        // Check All
        private void btnCheckAll_Click(object sender, EventArgs e){
            foreach (DataGridViewRow row in dataGridView1.Rows){ row.Cells["ConfirmCheckBox"].Value = true; }
        }
        // Uncheck All
        private void btnUncheckAll_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow row in dataGridView1.Rows) { row.Cells["ConfirmCheckBox"].Value = false; }
        }


        // Update Selected Counter
        private void UpdateSelectedCounter(){
            int LocalCountSelected = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows){
                if (Convert.ToBoolean(row.Cells["ConfirmCheckBox"].Value) == true) { LocalCountSelected++; }
            }
            labelCounter.Text = LocalCountSelected + " / " + CountConfirmations;
        }
    }
}
