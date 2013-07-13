// --------------------------------------------------------------------------------------------------------------------
// <author>BlackDragon</author>
// <url>http://dotnet-snippets.de/snippet/generische-verschluesselte-serialisierung-und-deserialisieru/734</url>
// -------------------------------------------------------------------------------------------------------------------- 
namespace ThirdParty
{
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>Mithilfe dieser Generischen Klasse ist es Möglich, ein Object auf einen Datenträger verschlüsselt zu Serialisieren und auch wieder entschlüsselt zu Deserialisieren.</summary>
    /// <example><code lang="C#">ToolSet.Crypto.GenericCryptoClass gcc = new ToolSet.Crypto.GenericCryptoClass();
    ///     gcc.Serialize<DataSet>(@"d:\ttest.bin", (DataSet)dataGridView1.DataSource);
    ///     dataGridView2.DataSource = gcc.Deserialize<DataSet>(@"d:\ttest.bin");
    ///     dataGridView2.DataMember = "table";</code></example>
    public class GenericCryptoClass
    {
        #region Konstanten und Felder

        /// <summary>Ein Default Initialisierungsvektor,dieser wird immer benutzt falls keiner angegeben wird.</summary>
        private const string DefaultVector = @"jOU\9_JpWt4"; // Note: Mit 100stelligem Key ersetzen für Standard!

        /// <summary>Ein Default Key, dieser wird immer benutzt falls keiner angegeben wird.</summary>
        private const string DefaultKey = "o[cc_IQETy"; // Note: Mit 100stelligem Key ersetzen für Standard!

        #endregion

        #region Öffentliche Methoden und Operatoren

        /// <summary>Deserialisiert ein verschlüsseltes, serialisiertes Abbild einer Klasse von einem Datenträger.</summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll.</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte Abbild erezugt werden soll.</param>
        /// <returns>Das Deserialisiert Object.</returns>
        public T Deserialize<T>(string path)
        {
            return Deserialize<T>(path, DefaultKey);
        }

        /// <summary>Deserialisiert ein verschlüsseltes, serialisiertes Abbild einer Klasse von einem Datenträger.</summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll.</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll.</param>
        /// <param name="key">Der Schlüssel, mit dem das Abbild verschlüsselt werden soll.</param>
        /// <returns>Das Deserialisiert Object.</returns>
        public T Deserialize<T>(string path, string key)
        {
            return Deserialize<T>(path, key, DefaultVector);
        }

        /// <summary>Deserialisiert ein verschlüsseltes, serialisiertes Abbild einer Klasse von einem Datenträger.</summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll.</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll.</param>
        /// <param name="key">Der Schlüssel, mit dem das Abbild verschlüsselt werden soll.</param>
        /// <param name="iv">Der Initialisierungsverktor, für die Initialisierung der Verschlüsselung.</param>
        /// <returns>Das Deserialisiert Object.</returns>
        public T Deserialize<T>(string path, string key, string iv)
        {
            FileStream fileStream = null;

            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException(string.Format("Die Datei, {0} konnte nicht geöffnet werden", path));
                }

                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                var cryptoStream = new CryptoStream(fileStream, CryptoTransformer(key, iv).CreateDecryptor(), CryptoStreamMode.Read);
                var xmlSerializer = new XmlSerializer(typeof(T));

                return (T)xmlSerializer.Deserialize(cryptoStream);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        /// <summary>Erzeugt eine verschluesselts, Serialisiertes Abbild der jeweiligen Klassen auf einem Datenträger.</summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll.</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll.</param>
        /// <param name="toSerialize">Das Object, das Serialisiert werden soll.</param>
        public void Serialize<T>(string path, T toSerialize)
        {
            Serialize(path, toSerialize, DefaultKey);
        }

        /// <summary>Erzeugt eine verschluesselts, Serialisiertes Abbild der jeweiligen Klassen auf einem Datenträger.</summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll.</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll.</param>
        /// <param name="toSerialize">Das Object, das Serialisiert werden soll.</param>
        /// <param name="key">Der Schlüssel, mit dem das Abbild verschlüsselt werden soll.</param>
        public void Serialize<T>(string path, T toSerialize, string key)
        {
            Serialize(path, toSerialize, key, DefaultVector);
        }

        /// <summary>Erzeugt eine verschluesselts, Serialisiertes Abbild der jeweiligen Klassen auf einem Datenträger.</summary>
        /// <typeparam name="T">Der Typ, Serialisiert werden soll</typeparam>
        /// <param name="path">Der Pfad, an dem das Serialisierte abbild erezugt werden soll</param>
        /// <param name="toSerialize">Das Object, das Serialisiert werden soll</param>
        /// <param name="key">Der Schlüssel, mit dem das Abbild verschlüsselt werden soll</param>
        /// <param name="iv">Der Initialisierungsverktor, für die Initialisierung der Verschlüsselung</param>
        public void Serialize<T>(string path, T toSerialize, string key, string iv)
        {
            CryptoStream cryptoStream = null;
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                cryptoStream = new CryptoStream(fileStream, CryptoTransformer(key, iv).CreateEncryptor(), CryptoStreamMode.Write);

                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(cryptoStream, toSerialize);

                fileStream.Flush();
                cryptoStream.Flush();
            }
            finally
            {
                if (cryptoStream != null)
                {
                    cryptoStream.Close();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        #endregion

        #region Methoden

        /// <summary>Erzeugt einen einfachen Rijndael CryptoTransformer.</summary>
        /// <param name="key">Der Schlüssel, mit dem das Abbild verschlüsselt werden soll.</param>
        /// <param name="iv">Der Initialisierungsverktor, für die Initialisierung der Verschlüsselung.</param>
        /// <returns>Ein Rijndael CryptoTransformer.</returns>
        private Rijndael CryptoTransformer(string key, string iv)
        {
            var cryptoTransformer = Rijndael.Create();
            cryptoTransformer.Key = Encoding.ASCII.GetBytes(GetPaddedString(key));
            cryptoTransformer.IV = Encoding.ASCII.GetBytes(GetPaddedString(iv));
            cryptoTransformer.Padding = PaddingMode.Zeros;

            return cryptoTransformer;
        }

        /// <summary>Diese Methode, verstärkt noch ein wenig die Verschlüsselung.</summary>
        /// <param name="val">ein Wert der gepaddet werden soll.</param>
        /// <returns>ein gepaddeter String.</returns>
        private string GetPaddedString(string val)
        {
            // Note: je nach länge wird entweder vorn oder hinten und mit unterschiedlichen Zeichen gepaddet
            if (val.Length % 2 == 0)
            {
                return val.PadLeft(16, (char)(val.Length + 64));
            }

            return val.PadRight(16, (char)(90 - val.Length));
        }

        #endregion
    }
}