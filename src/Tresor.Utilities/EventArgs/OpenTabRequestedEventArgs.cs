namespace Tresor.Utilities.EventArgs
{
    using System;

    using Tresor.Contracts.Utilities;

    /// <summary>EventArgs für ein Argument, dass einen Tab öffnen möchte.</summary>
    public class OpenTabRequestedEventArgs : EventArgs
    {
        #region Konstanten und Felder

        /// <summary>Mitglied der Eigenschaft <see cref="Content"/>.</summary>
        private IPassword content;

        #endregion

        #region Öffentliche Eigenschaften

        /// <summary>Holt das <see cref="IPassword"/>, welches geöffnet werden soll.</summary>
        public IPassword Content
        {
            get
            {
                return content;
            }

            private set
            {
                content = value;
            }
        }

        #endregion

        #region Konstruktoren und Destruktoren

        /// <summary> Initialisiert eine neue Instanz der <see cref="OpenTabRequestedEventArgs"/> Klasse. </summary>
        public OpenTabRequestedEventArgs(IPassword password)
        {
            Content = password;
        }

        #endregion
    }
}