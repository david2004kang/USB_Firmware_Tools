using System.Diagnostics;
using System.Reflection;
using System.IO.Compression;
using System.Text;
using System;
using System.Text.RegularExpressions;

namespace USB_Firmware_Tools
{
    public partial class Form1 : Form
    {
        public static bool bBypassExitCheck = false;
        private static string tempPath = Path.Combine(Path.GetTempPath(), "working");
        private ProgressDialog? mProgressDialog;
        private Progress<int> progress;
        Random random = new Random();
        public System.Timers.Timer? progressTimer;
        private int nProgressStep = 1;
        public Process? process;

        public void TryDeleteDirectory(string path)
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

        private void UpdateProgress(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (mProgressDialog != null)
            {
                int currentProgress = mProgressDialog.GetProgress();
                if (currentProgress < 100)
                {
                    currentProgress += nProgressStep;
                    mProgressDialog.SetProgress(currentProgress);
                }
                else
                {
                    mProgressDialog.SetMessage2("Firmware update finalizing...");
                }
            }
        }

        private void InitializeProgressTimer(int nTimeGap = 1000)
        {
            progressTimer = new System.Timers.Timer(nTimeGap);
            progressTimer.Elapsed += UpdateProgress;
            progressTimer.AutoReset = true;
        }

        public Form1()
        {
            InitializeComponent();
            progress = new Progress<int>(value => mProgressDialog?.SetProgress(value));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {
                if (!bBypassExitCheck)
                {
                    CustomExitDialog exitDialog = new CustomExitDialog();
                    exitDialog.StartPosition = FormStartPosition.Manual; // 手動設置位置
                    exitDialog.Location = new Point(
                        this.Location.X + (this.Width - exitDialog.Width) / 2,
                        this.Location.Y + (this.Height - exitDialog.Height) / 2
                    );
                    var result = exitDialog.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                TryDeleteDirectory(tempPath);
            }
            catch (Exception ex)
            {
            }
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
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Failed to open the help document: {ex.Message}");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show(
                    "The BIN file path is empty, please assign BIN file path before clicking on Update button.",
                    "Error, no BIN file assigned", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show(
                    "The BIN file doesn't exist on this path, please check BIN file path before click on Update button.",
                    "Error, BIN file didn't exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetGUI(false);

            var filePath = Path.Combine(tempPath, "dmcSecureUpdate.exe");

            if (!Path.Exists(filePath))
            {
                var nMax = random.Next(3, 8);
                await Task.Run(() => ExtractEmbeddedZip(progress, nMax));
            }

            InitializeProgressTimer(2300);
            progressTimer?.Start();
            string output = await RunProcessAsync(filePath, "-x");
            string lastFourDigits;
            Trace.WriteLine(output);
            Match match = Regex.Match(output, @"0{14}([0-9a-fA-F]{4})\s*$");
            if (match.Success)
            {
                lastFourDigits = match.Groups[1].Value;
                Trace.WriteLine($"Last four digits: {lastFourDigits}");
                mProgressDialog?.SetMessage2($"Updating firmware from version: {lastFourDigits},");
            }
            //mProgressDialog?.SetMessage1("Start updating firmware...");
            output = await RunProcessAsync(filePath, $"-f \"{textBox1.Text}\"");
            Trace.WriteLine(output);
            progressTimer?.Stop();
            progressTimer?.Dispose();
            progressTimer = null;

            string checkString = "DMC Firmware Update Successful";
            if (output.Contains(checkString))
            {
                mProgressDialog?.Close();
                mProgressDialog = null;
                FinishDialog? finishDialog = new FinishDialog();
                finishDialog.StartPosition = FormStartPosition.Manual; // 手動設置位置
                finishDialog.Location = new Point(
                    this.Location.X + (this.Width - finishDialog.Width) / 2,
                    this.Location.Y + (this.Height - finishDialog.Height) / 2
                );
                //finishDialog?.SetMessage1("Firmware update successful!");
                //finishDialog?.SetMessage2("New version updated\nPlease power cycle your Dock to apply\nthe new firmware.");
                finishDialog?.SetEnableButton1(true);
                finishDialog?.ShowDialog();
                finishDialog?.Close();
                finishDialog = null;
            }
            else
            {
                mProgressDialog?.Close();
                mProgressDialog = null;
                MessageBox.Show("Firmware update failed. ", "Error, update firmware failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SetGUI(true);
        }

        public void SetGUI(bool bEnable)
        {
            button1.Enabled = bEnable;
            button2.Enabled = bEnable;
            button3.Enabled = bEnable;
            button4.Enabled = bEnable;
            textBox1.Enabled = bEnable;
            if (bEnable)
            {
                mProgressDialog?.Close();
                mProgressDialog = null;
            }
            else
            {
                if (mProgressDialog == null)
                    mProgressDialog = new ProgressDialog();
                mProgressDialog.ShowDialog();
            }
        }

        private void ExtractEmbeddedZip(IProgress<int>? progress = null, int maxProgress = 0)
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
                        int progressPercentage = (int)((double)currentEntry / totalEntries * maxProgress);
                        if (progress != null)
                        {
                            progress.Report(progressPercentage);
                        }
                    }
                }
            }
        }

        private async Task<string> RunProcessAsync(string fileName, string arguments)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process = new Process { StartInfo = processStartInfo };

            var output = new StringBuilder();
            var error = new StringBuilder();

            process.OutputDataReceived += (sender, e) => { if (e.Data != null) output.AppendLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { if (e.Data != null) error.AppendLine(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                return $"Error: {error.ToString()}";
            }

            return output.ToString();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            SetGUI(false);
            mProgressDialog?.SetMessage1("Start checking DMC version...");

            var filePath = Path.Combine(tempPath, "dmcSecureUpdate.exe");

            if (!Path.Exists(filePath))
            {
                await Task.Run(() => ExtractEmbeddedZip(progress, random.Next(85, 95)));
            }
            string output = await RunProcessAsync(filePath, "-x");
            Trace.WriteLine(output);
            string checkString = "Read Customer Version command called";
            if (output.Contains(checkString))
            {
                mProgressDialog?.Close();
                mProgressDialog = null;

                string lastFourDigits = string.Empty;
                Match match = Regex.Match(output, @"0{14}([0-9a-fA-F]{4})\s*$");
                if (match.Success)
                {
                    lastFourDigits = match.Groups[1].Value;
                    Trace.WriteLine($"Last four digits: {lastFourDigits}");
                }

                string PID = string.Empty, VID = string.Empty;
                match = Regex.Match(output, @"VID:\s0x([0-9a-fA-F]{4}),\sPID:\s0x([0-9a-fA-F]{4})");
                if (match.Success)
                {
                    PID = match.Groups[1].Value;
                    VID = match.Groups[2].Value;
                    Trace.WriteLine($"VID: {VID}, PID: {PID}");
                }
                FinishDialog? finishDialog = new FinishDialog();
                finishDialog?.SetMessage1("DMC version check");
                finishDialog?.SetMessage2($"This DMC customer version: {lastFourDigits}\nPID: 0x{PID}, VID: 0x{VID}");
                finishDialog?.SetEnableButton1(true);
                finishDialog?.ShowDialog();
                finishDialog?.Close();
                finishDialog = null;
            }
            else
            {
                mProgressDialog?.Close();
                mProgressDialog = null;
                SetGUI(true);
                MessageBox.Show("Detect DMC version fail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            mProgressDialog?.Close();
            mProgressDialog = null;
            SetGUI(true);
        }
    }
}
