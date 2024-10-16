using System.Diagnostics;

namespace USB_Firmware_Tools
{

    public class ProgressDialog
    {
        private ProgressDialogForm progressDialogForm;
        public ProgressDialog()
        {
            progressDialogForm = new ProgressDialogForm();
        }

        public bool IsVisible()
        {
            return progressDialogForm.Visible;
        }

        public void ShowDialog()
        {
            progressDialogForm.TopMost = true;
            progressDialogForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            progressDialogForm.Show();
        }

        public string GetMessage1()
        {
            if (progressDialogForm.Visible)
                return progressDialogForm.GetLabel1Text();
            else
                return "";
        }

        public void SetMessage1(string message)
        {
            if (progressDialogForm.Visible)
                progressDialogForm.Invoke(new Action(() => progressDialogForm.SetMessage1(message)));
        }

        public string GetMessage2()
        {
            if (progressDialogForm.Visible)
                return progressDialogForm.GetLabel2Text();
            else
                return "";
        }

        public void SetMessage2(string message)
        {
            if (progressDialogForm.Visible)
                progressDialogForm.Invoke(new Action(() => progressDialogForm.SetMessage2(message)));
        }

        public int GetMaxProgress() 
        {
            return progressDialogForm.GetMaxProgress();
        }

        public int GetProgress()
        {
            return progressDialogForm.GetProgress();
        }
        public void SetProgress(int nProgress)
        {
            try
            {
                if (progressDialogForm.Visible)
                    progressDialogForm.Invoke(new Action(() => progressDialogForm.SetProgress(nProgress)));
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Some exception: {ex.Message}");
            }
        }

        public void SetProgressTopMost(bool bTopMost)
        {
            progressDialogForm.TopMost = bTopMost;
            progressDialogForm.Update();
        }

        public void Close()
        {
            progressDialogForm.Close();
        }
    }
}
