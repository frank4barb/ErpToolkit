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
    public class TestController : ControllerErp
    {

        //private static NLog.ILogger _logger; //private readonly ILogger<HomeController> _logger;

        public TestController()
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();

            //XXXXXXXX
            userModel = new User();
            //XXXXXXXX

        }




        //----------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------

        public IActionResult Privacy()
        {
            return View(this);
        }

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
        //[TempData]
        //public string StatusMessage { get; set; }





        // Tag Helper
        //https://learn.microsoft.com/it-it/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-8.0#the-select-tag-helper


        public string Country { get; set; }

        public List<SelectListItem> Countries { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "MX", Text = "Mexico" },
            new SelectListItem { Value = "CA", Text = "Canada" },
            new SelectListItem { Value = "US", Text = "USA"  },
        };




    }
}
