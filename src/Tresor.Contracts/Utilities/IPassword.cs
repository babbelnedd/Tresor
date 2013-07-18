namespace Tresor.Contracts.Utilities
{
    using System.ComponentModel;

    /// <summary>Schnittstelle für ein <see cref="Tresor.Utilities.Password"/>.</summary>
    public interface IPassword : INotifyPropertyChanged
    {
        #region Öffentliche Eigenschaften

        /// <summary>Holt oder setzt den <strong>Accountnamen</strong>.</summary>
        string Account { get; set; }

        /// <summary>Holt oder setzt die <strong>Beschreibung</strong>.</summary>
        string Description { get; set; }

        /// <summary>Holt oder setzt einen Wert, der angibt, ob das Passwort ungespeicherte Änderungen hat.</summary>
        bool IsDirty { get; set; }

        /// <summary>Holt oder setzt das eigentliche Passwort. </summary>
        string Key { get; set; }

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Beginnt die Bearbeitung eines Passworts.</summary>
        void BeginEdit();

        /// <summary>Bricht die Bearbeitung eines Passworts ab. Das Passwort wird auf seinen Ursprungszustand zurückgesetzt.</summary>
        void CancelEdit();

        /// <summary>Beendet die Bearbeitung eines Passworts.</summary>
        void EndEdit();

        /// <summary>Überschreibt das Basisverhalten.</summary>
        /// <returns>Gibt den <see cref="Account"/> zurück.</returns>
        string ToString();

        #endregion
    }
}