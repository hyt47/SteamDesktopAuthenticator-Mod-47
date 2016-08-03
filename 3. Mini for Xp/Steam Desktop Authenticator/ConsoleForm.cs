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
    public partial class ConsoleForm : Form
    {
        public static bool UpdateConsole = true;
        public ConsoleForm()
        {
            InitializeComponent();
            reachTextBoxConsole.ForeColor = Color.Silver;
        }

        public void SetConsoleText(string output, string Status) {
            if (UpdateConsole == true)
            {
                int max_lines = 200;

                string AddPrefix = "";
                if (Status == "ConsoleStatus_Task")
                {
                    // Task Started
                    AddPrefix = "► ";
                }
                else if(Status == "ConsoleStatus_TaskImportant")
                {
                    // Task Started
                    AddPrefix = "►►► ";
                }
                else if (Status == "ConsoleStatus_Return")
                {
                    // Task Ended
                    AddPrefix = "◄ ";
                }
                else if (Status == "ConsoleStatus_ReturnFaildFixIt")
                {
                    // Task Ended
                    AddPrefix = "◄ FAILED ► FIXING: ";
                }
                else if (Status == "ConsoleStatus_ReturnWarning")
                {
                    // Task Ended
                    AddPrefix = "◄ █ WARNING: ";
                }
                else if (Status == "ConsoleStatus_ReturnError")
                {
                    // Task Ended
                    AddPrefix = "◄ ██ ERROR: ";
                }
                else if (Status == "ConsoleStatus_Confirmed")
                {
                    // Confirming
                    AddPrefix = "◄◄◄►►► ";
                }
                else if (Status == "ConsoleStatus_Info")
                {
                    // Notification
                    AddPrefix = "(-_-) INFO: ";
                }
                else if (Status == "ConsoleStatus_Error")
                {
                    // error
                    AddPrefix = "██ ERROR: ";
                }

                string TimeStamp = DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff tt | "); //"hh:mm:ss dd.MM.yyy"
                reachTextBoxConsole.AppendText(TimeStamp + AddPrefix + output + "\r\n");



                if (reachTextBoxConsole.Lines.Length > max_lines)
                {
                    string[] newLines = new string[max_lines];

                    Array.Copy(reachTextBoxConsole.Lines, 1, newLines, 0, max_lines);

                    reachTextBoxConsole.Lines = newLines;
                }

                reachTextBoxConsole.SelectionStart = reachTextBoxConsole.Text.Length;
                reachTextBoxConsole.ScrollToCaret();
            }
        }


        private void ConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true; // this cancels the close event.
            }
            else
            {
                this.Close();
            }

        }

        private void btnStartStopConsoleUpdates_Click(object sender, EventArgs e)
        {
            if (UpdateConsole == true)
            {
                UpdateConsole = false;
                btnStartStopConsoleUpdates.Text = "Start Console Updates";
                SetConsoleText("Console Updates are stopped by the User", "ConsoleStatus_Info");
            }
            else {
                UpdateConsole = true;
                btnStartStopConsoleUpdates.Text = "Stop Console Updates ( to read the console )";
                SetConsoleText("Console Updates are enabled", "ConsoleStatus_Info");
            }
        }
    }
}
