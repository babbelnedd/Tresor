namespace Tresor
{
    using System;
    using System.ComponentModel;
    using System.Deployment.Application;
    using System.Timers;
    using System.Windows;

    using Logging;

    /// <summary>Grundgerüst der Anwendung.</summary>
    internal class App : Application
    {
        #region Public Methods and Operators

        private static bool isUpdating;

        /// <summary>Haupteinstiegspunkt der Anwendung.</summary>
        [STAThread]
        public static void Main()
        {
            CheckForUpdates();
            RunApplication();
        }

        /// <summary>Prüft ob es Updates von der Anwendung gibt.</summary>
        private static void CheckForUpdates()
        {
            var ad = ApplicationDeployment.CurrentDeployment;
            ad.CheckForUpdateCompleted += CheckForUpdateCompleted;
            ad.UpdateCompleted += UpdateCompleted;
            ad.CheckForUpdateAsync();

            var timer = new Timer();
            timer.Interval = 10000;
            timer.Start();
            timer.Elapsed += TimerElapsed;
        }

        private static void TimerElapsed(object sender, ElapsedEventArgs arguments)
        {
            if (!isUpdating)
            {
                ApplicationDeployment.CurrentDeployment.CheckForUpdateAsync();
            }
        }

        private static void UpdateCompleted(object sender, AsyncCompletedEventArgs arguments)
        {
            isUpdating = false;
            MessageBox.Show("Update fertig.");
        }

        private static void CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs arguments)
        {
            if (arguments.UpdateAvailable)
            {
                isUpdating = true;
                ApplicationDeployment.CurrentDeployment.UpdateAsync();
            }
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