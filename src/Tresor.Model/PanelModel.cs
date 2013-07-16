namespace Tresor.Model
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;

    using ThirdParty;

    using Tresor.Contracts.Model;
    using Tresor.Contracts.Utilities;
    using Tresor.Utilities;

    /// <summary>Das Datenmodel des PanelViewModels.</summary>
    public class PanelModel : NotifyPropertyChanged, IPanelModel
    {
        #region Konstanten und Felder

        /// <summary>Mitglied der Eigenschaft <see cref="Passwords"/>.</summary>
        private ObservableCollection<IPassword> passwords;

        /// <summary>Mitglied der Eigenschaft <see cref="Serializer"/>.</summary>
        private GenericCryptoClass serializer;

        #endregion

        #region Öffentliche Eigenschaften

        /// <summary>Holt einen Wert, der angibt, ob es ungespeicherte Änderungen gibt.</summary>
        public bool IsDirty
        {
            get
            {
                return Passwords.Any(password => password.IsDirty);
            }
        }

        /// <summary>Holt alle verwalteten Passwörter.</summary>
        public ObservableCollection<IPassword> Passwords
        {
            get
            {
                return passwords;
            }

            private set
            {
                passwords = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Eigenschaften

        /// <summary>Holt den Dateinamen der Datei welche die Passwörter enthält.</summary>
        private string FileName
        {
            get
            {
                return string.Format("{0}\\Tresor\\save.tsr", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }

        /// <summary>Holt den Serializer um Daten zu verschlüssen und zu speichern.</summary>
        private GenericCryptoClass Serializer
        {
            get
            {
                if (serializer == null)
                {
                    serializer = new GenericCryptoClass();
                }

                return serializer;
            }
        }

        #endregion

        #region Konstruktoren und Destruktoren

        /// <summary>Initialisiert eine neue Instanz der <see cref="PanelModel"/> Klasse.</summary>
        public PanelModel()
        {
            CreateAppDataFolder();
            Passwords = LoadPasswords();
            Passwords.CollectionChanged += CollectionChanged;
            ObservePasswords();
        }

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Fügt ein Passwort hinzu.</summary>
        /// <param name="password">Das hinzuzufügende Passwort.</param>
        public void AddPassword(IPassword password)
        {
            Passwords.Add(password);
        }

        #endregion

        #region Methoden

        /// <summary>Tritt ein wenn sich die Auflistung von Passwörtern geändert hat.</summary>
        /// <param name="sender">Dieser Parameter wird nicht verwendet.</param>
        /// <param name="arguments">Dieser Parameter wird nicht verwendet.</param>
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs arguments)
        {
            OnPropertyChanged("Passwords");

            foreach (var password in Passwords)
            {
                password.PropertyChanged -= PasswordChanged;
            }

            ObservePasswords();
        }

        /// <summary>Erzeugt den Ordner in dem die Passwörter gespeichert werden.</summary>
        /// <remarks>Der Ordnerpfad: %APPDATA%\Tresor</remarks>
        private void CreateAppDataFolder()
        {
            var path = Path.GetDirectoryName(FileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>Läd alle verwalteten Passwörter.</summary>
        /// <returns>Die geladenen Passwörter.</returns>
        private ObservableCollection<IPassword> LoadPasswords()
        {
            var result = new ObservableCollection<IPassword>();

            if (File.Exists(FileName))
            {
                result = Serializer.Deserialize<ObservableCollection<IPassword>>(FileName);
            }

            return result;
        }

        /// <summary>Beginnt die Überwachung aller Passwörter.</summary>
        private void ObservePasswords()
        {
            foreach (var password in Passwords)
            {
                password.PropertyChanged += PasswordChanged;
            }
        }

        /// <summary>Tritt ein wenn sich ein einzelnes Passwort geändert hat.</summary>
        /// <param name="sender">Dieser Parameter wird nicht verwendet.</param>
        /// <param name="arguments">Überprüft anhand der Eigenschaft PropertyName welche Eigenschaft sich geändert hat.</param>
        private void PasswordChanged(object sender, PropertyChangedEventArgs arguments)
        {
                OnPropertyChanged("IsDirty");
        }

        /// <summary>Speichert die reingereichten Passwörter. <strong>Hierbei werden die vorhandenen Passwörter überschrieben.</strong></summary>
        /// <param name="passwords">Die Passwörter welche gespeichert werden sollen.</param>
        private void Save(ObservableCollection<Password> passwords)
        {
            if (passwords == null)
            {
                throw new ArgumentNullException("passwords");
            }

            Serializer.Serialize(FileName, passwords);
        }

        #endregion
    }
}