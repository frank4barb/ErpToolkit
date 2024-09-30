using Google.Api;
using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using ErpToolkit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using MongoDB.Driver.Core.Configuration;
using System.Data.Entity.Infrastructure;

namespace ErpToolkit.Controllers
{
    public class DatatableController : ControllerErp
    {
        //private static NLog.ILogger _logger;
        public DatatableController()
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }


        [BindProperty]
        public FiltroTable Filtro { get; set; }
        [BindProperty]
        public List<Customers> ListCustomers { get {
                string sql = "SELECT PA__ICODE as CustomerID, PA_COD_SANITARIO as CompanyName, PA_COGNOME as ContactName, PA_NOME as ContactTitle,  PA_NOME as City,  PA_NOME as PostalCode, PA_NOME as Country, PA_NOME as Phone FROM PAZIENTE WHERE PA_COGNOME like 'BA%' ";
                //$$//DataTable dt = ErpContext.Instance.getSQLSERVERHelper("#connectionString_SQLSLocal").execQuery(sql);
                //$$//return SQLSERVERHelper.ConvertDataTable<Customers>(dt, "");
                return ErpContext.Instance.DogFactory.GetDog("SIO", "SqlServer", "#connectionString_SQLSLocal").ExecuteQuery<Customers>(sql, null);
                //$$//
            }
        }
        [BindProperty]
        public Customers RowCustomer { get; set; }


