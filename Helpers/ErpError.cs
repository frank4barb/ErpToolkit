namespace ErpToolkit.Helpers
{
    public static class ErpError
    {

        public const int SUCCESS = 0; // Nessun errore

        public const int ERR_NO_INPUT = -107; // Input obbligatorio non inserito
        public const int ERR_BAD_IDEN = -205; // Identificativo univoco vuoto o ambiguo

        public const int ERR_DB_TRANSACTION = -367; // Errore DB nell'esecuzione transazione
        public const int ERR_DB_DOMAIN = -366; // Il valore del campo è fuori dal dominio previsto
        public const int ERR_DB_DEADLOCK = -362; // Deadlock tra processi
        public const int ERR_DB_BLOB_ERR = -360; //Errore nella gestione dei BLOB
        public const int ERR_DB_BADCOLUMN = -333; // Colonna specificata inesistente
        public const int ERR_DB_TIMESTAMP = -327;  // Il timestamp non è più valido
        public const int ERR_DB_KO = -326; // Errore hardware o database corrotto (fatal)
        public const int ERR_DB_SYNTAX = -318; // Errore di sintassi nel comando sql
        public const int ERR_DB_BADTRAN = -316; // Transazione errata
        public const int ERR_DB_BADDATA = -315; //Tipo di dato errato
        public const int ERR_DB_UNKNOWN = -314; // Nome tabella od altro oggetto DB errato
        public const int ERR_DB_TIMEOUT = -313; // Timeout raggiunto
        public const int ERR_DB_USE = -347; // Connessione al database non riuscita
        public const int ERR_DB_LOGIN = -331; // Errore di 'Login' durante l'avvio della connessione con il database
        public const int ERR_DB_ERROR = -300; // Errore generico del database
        public const int ERR_DB_DEPENDENCY = -3000; // Il record non può essere cancellato perché è referenziato in altri record
        public const int ERR_DB_DUPLICATION = -2000; // Chiave univoca duplicata
        public const int ERR_DB_UNK_REF = -1000; // Violazione integrità referenziale: la chiave di relazione non esiste 


        public static string GetErrorMessage(int errorCode)
        {
            switch (errorCode)
            {
                case SUCCESS: return "Nessun errore";
                default: return "Errore sconosciuto";
            }
        }


    }
}