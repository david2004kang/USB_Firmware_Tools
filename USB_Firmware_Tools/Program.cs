using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;

namespace USB_Firmware_Tools
{
    public class TimestampedTraceListener : TextWriterTraceListener
    {
        public TimestampedTraceListener(TextWriter writer) : base(writer) { }
        public TimestampedTraceListener(Stream stream) : base(stream) { }
        public TimestampedTraceListener(string fileName) : base(fileName) { }

        public override void WriteLine(string message)
        {
            base.WriteLine($"{DateTime.Now:HH:mm:ss}: {message}");
        }

        public override void Write(string message)
        {
            base.Write($"{DateTime.Now:HH:mm:ss}: {message}");
        }
    }

    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        static readonly string mutexName = "Global\\MyUniqueMutexNameForSingleInstanceAppSOHODock";

        [STAThread]
        static void Main()
        {
            // Add Trace Listeners
            Trace.Listeners.Add(new TimestampedTraceListener(Console.Out));
            Trace.Listeners.Add(new TimestampedTraceListener(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt")));
            Trace.AutoFlush = true;

            Trace.WriteLine("Starting USB Firmware Tools");
            using (Mutex mutex = new Mutex(false, mutexName, out bool createdNew))
            {
                // Check if the mutex was newly created
                if (createdNew)
                {
                    AdminRelauncher();
                    // Run the application
                    ApplicationConfiguration.Initialize();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Trace.WriteLine("Exiting USB Firmware Tools");
        }

        static void AdminRelauncher()
        {
            bool IsRunAsAdmin = false;
            try
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(id);
                IsRunAsAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                IsRunAsAdmin = false;
            }

            if (!IsRunAsAdmin)
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Assembly.GetEntryAssembly().CodeBase;

                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("This program must be run as an administrator! \n\n" + ex.ToString());
                }
            }
        }
    }
}