        //https://datatables.net/forums/discussion/76110/paging-datatables-with-mvc
        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpGet]
        public IActionResult Table(string returnUrl = null)
        {
            //carico eventuali parametri presenti in TempData
            foreach (var item in TempData.Keys) ViewData[item] = TempData[item];


            return View(this);  //passo il Controller alla vista, come Model
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Table()
        {
            //leggo valori filtro selezione tabella
            var sanitario = Filtro.Sanitario;
            var testoLibero = Filtro.TestoLibero;
            var dataInizio = Filtro.DataInizio;
            var dataFine = Filtro.DataFine;
            if (String.IsNullOrWhiteSpace(sanitario) && String.IsNullOrWhiteSpace(testoLibero)) ModelState.AddModelError(string.Empty, "Almeno Sanitario o TestoLibero deve essere specificato!");
            if (!String.IsNullOrWhiteSpace(testoLibero) && (String.IsNullOrWhiteSpace(dataInizio) || String.IsNullOrWhiteSpace(dataFine))) ModelState.AddModelError(string.Empty, "Se è presente TestoLibero deve anche essere definito un intervallo di date!");
            if (String.IsNullOrWhiteSpace(sanitario) && !String.IsNullOrWhiteSpace(dataInizio) && !String.IsNullOrWhiteSpace(dataFine) &&
                DateTime.ParseExact(dataFine, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                .Subtract(DateTime.ParseExact(dataInizio, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture))
                .TotalDays > 3) ModelState.AddModelError(string.Empty, "Si può fare la ricerca su massimo 3 giorni!");
            return View(this);
        }



        [HttpPost]
        public ActionResult LoadData()
        {
            try
            {



                //legge valori dalla pagina da cui è partita la submit
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data
                //var customerData = (from tempcustomer in _context.Customers select tempcustomer);

                //CustomerID CompanyName ContactName ContactTitle City PostalCode Country Phone
                string sql = "SELECT PA__ICODE as CustomerID, PA_COD_SANITARIO as CompanyName, PA_COGNOME as ContactName, PA_NOME as ContactTitle,  PA_NOME as City,  PA_NOME as PostalCode, PA_NOME as Country, PA_NOME as Phone FROM PAZIENTE WHERE PA_COGNOME like 'BA%' ";
                //$$//DataTable dt = ErpContext.Instance.getSQLSERVERHelper("#connectionString_SQLSLocal").execQuery(sql);
                DataTable dt = ErpContext.Instance.DogFactory.GetDog("SIO", "SqlServer", "#connectionString_SQLSLocal").ExecuteQuery(sql, null);
                //$$//

                //applica Search + Sorting
                string sortStatement = "";
                string likeStatement = "";
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir))) //Sorting
                {
                    sortStatement = sortColumn + " " + sortColumnDir;
                }
                if (!string.IsNullOrEmpty(searchValue)) //Search (searching for random text and returning an array of rows with at least one cell that has a case-insensitive match)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        likeStatement += "Convert(" + dt.Columns[i].ColumnName + ", 'System.String') Like '%" + searchValue + "%'";
                        if (i != dt.Columns.Count - 1) likeStatement += " OR ";
                    }
                }
                if (likeStatement != "" || sortStatement != "")
                {
                    if (sortStatement != "") dt.DefaultView.Sort = sortStatement;
                    if (likeStatement != "") dt.DefaultView.RowFilter = likeStatement;
                    dt = dt.DefaultView.ToTable();
                }


                //$$//List<Customers> results = SQLSERVERHelper.ConvertDataTable<Customers>(dt, "");
                List<Customers> results = ErpContext.Instance.DogFactory.GetDog("SIO", "SqlServer", "#connectionString_SQLSLocal").DecodeSpecialTable<Customers>(dt, "");
                //$$//

                //List<object[]> results = SQLSERVERHelper.ConvertDataTable<object[]>(dt, "");


                //total number of rows count
                recordsTotal = results.Count();
                //Paging
                var idata = results.Skip(skip).Take(pageSize).ToList();
                //var data = results;
                //Returning Json Data
                JsonResult js = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = idata });
                //JsonResult js = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = new object[] { new Customers { CustomerID = "1" }, new Customers { CustomerID = "2" } } });
                //JsonResult js = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = new object[] { } });
                //JsonResult js = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = new object[] { new object[] { "0","1","2", "3", "4", "5", "6", "7", "8", "9" } } });
                js.ContentType = "application/json";
                js.SerializerSettings = null;
                js.StatusCode = 200;
                if (_logger.IsTraceEnabled) _logger.Trace("LoadData: " + JsonSerializer.Serialize(js));
                return js;
            }
            catch (Exception ex)
            {
                _logger.Error(new ErpConfigurationException(ex.Message));
                throw;
            }
        }




        [HttpPost]
        public IActionResult EditCustomer([FromBody] ModalParams parms)  //public IActionResult EditCustomer([FromBody] ModalParams parms)
        {
            Customers customer = new Customers();
            if (parms != null && !String.IsNullOrWhiteSpace(parms.Id))
            {

                string sql = "SELECT PA__ICODE as CustomerID, PA_COD_SANITARIO as CompanyName, PA_COGNOME as ContactName, PA_NOME as ContactTitle,  PA_NOME as City,  PA_NOME as PostalCode, PA_NOME as Country, PA_NOME as Phone FROM PAZIENTE WHERE PA__ICODE='" + parms.Id + "' ";
                //$$//DataTable dt = ErpContext.Instance.getSQLSERVERHelper("#connectionString_SQLSLocal").execQuery(sql);
                DataTable dt = ErpContext.Instance.DogFactory.GetDog("SIO", "SqlServer", "#connectionString_SQLSLocal").ExecuteQuery(sql, null);
                //$$//
                if (dt.Rows.Count > 0)
                {
                    //$$//customer = SQLSERVERHelper.GetItemDataTable<Customers>(dt.Rows[0], "");
                    customer = ErpContext.Instance.DogFactory.GetDog("SIO", "SqlServer", "#connectionString_SQLSLocal").DecodeSpecialRow<Customers>(dt.Rows[0], "");
                    //$$//
                }
            }
            ModelState.Clear(); //ModelState.ClearValidationState("CompanyName"); //FORZA RICONVALIDA MODELLO >>> https://learn.microsoft.com/it-it/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
            return PartialView("_PartialTableEdit", customer);
        }
        [HttpPost]
        public IActionResult SaveCustomer([FromBody] Customers customer)
        {
            ModelState.Clear(); //ModelState.ClearValidationState("CompanyName"); //FORZA RICONVALIDA MODELLO >>> https://learn.microsoft.com/it-it/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
            if (!TryValidateModel(customer))
            {
                ModelState.AddModelError(string.Empty, "Verifica valore dei campi: "+
                    string.Join(", ",
                        ModelState.Where(ms => ms.Value.Errors.Any())
                                              .Select(kvp => kvp.Key)
                                              .ToArray()
                    )
                );
                return PartialView("_PartialTableEdit", customer);
            }
            // salva e ricarica la pagina
            //StatusMessage = "User data was saved!";
            //return LocalRedirect(Url.Content("~/Datatable/Table"));
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("_PartialTableEdit", customer);
        }
        [HttpPost]
        public IActionResult AlertCustomer([FromBody] ModalParams parms)  //public IActionResult EditCustomer([FromBody] ModalParams parms)
        {
            Customers customer = new Customers();
            if (parms != null && !String.IsNullOrWhiteSpace(parms.Id))
            {

                string sql = "SELECT PA__ICODE as CustomerID, PA_COD_SANITARIO as CompanyName, PA_COGNOME as ContactName, PA_NOME as ContactTitle,  PA_NOME as City,  PA_NOME as PostalCode, PA_NOME as Country, PA_NOME as Phone FROM PAZIENTE WHERE PA__ICODE='" + parms.Id + "' ";
                //$$//DataTable dt = ErpContext.Instance.getSQLSERVERHelper("#connectionString_SQLSLocal").execQuery(sql);
                DataTable dt = ErpContext.Instance.DogFactory.GetDog("SIO", "SqlServer", "#connectionString_SQLSLocal").ExecuteQuery(sql, null);
                //$$//
                if (dt.Rows.Count > 0)
                {
                    //$$//customer = SQLSERVERHelper.GetItemDataTable<Customers>(dt.Rows[0], "");
                    customer = ErpContext.Instance.DogFactory.GetDog("SIO", "SqlServer", "#connectionString_SQLSLocal").DecodeSpecialRow<Customers>(dt.Rows[0], "");
                    //$$//
                }
            }
            ModelState.Clear(); //ModelState.ClearValidationState("CompanyName"); //FORZA RICONVALIDA MODELLO >>> https://learn.microsoft.com/it-it/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
            return PartialView("_PartialTableDelete", customer);
        }
        [HttpPost]
        public IActionResult DeleteCustomer([FromBody] Customers customer)
        {
            ModelState.Clear(); //ModelState.ClearValidationState("CompanyName"); //FORZA RICONVALIDA MODELLO >>> https://learn.microsoft.com/it-it/aspnet/core/mvc/models/validation?view=aspnetcore-8.0
            if (String.IsNullOrWhiteSpace(customer.CustomerID))
            {
                ModelState.AddModelError(string.Empty, "Identificativo nullo");
                return PartialView("_PartialTableDelete", customer);
            }
            // elimina record e ricarica la pagina
            //StatusMessage = "User data was deleted!";
            //return LocalRedirect(Url.Content("~/Datatable/Table"));
            //CLOSE WINDOW
            //return Json(new { success = true });
            //return Json(new { error = "Error! Can't Save Data!" });

            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("_PartialTableDelete", customer);


        }



    }
}
