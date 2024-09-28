using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;
using System.Text;
using ErpToolkit.Helpers.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

using MemoryPack;
using System.Data;


namespace ErpToolkit.Helpers
{
    //classe SINGLETON con il contesto dello scheduler 
    //eg: if ((param = ErpContext.Instance.GetParam("#SecondlyJob")) != "") await ConfigureSecondlyJob(scheduler, param);
    //[MemoryPackable(GenerateType.VersionTolerant)]
    public class ErpContext
    {
        public const int SessionMinuteTimeout = 20;   // timeout di sessione: la sessione viene cancellata 

        private const string SessionUserId = "_SessionUserId";

        //proprietà SERVER
        private static ErpContext? _instance = null;   //contesto server
        //private static ErpContext? _instanceCLONE = null;   //clone contesto server da usare per inizializzare le sessioni client
        private static IDictionary<string, ErpContext>? _sessions = null;    //contesto di sessione

        //proprietà CLIENT
        private IDictionary<string, string> _itemsString = new Dictionary<string, string>();
        private IDictionary<string, long> _itemsLong = new Dictionary<string, long>();
        private IDictionary<string, object> _itemsObject = new Dictionary<string, object>();  // <<<<--- gli oggetti non vengono clonati

        //propietà pubbliche
        public string CurrentDirectory = Environment.CurrentDirectory;
        public string PathIniFile = Environment.CurrentDirectory + "\\" + "ERPdesktop.ini";
        public string PathDogFile = Environment.CurrentDirectory + "\\" + "ERPdatamodel.cfg";
        public DateTime StartTime = DateTime.Now;
        public DateTime LastUpdateTime = DateTime.Now;
        //--
        public string UserId = "";
        public string UnitId = "";
        //--

        //propietà pubbliche gestore DB
        public DatabaseFactory DbFactory = new DatabaseFactory();

        // PROCESSO SCHEDULATO CHE CANCELLA LE SESSIONI SCADUTE
        private void ScheduledCleanSessionJob()
        {
            System.Threading.Timer timer = null;
            timer = new Timer( s =>
            {
                try
                {
                    DateTime Now = DateTime.Now;
                    foreach (string key in _sessions.Keys) {
                        if ((int)Now.Subtract(LastUpdateTime).TotalMinutes > SessionMinuteTimeout) {  
                            // libera memoria sessione se superato il timeout 
                            _sessions[key]._itemsString.Clear();
                            _sessions[key]._itemsLong.Clear();
                            _sessions[key]._itemsObject.Clear();
                            _sessions.Remove(key);
                        }
                    }   
                }
                catch
                {
                    // SKIP // Handle the exception here, but make sure next job is scheduled if an exception is thrown by your code.
                }
                ScheduledCleanSessionJob(); // Schedule next job
                timer?.Dispose(); // Dispose the timer for the current job
            }, null, 60000, Timeout.Infinite);  // esegue ogni minuto
        }


        //COSTRUTTORE STATICO SINGLETON
        private ErpContext()
        {
        }
        ~ErpContext()
        {
            Dispose();
        }
        public static void Init()
        {
            if (_instance == null)
            {
                _instance = new ErpContext();
                // check INI file
                if (!File.Exists(_instance.PathIniFile)) { _instance.PathIniFile = Environment.CurrentDirectory + "\\..\\" + "ERPdesktop.ini"; }
                if (!File.Exists(_instance.PathIniFile)) { throw new ErpException("Impossibile caricare il file di inizializzazione: " + _instance.PathIniFile); }
                //Load Context
                _instance._itemsString = readIniFile(_instance.PathIniFile, _instance._itemsString);  //load DHEdesktop.ini
                                                                                                      //_instanceCLONE = UtilHelper.DeepCopy<ErpContext>(_instance); // crea una copia da assegnare alla sessione

                //Load DOG Layer
                if (!File.Exists(_instance.PathDogFile)) { _instance.PathDogFile = Environment.CurrentDirectory + "\\..\\" + "ERPdatamodel.cfg"; }
                if (!File.Exists(_instance.PathDogFile)) { throw new ErpException("Impossibile caricare il file DOG di configurazione: " + _instance.PathDogFile); }
                DogManager.Init(_instance.PathDogFile, "SqlServer", "#connectionString_SQLSLocal");

                _sessions = new Dictionary<string, ErpContext>();    //inizializza sessioni del server
                _instance.ScheduledCleanSessionJob();   // attiva la schedulazione del task di cancellazione delle sessioni scadute
            }
        }
        public void Dispose()
        {
            // Rilascia risorse non gestite
            if (DbFactory != null) { DbFactory.Dispose(); DbFactory = null; }
            if (_itemsObject != null)
            {
                foreach (var key in _itemsObject.Keys) { if (_itemsObject[key] is IDisposable disposable) { disposable.Dispose(); } _itemsObject.Remove(key); }
                _itemsObject.Clear(); _itemsObject = null;
            }
            if (_itemsLong != null) { _itemsLong.Clear(); _itemsLong = null; }
            if (_itemsString != null) { _itemsString.Clear(); _itemsString = null; }
            // .........
            GC.SuppressFinalize(this);
        }


