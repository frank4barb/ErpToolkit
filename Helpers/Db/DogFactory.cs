
using K4os.Compression.LZ4.Internal;
using MongoDB.Driver.Core.Configuration;
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


        public DogManager GetDog(string modelName, string dbType, string connectionStringName, string databaseName = "")
        {
            if (String.IsNullOrWhiteSpace(modelName)) throw new ArgumentException("Errore: GetDog: modelName vuota.");
            if (String.IsNullOrWhiteSpace(dbType)) throw new ArgumentException("Errore: GetDog: dbType vuota.");
            if (String.IsNullOrWhiteSpace(connectionStringName)) throw new ArgumentException("Errore: GetDog: connectionStringName vuota.");
            string key = modelName + "***" + dbType + "***" + connectionStringName;
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