namespace Tresor.Contracts.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using Tresor.Contracts.Utilities;

    /// <summary>Schnittstelle für das PanelModel.</summary>
    public interface IPanelModel : INotifyPropertyChanged
    {
        #region Öffentliche Eigenschaften

        /// <summary>Holt einen Wert, der angibt, ob es ungespeicherte Änderungen gibt.</summary>
        bool IsDirty { get; }

        /// <summary>Holt alle verwalteten Passwörter.</summary>
        ObservableCollection<IPassword> Passwords { get; }

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Fügt ein Passwort hinzu.</summary>
        /// <param name="password">Das hinzuzufügende Passwort.</param>
        void AddPassword(IPassword password);

        /// <summary>Prüft ob der Schlüssel zur Deserialisierung richtig ist.</summary>
        /// <param name="key">Der zu überprüfende Schlüssel.</param>
        bool IsKeyCorrect(string key);

        /// <summary>Speichert die reingereichten Passwörter. <strong>Hierbei werden die vorhandenen Passwörter überschrieben.</strong></summary>
        /// <param name="passwords">Die Passwörter welche gespeichert werden sollen.</param>
        void Save(IEnumerable<IPassword> passwords);

        #endregion
    }
}