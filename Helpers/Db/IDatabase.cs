
using System.Data;

namespace ErpToolkit.Helpers.Db
{
    // Interfaccia IDatabase per un approccio a codice disaccoppiato
    public interface IDatabase
    {
        void ExecuteNonQuery(string sql, IDbTransaction transaction = null);
        DataTable ExecQuery(string sql, IDbTransaction transaction = null, int maxRecords = 10000);
        void InsertOrUpdateRecord(Dictionary<string, object> fields, string tableName, string keyField, string timestampField, bool isInsert, IDbTransaction transaction = null);
        void DeleteRecord(string tableName, Dictionary<string, object> fields, string keyField, IDbTransaction transaction = null);
        void ExportTableToCsv(string tableName, string filePath, string whereClause = null, int chunkSize = 10000);
        void ImportCsvToTable(string tableName, string filePath, IDbTransaction transaction = null);
    }
}