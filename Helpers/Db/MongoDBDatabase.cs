using System.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data.Common;
using static ErpToolkit.Helpers.ErpError;

namespace ErpToolkit.Helpers.Db
{
    public class MongoDbDatabase : IDatabase, IDisposable
    {
        //private MongoClient _connection;
        //private IMongoDatabase _database;
        //private MongoDbTransaction _transaction;

        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly IMongoDatabase _database;
        private IClientSessionHandle _session = null;
        private static readonly object _lock = new object();

        public MongoDbDatabase(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
            var client = new MongoClient(_connectionString);
            _database = client.GetDatabase(_databaseName);
        }
        ~MongoDbDatabase()
        {
            Dispose();
        }
        public void Dispose()
        {
            try { /* LIBERA MEMORIA ALLOCATA */ } catch (Exception ex) { /*skip*/ }
            GC.SuppressFinalize(this);
        }

        // Gestione connessione (MongoDB mantiene la connessione aperta attraverso MongoClient)
        public IDbConnection NewConnection()
        {
            throw new NotImplementedException("MongoDB gestisce le connessioni attraverso MongoClient e non utilizza connessioni IDbConnection.");
        }

        public void ReleaseConnection(IDbConnection connection)
        {
            throw new NotImplementedException("MongoDB gestisce le connessioni attraverso MongoClient e non utilizza connessioni IDbConnection.");
        }

        // MongoDB non utilizza comandi SQL ma operazioni su documenti
        public IDbCommand NewCommand(string sql, IDbConnection connection)
        {
            throw new NotImplementedException("MongoDB non utilizza comandi SQL.");
        }

        public DataTable QueryReader(IDbCommand command, int maxRecords)
        {
            throw new NotImplementedException("Utilizzare le operazioni di lettura del driver MongoDB.");
        }

        // Bulk Insert di una DataTable come documenti BSON
        public void BulkInsertDataTable(string tableName, DataTable dataTable)
        {
            var collection = _database.GetCollection<BsonDocument>(tableName);

            var documents = new List<BsonDocument>();
            foreach (DataRow row in dataTable.Rows)
            {
                var document = new BsonDocument();
                foreach (DataColumn column in dataTable.Columns)
                {
                    document[column.ColumnName] = BsonValue.Create(row[column]);
                }
                documents.Add(document);
            }

            if (_session != null)
            {
                collection.InsertMany(_session, documents);
            }
            else
            {
                collection.InsertMany(documents);
            }
        }

        // Gestione transazioni
        public void BeginTransaction(string transactionName)
        {
            if (_session == null)
            {
                var client = _database.Client;
                _session = client.StartSession();
                _session.StartTransaction();
            }
        }

        public void SavePointTransaction(string savePointName)
        {
            // MongoDB non supporta direttamente i savepoints nelle transazioni
            throw new NotImplementedException("Savepoints non sono supportati in MongoDB.");
        }

        public void CommitSavePoint(string savePointName)
        {
            // Savepoints non applicabili in MongoDB
            throw new NotImplementedException("Commit Savepoint non supportato in MongoDB.");
        }

        public void RollbackSavePoint(string savePointName)
        {
            // Savepoints non applicabili in MongoDB
            throw new NotImplementedException("Rollback Savepoint non supportato in MongoDB.");
        }

        public void CommitTransaction(string transactionName)
        {
            if (_session != null)
            {
                _session.CommitTransaction();
                _session.Dispose();
                _session = null;
            }
        }

        public void RollbackTransaction(string transactionName)
        {
            if (_session != null)
            {
                _session.AbortTransaction();
                _session.Dispose();
                _session = null;
            }
        }

