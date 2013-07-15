namespace Tresor.Logging
{
    using System;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    /// <summary>NLog Logger.</summary>
    public static class Log
    {
        #region Öffentliche Eigenschaften

        /// <summary>Holt die Instanz des Loggers.</summary>
        public static Logger Instance { get; private set; }

        #endregion

        #region Konstruktoren und Destruktoren

        /// <summary>Initialisiert statische Member der <see cref="Log"/> Klasse.</summary>
        static Log()
        {
#if DEBUG
            var sentinelTarget = new NLogViewerTarget() { Name = "sentinel", Address = "udp://127.0.0.1:9999", IncludeNLogData = false };
            var sentinalRule = new LoggingRule("*", LogLevel.Trace, sentinelTarget);
            LogManager.Configuration.AddTarget("log4view", sentinelTarget);
            LogManager.Configuration.LoggingRules.Add(sentinalRule);
#endif
            LogManager.ReconfigExistingLoggers();
            Instance = LogManager.GetCurrentClassLogger();
        }

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Führt eine Methode aus, loggt Fehler.</summary>
        /// <param name="action">Die Methode die ausgeführt werden soll.</param>
        public static void Run(Action action)
        {
            try
            {
                action();
            }
            catch (Exception exc)
            {
                Instance.ErrorException(exc.Message, exc);
            }
        }

        #endregion
    }
}