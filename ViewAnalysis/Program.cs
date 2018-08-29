using System;
using System.Windows.Forms;
using NLog;

namespace ViewAnalysis
{
    internal static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Logger.Info("Project Analysis has started");

            Application.ThreadException += Application_ThreadException;
            Application.ApplicationExit += Application_ApplicationExit;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ViewAnalysis());
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Logger.Info("Project Analysis has stopped");

            try
            {
                LogManager.Flush();

            }
            catch (Exception)
            {
                // LogManager.Flush in log4net was proven to throw Exceptions during app shtudown
            }

            try
            {
                LogManager.Shutdown();
            }
            catch (Exception)
            {
                // LogManager.Shutdown in log4net was proven to throw Exceptions during app shtudown
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logger.Error(e.Exception, "Error caught in a thread");
        }
    }
}
