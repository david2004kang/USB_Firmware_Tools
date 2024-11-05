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
    public partial class ProgressDialogForm : Form
    {
        private string string2Backup = "";
        public ProgressDialogForm()
        {
            InitializeComponent();
        }

        public void SetMessage1(string message)
        {
            Label1.Text = message;
        }

        public void SetMessage2(string message)
        {
            Label2.Text = message;
            string2Backup = message;
        }

        public int GetMaxProgress()
        {
            return progressBar1.Maximum;
        }

        public int GetProgress()
        {
            return progressBar1.Value;
        }
        
        public void SetProgress(int progress)
        {
            if (progress <= progressBar1.Maximum)
            {
                if (string2Backup == "")
                {
                    if (!Label2.Text.EndsWith(")"))
                    {
                        string2Backup = Label2.Text;
                    }
                    else
                    {
                        var temp = Label2.Text.Split('(', 2);
                        string2Backup = temp[0];
                    }
                }
                progressBar1.Value = progress;
                Label2.Text = string2Backup + " (" + progress + "%)";
            }
        }
    }
}
