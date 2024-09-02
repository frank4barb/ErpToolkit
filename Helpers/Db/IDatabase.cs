
using System.Data;
using System.Data.Common;

namespace ErpToolkit.Helpers.Db
{
    public interface IDatabase
    {
        //Gestione connessione comando reader e bulk
        IDbConnection NewConnection();
        void ReleaseConnection(IDbConnection connection);
        IDbCommand NewCommand(string sql, IDbConnection connection);
        DataTable QueryReader(IDbCommand command, int maxRecords);
        // Gestione transazioni
        void BeginTransaction(string transactionName);
        void SavePointTransaction(string savePointName);
        void CommitSavePoint(string savePointName);
        void RollbackSavePoint(string savePointName);
        void CommitTransaction(string transactionName);
        void RollbackTransaction(string transactionName);
        //Util errori
        bool IsTransient(Exception ex);
        bool HandleException(Exception ex);
    }
}


/***********

//Vantaggi di questo Approccio:
//Modularità e Flessibilità: La classe SqlServerDatabase implementa l'interfaccia IDatabase, rendendo possibile l'implementazione di altre classi per diversi DBMS senza modificare il codice che le utilizza.

//Gestione Automatica della Connessione: La classe SqlServerDatabase gestisce automaticamente la connessione al database e si riconnette se la connessione è persa.

//Transazioni Gestite in Modo Flessibile: La gestione delle transazioni è esterna, consentendo l'esecuzione di più operazioni all'interno della stessa transazione.

//Semplicità nell'Utilizzo: Una volta creata un'istanza del database, puoi utilizzare i metodi InsertOrUpdateRecord e RecordExists senza preoccuparti delle specifiche del DBMS sottostante.

//Estensione ad Altri DBMS
//Per supportare altri DBMS (ad esempio MySQL, PostgreSQL, ecc.), devi solo implementare una nuova classe che implementi IDatabase per quel particolare DBMS. Le chiamate al database rimarranno indistinguibili, mantenendo il tuo codice pulito e mantenibile.

static void Main()
{
    string connectionString = "your_connection_string_here";

    // Crea l'istanza del database (SQL Server in questo caso)
    using (IDatabase database = new SqlServerDatabase(connectionString))
    {
        // Parametri di esempio
        int id = 1;
        var campiValori = new Dictionary<string, object>
            {
                { "Nome", "Esempio Nome" },
                { "Valore", "Esempio Valore" }
            };

        try
        {
            using (IDbTransaction transaction = database.BeginTransaction())
            {
                // Esegui il primo aggiornamento
                database.InsertOrUpdateRecord("LaTuaTabella", "Id", id, campiValori, "UPDATE", transaction);

                // Esegui un altro aggiornamento o inserimento all'interno della stessa transazione
                id = 2;
                campiValori["Nome"] = "Un altro Nome";
                database.InsertOrUpdateRecord("LaTuaTabella", "Id", id, campiValori, "INSERT", transaction);

                // Conferma la transazione
                transaction.Commit();
                Console.WriteLine("Operazioni completate con successo.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante l'operazione: {ex.Message}");
        }
    }
}


//ExecQuery:

//Esegue una query SQL generica.
//Supporta parametri SQL per prevenire SQL injection.
//Permette di specificare un timeout.
//Limita il numero di record restituiti tramite il parametro maxRecords.
//Utilizza SqlDataAdapter per riempire un DataTable, che contiene i risultati della query.
//Gestione del Cursore:

//La funzione adapter.Fill(0, maxRecords, resultTable) gestisce il cursore e limita il numero di record riempiti nel DataTable.
//Vantaggi di Questo Approccio
//Flessibilità: Puoi eseguire qualsiasi query SQL e limitare i risultati per evitare carichi pesanti.
//Sicurezza: L'uso di parametri protegge contro SQL injection.
//Timeout Gestito: Prevede un tempo massimo di esecuzione della query per evitare blocchi.
//Gestione del Cursore: Puoi limitare il numero di record restituiti, utile per query su tabelle molto grandi.

static void Main1()
{
    string connectionString = "your_connection_string_here";

    using (IDatabase database = new SqlServerDatabase(connectionString))
    {
        string query = "SELECT * FROM LaTuaTabella WHERE Nome = @Nome";
        var parameters = new Dictionary<string, object>
            {
                { "@Nome", "Esempio Nome" }
            };

        try
        {
            // Prima pagina (record 1-100)
            DataTable resultPage1 = database.ExecQuery(query, parameters, 30, 100, 1, 100);
            Console.WriteLine("Risultati prima pagina:");
            PrintResults(resultPage1);

            // Seconda pagina (record 101-200)
            DataTable resultPage2 = database.ExecQuery(query, parameters, 30, 100, 2, 100);
            Console.WriteLine("Risultati seconda pagina:");
            PrintResults(resultPage2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante l'esecuzione della query: {ex.Message}");
        }
    }
}




//Conclusioni
//Resilienza: Il codice è ora più resiliente contro errori temporanei che possono verificarsi durante le operazioni di lettura o scrittura di BLOB.

//Flessibilità: Puoi controllare il numero di ritentativi e la durata dell'intervallo tra i tentativi, permettendo di adattare il comportamento a diversi ambienti e condizioni operative.

//Facilità di Debug: I messaggi di errore sono più descrittivi, includendo il numero di tentativi effettuati e il contesto dell'errore.


static void Main2()
{
    string connectionString = "your_connection_string_here";

    using (IDatabase database = new SqlServerDatabase(connectionString))
    {
        try
        {
            byte[] largeBlobData = new byte[50 * 1024 * 1024]; // 50 MB di dati
            new Random().NextBytes(largeBlobData);

            // Scrivi il BLOB in blocchi da 1 MB, con ritentativo
            int blockSize = 1 * 1024 * 1024; // 1 MB
            for (int i = 0; i < largeBlobData.Length; i += blockSize)
            {
                byte[] block = largeBlobData.Skip(i).Take(blockSize).ToArray();
                database.WriteBlob("LaTuaTabella", "Id", 1, "IlCampoBlob", block, null, 5, 2000); // 5 tentativi, 2 secondi tra un tentativo e l'altro
            }
            Console.WriteLine("BLOB scritto con successo.");

            // Leggi il BLOB in blocchi da 1 MB, con ritentativo
            List<byte[]> blobChunks = new List<byte[]>();
            int pageSize = 1 * 1024 * 1024; // 1 MB
            int pageNumber = 1;

            byte[] chunk;
            do
            {
                chunk = database.ReadBlob("LaTuaTabella", "Id", 1, "IlCampoBlob", pageSize, pageNumber, 5, 2000); // 5 tentativi, 2 secondi tra un tentativo e l'altro
                blobChunks.Add(chunk);
                pageNumber++;
            } while (chunk.Length > 0);

            byte[] completeBlob = blobChunks.SelectMany(b => b).ToArray();
            Console.WriteLine($"BLOB letto con successo. Lunghezza: {completeBlob.Length} bytes.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante l'operazione: {ex.Message}");
        }
    }
}


*************/