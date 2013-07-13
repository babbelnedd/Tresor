namespace Tresor
{
    using System;
    using System.Windows;

    using global::Tresor.Logging;

    /// <summary>Grundgerüst der Anwendung.</summary>
    internal class Tresor : Application
    {
        /// <summary>Der Anwendungslogger.</summary>
        private static readonly NLog.Logger Logger = Log.Instance;

        #region Öffentliche Methoden und Operatoren

        // <summary>Einstigespunkt der Anwendung.</summary>
        [STAThread]
        public static void Main()
        {
            RunApplication();
        }

        /// <summary>Führt die Anwendung aus.</summary>
        private static void RunApplication()
        {
            Logger.Trace("Anwendung gestartet.", false);
            Log.Run(Bootstrapper.LoadApplication);
            Logger.Trace("Anwendung beendet.", false);
        }

        #endregion
    }
}