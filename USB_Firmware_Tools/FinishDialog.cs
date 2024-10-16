using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USB_Firmware_Tools
{
    public partial class FinishDialog : Form
    {
        private bool bExitProgram = false;
        public FinishDialog()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public void SetEnableButton1(bool enable)
        {
            button1.Enabled = enable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bExitProgram)
            {
                Form1.bBypassExitCheck = true;
                Application.Exit();
            }
            else
            {
                this.Close();
            }
        }
        
        public void SetMessage1(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                label1.Text = message;
            }
        }

        public void SetMessage2(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                label2.Text = message;
            }
        }

    }
}
