namespace Tresor
{
    using System;
    using System.Windows;

    using ClickOnce;

    using Logging;


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
            const string database = "PatchNotes.s3db";

            var mgr = new DeploymentManager(database) { ShowPatchNotes = true };

            if (mgr.TryUpdate())
            {
                mgr.Restart();
            }
        }
    }
}