        private ErpContext(ErpContext fromObj)
        {
            UserId = fromObj.UserId;
            UnitId = fromObj.UserId;
            foreach (string key in fromObj._itemsString.Keys) { _itemsString[key] = fromObj._itemsString[key]; }
            foreach (string key in fromObj._itemsLong.Keys) { _itemsLong[key] = fromObj._itemsLong[key]; }
            //!! non duplico gli oggetti !!
        }
        public static ErpContext Instance
        {
            get
            {
                Init();  // inizializza solo la prima volta
                return _instance;
            }
        }

        //FUNZIONI STATICHE GESTIONE SESSIONE CLIENT & LOGIN
        public static ErpContext? Session(HttpContext httpContext)
        {
            if (httpContext == null || httpContext.Session == null) return null;
            string? sessionUserId = httpContext.Session.GetString(SessionUserId);
            ErpContext? clientSession = _sessions[httpContext.Session.Id];
            if (sessionUserId == null || clientSession == null || sessionUserId != clientSession.UserId) 
            {
                ErpContext.TermSessionAsync(httpContext); return null;
                //ErpContext.TermSession(httpContext); return null;
            }
            return clientSession;
        }



        //InitSessionAsync -- TermSessionAsync

        public static async Task<bool> InitSessionAsync(HttpContext httpContext, string userId, string userPwd, string unitId = "")
        {
            //check for allowed user
            if (!ErpContext.Instance.GetString("#usersAllowedList").Contains(userId)) return false; // Matricola o Password non valide!
            //check LDAP  
            string ldap_server = ErpContext.Instance.GetString("#ldapServerAddress");
            if (ldap_server == "" && userPwd == "testLdap") ; // skip LDAP for test
            else if (!UtilHelper.LoginLdap(ldap_server, userId, userPwd)) return false; // Matricola o Password non valide!

            // LOGIN
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userId),   // << consente di recuperare la matricola di login tramite:  User.Identity?.Name
                    new Claim("UserDefined", "whatever"),
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,        // LOGIN
                    principal,
                    new AuthenticationProperties { IsPersistent = true });

            //save session ErpContext

            //ErpContext clientSession = UtilHelper.DeepCopy<ErpContext>(ErpContext.Instance); // crea una copia da assegnare alla sessione
            //ErpContext clientSession = UtilHelper.DeepCopy<ErpContext>(_instanceCLONE); // crea una copia da assegnare alla sessione

            //ErpContext clientSession = new ErpContext(); // crea una copia da assegnare alla sessione (rilegendo il file .ini)  ???????????????
            ErpContext clientSession = new ErpContext(ErpContext.Instance); // crea una copia CLONE da assegnare alla sessione 


