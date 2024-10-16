using System.Diagnostics;
using System.Reflection;
using System.IO.Compression;

namespace USB_Firmware_Tools
{
    public partial class Form1 : Form
    {
        public static bool bBypassExitCheck = false;
        private static string tempPath = Path.Combine(Path.GetTempPath(), "working");

        private void TryDeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Failed to delete directory {path}: {ex.Message}");
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!bBypassExitCheck)
            {
                if (MessageBox.Show("Firmware flashing did not finish.\nDo you really want to exit?", "Exit confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            TryDeleteDirectory(tempPath);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                Trace.WriteLine($"Opening file: {fileName}");
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Please select a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show("File does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //using (var progressDialog = new ProgressDialog())
            //{
            //    progressDialog.Show();

            //    var progress = new Progress<int>(value =>
            //    {
            //        progressDialog.UpdateProgress(value);
            //    });

            //    await Task.Run(() => ExtractEmbeddedZip(progress));

            //    progressDialog.Close();
            //}
        }

        private void ExtractEmbeddedZip(IProgress<int> progress)
        {
            TryDeleteDirectory(tempPath);
            Directory.CreateDirectory(tempPath);

            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string resourceName = $"{assemblyName}.dmcSecureUpdate.zip";

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    Trace.WriteLine($"Resource {resourceName} not found.");
                    return;
                }

                using (ZipArchive archive = new ZipArchive(stream))
                {
                    int totalEntries = archive.Entries.Count;
                    int currentEntry = 0;

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string destinationPath = Path.Combine(tempPath, entry.FullName);
                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            Directory.CreateDirectory(destinationPath);
                        }
                        else
                        {
                            entry.ExtractToFile(destinationPath, true);
                        }
                        currentEntry++;
                        int progressPercentage = (int)((double)currentEntry / totalEntries * 100);
                        progress.Report(progressPercentage);

                        //using (Stream entryStream = entry.Open())
                        //{
                        //    using (FileStream fileStream = new FileStream(entryPath, FileMode.Create))
                        //    {
                        //        entryStream.CopyTo(fileStream);
                        //    }
                        //}

                        progress.Report((int)((float)++currentEntry / totalEntries * 100));
                    }
                }
            }
        }
    }
}
