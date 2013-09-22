﻿using System.Deployment.Application;

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
            //var ad = ApplicationDeployment.CurrentDeployment;
            //ad.CheckForUpdateCompleted += CheckForUpdateCompleted;
            //ad.CheckForUpdateAsync();

            RunApplication();
        }

        private static void CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs arguments)
        {
            if (arguments.UpdateAvailable)
            {
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