using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

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
        [STAThread]
        static void Main()
        {
            // Add Trace Listeners
            Trace.Listeners.Add(new TimestampedTraceListener(Console.Out));
            Trace.Listeners.Add(new TimestampedTraceListener(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt")));
            Trace.AutoFlush = true;

            Trace.WriteLine("Starting USB Firmware Tools");
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
            Trace.WriteLine("Exiting USB Firmware Tools");
        }
    }
}