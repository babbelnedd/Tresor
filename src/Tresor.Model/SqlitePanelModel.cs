namespace Tresor.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SQLite;
    using System.Linq;

    using Tresor.Contracts.Model;
    using Tresor.Contracts.Utilities;
    using Tresor.Utilities;

    /// <summary>Implementierung eines IPanelModel. Benutzt SQlite um Daten zu verwalten.</summary>
    public class SqlitePanelModel : NotifyPropertyChanged, IPanelModel
    {
        #region Konstanten und Felder

        /// <summary>Der Datenbankname.</summary>
        private readonly string database;

        /// <summary>Mitglied der Eigenschaft <see cref="Connection"/>.</summary>
        private SQLiteConnection connection;

        /// <summary>Das Passwort für die Datenbank.</summary>
        private string databasePassword;

        /// <summary>Mitglied der Eigenschaft <see cref="IsDirty"/>.</summary>
        private bool isDirty;

        #endregion

        #region Öffentliche Eigenschaften

        /// <summary>Holt einen Wert, der angibt, ob es ungespeicherte Änderungen gibt.</summary>
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }

            private set
            {
                isDirty = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Holt alle verwalteten Passwörter.</summary>
        public ObservableCollection<IPassword> Passwords { get; private set; }

        #endregion

        #region Eigenschaften

        /// <summary>Holt die Sql Verbindung.</summary>
        private SQLiteConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    var connectionString = string.Format("Data Source={0}", database);
                    var sqlConnection = new SQLiteConnection(connectionString);
                    connection = new SQLiteConnection(sqlConnection);
                }

                return connection;
            }
        }

        #endregion

        #region Konstruktoren und Destruktoren

        /// <summary>Initialisiert eine neue Instanz der <see cref="SqlitePanelModel"/> Klasse.</summary>
        /// <param name="database">Der Name der Datenbank die geladen bzw erzeugt werden soll.</param>
        public SqlitePanelModel(string database)
        {
            Passwords = new ObservableCollection<IPassword>();
            this.database = database;
        }

        /// <summary>Finalisiert eine Instanz der <see cref="SqlitePanelModel"/> Klasse.</summary>
        /// <remarks>Gibt die Sql Verbindung frei.</remarks>
        ~SqlitePanelModel()
        {
            Connection.Dispose();
        }

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Fügt ein Passwort hinzu.</summary>
        /// <param name="password">Das hinzuzufügende Passwort.</param>
        /// <param name="encryptionKey">Der Schlüssel zum Verschlüsseln der Passwörter. Falls nicht angegeben wird der vorher festgelegte benutzt.</param> 
        public void AddPassword(IPassword password, string encryptionKey = null)
        {
            SetEncryptionKey(encryptionKey);

            var command = string.Format("INSERT INTO Password(RecordID, Account, Password, Description) VALUES('{0}','{1}','{2}','{3}')", password.RecordID, password.Account, password.Key, password.Description);
            ExecuteNonQuery(command);
            Passwords.Add(password);

            OnPropertyChanged("Passwords");
        }

        /// <summary>Prüft ob der Schlüssel zur Deserialisierung richtig ist.</summary>
        /// <param name="key">Der zu überprüfende Schlüssel.</param>
        /// <returns>True falls der Schlüssel korrekt ist, andernfalls False.</returns>
        public bool IsKeyCorrect(string key)
        {
            bool result;

            try
            {
                SetEncryptionKey(key);
                CreateDatabase();
                LoadPasswords();
                result = true;
            }
            catch (Exception)
            {
                SetEncryptionKey(string.Empty);
                result = false;
                CloseConnection();
            }

            return result;
        }

        /// <summary>Speichert die reingereichten Passwörter. <strong>Hierbei werden die vorhandenen Passwörter überschrieben.</strong></summary>
        /// <param name="passwords">Die Passwörter welche gespeichert werden sollen.</param>
        /// <param name="encryptionKey">Der Schlüssel zum Verschlüsseln der Passwörter. Falls nicht angegeben wird der vorher festgelegte benutzt.</param> 
        public void Save(ObservableCollection<IPassword> passwords, string encryptionKey = null)
        {
            SetEncryptionKey(encryptionKey);

            foreach (var password in passwords)
            {
                if (encryptionKey != null)
                {
                    Save(password, encryptionKey);
                }
                else
                {
                    Save(password);
                }
            }
        }

        /// <summary>Speichert das reingereichten Passwörter. <strong>Hierbei werden die vorhandenen Passwörter überschrieben.</strong></summary>
        /// <param name="password">Das Passwort welches gespeichert werden sollen.</param>
        /// <param name="encryptionKey">Der Schlüssel zum Verschlüsseln der Datenbank. Falls nicht angegeben wird der vorher festgelegte benutzt.</param> 
        public void Save(IPassword password, string encryptionKey = null)
        {
            SetEncryptionKey(encryptionKey);

            if (PasswordExists(password))
            {
                UpdatePassword(password);
                password.IsDirty = false;
            }
            else
            {
                AddPassword(password);
                password.IsDirty = false;
                password.IsNew = false;
            }
        }

        #endregion

        #region Methoden

        /// <summary>Überwacht den Zustand des Passworts und steuert die Eigenschaft IsDirty.</summary>
        /// <param name="password">Das zu überwachende Passwort.</param>
        private static void ObserveIsDirty(IPassword password)
        {
            password.BeginEdit();
            password.PropertyChanged += (sender, arguments) =>
                {
                    if (arguments.PropertyName != "IsDirty")
                    {
                        password.IsDirty = !password.IsCloneEqual();
                    }
                };
        }

        /// <summary>Schließt die Sql Verbindung.</summary>
        private void CloseConnection()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }

        /// <summary>Erzeugt die Datenbank, falls diese noch nicht vorhanden ist.</summary>
        private void CreateDatabase()
        {
            // Note: AutoIncrement Spalte für schnellere Abfragen erstellen???
            ExecuteNonQuery(
                "CREATE TABLE IF NOT EXISTS Password(RecordID UNIQUEIDENTIFIER PRIMARY KEY NOT NULL, Account NVARCHAR NOT NULL, Password NVARCHAR NOT NULL, Description NVARCHAR)");
        }

        /// <summary>Erzeugt ein SQliteCommand und führt dieses aus.</summary>
        /// <param name="commandText">Der Sql Befehl welcher ausgeführt werden soll.</param>
        private void ExecuteNonQuery(string commandText)
        {
            OpenConnection();
            var command = new SQLiteCommand { Connection = Connection, CommandText = commandText };
            command.ExecuteNonQuery();
            command.Dispose();
            CloseConnection();
        }

        /// <summary>Führt die Methode ExecuteReader eines SQliteCommands aus.</summary>
        /// <param name="commandText">Der Sql Befehl welcher ausgeführt werden soll.</param>
        /// <returns>Eine Auflistung von ausgelesenen Einträgen.</returns>
        private IEnumerable<SQLiteDataReader> ExecuteReader(string commandText)
        {
            OpenConnection();

            var command = new SQLiteCommand { Connection = Connection, CommandText = commandText };
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return reader;
            }

            command.Dispose();
            CloseConnection();
        }

        /// <summary>Lädt alle Passwörter aus der Datenbank.</summary>
        private void LoadPasswords()
        {
            foreach (var password in ExecuteReader("SELECT RecordID, Account, Password FROM PASSWORD"))
            {
                var newPw = new Password
                                {
                                    RecordID = Guid.Parse(password[0].ToString()),
                                    Account = password[1].ToString(),
                                    Key = password[2].ToString()
                                };

                Passwords.Add(newPw);
                ObserveIsDirty(newPw);
            }
        }

        /// <summary>Öffnet die Sql Verbindung.</summary>
        private void OpenConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.SetPassword(databasePassword);
                Connection.Open();
            }
        }

        /// <summary>Holt einen Wert, der angibt, ob ein Passwort bereits existiert. Wird anhand der eindeutigen RecordID geprüft.</summary>
        /// <param name="password">Das Passwort welches geprüft werden soll.</param>
        /// <returns>True falls das Passwort bereits existiert, andernfalls False.</returns>
        private bool PasswordExists(IPassword password)
        {
            var command = string.Format("SELECT RecordID FROM Password WHERE RecordID='{0}'", password.RecordID);
            return ExecuteReader(command).ToList().Any();
        }

        /// <summary>Setzt das Datenbankpasswort.</summary>
        /// <param name="encryptionKey">Das Passwort welches gesetzt werden soll.</param>
        private void SetEncryptionKey(string encryptionKey)
        {
            if (encryptionKey != null)
            {
                databasePassword = encryptionKey;
            }
        }

        /// <summary>Aktualisiert ein bestehendes Passwort.</summary>
        /// <param name="password">Das aktualisierte Passwort welches in die Datenbank übernommen werden soll.</param>
        private void UpdatePassword(IPassword password)
        {
            var command = string.Format(
                "UPDATE Password SET Account='{0}', Password='{1}', Description='{2}' WHERE RecordID='{3}'",
                password.Account,
                password.Key,
                password.Description,
                password.RecordID);
            ExecuteNonQuery(command);
            OnPropertyChanged("Passwords");
        }

        #endregion
    }
}