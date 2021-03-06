﻿namespace Tresor.Contracts.Utilities
{
    using System;
    using System.ComponentModel;

    /// <summary>Schnittstelle für ein <see cref="Tresor.Utilities.Password"/>.</summary>
    public interface IPassword : INotifyPropertyChanged
    {
        #region Public Properties

        /// <summary>Holt oder setzt den <strong>Accountnamen</strong>.</summary>
        string Account { get; set; }

        /// <summary>Holt oder setzt die <strong>Beschreibung</strong>.</summary>
        string Description { get; set; }

        /// <summary>Holt oder setzt einen Wert, der angibt, ob das Passwort ungespeicherte Änderungen hat.</summary>
        bool IsDirty { get; set; }

        /// <summary>Holt oder setzt einen Wert, der angibt, ob das Passwort neu und ungespeichert ist.</summary>
        bool IsNew { get; set; }

        /// <summary>Holt oder setzt das eigentliche Passwort. </summary>
        string Key { get; set; }

        Guid RecordID { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Beginnt die Bearbeitung eines Passworts.</summary>
        void BeginEdit();

        /// <summary>Bricht die Bearbeitung eines Passworts ab. Das Passwort wird auf seinen Ursprungszustand zurückgesetzt.</summary>
        void CancelEdit();

        /// <summary>Beendet die Bearbeitung eines Passworts.</summary>
        void EndEdit();

        /// <summary>Holt einen Wert, der angibt, ob das Passwort schmutzig ist.</summary>
        /// <returns>True wenn ungespeicherte Änderungen ausstehen, andernfalls False.</returns>
        bool IsCloneEqual();

        /// <summary>Überschreibt das Basisverhalten.</summary>
        /// <returns>Gibt den <see cref="Account"/> zurück.</returns>
        string ToString();

        #endregion
    }
}