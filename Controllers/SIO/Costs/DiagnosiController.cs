using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using ErpToolkit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ErpToolkit.Models.SIO.Costs;

namespace ErpToolkit.Controllers.SIO.Costs
{
    public class DiagnosiController : ControllerErp
    {
        //private static NLog.ILogger _logger;
        public DiagnosiController()
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }


        [HttpGet]
        public JsonResult AutocompleteGetAll()
        {
            try
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                string sql = $"select DG_CODICE + {DogManager.addParam(" - ", ref parameters)} + DG_DESCRIZIONE as label, DG__ICODE as value from DIAGNOSI where DG__DELETED = {DogManager.addParam("N", ref parameters)} ";
                return Json(ErpContext.Instance.DogFactory.GetDog(dogId).ExecuteQuery<Choice>(sql, parameters));
            }
            catch (Exception ex) { return Json(new { error = "Problemi in accesso al DB: AutocompleteGetAll Diagnosi: " + ex.Message }); }
        }
        [HttpGet]
        public JsonResult AutocompleteGetSelect(string term)
        {
            try
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                string sql = $"select DG_CODICE + {DogManager.addParam(" - ", ref parameters)} + DG_DESCRIZIONE as label, DG__ICODE as value from DIAGNOSI where DG__DELETED = {DogManager.addParam("N", ref parameters)} and upper({DogManager.addParam(" ", ref parameters)} + DG_CODICE + {DogManager.addParam(" - ", ref parameters)} + DG_DESCRIZIONE + {DogManager.addParam(" ", ref parameters)}) like {DogManager.addParam("%" + term.ToUpper() + "%", ref parameters)} ";
                return Json(ErpContext.Instance.DogFactory.GetDog(dogId).ExecuteQuery<Choice>(sql, parameters));
            }
            catch (Exception ex)  { return Json(new { error = "Problemi in accesso al DB: AutocompleteGetSelect Diagnosi: " + ex.Message }); }
        }
        [HttpPost]
        public JsonResult AutocompletePreLoad([FromBody] List<string> values)
        {
            try
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                string sql = $"select DG_CODICE + {DogManager.addParam(" - ", ref parameters)} + DG_DESCRIZIONE as label, DG__ICODE as value from DIAGNOSI where DG__DELETED = {DogManager.addParam("N", ref parameters)} and DG__ICODE in (" + string.Join(", ", DogManager.addListParam(values.ToList<object>(), ref parameters)) + ")";
                return Json(ErpContext.Instance.DogFactory.GetDog(dogId).ExecuteQuery<Choice>(sql, parameters));
            }
            catch (Exception ex) { return Json(new { error = "Problemi in accesso al DB: AutocompletePreLoad Diagnosi: " + ex.Message }); }
        }
        [BindProperty]
        public SelDiagnosi Select { get; set; }
        [BindProperty]
        public List<Diagnosi> List { get; set; } = new List<Diagnosi>();
        [BindProperty]
        public Diagnosi Row { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            this.Select = new SelDiagnosi();
            foreach (var key in Request.Query.Keys) DogManager.setPropertyValue(this.Select, key, Request.Query[key]); // carica parametri QueryString
            this.List = new List<Diagnosi>();
            //carico eventuali parametri presenti in TempData
            foreach (var item in TempData.Keys) ViewData[item] = TempData[item];
            return View("~/Views/SIO/Costs/Diagnosi/Index.cshtml", this);  //passo il Controller alla vista, come Model
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(SelDiagnosi selobj)
        {
            if (selobj != null) { this.Select = selobj; }
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO
            if (!TryValidateModel(this.Select))
            {
                ModelState.AddModelError(string.Empty, "Verifica valore dei campi.");
                return View("~/Views/SIO/Costs/Diagnosi/Index.cshtml", this);
            }
            if (!this.Select.TryValidateInt(ModelState)) {
                return View("~/Views/SIO/Costs/Diagnosi/Index.cshtml", this);
            }
            //carica lista
            try { this.List = ErpContext.Instance.DogFactory.GetDog(dogId).List<Diagnosi>(this.Select); }
            catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: List: " + ex.Message); }
            this.StatusMessage = "Lista caricata!";
            return View("~/Views/SIO/Costs/Diagnosi/Index.cshtml", this);
        }

        [HttpPost]
        public IActionResult ReadForEdit([FromBody] ModelParam parms)  
        {
            Diagnosi obj = this.ReadForEditModel<Diagnosi>(parms);
            return PartialView("~/Views/SIO/Costs/Diagnosi/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Save([FromBody] ModelObject dataObj)
        {
            Diagnosi obj = this.SaveModel<Diagnosi>(dataObj);
            if (!TryValidateModel(obj))
            {
                return PartialView("~/Views/SIO/Costs/Diagnosi/_PartialEdit.cshtml", obj);
            }
            this.StatusMessage = "Record aggiornato!";
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/Costs/Diagnosi/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult ReadForDelete([FromBody] ModelParam parms)  
        {
            Diagnosi obj = this.ReadForDeleteModel<Diagnosi>(parms);
            return PartialView("~/Views/SIO/Costs/Diagnosi/_PartialDelete.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Delete([FromBody] ModelObject dataObj)
        {
            Diagnosi obj = this.DeleteModel<Diagnosi>(dataObj);
            if (ModelState.ErrorCount > 0)
            {
                return PartialView("~/Views/SIO/Costs/Diagnosi/_PartialDelete.cshtml", obj);
            }
            this.StatusMessage = "Record cancellato!";
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/Costs/Diagnosi/_PartialDelete.cshtml", obj);
        }
    }
}
