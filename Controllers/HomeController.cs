using ErpToolkit.Helpers;
using ErpToolkit.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using Quartz;
using System.Linq;
using System.Security.Cryptography.Xml;
using NLog.Filters;
using Google.Api;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using ErpToolkit.Helpers.Db;


namespace ErpToolkit.Controllers
{
    /// <summary>
    /// A <see cref="FeatureController"/> that implements API and RPC methods for the connection manager.
    /// </summary>
    public class HomeController : ControllerErp
    {

        //private static NLog.ILogger _logger; //private readonly ILogger<HomeController> _logger;

        public HomeController()
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }



        ////==========================================================================================================
        ////==========================================================================================================

        //// GESTIONE LOGIN SULLA PAGINA HOME
        ////---------------------------------

        //[Authorize(AuthenticationSchemes = "Cookies")]  //<<< non scommentare: innesca un cilco
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            if (returnUrl != null)
            {
                ModelState.AddModelError(string.Empty, "E' necessario effettuare la login per accedere alla pagina!");
                HttpContext.Session.SetString(SessionReturnUrl, returnUrl);
            }
            //return View();
            return View("~/Views/Home/Index.cshtml", this);  //passo il Controller alla vista, come Model

        }

        //[Authorize(AuthenticationSchemes = "Cookies")]  //<<< non scommentare: innesca un cilco
        [HttpPost]
        public async Task<IActionResult> Index(InputLogin Input)
        {
            base.Input2 = Input;
            ModelState.Clear(); //ModelState.ClearValidationState("CompanyName"); //FORZA RICONVALIDA MODELLO >>> https://learn.microsoft.com/it-it/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
            if (!TryValidateModel(Input))
            {
                // logout
                await ErpContext.TermSessionAsync(HttpContext); //clean current session ErpContext and LOGOUT
                //ErpContext.TermSession(HttpContext); //clean current session ErpContext and LOGOUT
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated) return LocalRedirect(Url.Content("~/"));  // <<<-- ricarica la pagina di login dopo LOGOUT
                //return View(); // <<<-- visualizza gli errori LOGIN
                return View("~/Views/Home/Index.cshtml", this);  //passo il Controller alla vista, come Model
            }



            // login and new session
            bool login = await ErpContext.InitSessionAsync(HttpContext, Input.Matricola, Input.Password, "");
            //bool login = ErpContext.InitSession(HttpContext, Input.Matricola, Input.Password, "");
            if (!login)
            {
                //errore utente non abilitato
                ModelState.AddModelError(string.Empty, "Matricola o Password non valide!");
                //return View(); // <<<-- visualizza gli errori
                return View("~/Views/Home/Index.cshtml", this);  //passo il Controller alla vista, come Model
            }

            //redirect to ReturnUrl
            string? returnUrl = HttpContext.Session.GetString(SessionReturnUrl); HttpContext.Session.Remove(SessionReturnUrl); //scarico il ReturnUrl
            returnUrl ??= Url.Content("~/"); //default
            return LocalRedirect(returnUrl);
        }

        ///// <summary>
        ///// Sends a command to the connection manager.
        ///// </summary>
        ///// <param name="endpoint">The endpoint in string format. Specify an IP address. The default port for the network will be added automatically.</param>
        ///// <param name="command">The command to run. {add, remove, onetry}</param>
        ///// <returns>Json formatted <c>True</c> indicating success. Returns <see cref="IActionResult"/> formatted exception if fails.</returns>
        ///// <remarks>This is an API implementation of an RPC call.</remarks>
        ///// <exception cref="ArgumentException">Thrown if either command not supported/empty or if endpoint is invalid/empty.</exception>
        //[HttpGet]
        ////public IActionResult Index()
        ////{
        ////    return View();
        ////}
        //[HttpGet]
        //public async Task<IActionResult> Index(string returnUrl = null)
        //{
        //    if (returnUrl != null)
        //    {
        //        ModelState.AddModelError(string.Empty, "E' necessario effettuare la login per accedere alla pagina!");
        //        HttpContext.Session.SetString(SessionReturnUrl, returnUrl);
        //    }
        //    return View();
        //}
        ////[HttpPost]
        ////public async Task<IActionResult> Index()
        ////{
        ////    if (!ModelState.IsValid)
        ////    {
        ////        // logout
        ////        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        ////        ErpContext.TermSession(HttpContext); //clean current session ErpContext
        ////        if (HttpContext != null && HttpContext.Session != null) HttpContext.Session.Clear();
        ////        return LocalRedirect(Url.Content("~/"));  // <<<-- ricarica la pagina di login
        ////    }


        ////    //check for allowed user
        ////    if (!ErpContext.Instance.GetString("#usersAllowedList").Contains(Input.Matricola))
        ////    {
        ////        //errore utente non abilitato
        ////        ModelState.AddModelError(string.Empty, "Matricola o Password non valide!");
        ////        return View(); // <<<-- visualizza gli errori
        ////    }
        ////    //check LDAP  ???????
        ////    //....
        ////    //....
        ////    //....
        ////    //....

        ////    var claims = new List<Claim>
        ////        {
        ////            new Claim(ClaimTypes.NameIdentifier, Input.Matricola),
        ////            new Claim(ClaimTypes.Name, Input.Matricola),   // << consente di recuperare la matricola di login tramite:  User.Identity?.Name
        ////            new Claim("UserDefined", "whatever"),
        ////        };

        ////    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ////    var principal = new ClaimsPrincipal(identity);

        ////    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        ////            principal,
        ////            new AuthenticationProperties { IsPersistent = true });

        ////    //save session ErpContext
        ////    ErpContext.InitSession(HttpContext, Input.Matricola, "");

        ////    //redirect to ReturnUrl
        ////    string? returnUrl = HttpContext.Session.GetString(SessionReturnUrl); HttpContext.Session.Remove(SessionReturnUrl); //scarico il ReturnUrl
        ////    returnUrl ??= Url.Content("~/"); //default
        ////    return LocalRedirect(returnUrl);
        ////}



        ////==========================================================================================================
        ////==========================================================================================================
        //// GESTIONE MENU' PERCORSI
        ////---------------------------------

        //Gestore dei Menù a Percorsi.
        //----------------------------
        // - Ogni voce di menù prevede l'esecuzione di un percorso di pagine
        // - Si passa alla pagina successiva mediante il link della tabella (ie: campo [UID] del Modello)
        // - Nell'accesso alla pagina successiva possono essere forzati dei valori di default per i campi del modello, e impostate le variabili readOnly e visible dei campi del filtro di selezione (eg: AddDefault("Te1Icode", "UCSC", false, true) -->  Te1Icode__NY=UCSC)
        // - Si può impostare solo readOnly e visible passando il contenuto del campo vuoto
        // - Le impostazioni di Default dei campi possono dipendere dall'appartenenza di Gruppi e Profili dell'utente che ha fatto Login
        // - .... è possibile impostare dei controlli al passagio di pagina, e bloccare gli utenti se non hanno compilato tutti i campi previsti per il profilo.


        //<ul class="navbar-nav mr-auto">
        //    <li class="nav-item">
        //        <a class="nav-link" asp-controller="Shared" asp-action="Percorso1Start">Percorso 1</a>
        //    </li>
        //    <li class="nav-item">
        //        <a class="nav-link" asp-controller="Shared" asp-action="Percorso2Start">Percorso 2</a>
        //    </li>
        //</ul>

        //// Use HomeController's method to determine the next page
        //return RedirectToAction("RedirectToNextPage", "Shared", new { currentPage = "Page1", .....  });
        //
        //eg:   @Html.ActionLink(item.PaCodSanitario, "RedirectToNextPage", "Shared", null, null, null, new { provenienzaPagina = "Paziente", Pa1Icode = item.Pa1Icode, Te1Icode="UCSC"}, null)



        //Impostazione campi da applicare al filtro selezione della pagina del percorso
        // 1) valore di default del campo
        // 2) campo readOnly: Y/N (default: N)
        // 3) campo visibile: Y/N (default: Y)
        //
        // formato: <nome campo modello>=<valore preimpostato del campo>
        // formato: <nome campo modello>_Attr=<Y/N readOnly><Y/N visibile>
        // nota: i nomi campo del modello non possono contenere '_' nel nome del campo

        //pagina del percorso
        public class Page
        {
            public string pageName { get; set; } = "";
            //public List<DefaultFieldValue> defaultFields { get; set; } = new List<DefaultFieldValue>();
            public Dictionary<string, string?> defaultFields { get; set; } = new Dictionary<string, string?>();
            public Page(string name) { pageName = name; }
            public Page AddDefault(string name, string? value) { defaultFields[name] = value; return this; }
        }

        //------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------

        public static readonly Dictionary<string, List<Page>> PathMenu = new Dictionary<string, List<Page>> {
             { "Percorso1", new List<Page> {
                 new Page("Paziente")
                        .AddDefault("PaNome_Attr", DogManager.FieldAttr.strAttr(true, false))
                 , new Page("Episodio").AddDefault("Te1Icode", "UCSC")
                        .AddDefault("Te1Icode_Attr", DogManager.FieldAttr.strAttr(true, true))
                        .AddDefault("EpLetto_Attr", DogManager.FieldAttr.strAttr(true, true)) } }
            ,{ "Percorso2", new List<Page> { new Page("Page2"), new Page("Page1") } }
            };

        //------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------

        //[Authorize(AuthenticationSchemes = "Cookies")]  //<<< non scommentare: innesca un cilco
        [HttpGet]
        public IActionResult Percorso1Start() { return RedirectToStartPage("Percorso1"); }

        //[Authorize(AuthenticationSchemes = "Cookies")]  //<<< non scommentare: innesca un cilco
        [HttpGet]
        public IActionResult Percorso2Start() { return RedirectToStartPage("Percorso2"); }

        //------------------------------------------------------------------------------------------------------------------------------------------

        public IActionResult RedirectToStartPage(string nomePercorso)
        {
            TempData["NomeSequenzaPagine"] = nomePercorso; // Set data in TempData (i dati rimangono in memoria solo per la prossima request, poi sono cancellati dalla struttura
            return RedirectToAction("Index", PathMenu[nomePercorso][0].pageName);
        }

        public IActionResult RedirectToNextPage(string provenienzaPagina)
        {
            try
            {
                RouteValueDictionary routeValuesDictionary = new RouteValueDictionary();
                foreach (var key in Request.Query.Keys) routeValuesDictionary.Add(key, Request.Query[key]);
                //routeValuesDictionary.Add("AnotherFixedParm", "true");

                // trovo percorso
                string nomePercorso = TempData["NomeSequenzaPagine"] as string; TempData["NomeSequenzaPagine"] = nomePercorso; //ricarico per mantenere in memoria
                List<Page> sequenzaPagine = PathMenu[nomePercorso];
                // calcolo prossima pagina in precorso
                int provenienzaPaginaIdx = sequenzaPagine.FindIndex(page => page.pageName.Equals(provenienzaPagina, StringComparison.Ordinal));  //int provenienzaPaginaIdx = sequenzaPagine.IndexOf(provenienzaPagina);
                int successivaPaginaIdx = provenienzaPaginaIdx + 1;
                var nextPage = sequenzaPagine[0]; // pagina di default = prima pagina (If there are no more pages in the sequence, redirect to the first page)
                if (successivaPaginaIdx < sequenzaPagine.Count) nextPage = sequenzaPagine[successivaPaginaIdx]; // trovata pagina successiva
                //inserisco inpostazioni di default per i cxampi di selezione
                foreach (var key in nextPage.defaultFields.Keys) routeValuesDictionary.Add(key, nextPage.defaultFields[key]);
                //redirect
                return RedirectToAction("Index", nextPage.pageName, routeValuesDictionary);
            }
            catch (Exception ex) { return RedirectToAction("Index", provenienzaPagina); }  // in caso di problemi rimango sulla stessa pagina
        }


        //##############################################################################################
        //####
        //#### ERROR
        //####
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //##############################################################################################
        //####
        //#### MENU COMUNI
        //####


        ////////////////////////public IActionResult Index()
        ////////////////////////{
        ////////////////////////    //return RedirectToAction("Index", "Home");
        ////////////////////////    return RedirectToAction("Privacy", "Home");
        ////////////////////////}

        ////////////////////////public IActionResult Privacy()
        ////////////////////////{
        ////////////////////////    return RedirectToAction("Privacy", "Home");
        ////////////////////////}
        ////////////////////////public IActionResult Mermaid()
        ////////////////////////{
        ////////////////////////    return RedirectToAction("Mermaid", "Home");
        ////////////////////////}
        ////////////////////////public IActionResult ERModel()
        ////////////////////////{
        ////////////////////////    return RedirectToAction("ERModel", "Home");
        ////////////////////////}
        ////////////////////////public IActionResult Table()
        ////////////////////////{
        ////////////////////////    // Uso di XXXX https://dotnettutorials.net/lesson/viewdata-asp-net-core-mvc/#:~:text=To%20use%20ViewData%20in%20the,key%2C%20such%20as%20the%20following.

        ////////////////////////    // Set data in TempData (i dati rimangono in memoria solo per la prossima request, poi sono cancellati dalla struttura
        ////////////////////////    TempData["Title"] = "Table from Home";

        ////////////////////////    //return LocalRedirect(Url.Content("~/Datatable/Table"));
        ////////////////////////    return RedirectToAction("Table", "Datatable");

        ////////////////////////}


        //----------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------

        public IActionResult DefaultPage()
        {
            return RedirectToAction("Index", "Home");
        }

        //public IActionResult Privacy()
        //{
        //    return View(this);
        //}
        public IActionResult Mermaid()
        {
            return View(this);
        }
        public IActionResult ERModel()
        {
            return View(this);
        }
        public IActionResult Table()
        {
            // Uso di XXXX https://dotnettutorials.net/lesson/viewdata-asp-net-core-mvc/#:~:text=To%20use%20ViewData%20in%20the,key%2C%20such%20as%20the%20following.

            // Set data in TempData (i dati rimangono in memoria solo per la prossima request, poi sono cancellati dalla struttura
            TempData["Title"] = "Table from Home";

            //return LocalRedirect(Url.Content("~/Datatable/Table"));
            return RedirectToAction("Table","Datatable");

        }

    }
}
