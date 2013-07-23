namespace Tresor
{
    using System;
    using System.Windows;

    using Tresor.Logging;

    /// <summary>Grundgerüst der Anwendung.</summary>
    internal class App : Application
    {
        #region Öffentliche Methoden und Operatoren

        /// <summary>Haupteinstiegspunkt der Anwendung.</summary>
        [STAThread]
        public static void Main()
        {
            RunApplication();
        }

        #endregion

        #region Methoden

        /// <summary>Führt die Anwendung aus.</summary>
        private static void RunApplication()
        {
            Log.Trace("Anwendung gestartet.");
            Log.Run(new Bootstrapper().LoadApplication);
            Log.Trace("Anwendung beendet.");
        }

        #endregion
    }
}