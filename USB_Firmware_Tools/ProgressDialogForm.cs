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
        }

        public int GetMaxProgress()
        {
            return progressBar1.Maximum;
        }

        public void GetProgress(int progress)
        {
            if(progress <= progressBar1.Maximum)
               progressBar1.Value = progress;
        }
        
        public void SetProgress(int progress)
        {
            progressBar1.Value = progress;
        }

    }
}
