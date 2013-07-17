namespace Tresor.Utilities.EventArgs
{
    using System;

    using Tresor.Contracts.Utilities;

    /// <summary>EventArgs für ein Argument, dass einen Tab öffnen möchte.</summary>
    public class OpenTabRequestedEventArgs<T> : EventArgs
    {
        /// <summary>Mitglied der Eigenschaft <see cref="Content"/>.</summary>
        private T content;

        /// <summary>Holt oder setzt das Objekt, welches geöffnet werden soll.</summary>
        public T Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }
    }
}
