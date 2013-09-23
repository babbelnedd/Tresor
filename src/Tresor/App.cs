﻿namespace Tresor
{
    using System;
    using System.Windows;

    using Logging;

    using Utilities;

    /// <summary>Grundgerüst der Anwendung.</summary>
    internal class App : Application
    {
        #region Public Methods and Operators

        /// <summary>Haupteinstiegspunkt der Anwendung.</summary>
        [STAThread]
        public static void Main()
        {
            RunUpdateManager();
            RunApplication();
        }

        private static void RunUpdateManager()
        {
#if RELEASE
            var updateManager = new ClickOnceUpdateManager();
            updateManager.Start();
#endif
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