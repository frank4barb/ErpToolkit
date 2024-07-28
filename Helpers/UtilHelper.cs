using Google.Api;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using NLog;
using Quartz.Util;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.DirectoryServices;

//using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace ErpToolkit.Helpers
{
    public static class UtilHelper
    {
        //configura NLog per la classe
        public static NLog.Config.LoggingConfiguration GetNLogConfig()
        {
            var config = new NLog.Config.LoggingConfiguration();
            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "backupclientlogfile_backupservice.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            // Rules for mapping loggers to targets            
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
            return config;
        }

        //Cripta & Decripta -- Simple3Des
        private const string CRYP_KEY_STR = "&%£73Erp#$";

        public static string EncryptData(string plaintext)
        {
            TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
            // Initialize the crypto provider.
            TripleDes.Key = TruncateHash(CRYP_KEY_STR, TripleDes.KeySize / 8);
            TripleDes.IV = TruncateHash("", TripleDes.BlockSize / 8);

            // Convert the plaintext string to a byte array.
            byte[] plaintextBytes = System.Text.Encoding.Unicode.GetBytes(plaintext);

            // Create the stream.
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            // Create the encoder to write to the stream.
            CryptoStream encStream = new CryptoStream(ms, TripleDes.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

            // Use the crypto stream to write the byte array to the stream.
            encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
            encStream.FlushFinalBlock();

            // Convert the encrypted stream to a printable string.
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string DecryptData(string encryptedtext)
        {
            TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
            // Initialize the crypto provider.
            TripleDes.Key = TruncateHash(CRYP_KEY_STR, TripleDes.KeySize / 8);
            TripleDes.IV = TruncateHash("", TripleDes.BlockSize / 8);

            // Convert the encrypted text string to a byte array.
            byte[] encryptedBytes = Convert.FromBase64String(encryptedtext);

            // Create the stream.
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            // Create the decoder to write to the stream.
            CryptoStream decStream = new CryptoStream(ms, TripleDes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

            // Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            decStream.FlushFinalBlock();

            // Convert the plaintext stream to a string.
            return System.Text.Encoding.Unicode.GetString(ms.ToArray());
        }
        private static byte[] TruncateHash(string key, int length)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            // Hash the key.
            byte[] keyBytes = System.Text.Encoding.Unicode.GetBytes(key);
            byte[] hash = sha1.ComputeHash(keyBytes);
            var oldHash = hash;
            hash = new byte[length - 1 + 1];

            // Truncate or pad the hash.
            if (oldHash != null)
                Array.Copy(oldHash, hash, Math.Min(length - 1 + 1, oldHash.Length));
            return hash;
        }

        // Converto Object in Byte Array

        //public static StructureType ByteArrayToObject2<StructureType>(byte[] Bytes) where StructureType : struct
        //{
        //    int Length = Marshal.SizeOf(typeof(StructureType));
        //    IntPtr Handle = Marshal.AllocHGlobal(Length);
        //    Marshal.Copy(Bytes, 0, Handle, Length);
        //    StructureType Result = (StructureType)Marshal.PtrToStructure(Handle, typeof(StructureType));
        //    Marshal.FreeHGlobal(Handle);
        //    return Result;
        //}

        //public static T ByteArrayToObject<T>(byte[] Bytes)
        //{
        //    if (Bytes == null) throw new ArgumentNullException(nameof(Bytes));
        //    int Length = Marshal.SizeOf(typeof(T));
        //    IntPtr Handle = Marshal.AllocHGlobal(Length);
        //    Marshal.Copy(Bytes, 0, Handle, Length);
        //    T Result = (T)Marshal.PtrToStructure(Handle, typeof(T));
        //    Marshal.FreeHGlobal(Handle);
        //    return Result;
        //}
        //public static byte[] ObjectToByteArray(object Structure)
        //{
        //    int Length = Marshal.SizeOf(Structure);
        //    byte[] Bytes = new byte[Length];
        //    IntPtr Handle = Marshal.AllocHGlobal(Length);
        //    Marshal.StructureToPtr(Structure, Handle, true);
        //    Marshal.Copy(Handle, Bytes, 0, Length);
        //    Marshal.FreeHGlobal(Handle);
        //    return Bytes;
        //}

        //https://github.com/Cysharp/MemoryPack (https://steven-giesel.com/blogPost/4271d529-5625-4b67-bd59-d121f2d8c8f6)

        /// <summary>
        /// Convert an object to a Byte Array, using Protobuf.
        /// </summary>
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null) throw new ArgumentNullException("ObjectToByteArray null " + nameof(obj));
            return MemoryPack.MemoryPackSerializer.Serialize(obj);
        }

        /// <summary>
        /// Convert a byte array to an Object of T, using Protobuf.
        /// </summary>
        public static T ByteArrayToObject<T>(byte[] arrBytes)
        {
            if (arrBytes == null) throw new ArgumentNullException("ByteArrayToObject null " + nameof(arrBytes));
            return MemoryPack.MemoryPackSerializer.Deserialize<T>(arrBytes);
        }


        //Clona Oggetto mediante serializzazione
        //https://www.wwt.com/article/how-to-clone-objects-in-dotnet-core
        public static T DeepCopy<T>(this T self)
        {
            //var serialized = JsonConvert.SerializeObject(self);
            //return JsonConvert.DeserializeObject<T>(serialized);
            if (self == null) throw new ArgumentNullException("DeepCopy null " + nameof(self));
            var serialized = ObjectToByteArray(self);
            return ByteArrayToObject<T>(serialized);
        }

        // LDAP autenticazione
        // If Not LoginLdap(tmpUname, tmpPswd) Then ERROR
        public static bool LoginLdap(string ldap_server, string Username, string Password)
        {
            bool Autenticato = false; string tmpPassword = "", tmpUsername = "", errorMessage = "";
            var entry = new System.DirectoryServices.DirectoryEntry(ldap_server, Username, Password, System.DirectoryServices.AuthenticationTypes.Secure);
            try
            {
                var searcher = new DirectorySearcher(entry);
                searcher.SearchScope = SearchScope.OneLevel;
                if (searcher.FindOne() != null) Autenticato = true;
                else
                {
                    Autenticato = false;
                    LogManager.GetCurrentClassLogger().Info("Autenticazione [" + tmpUsername + "] non riuscita");
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error("Errore di autenticazione [" + tmpUsername + "]: " + ex.Message);
                Autenticato = false;
            }
            return Autenticato;
        }

        //#############################################################################

        //https://github.com/dotnet/efcore/issues/4675
        //convert: SqlDataReader to DbSet
        //Usage: var rows = await dbContext.TestObjects.FromReaderAsync(dbDataReader);
        //public static async Task<IReadOnlyList<T>> FromReaderAsync<T>(this DbSet<T> dbSet, DbDataReader reader) where T : class
        //{
        //    var valueBufferParameter = Expression.Parameter(typeof(ValueBuffer));
        //    var materializer = Expression.Lambda<Func<ValueBuffer, T>>(
        //        dbSet.GetService<IEntityMaterializerSource>().CreateMaterializeExpression(
        //            dbSet.GetService<IModel>().FindEntityType(typeof(T)),
        //            valueBufferParameter),
        //        valueBufferParameter).Compile();

        //    var valueBufferFactory = dbSet.GetService<IRelationalValueBufferFactoryFactory>().Create(new[] { typeof(T) }, null);


        //    var r = new List<T>();

        //    while (await reader.ReadAsync())
        //        r.Add(materializer.Invoke(valueBufferFactory.Create(reader)));

        //    return r.AsReadOnly();
        //}


        //https://www.entityframeworktutorial.net/entityframework6/raw-sql-query-in-entity-framework.aspx
        //https://learn.microsoft.com/en-us/dotnet/api/system.data.linq.datacontext.executequery?view=netframework-4.8.1&redirectedfrom=MSDN#System_Data_Linq_DataContext_ExecuteQuery__1_System_String_System_Object___
        //https://mcuslu.medium.com/executing-raw-sql-queries-using-entity-framework-core-and-returns-to-generic-data-model-534356b1c2b3



    }
}
