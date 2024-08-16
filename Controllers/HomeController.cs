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

            //XXXXXXXX
            userModel = new User();
            //XXXXXXXX



        }



        ////==========================================================================================================
        ////==========================================================================================================

        //// GESTIONE LOGIN SULLA PAGINA HOME
        ////---------------------------------

        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl = null)
        {
            if (returnUrl != null)
            {
                ModelState.AddModelError(string.Empty, "E' necessario effettuare la login per accedere alla pagina!");
                HttpContext.Session.SetString(SessionReturnUrl, returnUrl);
            }
            //return View();
            return View("~/Views/Home/Index.cshtml", this);  //passo il Controller alla vista, come Model

        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpPost]
        public async Task<IActionResult> Index(InputLogin Input)
        {
            base.Input2 = Input;
            ModelState.Clear(); //ModelState.ClearValidationState("CompanyName"); //FORZA RICONVALIDA MODELLO >>> https://learn.microsoft.com/it-it/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
            if (!TryValidateModel(Input))
            {
                // logout
                ErpContext.TermSessionAsync(HttpContext); //clean current session ErpContext and LOGOUT
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated) return LocalRedirect(Url.Content("~/"));  // <<<-- ricarica la pagina di login dopo LOGOUT
                //return View(); // <<<-- visualizza gli errori LOGIN
                return View("~/Views/Home/Index.cshtml", this);  //passo il Controller alla vista, come Model
            }



            // login and new session
            bool login = await ErpContext.InitSessionAsync(HttpContext, Input.Matricola, Input.Password, "");
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






        public IActionResult Privacy()
        {
            return View(this);
        }
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
        //



        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        public partial class User
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public string Surname { get; set; }
        }

        [BindProperty]
        public User userModel { get; set; }
        [TempData]
        public string StatusMessage { get; set; }





        // Tag Helper
        //https://learn.microsoft.com/it-it/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-8.0#the-select-tag-helper


        public string Country { get; set; }

        public List<SelectListItem> Countries { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "MX", Text = "Mexico" },
            new SelectListItem { Value = "CA", Text = "Canada" },
            new SelectListItem { Value = "US", Text = "USA"  },
        };







        public void OnGet()
        {
            userModel = new User();
        }

        public IActionResult OnPostUserEdited()
        {
            ModelState.Clear();
            if (!TryValidateModel(userModel))
            {
                return View();
            }

            StatusMessage = "User data was saved!";
            return LocalRedirect(Url.Content("~/"));
        }


        //public JsonResult SearchCountry(string query)
        //{
        //    var dbResult = new List<string>() { "Antonio", "Giovanno", "Mercurio", "Leonida", "Ernesto" }; // _context.Countries.Where(x => x.Name.ToLower().StartWith(query.ToLower())).Take(20).ToList();
        //    return Json(dbResult);
        //}

        [HttpPost]
        public JsonResult GetCountries(string Prefix)
        {
            var Countries = new List<string>() { "Antonio", "Giovanno", "Mercurio", "Leonida", "Ernesto" }; // _context.Countries.Where(x => x.Name.ToLower().StartWith(query.ToLower())).Take(20).ToList();

            //var Countries = (from c in db.Countries
            //                 where c.Name.StartsWith(Prefix)
            //                 select new { c.Name, c.Id });
            return Json(Countries);
        }

        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx





    }
}
