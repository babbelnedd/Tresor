namespace Tresor
{
    using System;
    using System.Diagnostics;
    using System.Windows;

    using Logging;

    using Utilities;

    internal class App : Application
    {
        [STAThread]
        public static void Main()
        {
            RunUpdateManager();
            RunApplication();
        }

        private static void RunApplication()
        {
            Log.Trace("Bootstrapper wird initialisiert.");
            var bootstrapper = new Bootstrapper();
            Log.Trace("Anwendung wird gestartet.");
            Log.Run(bootstrapper.LoadApplication);
            Log.Trace("Anwendung wurde beendet.");
        }


        private static void RunUpdateManager()
        {
#if RELEASE
            var updateManager = new ClickOnceUpdateManager();
            updateManager.TryManualUpdate();
            updateManager.Start();
#endif
        }
    }
}