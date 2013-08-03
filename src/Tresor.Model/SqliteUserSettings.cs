namespace Tresor.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Tresor.Contracts.Utilities;

    /// <summary>Ermöglicht es Benutzereinstellungen zu verwalten.</summary>
    /// <remarks>Eine Implementierung von IUserSettings welche auf SQLite basiert.</remarks>
    public class SqliteUserSettings : IUserSettings
    {
        #region Fields

        /// <summary>Eine Instanz der Datenbank.</summary>
        private readonly IDatabase database;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initialisiert eine neue Instanz der <see cref="SqliteUserSettings"/> Klasse.</summary>
        /// <param name="database">Die Datenbank, welche die Benutzereinstellungen speichert.</param>
        public SqliteUserSettings(IDatabase database)
        {
            this.database = database;
            database.OpenConnection();
        }

        /// <summary>Finalisiert eine Instanz der <see cref="SqliteUserSettings"/> Klasse.</summary>
        ~SqliteUserSettings()
        {
            database.CloseConnection();
            database.DisposeConnection();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Legt eine neue Kategorie an.</summary>
        /// <param name="category">Der Name der Kategorie.</param>
        /// <exception cref="Exception">Die Kategorie existiert bereits.</exception>
        public void Add(string category)
        {
            var commandText = string.Format("CREATE TABLE {0}(Key NVARCHAR PRIMARY KEY NOT NULL, Value NVARCHAR)", category);
            database.Execute(commandText);
        }

        /// <summary>Fügt einer Kategorie einen Schlüssel hinzu.</summary>
        /// <param name="category">Die Kategorie welcher ein Schlüssel hinzugefügt werden soll.</param>
        /// <param name="key">Der Name des Schlüssels.</param>
        /// <param name="value">Der einzutragende Wert.</param>
        public void Add(string category, string key, string value)
        {
            var commandText = string.Format("INSERT INTO {0}(Key, Value) VALUES('{1}','{2}')", category, key, value);
            database.Execute(commandText);
        }

        /// <summary>Holt alle Key-Value Paare.</summary>
        /// <param name="category">Die Kategorie welche ausgelesen werden soll.</param>
        /// <returns>Alle Key-Value Paare aus einer bestimmten Kategorie.</returns>
        public Dictionary<string, string> Read(string category)
        {
            var result = new Dictionary<string, string>();
            var commandText = string.Format("SELECT * FROM {0}", category);
            var entries = database.Get(commandText);
            entries.ForEach(entry => result.Add(entry[0], entry[1]));

            return result;
        }

        /// <summary>Holt den Wert zu einem bestimmten Schlüssel.</summary>
        /// <param name="category">Die Kategorie, in der sich der Schlüssel befindet.</param>
        /// <param name="key">Der auszulesende Schlüssel.</param>
        /// <returns>Der Wert zu dem Schlüssel.</returns>
        public string Read(string category, string key)
        {
            var commandText = string.Format("SELECT * FROM {0} WHERE Key = '{1}'", category, key);
            var result = database.Get(commandText).First();
            return result[1];
        }

        /// <summary>Überschreibt einen Wert.</summary>
        /// <param name="category">Die Kategorie, in der sich der Schlüssel befindet.</param>
        /// <param name="key">Der Schlüssel, dessen Wert überschrieben werden soll.</param>
        /// <param name="value">Der neue Wert.</param>
        /// <remarks>Es wird kein neuer Schlüssel angelegt.</remarks>
        public void Write(string category, string key, string value)
        {
            var commandText = string.Format("UPDATE {0} SET Value = '{1}' WHERE Key = '{2}'", category, value, key);
            database.Execute(commandText);
        }

        /// <summary>Überschreibt einen Wert.</summary>
        /// <param name="category">Die Kategorie in welcher das Key-Value Paar gespeichert werden soll.</param>
        /// <param name="pair">Kategorie und Schlüssel welcher überschrieben werden soll.</param>
        /// <remarks>Es wird kein neuer Schlüssel angelegt.</remarks>
        /// <exception cref="Exception">Die Kategorie existiert nicht.</exception>
        /// <exception cref="Exception">Der Wert existiert bereits.</exception>
        public void Write(string category, Dictionary<string, string> pair)
        {
            foreach (var entry in pair)
            {
                Write(category, entry.Key, entry.Value);
            }
        }

        #endregion
    }
}