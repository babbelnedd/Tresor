namespace Tresor.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;

    using Tresor.Contracts.Utilities;

    /// <summary>Implementierung von <see cref="IDatabase" /> basierend auf Sqlite.</summary>
    public class SqliteDatabase : IDatabase
    {
        #region Fields

        /// <summary>Die SQL Verbindung zur Datenbank.</summary>
        private SQLiteConnection connection;

        /// <summary>Beinhaltet das für die Datenbank gesetzt Passwort.</summary>
        private string password;

        #endregion

        #region Public Properties

        /// <summary>Holt den Namen der Datenbank.</summary>
        public string Name { get; private set; }

        /// <summary>Holt den Pfad der Datenbank.</summary>
        public string Path { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Schließt die Verbindung zur Datenbank.</summary>
        public void CloseConnection()
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }

        /// <summary>Zerstört die Verbindung zur Datenbank.</summary>
        public void DisposeConnection()
        {
            if (connection != null)
            {
                connection.Dispose();
            }
        }

        /// <summary>Setzt einen SQL Befehl auf die Datenbank ab.</summary>
        /// <param name="commandText">Der auszuführende Befehl.</param>
        public void Execute(string commandText)
        {
            var command = new SQLiteCommand { CommandText = commandText, Connection = connection };
            command.ExecuteNonQuery();
            command.Dispose();
        }

        /// <summary>Setzt einen SQL Befehl auf die Datenbank ab und liefert eine Ergebnismenge zurück.</summary>
        /// <param name="commandText">Der auszuführende Befehl.</param>
        /// <returns>Eine Auflistung aller Ergebnisse.</returns>
        /// <remarks>Die äußere Liste enthält alle Ergebnisse. Die innere Liste enhält die Werte aller abgefragten Felder.</remarks>
        /// <example>
        /// Die Tabelle Person hat 5 Einträge
        /// SELECT Name, LastName, Age, Sex FROM Person
        /// Ergebnis:   Äußere Liste hat 5 Einträge. (Enthält alle Ergebnisse)
        ///             Die einzelnen Listen haben jeweils 4 Einträge. (Für jedes abgefragte Feld der Tabelle Person)
        /// </example>
        public List<List<string>> Get(string commandText)
        {
            var command = new SQLiteCommand { Connection = connection, CommandText = commandText };
            var reader = command.ExecuteReader();
            var result = new List<List<string>>();

            while (reader.Read())
            {
                var r = new List<string>();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    r.Add((string)reader[i]);
                }

                result.Add(r);
            }

            command.Dispose();

            return result;
        }

        /// <summary>Öffnet die Verbindung zur Datenbank.</summary>
        public void OpenConnection()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new Exception("Es wurde kein Datenbankname vergeben.");
            }

            if (connection == null)
            {
                var connectionString = string.Format("Data Source={0}", Name);
                connection = new SQLiteConnection(connectionString);
                connection.SetPassword(password);
            }

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        /// <summary>Setzt den Namen der Datenbank.</summary>
        /// <param name="name">Der zu setzende Name.</param>
        public void SetName(string name)
        {
            Name = name;
        }

        /// <summary>Setzt das Passwort der Datenbank.</summary>
        /// <param name="password">Das Passwort das gesetzt werden soll.</param>
        public void SetPassword(string password)
        {
            this.password = password;
        }

        /// <summary>Setzt den Pfad der Datenbank. Optional.</summary>
        /// <param name="path">Der Pfad wo die Datenbank zu finden bzw. anzulegen ist.</param>
        public void SetPath(string path)
        {
            Path = path;
        }

        #endregion
    }
}