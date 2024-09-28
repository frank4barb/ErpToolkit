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
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Policy;


namespace ErpToolkit.Controllers
{
    /// <summary>
    /// A <see cref="FeatureController"/> that implements API and RPC methods for the connection manager.
    /// </summary>
    public class ControllerErp : Controller
    {
        
        protected static NLog.ILogger _logger; //private readonly ILogger<HomeController> _logger;

        public ControllerErp()
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }

        //==========================================================================================================
        //==========================================================================================================

        // VARIABILI GLOBALI
        //---------------

        public const string DbConnectionString = "#connectionString_SQLSLocal";


        //==========================================================================================================
        //==========================================================================================================

        // GESTIONE LOGIN
        //---------------

        public const string SessionReturnUrl = "_ReturnUrl";


        [BindProperty]
        public InputLogin Input2 { get; set; }


        //==========================================================================================================
        //==========================================================================================================



        //[HttpGet]
        // //public IActionResult Index()
        // //{
        // //    return View();
        // //}
        // [HttpGet]
        // public async Task<IActionResult> Index(string returnUrl = null)
        // {
        //     if (returnUrl != null)
        //     {
        //         ModelState.AddModelError(string.Empty, "E' necessario effettuare la login per accedere alla pagina!");
        //         HttpContext.Session.SetString(SessionReturnUrl, returnUrl);
        //     }
        //     return View();
        // }

        //##############################################################################################
        //####
        //#### LOGIN
        //####

        //[HttpPost]
        //public async Task<IActionResult> Login(InputLogin Input)
        //{


        //    ModelState.Clear(); //ModelState.ClearValidationState("CompanyName"); //FORZA RICONVALIDA MODELLO >>> https://learn.microsoft.com/it-it/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
        //    if (!TryValidateModel(Input))
        //    {
        //        // logout
        //        ErpContext.TermSessionAsync(HttpContext); //clean current session ErpContext and LOGOUT
        //        if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated) return LocalRedirect(Url.Content("~/"));  // <<<-- ricarica la pagina di login dopo LOGOUT
        //        return View(); // <<<-- visualizza gli errori LOGIN
        //    }



        //    // login and new session
        //    bool login = await ErpContext.InitSessionAsync(HttpContext, Input.Matricola, Input.Password, "");
        //    if (!login)
        //    {
        //        //errore utente non abilitato
        //        ModelState.AddModelError(string.Empty, "Matricola o Password non valide!");
        //        return View(); // <<<-- visualizza gli errori
        //    }

        //    //redirect to ReturnUrl
        //    string? returnUrl = HttpContext.Session.GetString(SessionReturnUrl); HttpContext.Session.Remove(SessionReturnUrl); //scarico il ReturnUrl
        //    returnUrl ??= Url.Content("~/"); //default
        //    return LocalRedirect(returnUrl);
        //}


        //[HttpPost]
        //public async Task<IActionResult> Login(InputLogin Input)
        //{


        //    ModelState.Clear(); //ModelState.ClearValidationState("CompanyName"); //FORZA RICONVALIDA MODELLO >>> https://learn.microsoft.com/it-it/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
        //    if (!TryValidateModel(Input))
        //    {
        //        // logout
        //        ErpContext.TermSessionAsync(HttpContext); //clean current session ErpContext and LOGOUT
        //        if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated) return LocalRedirect(Url.Content("~/"));  // <<<-- ricarica la pagina di login dopo LOGOUT
        //        return View(); // <<<-- visualizza gli errori LOGIN
        //    }



        //    // login and new session
        //    bool login = await ErpContext.InitSessionAsync(HttpContext, Input.Matricola, Input.Password, "");
        //    if (!login)
        //    {
        //        //errore utente non abilitato
        //        ModelState.AddModelError(string.Empty, "Matricola o Password non valide!");
        //        return View(); // <<<-- visualizza gli errori
        //    }

        //    //redirect to ReturnUrl
        //    string? returnUrl = HttpContext.Session.GetString(SessionReturnUrl); HttpContext.Session.Remove(SessionReturnUrl); //scarico il ReturnUrl
        //    returnUrl ??= Url.Content("~/"); //default
        //    return LocalRedirect(returnUrl);
        //}


        //==========================================================================================================
        //==========================================================================================================
        //
        //TRIGGER prima-dopo costruzione pagina HTML


        /// <summary>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext"/>.</param>
        void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"- {nameof(ControllerErp)}.{nameof(OnActionExecuting)}");
            _logger.Info($"- {nameof(ControllerErp)}.{nameof(OnActionExecuting)}");

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutedContext"/>.</param>
        void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"- {nameof(ControllerErp)}.{nameof(OnActionExecuted)}");
            _logger.Info($"- {nameof(ControllerErp)}.{nameof(OnActionExecuted)}");

            base.OnActionExecuted(context);
        }


    }

    //La classe base ActionFilterAttribute include i metodi seguenti che è possibile eseguire l'override:
    //  ---
    //  OnActionExecuting: questo metodo viene chiamato prima dell'esecuzione di un'azione del controller.
    //  OnActionExecuted: questo metodo viene chiamato dopo l'esecuzione di un'azione del controller.
    //  OnResultExecuting: questo metodo viene chiamato prima dell'esecuzione di un risultato dell'azione del controller.
    //  OnResultExecuted: questo metodo viene chiamato dopo l'esecuzione di un risultato dell'azione del controller.
    //  ---
    //  https://learn.microsoft.com/it-it/aspnet/mvc/overview/older-versions-1/controllers-and-routing/understanding-action-filters-cs
    //  ---

    public class LogActionFilter : ActionFilterAttribute

    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log("OnActionExecuting", filterContext.RouteData);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext.RouteData);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData);
        }


        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
            Debug.WriteLine(message, "Action Filter Log");
        }

    }

}
