namespace Tresor.UnitTesting.Tools
{
    using System;
    using System.IO;

    /// <summary>Behandelt die Datei welche die serialisierten Passwörter aufbewahrt.</summary>
    internal static class PasswordStorage
    {
        #region Öffentliche Methoden und Operatoren

        /// <summary>Löscht die Datei save.tsr und bennent save.tsr.bak in save.tsr.</summary>
        internal static void MoveBackupToOrigin()
        {
            if (File.Exists(string.Format("{0}\\Tresor\\save.tsr", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))))
            {
                File.Delete(string.Format("{0}\\Tresor\\save.tsr", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
            }

            if (File.Exists(string.Format("{0}\\Tresor\\save.tsr.bak", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))))
            {
                File.Move(
                    string.Format("{0}\\Tresor\\save.tsr.bak", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)),
                    string.Format("{0}\\Tresor\\save.tsr", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
            }
        }

        /// <summary>Benennt die Datei save.tsr um in save.tsr.bak.</summary>
        internal static void MoveOriginToBackup()
        {
            if (File.Exists(string.Format("{0}\\Tresor\\save.tsr", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))))
            {
                File.Move(
                    string.Format("{0}\\Tresor\\save.tsr", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)),
                    string.Format("{0}\\Tresor\\save.tsr.bak", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
            }
        }

        #endregion
    }
}