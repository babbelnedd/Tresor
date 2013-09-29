namespace Tresor.Logging
{
    using System;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    /// <summary>NLog Logger.</summary>
    public static class Log
    {
        #region Constructors and Destructors

        /// <summary>Initialisiert statische Member der <see cref="Log"/> Klasse.</summary>
        static Log()
        {
            SetupLog4View();
            LogManager.ReconfigExistingLoggers();
            Instance = LogManager.GetCurrentClassLogger();
            Trace("Anwendungslogger initalisiert.");
        }

        #endregion

        #region Public Properties

        /// <summary>Holt die Instanz des Loggers.</summary>
        public static Logger Instance { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Führt eine Methode aus, loggt Fehler.</summary>
        /// <param name="action">Die Methode die ausgeführt werden soll.</param>
        public static void Run(Action action)
        {
            try
            {
                Trace(string.Format("Die Methode '{0}' wird nun über den Logger ausgefuehrt.", action.Method));
                action();
            }
            catch (Exception exc)
            {
                Instance.ErrorException(exc.Message, exc);
            }
        }

        /// <summary>Sendet eine Nachricht an den Logger.</summary>
        /// <param name="message">Die Nachricht die gesendet werden soll.</param>
        public static void Trace(string message)
        {
            Instance.Trace(message);
        }

        #endregion

        #region Methods

        /// <summary>Initialisiert die Ablaufverfolgung für Log4View.</summary>
        /// <remarks>Wird nur ausgeführt falls die Anwendung im Debug Modus läuft.</remarks>
        private static void SetupLog4View()
        {
            var sentinelTarget = new NLogViewerTarget { Name = "sentinel", Address = "udp://127.0.0.1:9999", IncludeNLogData = false };
            var sentinalRule = new LoggingRule("*", LogLevel.Trace, sentinelTarget);
            LogManager.Configuration.AddTarget("log4view", sentinelTarget);
            LogManager.Configuration.LoggingRules.Add(sentinalRule);
        }

        #endregion
    }
}