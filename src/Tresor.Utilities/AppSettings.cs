namespace Tresor.Utilities
{
    using System;
    using System.Configuration;

    /// <summary>Handhabt die AppSettings der App.config.</summary>
    public static class AppSettings
    {
        #region Öffentliche Methoden und Operatoren

        /// <summary>Holt den vollständigen Pfad zu der Datei in der die Passwörter verwaltet werden.</summary>
        /// <returns>Der vollständige Pfad zu der Datei, welche die Passwörter verwaltet.</returns>
        public static string GetStorageFile()
        {
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var storageFolder = Read("StorageFolder");
            var storageFile = Read("StorageFile");
            var storageExtension = Read("StorageExtension");
            return string.Format("{0}\\{1}\\{2}.{3}", appDataFolder, storageFolder, storageFile, storageExtension);
        }

        /// <summary>Holt einen Wert zu dem übergenem Schlüssel aus den AppSettings.</summary>
        /// <param name="property">Der Schlüssel dessen Wert geholt werden soll.</param>
        /// <exception cref="System.Exception">Falls der Wert leer oder nicht vorhanden ist.</exception>
        /// <returns>Der gefundenen Wert.</returns>
        public static string Read(string property)
        {
            var setting = ConfigurationManager.AppSettings[property];

            if (string.IsNullOrEmpty(setting))
            {
                throw new Exception(string.Format("Der Wert {0} ist nicht vorhanden oder leer.", setting));
            }

            return setting;
        }

        #endregion
    }
}