
namespace ErpToolkit.Helpers.Db
{
    // Eccezione personalizzata per la gestione degli errori
    public class DatabaseException : Exception
    {
        public int ErrorCode { get; }

        public DatabaseException(int errorCode, string message, Exception innerException = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}