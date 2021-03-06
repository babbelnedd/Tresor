﻿namespace Tresor
{
    using System;

    using ClickOnce;

    using Logging;

    using Application = System.Windows.Application;

    internal class App : Application
    {
        [STAThread]
        public static void Main()
        {
            Log.Run(RunUpdateManager);
            Log.Run(RunApplication);
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

            var mgr = new DeploymentManager(database) {ShowPatchNotes = true};

            if (mgr.TryUpdate())
            {
                Log.Trace("Anwendung wird neu gestartet.");
                Log.Run(mgr.Restart);
            }
        }
    }
}