            clientSession.UserId = userId;
            clientSession.UnitId = unitId;
            clientSession.StartTime = DateTime.Now;
            clientSession.LastUpdateTime = DateTime.Now;
            httpContext.Session.SetString(SessionUserId, userId);
            _sessions[httpContext.Session.Id] = clientSession;
            return true;
        }
        public static async Task TermSessionAsync(HttpContext httpContext)
        {
            if (httpContext != null)
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  // LOGOUT
                if (httpContext.Session != null)
                {
                    _sessions.Remove(httpContext.Session.Id);
                    httpContext.Session.Remove(SessionUserId); httpContext.Session.Clear();

                }
            }
        }

        //InitSession -- TermSession

        //public static bool InitSession(HttpContext httpContext, string userId, string userPwd, string unitId = "")
        //{
        //    //check for allowed user
        //    if (!ErpContext.Instance.GetString("#usersAllowedList").Contains(userId)) return false; // Matricola o Password non valide!
        //    //check LDAP  
        //    string ldap_server = ErpContext.Instance.GetString("#ldapServerAddress");
        //    if (ldap_server == "" && userPwd == "testLdap") ; // skip LDAP for test
        //    else if (!UtilHelper.LoginLdap(ldap_server, userId, userPwd)) return false; // Matricola o Password non valide!

        //    // LOGIN
        //    var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, userId),
        //            new Claim(ClaimTypes.Name, userId),   // << consente di recuperare la matricola di login tramite:  User.Identity?.Name
        //            new Claim("UserDefined", "whatever"),
        //        };

        //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    var principal = new ClaimsPrincipal(identity);

        //    httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,        // LOGIN
        //            principal,
        //            new AuthenticationProperties { IsPersistent = true });

        //    //save session ErpContext

        //    //ErpContext clientSession = UtilHelper.DeepCopy<ErpContext>(ErpContext.Instance); // crea una copia da assegnare alla sessione
        //    //ErpContext clientSession = UtilHelper.DeepCopy<ErpContext>(_instanceCLONE); // crea una copia da assegnare alla sessione

        //    //ErpContext clientSession = new ErpContext(); // crea una copia da assegnare alla sessione (rilegendo il file .ini)  ???????????????
        //    ErpContext clientSession = new ErpContext(ErpContext.Instance); // crea una copia CLONE da assegnare alla sessione 


        //    clientSession.UserId = userId;
        //    clientSession.UnitId = unitId;
        //    clientSession.StartTime = DateTime.Now;
        //    clientSession.LastUpdateTime = DateTime.Now;
        //    httpContext.Session.SetString(SessionUserId, userId);
        //    _sessions[httpContext.Session.Id] = clientSession;
        //    return true;
        //}
        //public static void TermSession(HttpContext httpContext)
        //{
        //    if (httpContext != null)
        //    {
        //        httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  // LOGOUT
        //        if (httpContext.Session != null)
        //        {
        //            _sessions.Remove(httpContext.Session.Id);
        //            httpContext.Session.Remove(SessionUserId); httpContext.Session.Clear();

        //        }
        //    }
        //}






        //RILEGGI FILE INI
        public void ReloadIniFile()
        {
            LastUpdateTime = DateTime.Now;
            _itemsString = readIniFile(PathIniFile, _itemsString);  //load DHEdesktop.ini
        }

        //GESTIONE PARAMETRI
        public string GetString(string name)
        {
            LastUpdateTime = DateTime.Now;
            if (_itemsString.ContainsKey(name)) return _itemsString[name];
            return "";
        }
        public long GetLong(string name)
        {
            LastUpdateTime = DateTime.Now;
            if (_itemsLong.ContainsKey(name)) return _itemsLong[name];
            return 0L;
        }
        public object? GetObject(string name)
        {
            LastUpdateTime = DateTime.Now;
            if (_itemsObject.ContainsKey(name)) return _itemsObject[name];
            return null;
        }
        public void Set(string name, object? value)
        {
            LastUpdateTime = DateTime.Now;
            if (value == null) { _itemsString.Remove(name); _itemsLong.Remove(name); _itemsObject.Remove(name); }
            else if (value is string) _itemsString[name] = (string)value;   //if the compatibility check is sufficient, use the is operator; instead of: typeof, GetType (https://pvs-studio.com/en/blog/terms/6527/)
            else if (value is long) _itemsLong[name] = (long)value;   //if the compatibility check is sufficient, use the is operator; instead of: typeof, GetType (https://pvs-studio.com/en/blog/terms/6527/)
            else _itemsObject[name] = value; 
        }



        //GESTIONE DB
        public SQLSERVERHelper getSQLSERVERHelper(string connStringName)
        {
            SQLSERVERHelper? sqlsv = (SQLSERVERHelper)GetObject("SQLSERVERhelper" + connStringName);
            if ((sqlsv == null))
            {
                sqlsv = new SQLSERVERHelper(connStringName);
                Set("SQLSERVERhelper" + connStringName, sqlsv);
            }
            return sqlsv;
        }
        public IRISHelper getIRISHelper(string connStringName)
        {
            IRISHelper? sqlsv = (IRISHelper)GetObject("IRIShelper" + connStringName);
            if ((sqlsv == null))
            {
                sqlsv = new IRISHelper(connStringName);
                Set("IRIShelper" + connStringName, sqlsv);
            }
            return sqlsv;
        }


        //=========================================================================================================================================
        //=========================================================================================================================================

        //private
        private static IDictionary<string,string> readIniFile(string FileName, IDictionary<string, string> items)
        {
            FileStream? fs; StreamReader? sr;
            if (System.IO.File.Exists(FileName))
            {
                try
                {
                    fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
                    sr = new StreamReader(fs, Encoding.UTF8);
                    while (true)
                    {
                        string line = Strings.Trim(sr.ReadLine()); line = Strings.Replace(line, Constants.vbTab, " "); line = Strings.Trim(line);
                        if (line != "" && line.StartsWith(";") == false)
                        {
                            string name = ""; string value = ""; int idx = line.IndexOf(' ');
                            if (idx > 0)
                            {
                                name = Strings.Trim(Strings.Mid(line, 1, idx + 1));
                                value = Strings.Trim(Strings.Mid(line, idx + 1));
                                items.Add(name, value);
                            }
                        }
                        if (sr.EndOfStream) break;
                    }
                    sr.Close(); fs.Close(); sr = null; fs = null;
                }
                catch (Exception ex) { sr = null; fs = null; }
            }
            return items;
        }


    }
}
