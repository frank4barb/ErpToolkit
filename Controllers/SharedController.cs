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


namespace ErpToolkit.Controllers
{
    /// <summary>
    /// A <see cref="FeatureController"/> that implements API and RPC methods for the connection manager.
    /// </summary>
    public class SharedController : ControllerErp
    {
        
        public SharedController()
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }


        //##############################################################################################
        //####
        //#### MENU
        //####

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return RedirectToAction("Privacy", "Home");
        }
        public IActionResult Mermaid()
        {
            return RedirectToAction("Mermaid", "Home");
        }
        public IActionResult ERModel()
        {
            return RedirectToAction("ERModel", "Home");
        }
        public IActionResult Table()
        {
            // Uso di XXXX https://dotnettutorials.net/lesson/viewdata-asp-net-core-mvc/#:~:text=To%20use%20ViewData%20in%20the,key%2C%20such%20as%20the%20following.

            // Set data in TempData (i dati rimangono in memoria solo per la prossima request, poi sono cancellati dalla struttura
            TempData["Title"] = "Table from Home";

            //return LocalRedirect(Url.Content("~/Datatable/Table"));
            return RedirectToAction("Table","Datatable");

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
    }
}
