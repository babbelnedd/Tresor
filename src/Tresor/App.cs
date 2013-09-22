using System.Deployment.Application;

namespace Tresor
{
    using System;
    using System.Windows;

    using Logging;

    /// <summary>Grundgerüst der Anwendung.</summary>
    internal class App : Application
    {
        #region Public Methods and Operators

        /// <summary>Haupteinstiegspunkt der Anwendung.</summary>
        [STAThread]
        public static void Main()
        {
            var ad = ApplicationDeployment.CurrentDeployment;
            var newUpdateAvailable = ad.CheckForUpdate();

            if (newUpdateAvailable)
            {
                MessageBox.Show("Hallo Update!");
            }
            else
            {
                MessageBox.Show("Kein Update!");
            }

            RunApplication();
        }

        #endregion

        #region Methods

        /// <summary>Führt die Anwendung aus.</summary>
        private static void RunApplication()
        {
            Log.Trace("Bootstrapper wird initialisiert.");
            var bootstrapper = new Bootstrapper();
            Log.Trace("Anwendung wird gestartet.");
            Log.Run(bootstrapper.LoadApplication);
            Log.Trace("Anwendung wurde beendet.");
        }

        #endregion
    }
}