namespace Tresor.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SQLite;

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
        public void AddPassword(IPassword password)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>Prüft ob der Schlüssel zur Deserialisierung richtig ist.</summary>
        /// <param name="key">Der zu überprüfende Schlüssel.</param>
        /// <returns>True falls der Schlüssel korrekt ist, andernfalls False.</returns>
        public bool IsKeyCorrect(string key)
        {
            bool result;

            try
            {
                databasePassword = key;
                CreateDatabase();
                LoadPasswords();
                result = true;
            }
            catch (Exception)
            {
                databasePassword = string.Empty;
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
            throw new System.NotImplementedException();
        }

        #endregion

        #region Methoden

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
                Passwords.Add(
                    new Password { RecordID = Guid.Parse(password[0].ToString()), Account = password[1].ToString(), Key = password[2].ToString() });
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

        #endregion
    }
}