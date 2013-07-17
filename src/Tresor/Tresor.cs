namespace Tresor
{
    using System;
    using System.Windows;

    using global::Tresor.Logging;

    /// <summary>Grundgerüst der Anwendung.</summary>
    internal class Tresor : Application
    {
        // <summary>Einstigespunkt der Anwendung.</summary>
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
            Log.Run(Bootstrapper.LoadApplication);
            Log.Trace("Anwendung beendet.");
        }

        #endregion
    }
}