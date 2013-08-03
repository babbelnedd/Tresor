namespace Tresor.Contracts.Utilities
{
    using System.Collections.Generic;

    /// <summary>Schnittstelle für eine Datenbank.</summary>
    public interface IDatabase
    {
        #region Public Properties

        /// <summary>Holt den Namen der Datenbank.</summary>
        string Name { get; }

        /// <summary>Holt den Pfad der Datenbank.</summary>
        string Path { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Schließt die Verbindung zur Datenbank.</summary>
        void CloseConnection();

        /// <summary>Zerstört die Verbindung zur Datenbank.</summary>
        void DisposeConnection();

        /// <summary>Setzt einen SQL Befehl auf die Datenbank ab.</summary>
        /// <param name="commandText">Der auszuführende Befehl.</param>
        void Execute(string commandText);

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
        List<List<string>> Get(string commandText);

        /// <summary>Öffnet die Verbindung zur Datenbank.</summary>
        void OpenConnection();

        /// <summary>Setzt den Namen der Datenbank.</summary>
        /// <param name="name">Der zu setzende Name.</param>
        void SetName(string name);

        /// <summary>Setzt das Passwort der Datenbank.</summary>
        /// <param name="password">Das Passwort das gesetzt werden soll.</param>
        void SetPassword(string password);

        /// <summary>Setzt den Pfad der Datenbank. Optional.</summary>
        /// <param name="path">Der Pfad wo die Datenbank zu finden bzw. anzulegen ist.</param>
        void SetPath(string path);

        #endregion
    }
}