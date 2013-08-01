namespace Tresor.Contracts.Utilities
{
    using System.Collections.Generic;

    /// <summary>Schnittstelle für eine Datenbank.</summary>
    public interface IDatabase
    {
        #region Public Properties

        /// <summary>Setzt den Namen der Datenbank.</summary>
        /// <param name="name">Der zu setzende Name.</param>
        void SetName(string name);

        /// <summary>Setzt den Pfad der Datenbank. Optional.</summary>
        /// <param name="path">Der Pfad wo die Datenbank zu finden bzw. anzulegen ist.</param>
        void SetPath(string path);

        #endregion

        #region Public Methods and Operators

        /// <summary>Schließt die Verbindung zur Datenbank.</summary>
        void CloseConnection();

        /// <summary>Zerstört die Verbindung zur Datenbank.</summary>
        void DisposeConnection();

        /// <summary>Setzt einen SQL Befehl auf die Datenbank ab.</summary>
        /// <param name="command">Der auszuführende Befehl.</param>
        void Execute(string command);

        /// <summary>Setzt einen SQL Befehl auf die Datenbank ab und liefert eine Ergebnismenge zurück.</summary>
        /// <param name="command">Der auszuführende Befehl.</param>
        /// <returns>Eine Auflistung aller Ergebnisse.</returns>
        /// <remarks>Die äußere Liste enthält alle Ergebnisse. Die innere Liste enhält die Werte aller abgefragten Felder.</remarks>
        /// <example>
        /// Die Tabelle Person hat 5 Einträge
        /// SELECT Name, LastName, Age, Sex FROM Person
        /// Ergebnis:   Äußere Liste hat 5 Einträge. (Enthält alle Ergebnisse)
        ///             Die einzelnen Listen haben jeweils 4 Einträge. (Für jedes abgefragte Feld der Tabelle Person)
        /// </example>
        List<List<string>> Get(string command);

        /// <summary>Öffnet die Verbindung zur Datenbank.</summary>
        void OpenConnection();

        /// <summary>Setzt das Passwort der Datenbank.</summary>
        void SetPassword();

        #endregion
    }
}