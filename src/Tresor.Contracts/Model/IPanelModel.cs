namespace Tresor.Contracts.Model
{
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
        void AddPassword(IPassword password, string encryptionKey = null);

        /// <summary>Prüft ob der Schlüssel zur Deserialisierung richtig ist.</summary>
        /// <param name="key">Der zu überprüfende Schlüssel.</param>
        /// <returns>True falls der Schlüssel korrekt ist, andernfalls False.</returns>
        bool IsKeyCorrect(string key);

        /// <summary>Speichert das reingereichten Passwörter. <strong>Hierbei werden die vorhandenen Passwörter überschrieben.</strong></summary>
        /// <param name="password">Das Passwort welches gespeichert werden sollen.</param>
        /// <param name="encryptionKey">Der Schlüssel zum Verschlüsseln der Datenbank. Falls nicht angegeben wird der vorher festgelegte benutzt.</param> 
        void Save(IPassword password, string encryptionKey = null);

        /// <summary>Speichert die reingereichten Passwörter. <strong>Hierbei werden die vorhandenen Passwörter überschrieben.</strong></summary>
        /// <param name="passwords">Die Passwörter welche gespeichert werden sollen.</param>
        /// <param name="encryptionKey">Der Schlüssel zum Verschlüsseln der Passwörter. Falls nicht angegeben wird der vorher festgelegte benutzt.</param> 
        void Save(ObservableCollection<IPassword> passwords, string encryptionKey = null);

        #endregion
    }
}