        // Util errori
        public bool IsTransient(Exception ex)
        {
            // MongoDB utilizza MongoException e ha proprie regole per determinare gli errori transitori
            if (ex is MongoException mongoEx)
            {
                // Esempio di codice per rilevare errori transitori in MongoDB
                if (mongoEx.HasErrorLabel("TransientTransactionError") ||
                    mongoEx.HasErrorLabel("UnknownTransactionCommitResult"))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HandleException(Exception ex)
        {
            if (ex is MongoException mongoEx)
            {
                switch (mongoEx.HResult)
                {
                    case 11000: // Duplicate key error
                        throw new DatabaseException(ERR_DB_DUPLICATION, "Duplicate key error in MongoDB.", ex);
                    case 50: // Exceeded time limit
                        throw new DatabaseException(ERR_DB_TIMEOUT, "Operation exceeded time limit.", ex);
                    default:
                        throw new DatabaseException(ERR_DB_ERROR, "A MongoDB error occurred.", ex);
                }
            }
            else return false;
        }


        //**************************************************************************************

        public static void Test1()
        {
            string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("testdb");
            var collection = database.GetCollection<BsonDocument>("testcollection");

            var document = new BsonDocument
        {
            { "Id", 1 },
            { "Name", "Name1" },
            { "Value", "Value1" }
        };

            UpsertDocument1(collection, document);
        }
        private static void UpsertDocument1(IMongoCollection<BsonDocument> collection, BsonDocument document)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Id", document["Id"]);
            var update = Builders<BsonDocument>.Update
                .Set("Name", document["Name"])
                .Set("Value", document["Value"]);

            var options = new UpdateOptions { IsUpsert = true };

            var result = collection.UpdateOne(filter, update, options);
            Console.WriteLine($"Matched: {result.MatchedCount}, Modified: {result.ModifiedCount}, Upserted: {result.UpsertedId}");
        }

        public static void Test2()
        {
            string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("testdb");
            var collection = database.GetCollection<BsonDocument>("testcollection");

            using (var session = client.StartSession())
            {
                session.StartTransaction();

                try
                {
                    var document = new BsonDocument
                {
                    { "Id", 1 },
                    { "Name", "Name1" },
                    { "Value", "Value1" }
                };

                    UpsertDocument2(session, collection, document);

                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Transaction aborted due to an error: {ex.Message}");
                    session.AbortTransaction();
                }
            }
        }
        private static void UpsertDocument2(IClientSessionHandle session, IMongoCollection<BsonDocument> collection, BsonDocument document)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Id", document["Id"]);
            var update = Builders<BsonDocument>.Update
                .Set("Name", document["Name"])
                .Set("Value", document["Value"]);

            var options = new UpdateOptions { IsUpsert = true };

            var result = collection.UpdateOne(session, filter, update, options);
            Console.WriteLine($"Matched: {result.MatchedCount}, Modified: {result.ModifiedCount}, Upserted: {result.UpsertedId}");
        }


        //Esempio di Salvataggio di Dati Vettoriali
        public class VectorData
        {
            public ObjectId Id { get; set; }
            public string Description { get; set; }
            public double[] Vector { get; set; }
        }
        public static void TestVector()
        {
            string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("vectorsdb");
            var collection = database.GetCollection<VectorData>("vectors");

            var vectorData = new VectorData
            {
                Description = "Sample vector",
                Vector = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5 }
            };

            collection.InsertOne(vectorData);
            Console.WriteLine("Vector data saved successfully.");
        }

        public static void TestRecuperoVettoriVicini()
        {
            string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("vectorsdb");
            var collection = database.GetCollection<VectorData>("vectors");

            // Esempio di vettore di query
            var queryVector = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5 };

            var pipeline = new[]
            {
            new BsonDocument("$vectorSearch", new BsonDocument
            {
                { "queryVector", new BsonArray(queryVector) },
                { "path", "Vector" },
                { "k", 5 } // Numero di vettori vicini da recuperare
            })
        };

            var result = collection.Aggregate<BsonDocument>(pipeline).ToList();

