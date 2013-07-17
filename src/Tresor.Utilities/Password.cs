﻿namespace Tresor.Utilities
{
    using System;
    using System.Runtime.Serialization;

    using Tresor.Contracts.Utilities;

    /// <summary>Stellt ein Passwort dar.</summary>
    [Serializable]
    public class Password : NotifyPropertyChanged, IPassword
    {
        #region Konstanten und Felder

        /// <summary>Mitglied der Eigenschaft <see cref="Description"/>.</summary>
        private string description = string.Empty;

        /// <summary>Mitglied der Eigenschaft <see cref="IsDirty"/>.</summary>
        private bool isDirty;

        /// <summary>Mitglied der Eigenschaft <see cref="Key"/>.</summary>
        private string key = string.Empty;

        /// <summary>Mitglied der Eigenschaft <see cref="Account"/>.</summary>
        private string account = string.Empty;

        #endregion

        #region Öffentliche Eigenschaften

        /// <summary>Holt oder setzt den <strong>Account</strong> des Passworts.</summary>
        public string Account
        {
            get
            {
                return account;
            }

            set
            {
                account = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Holt oder setzt die <strong>Beschreibung</strong> des Passworts.</summary>
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Holt einen Wert, der angibt, ob das Passwort ungespeicherte Änderungen hat.</summary>
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }

            set
            {
                isDirty = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Holt oder setzt den <strong>Schlüssel</strong> des Passworts.</summary>
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Überschreibt das Basisverhalten.</summary>
        /// <returns>Gibt den <see cref="Account"/> zurück.</returns>
        public override string ToString()
        {
            return Account;
        }
        
        private Password clone;

        public void BeginEdit()
        {
            clone = (Password)MemberwiseClone();
        }

        public void EndEdit()
        {
            clone = null;
            IsDirty = true;
        }

        public void CancelEdit()
        {
            // Note: Moment mal.. Will ich denn alles zurücksetzen? Eigentlich nicht! Hier Parameter fragen welche eigenschaft zurückgesetzt werden soll?
            Description = clone.Description;
            Key = clone.Key;
            Account = clone.Account;
            IsDirty = false; // Note: siehe erste Note! Könnte ja schon was Dirty sein?!
            clone = null;
        }

        #endregion
    }
}