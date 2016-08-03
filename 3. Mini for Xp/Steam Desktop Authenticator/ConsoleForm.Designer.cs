namespace Steam_Desktop_Authenticator {
    partial class ConsoleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleForm));
            this.reachTextBoxConsole = new System.Windows.Forms.RichTextBox();
            this.btnStartStopConsoleUpdates = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // reachTextBoxConsole
            // 
            this.reachTextBoxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reachTextBoxConsole.BackColor = System.Drawing.SystemColors.MenuText;
            this.reachTextBoxConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.reachTextBoxConsole.Cursor = System.Windows.Forms.Cursors.Default;
            this.reachTextBoxConsole.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reachTextBoxConsole.ForeColor = System.Drawing.SystemColors.Window;
            this.reachTextBoxConsole.Location = new System.Drawing.Point(5, 2);
            this.reachTextBoxConsole.Name = "reachTextBoxConsole";
            this.reachTextBoxConsole.ReadOnly = true;
            this.reachTextBoxConsole.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.reachTextBoxConsole.Size = new System.Drawing.Size(917, 348);
            this.reachTextBoxConsole.TabIndex = 1;
            this.reachTextBoxConsole.Text = "";
            // 
            // btnStartStopConsoleUpdates
            // 
            this.btnStartStopConsoleUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartStopConsoleUpdates.Location = new System.Drawing.Point(1, 351);
            this.btnStartStopConsoleUpdates.Name = "btnStartStopConsoleUpdates";
            this.btnStartStopConsoleUpdates.Size = new System.Drawing.Size(922, 23);
            this.btnStartStopConsoleUpdates.TabIndex = 2;
            this.btnStartStopConsoleUpdates.Text = "Stop Console Updates ( to read the console )";
            this.btnStartStopConsoleUpdates.UseVisualStyleBackColor = true;
            this.btnStartStopConsoleUpdates.Click += new System.EventHandler(this.btnStartStopConsoleUpdates_Click);
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(924, 375);
            this.Controls.Add(this.btnStartStopConsoleUpdates);
            this.Controls.Add(this.reachTextBoxConsole);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 200);
            this.Name = "ConsoleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Console";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConsoleForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.RichTextBox reachTextBoxConsole;
        private System.Windows.Forms.Button btnStartStopConsoleUpdates;
    }
}