            foreach (var doc in result)
            {
                Console.WriteLine(doc.ToJson());
            }
        }

        //Spiegazione
        //Connessione al Database: Utilizza MongoClient per connetterti a MongoDB.
        //Definizione della Collezione: Definisci una collezione VectorData per memorizzare i dati vettoriali.
        //Pipeline di Aggregazione: Utilizza lo stadio di aggregazione $vectorSearch per eseguire una query di ricerca vettoriale. Specifica il vettore di query, il percorso del campo vettoriale e il numero di vicini (k) da recuperare.
        //Esecuzione della Query: Esegui la pipeline di aggregazione e recupera i documenti che contengono i vettori più vicini al vettore di query.
        //Considerazioni
        //Indice Vettoriale: Assicurati di avere un indice vettoriale sul campo Vector per migliorare le prestazioni della ricerca.
        //Modelli di Embedding: Puoi utilizzare modelli di embedding come quelli forniti da OpenAI o Hugging Face per generare i vettori da memorizzare e cercare.
        // 1>>> https://www.mongodb.com/docs/atlas/atlas-vector-search/create-embeddings/
        // 2>>> https://www.mongodb.com/it-it/products/platform/atlas-vector-search
        //Questo approccio ti permette di eseguire ricerche vettoriali efficienti in MongoDB, recuperando i vettori più vicini al vettore di query specificato.


        //********************************
        //Unire il contenuto di un database vettoriale su MongoDB con un modello linguistico di grandi dimensioni(LLM) per fare domande testuali sui documenti è un approccio potente per creare applicazioni di intelligenza artificiale avanzate.Ecco come puoi farlo:

        //Passaggi Generali
        //Creazione degli Embeddings: Utilizza un modello di embedding per trasformare i documenti in vettori.
        //        Salvataggio degli Embeddings in MongoDB: Memorizza questi vettori in un database vettoriale su MongoDB.
        //Integrazione con un LLM: Utilizza un LLM per interpretare le domande testuali e confrontarle con i vettori memorizzati.
        //Ricerca Vettoriale: Esegui una ricerca vettoriale su MongoDB per trovare i documenti più rilevanti.
        //Risposta alle Domande: Utilizza i risultati della ricerca per generare risposte alle domande.
        //Esempio di Implementazione
        //1. Creazione degli Embeddings
        //Puoi utilizzare un modello di embedding come BERT, GPT-3, o un altro modello di embedding per trasformare i documenti in vettori.

        //Python

        //from transformers import AutoTokenizer, AutoModel
        //import torch

        //tokenizer = AutoTokenizer.from_pretrained("bert-base-uncased")
        //model = AutoModel.from_pretrained("bert-base-uncased")

        //def get_embedding(text):
        //    inputs = tokenizer(text, return_tensors = "pt")
        //    outputs = model(**inputs)
        //    return outputs.last_hidden_state.mean(dim= 1).detach().numpy()
        //Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //2. Salvataggio degli Embeddings in MongoDB
        //Utilizza il driver MongoDB per Python per memorizzare i vettori.

        //Python

        //from pymongo import MongoClient
        //import numpy as np

        //client = MongoClient("mongodb://localhost:27017")
        //db = client["vectorsdb"]
        //collection = db["vectors"]

        //document = {
        //    "text": "Sample document text",
        //    "vector": get_embedding("Sample document text").tolist()
        //}

        //    collection.insert_one(document)
        //    Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //3. Integrazione con un LLM
        //Utilizza un LLM per interpretare le domande e generare embeddings per le query.

        //Python

        //query = "What is the sample document about?"
        //query_vector = get_embedding(query)
        //Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //4. Ricerca Vettoriale
        //Esegui una ricerca vettoriale su MongoDB per trovare i documenti più rilevanti.

        //Python

        //from bson import SON

        //pipeline = [
        //    {
        //        "$vectorSearch": {
        //            "queryVector": query_vector.tolist(),
        //            "path": "vector",
        //            "k": 5
        //        }
        //    }
        //]

        //results = collection.aggregate(pipeline)
        //for result in results:
        //    print(result["text"])
        //Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //5. Risposta alle Domande
        //Utilizza i risultati della ricerca per generare risposte alle domande.

        //Python

        //for result in results:
        //    print(f"Relevant document: {result['text']}")
        //Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //Considerazioni
        //Indice Vettoriale: Assicurati di avere un indice vettoriale sul campo vector per migliorare le prestazioni della ricerca1.
        //Modelli di Embedding: Puoi utilizzare modelli di embedding come quelli forniti da OpenAI o Hugging Face per generare i vettori da memorizzare e cercare2.
        //MongoDB Atlas: MongoDB Atlas offre funzionalità di ricerca vettoriale integrate che possono semplificare questo processo1.
        //Questo approccio ti permette di combinare la potenza dei modelli linguistici di grandi dimensioni con la flessibilità e la scalabilità di MongoDB per creare applicazioni di intelligenza artificiale avanzate.

        //************************************
        //Sì, puoi utilizzare C# e .NET Core 8 per creare embeddings e lavorare con MongoDB. Esistono diverse librerie NuGet che possono aiutarti a generare embeddings e a interagire con MongoDB. Ecco un esempio di come puoi farlo:

        //Passaggi Generali
        //Installazione delle Librerie NuGet:
        //MongoDB.Driver: Per interagire con MongoDB.
        //ML.NET: Per creare embeddings.
        //Creazione degli Embeddings: Utilizza ML.NET per generare embeddings dai tuoi dati testuali.
        //Salvataggio degli Embeddings in MongoDB: Memorizza questi embeddings in MongoDB.
        //Ricerca Vettoriale: Esegui una ricerca vettoriale su MongoDB per trovare i documenti più rilevanti.
        //Esempio di Implementazione
        //1. Installazione delle Librerie NuGet
        //Aggiungi i seguenti pacchetti NuGet al tuo progetto:

        //dotnet add package MongoDB.Driver
        //dotnet add package Microsoft.ML

        //2. Creazione degli Embeddings
        //Utilizza ML.NET per generare embeddings dai tuoi dati testuali.

        //C#

        //using System;
        //using Microsoft.ML;
        //using Microsoft.ML.Transforms.Text;

        //public class EmbeddingGenerator
        //    {
        //        private readonly MLContext _mlContext;

        //        public EmbeddingGenerator()
        //        {
        //            _mlContext = new MLContext();
        //        }

        //        public float[] GenerateEmbedding(string text)
        //        {
        //            var data = new[] { new InputData { Text = text } };
        //            var dataView = _mlContext.Data.LoadFromEnumerable(data);

        //            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(InputData.Text));
        //            var model = pipeline.Fit(dataView);
        //            var transformedData = model.Transform(dataView);

        //            var embeddings = _mlContext.Data.CreateEnumerable<TransformedData>(transformedData, reuseRowObject: false);
        //            return embeddings.First().Features;
        //        }

        //        private class InputData
        //        {
        //            public string Text { get; set; }
        //        }

        //        private class TransformedData
        //        {
        //            public float[] Features { get; set; }
        //        }
        //    }
        //    Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //3. Salvataggio degli Embeddings in MongoDB
        //Utilizza il driver MongoDB per memorizzare gli embeddings.

        //C#

        //using MongoDB.Bson;
        //using MongoDB.Driver;

        //public class MongoDBHandler
        //    {
        //        private readonly IMongoCollection<BsonDocument> _collection;

        //        public MongoDBHandler(string connectionString, string databaseName, string collectionName)
        //        {
        //            var client = new MongoClient(connectionString);
        //            var database = client.GetDatabase(databaseName);
        //            _collection = database.GetCollection<BsonDocument>(collectionName);
        //        }

        //        public void InsertDocument(string text, float[] embedding)
        //        {
        //            var document = new BsonDocument
        //        {
        //            { "text", text },
        //            { "embedding", new BsonArray(embedding) }
        //        };

        //            _collection.InsertOne(document);
        //        }
        //    }
        //    Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //4. Ricerca Vettoriale
        //Esegui una ricerca vettoriale su MongoDB per trovare i documenti più rilevanti.

        //C#

        //using MongoDB.Bson;
        //using MongoDB.Driver;

        //public class VectorSearch
        //    {
        //        private readonly IMongoCollection<BsonDocument> _collection;

        //        public VectorSearch(string connectionString, string databaseName, string collectionName)
        //        {
        //            var client = new MongoClient(connectionString);
        //            var database = client.GetDatabase(databaseName);
        //            _collection = database.GetCollection<BsonDocument>(collectionName);
        //        }

        //        public void SearchSimilarDocuments(float[] queryVector, int k)
        //        {
        //            var pipeline = new[]
        //            {
        //            new BsonDocument("$vectorSearch", new BsonDocument
        //            {
        //                { "queryVector", new BsonArray(queryVector) },
        //                { "path", "embedding" },
        //                { "k", k }
        //            })
        //        };

        //            var results = _collection.Aggregate<BsonDocument>(pipeline).ToList();

        //            foreach (var result in results)
        //            {
        //                Console.WriteLine(result["text"]);
        //            }
        //        }
        //    }
        //    Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //Utilizzo dell’Esempio
        //Ecco come puoi utilizzare le classi sopra per generare embeddings, salvarli in MongoDB e cercare documenti simili:

        //C#

        //public class Program
        //    {
        //        public static void Main()
        //        {
        //            var embeddingGenerator = new EmbeddingGenerator();
        //            var mongoDBHandler = new MongoDBHandler("mongodb://localhost:27017", "vectorsdb", "vectors");
        //            var vectorSearch = new VectorSearch("mongodb://localhost:27017", "vectorsdb", "vectors");

        //            string text = "Sample document text";
        //            float[] embedding = embeddingGenerator.GenerateEmbedding(text);

        //            mongoDBHandler.InsertDocument(text, embedding);

        //            float[] queryVector = embeddingGenerator.GenerateEmbedding("Query text");
        //            vectorSearch.SearchSimilarDocuments(queryVector, 5);
        //        }
        //    }
        //    Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //Considerazioni
        //Indice Vettoriale: Assicurati di avere un indice vettoriale sul campo embedding per migliorare le prestazioni della ricerca.
        //Modelli di Embedding: Puoi esplorare altri modelli di embedding disponibili in ML.NET o altre librerie compatibili con.NET.
        //Questo approccio ti permette di utilizzare C# e .NET Core 8 per creare embeddings, memorizzarli in MongoDB e eseguire ricerche vettoriali, integrando così funzionalità avanzate di intelligenza artificiale nella tua applicazione.

        //******************************************************************
        //Certo! Ecco alcune librerie che puoi utilizzare in .NET Core 8.0 per leggere il contenuto di documenti PDF e effettuare operazioni di OCR:

        //1. IronPDF
        //IronPDF è una libreria completa per la manipolazione dei PDF in .NET.Supporta la lettura, la creazione e la modifica dei PDF, oltre a funzionalità di OCR.

        //Installazione:
        //dotnet add package IronPdf

        //Esempio di utilizzo:
        //C#

        //using IronPdf;

        //var pdf = PdfDocument.FromFile("example.pdf");
        //    var text = pdf.ExtractAllText();
        //    Console.WriteLine(text);

        //var ocr = new IronOcr.AutoOcr();
        //    var ocrResult = ocr.ReadPdf("example.pdf");
        //    Console.WriteLine(ocrResult.Text);
        //Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //2. Aspose.PDF
        //Aspose.PDF è una libreria potente per la creazione e la manipolazione dei PDF. Supporta anche l’OCR tramite Aspose.OCR.

        //Installazione:
        //dotnet add package Aspose.PDF
        //dotnet add package Aspose.OCR

        //Esempio di utilizzo:
        //C#

        //using Aspose.Pdf;
        //using Aspose.OCR;

        //Document pdfDocument = new Document("example.pdf");
        //    TextAbsorber textAbsorber = new TextAbsorber();
        //    pdfDocument.Pages.Accept(textAbsorber);
        //string extractedText = textAbsorber.Text;
        //    Console.WriteLine(extractedText);

        //AsposeOcr ocrEngine = new AsposeOcr();
        //    var ocrResult = ocrEngine.RecognizePdf("example.pdf");
        //    Console.WriteLine(ocrResult.Text);
        //Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //3. Tesseract OCR
        //Tesseract è un motore OCR open source che può essere utilizzato in combinazione con altre librerie PDF per estrarre testo dai PDF.

        //Installazione:
        //dotnet add package Tesseract

        //Esempio di utilizzo:
        //C#

        //using System.Drawing;
        //using Tesseract;

        //var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
        //using (var img = Pix.LoadFromFile("example.png"))
        //{
        //    using (var page = engine.Process(img))
        //    {
        //        var text = page.GetText();
        //    Console.WriteLine(text);
        //    }
        //}
        //Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //4. Docotic.Pdf
        //Docotic.Pdf è una libreria ad alte prestazioni per la manipolazione dei PDF in .NET. Supporta l’estrazione del testo e altre operazioni sui PDF.

        //Installazione:
        //dotnet add package BitMiracle.Docotic.Pdf

        //Esempio di utilizzo:
        //C#

        //using BitMiracle.Docotic.Pdf;

        //using (var pdf = new PdfDocument("example.pdf"))
        //{
        //    var options = new PdfTextExtractionOptions { SkipInvisibleText = true, WithFormatting = true };
        //    string formattedText = pdf.GetText(options);
        //    Console.WriteLine(formattedText);
        //}
        //Contenuto generato dall'intelligenza artificiale. Rivedi e usa con attenzione. Ulteriori informazioni su FAQ.
        //Considerazioni
        //IronPDF e Aspose.PDF offrono funzionalità complete per la manipolazione dei PDF e l’OCR.
        //Tesseract OCR è una buona scelta se hai bisogno di un motore OCR open source.
        //Docotic.Pdf è una libreria ad alte prestazioni per la manipolazione dei PDF.
        //Queste librerie ti permetteranno di leggere il contenuto dei documenti PDF e di effettuare operazioni di OCR senza aprire manualmente il contenuto del PDF.

    }
}