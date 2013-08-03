namespace Tresor.Contracts.Utilities
{
    using System;
    using System.Collections.Generic;

    /// <summary>Schnittstelle für die Verwaltung von Benutzereinstellungen.</summary>
    public interface IUserSettings
    {
        #region Public Methods and Operators

        /// <summary>Legt eine neue Kategorie an.</summary>
        /// <param name="category">Der Name der Kategorie.</param>
        /// <exception cref="Exception">Die Kategorie existiert bereits.</exception>
        void Add(string category);

        /// <summary>Fügt einer Kategorie einen Schlüssel hinzu.</summary>
        /// <param name="category">Die Kategorie welcher ein Schlüssel hinzugefügt werden soll.</param>
        /// <param name="key">Der Name des Schlüssels.</param>
        /// <param name="value">Der einzutragende Wert.</param>
        void Add(string category, string key, string value);

        /// <summary>Holt alle Key-Value Paare.</summary>
        /// <param name="category">Die Kategorie welche ausgelesen werden soll.</param>
        /// <returns>Alle Key-Value Paare aus einer bestimmten Kategorie.</returns>
        Dictionary<string, string> Read(string category);

        /// <summary>Holt den Wert zu einem bestimmten Schlüssel.</summary>
        /// <param name="category">Die Kategorie, in der sich der Schlüssel befindet.</param>
        /// <param name="key">Der auszulesende Schlüssel.</param>
        /// <returns>Der Wert zu dem Schlüssel.</returns>
        string Read(string category, string key);

        /// <summary>Überschreibt einen Wert.</summary>
        /// <param name="category">Die Kategorie, in der sich der Schlüssel befindet.</param>
        /// <param name="key">Der Schlüssel, dessen Wert überschrieben werden soll.</param>
        /// <param name="value">Der neue Wert.</param>
        /// <remarks>Es wird kein neuer Schlüssel angelegt.</remarks>
        void Write(string category, string key, string value);

        /// <summary>Überschreibt einen Wert.</summary>
        /// <param name="category">Die Kategorie in welcher das Key-Value Paar gespeichert werden soll.</param>
        /// <param name="pair">Kategorie und Schlüssel welcher überschrieben werden soll.</param>
        /// <remarks>Es wird kein neuer Schlüssel angelegt.</remarks>
        /// <exception cref="Exception">Die Kategorie existiert nicht.</exception>
        /// <exception cref="Exception">Der Wert existiert bereits.</exception>
        void Write(string category, Dictionary<string, string> pair);

        #endregion
    }
}