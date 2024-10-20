
using K4os.Compression.LZ4.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver.Core.Configuration;
using System.Data;
using System.Data.Entity;
using System.Reflection.Metadata;

namespace ErpToolkit.Helpers.Db
{
    //------------------- 
    //Data Object Gateway
    //-------------------
    // Funzioni di gestione accesso al Database, con il supporto del Data Model 
    public class DogFactory : IDisposable
    {

        public class DogId {
            private string _modelName, _dbType, _connectionStringName;
            public string ModelName { get { return _modelName; } }
            public string DbType { get { return _dbType; } }
            public string ConnectionStringName { get { return _connectionStringName; } }
            public DogId(string modelName, string dbType, string connectionStringName)
            {
                _modelName = modelName; _dbType = dbType; _connectionStringName = connectionStringName;
            }
            public DogId(string connectionStringFull_NameTypeModel, string databaseName = "")
            {
                if (String.IsNullOrWhiteSpace(connectionStringFull_NameTypeModel)) throw new ArgumentException("Errore: GetDog: connectionStringFull_NameTypeModel vuota.");
                string[] comp = connectionStringFull_NameTypeModel.Split("__");
                if (comp.Length != 3) throw new ArgumentException("Errore: GetDog: connectionStringFull_NameTypeModel bad syntax: #<DB name>__<DB type>__<Model Name>");
                _modelName = comp[2]; _dbType = comp[1]; _connectionStringName = connectionStringFull_NameTypeModel;
            }
        }

        private IDictionary<string, DogManager> _itemsDOG = new Dictionary<string, DogManager>();  //

        public DogFactory()
        {
        }
        ~DogFactory()
        {
            Dispose();
        }
        public void Dispose()
        {
            // Rilascia risorse non gestite
            if (_itemsDOG != null)
            {
                foreach (var key in _itemsDOG.Keys) { _itemsDOG[key].Dispose(); _itemsDOG.Remove(key); }
                _itemsDOG.Clear(); _itemsDOG = null;
            }
            //.........
            GC.SuppressFinalize(this);
        }

        private static readonly object _lockObject = new object();

        public DogManager GetDog(DogId dogId, string databaseName = "")
        {
            if (dogId == null) throw new ArgumentException("Errore: GetDog: dogId=null.");
            return GetDog(dogId.ModelName, dogId.DbType, dogId.ConnectionStringName, databaseName);
        }

        public DogManager GetDog(string modelName, string dbType, string connectionStringName, string databaseName = "")
        {
            if (String.IsNullOrWhiteSpace(modelName)) throw new ArgumentException("Errore: GetDog: modelName vuota.");
            if (String.IsNullOrWhiteSpace(dbType)) throw new ArgumentException("Errore: GetDog: dbType vuota.");
            if (String.IsNullOrWhiteSpace(connectionStringName)) throw new ArgumentException("Errore: GetDog: connectionStringName vuota.");
            string key = modelName + "***" + dbType + "***" + connectionStringName;
            lock (_lockObject)
            {
                DogManager dog = null; if (_itemsDOG.ContainsKey(key)) dog = _itemsDOG[key];
                if (dog == null)
                {
                    dog = new DogManager(modelName, dbType, connectionStringName);
                    if (dog == null) throw new ArgumentException("Errore: impossibile creare db {dbType} {databaseName}  ({connectionString}) ");
                    _itemsDOG[key] = dog;
                }
                return dog;
            }
        }

    }
}