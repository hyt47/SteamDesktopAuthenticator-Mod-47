namespace Steam_Desktop_Authenticator
{

    partial class ConfirmationForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmationForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnAcceptConfirmation = new System.Windows.Forms.Button();
            this.btnDenyConfirmation = new System.Windows.Forms.Button();
            this.btnRefreshConfirmation = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ConfirmNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConfirmInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConfirmCheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ConfirmationType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCheckSelected = new System.Windows.Forms.Button();
            this.btnUncheckSelected = new System.Windows.Forms.Button();
            this.btnCheckMarket = new System.Windows.Forms.Button();
            this.btnUncheckMarket = new System.Windows.Forms.Button();
            this.btnCheckTrades = new System.Windows.Forms.Button();
            this.btnUncheckTrades = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.labelCounter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAcceptConfirmation
            // 
            this.btnAcceptConfirmation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAcceptConfirmation.BackColor = System.Drawing.Color.Honeydew;
            this.btnAcceptConfirmation.Enabled = false;
            this.btnAcceptConfirmation.FlatAppearance.BorderColor = System.Drawing.Color.DarkSeaGreen;
            this.btnAcceptConfirmation.FlatAppearance.BorderSize = 2;
            this.btnAcceptConfirmation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcceptConfirmation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAcceptConfirmation.Location = new System.Drawing.Point(269, 434);
            this.btnAcceptConfirmation.Name = "btnAcceptConfirmation";
            this.btnAcceptConfirmation.Size = new System.Drawing.Size(202, 39);
            this.btnAcceptConfirmation.TabIndex = 3;
            this.btnAcceptConfirmation.Text = "Accept Selected";
            this.btnAcceptConfirmation.UseVisualStyleBackColor = false;
            this.btnAcceptConfirmation.Click += new System.EventHandler(this.btnAcceptConfirmation_Click);
            // 
            // btnDenyConfirmation
            // 
            this.btnDenyConfirmation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDenyConfirmation.BackColor = System.Drawing.Color.OldLace;
            this.btnDenyConfirmation.Enabled = false;
            this.btnDenyConfirmation.FlatAppearance.BorderColor = System.Drawing.Color.BurlyWood;
            this.btnDenyConfirmation.FlatAppearance.BorderSize = 2;
            this.btnDenyConfirmation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDenyConfirmation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDenyConfirmation.Location = new System.Drawing.Point(6, 434);
            this.btnDenyConfirmation.Name = "btnDenyConfirmation";
            this.btnDenyConfirmation.Size = new System.Drawing.Size(202, 39);
            this.btnDenyConfirmation.TabIndex = 4;
            this.btnDenyConfirmation.Text = "Cancel Selected";
            this.btnDenyConfirmation.UseVisualStyleBackColor = false;
            this.btnDenyConfirmation.Click += new System.EventHandler(this.btnDenyConfirmation_Click);
            // 
            // btnRefreshConfirmation
            // 
            this.btnRefreshConfirmation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshConfirmation.BackColor = System.Drawing.Color.AliceBlue;
            this.btnRefreshConfirmation.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            this.btnRefreshConfirmation.FlatAppearance.BorderSize = 2;
            this.btnRefreshConfirmation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshConfirmation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshConfirmation.Location = new System.Drawing.Point(352, 6);
            this.btnRefreshConfirmation.Name = "btnRefreshConfirmation";
            this.btnRefreshConfirmation.Size = new System.Drawing.Size(120, 39);
            this.btnRefreshConfirmation.TabIndex = 5;
            this.btnRefreshConfirmation.Text = "Refresh";
            this.btnRefreshConfirmation.UseVisualStyleBackColor = false;
            this.btnRefreshConfirmation.Click += new System.EventHandler(this.btnRefreshConfirmation_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ConfirmNo,
            this.ConfirmInfo,
            this.ConfirmCheckBox,
            this.ConfirmationType});
            this.dataGridView1.Location = new System.Drawing.Point(6, 22);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(339, 406);
            this.dataGridView1.TabIndex = 2;
            // 
            // ConfirmNo
            // 
            this.ConfirmNo.HeaderText = "No.";
            this.ConfirmNo.Name = "ConfirmNo";
            this.ConfirmNo.ReadOnly = true;
            this.ConfirmNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ConfirmNo.Width = 30;
            // 
            // ConfirmInfo
            // 
            this.ConfirmInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ConfirmInfo.HeaderText = "Confirmations:";
            this.ConfirmInfo.Name = "ConfirmInfo";
            this.ConfirmInfo.ReadOnly = true;
            // 
            // ConfirmCheckBox
            // 
            this.ConfirmCheckBox.HeaderText = "";
            this.ConfirmCheckBox.Name = "ConfirmCheckBox";
            this.ConfirmCheckBox.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ConfirmCheckBox.Width = 22;
            // 
            // ConfirmationType
            // 
            this.ConfirmationType.HeaderText = "";
            this.ConfirmationType.Name = "ConfirmationType";
            this.ConfirmationType.ReadOnly = true;
            this.ConfirmationType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ConfirmationType.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Account: ...";
            // 
            // btnCheckSelected
            // 
            this.btnCheckSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckSelected.BackColor = System.Drawing.SystemColors.Control;
            this.btnCheckSelected.FlatAppearance.BorderColor = System.Drawing.Color.RosyBrown;
            this.btnCheckSelected.FlatAppearance.BorderSize = 2;
            this.btnCheckSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckSelected.Location = new System.Drawing.Point(351, 68);
            this.btnCheckSelected.Name = "btnCheckSelected";
            this.btnCheckSelected.Size = new System.Drawing.Size(120, 31);
            this.btnCheckSelected.TabIndex = 6;
            this.btnCheckSelected.Text = "Select Highlighted";
            this.btnCheckSelected.UseVisualStyleBackColor = false;
            this.btnCheckSelected.Click += new System.EventHandler(this.btnCheckSelected_Click);
            // 
            // btnUncheckSelected
            // 
            this.btnUncheckSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUncheckSelected.BackColor = System.Drawing.SystemColors.Control;
            this.btnUncheckSelected.FlatAppearance.BorderColor = System.Drawing.Color.DarkSeaGreen;
            this.btnUncheckSelected.FlatAppearance.BorderSize = 2;
            this.btnUncheckSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUncheckSelected.Location = new System.Drawing.Point(351, 103);
            this.btnUncheckSelected.Name = "btnUncheckSelected";
            this.btnUncheckSelected.Size = new System.Drawing.Size(120, 31);
            this.btnUncheckSelected.TabIndex = 7;
            this.btnUncheckSelected.Text = "Unselect Highlighted";
            this.btnUncheckSelected.UseVisualStyleBackColor = false;
            this.btnUncheckSelected.Click += new System.EventHandler(this.btnUncheckSelected_Click);
            // 
            // btnCheckMarket
            // 
            this.btnCheckMarket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckMarket.BackColor = System.Drawing.SystemColors.Control;
            this.btnCheckMarket.FlatAppearance.BorderColor = System.Drawing.Color.RosyBrown;
            this.btnCheckMarket.FlatAppearance.BorderSize = 2;
            this.btnCheckMarket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckMarket.Location = new System.Drawing.Point(351, 153);
            this.btnCheckMarket.Name = "btnCheckMarket";
            this.btnCheckMarket.Size = new System.Drawing.Size(120, 31);
            this.btnCheckMarket.TabIndex = 8;
            this.btnCheckMarket.Text = "Select Market";
            this.btnCheckMarket.UseVisualStyleBackColor = false;
            this.btnCheckMarket.Click += new System.EventHandler(this.btnCheckMarket_Click);
            // 
            // btnUncheckMarket
            // 
            this.btnUncheckMarket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUncheckMarket.BackColor = System.Drawing.SystemColors.Control;
            this.btnUncheckMarket.FlatAppearance.BorderColor = System.Drawing.Color.DarkSeaGreen;
            this.btnUncheckMarket.FlatAppearance.BorderSize = 2;
            this.btnUncheckMarket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUncheckMarket.Location = new System.Drawing.Point(351, 188);
            this.btnUncheckMarket.Name = "btnUncheckMarket";
            this.btnUncheckMarket.Size = new System.Drawing.Size(120, 31);
            this.btnUncheckMarket.TabIndex = 9;
            this.btnUncheckMarket.Text = "Unselect Market";
            this.btnUncheckMarket.UseVisualStyleBackColor = false;
            this.btnUncheckMarket.Click += new System.EventHandler(this.btnUncheckMarket_Click);
            // 
            // btnCheckTrades
            // 
            this.btnCheckTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckTrades.BackColor = System.Drawing.SystemColors.Control;
            this.btnCheckTrades.FlatAppearance.BorderColor = System.Drawing.Color.RosyBrown;
            this.btnCheckTrades.FlatAppearance.BorderSize = 2;
            this.btnCheckTrades.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckTrades.Location = new System.Drawing.Point(351, 239);
            this.btnCheckTrades.Name = "btnCheckTrades";
            this.btnCheckTrades.Size = new System.Drawing.Size(120, 31);
            this.btnCheckTrades.TabIndex = 10;
            this.btnCheckTrades.Text = "Select Trades";
            this.btnCheckTrades.UseVisualStyleBackColor = false;
            this.btnCheckTrades.Click += new System.EventHandler(this.btnCheckTrades_Click);
            // 
            // btnUncheckTrades
            // 
            this.btnUncheckTrades.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUncheckTrades.BackColor = System.Drawing.SystemColors.Control;
            this.btnUncheckTrades.FlatAppearance.BorderColor = System.Drawing.Color.DarkSeaGreen;
            this.btnUncheckTrades.FlatAppearance.BorderSize = 2;
            this.btnUncheckTrades.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUncheckTrades.Location = new System.Drawing.Point(351, 274);
            this.btnUncheckTrades.Name = "btnUncheckTrades";
            this.btnUncheckTrades.Size = new System.Drawing.Size(120, 31);
            this.btnUncheckTrades.TabIndex = 11;
            this.btnUncheckTrades.Text = "Unselect Trades";
            this.btnUncheckTrades.UseVisualStyleBackColor = false;
            this.btnUncheckTrades.Click += new System.EventHandler(this.btnUncheckTrades_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUncheckAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnUncheckAll.FlatAppearance.BorderColor = System.Drawing.Color.DarkSeaGreen;
            this.btnUncheckAll.FlatAppearance.BorderSize = 2;
            this.btnUncheckAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUncheckAll.Location = new System.Drawing.Point(351, 360);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(120, 31);
            this.btnUncheckAll.TabIndex = 13;
            this.btnUncheckAll.Text = "Unselect All";
            this.btnUncheckAll.UseVisualStyleBackColor = false;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnCheckAll.FlatAppearance.BorderColor = System.Drawing.Color.RosyBrown;
            this.btnCheckAll.FlatAppearance.BorderSize = 2;
            this.btnCheckAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckAll.Location = new System.Drawing.Point(351, 325);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(120, 31);
            this.btnCheckAll.TabIndex = 12;
            this.btnCheckAll.Text = "Select All";
            this.btnCheckAll.UseVisualStyleBackColor = false;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // labelCounter
            // 
            this.labelCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCounter.Location = new System.Drawing.Point(351, 409);
            this.labelCounter.Name = "labelCounter";
            this.labelCounter.Size = new System.Drawing.Size(120, 16);
            this.labelCounter.TabIndex = 14;
            this.labelCounter.Text = "...";
            this.labelCounter.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ConfirmationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(477, 479);
            this.Controls.Add(this.labelCounter);
            this.Controls.Add(this.btnUncheckAll);
            this.Controls.Add(this.btnCheckAll);
            this.Controls.Add(this.btnUncheckTrades);
            this.Controls.Add(this.btnCheckTrades);
            this.Controls.Add(this.btnUncheckMarket);
            this.Controls.Add(this.btnCheckMarket);
            this.Controls.Add(this.btnUncheckSelected);
            this.Controls.Add(this.btnCheckSelected);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnRefreshConfirmation);
            this.Controls.Add(this.btnDenyConfirmation);
            this.Controls.Add(this.btnAcceptConfirmation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(493, 518);
            this.Name = "ConfirmationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Trade Confirmations List";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnAcceptConfirmation;
        private System.Windows.Forms.Button btnDenyConfirmation;
        private System.Windows.Forms.Button btnRefreshConfirmation;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCheckSelected;
        private System.Windows.Forms.Button btnUncheckSelected;
        private System.Windows.Forms.Button btnCheckMarket;
        private System.Windows.Forms.Button btnUncheckMarket;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConfirmNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConfirmInfo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ConfirmCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConfirmationType;
        private System.Windows.Forms.Button btnCheckTrades;
        private System.Windows.Forms.Button btnUncheckTrades;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Label labelCounter;
    }
}

