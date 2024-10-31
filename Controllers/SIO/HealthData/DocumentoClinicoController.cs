using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using ErpToolkit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ErpToolkit.Models.SIO.HealthData;

namespace ErpToolkit.Controllers.SIO.HealthData
{
    public class DocumentoClinicoController : ControllerErp
    {
        //private static NLog.ILogger _logger;
        public DocumentoClinicoController()
        {
            //SetUpNLog();
            NLog.LogManager.Configuration = UtilHelper.GetNLogConfig(); // Apply config
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }


        [HttpGet]
        public JsonResult AutocompleteGetSelect(string term)
        {
            try
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                string sql = $"select DC__ICODE + {DogManager.addParam(" - ", ref parameters)} + DC_NOTE as label, DC__ICODE as value from DOCUMENTO_CLINICO where DC__DELETED = {DogManager.addParam("N", ref parameters)} and upper({DogManager.addParam(" ", ref parameters)} + DC__ICODE + {DogManager.addParam(" - ", ref parameters)} + DC_NOTE + {DogManager.addParam(" ", ref parameters)}) like {DogManager.addParam("%" + term.ToUpper() + "%", ref parameters)} ";
                return Json(ErpContext.Instance.DogFactory.GetDog(dogId).ExecuteQuery<Choice>(sql, parameters));
            }
            catch (Exception ex)  { return Json(new { error = "Problemi in accesso al DB: AutocompleteGetSelect DocumentoClinico: " + ex.Message }); }
        }
        [HttpPost]
        public JsonResult AutocompletePreLoad([FromBody] List<string> values)
        {
            try
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                string sql = $"select DC__ICODE + {DogManager.addParam(" - ", ref parameters)} + DC_NOTE as label, DC__ICODE as value from DOCUMENTO_CLINICO where DC__DELETED = {DogManager.addParam("N", ref parameters)} and DC__ICODE in (" + string.Join(", ", DogManager.addListParam(values.ToList<object>(), ref parameters)) + ")";
                return Json(ErpContext.Instance.DogFactory.GetDog(dogId).ExecuteQuery<Choice>(sql, parameters));
            }
            catch (Exception ex) { return Json(new { error = "Problemi in accesso al DB: AutocompletePreLoad DocumentoClinico: " + ex.Message }); }
        }
        [BindProperty]
        public SelDocumentoClinico Select { get; set; }
        [BindProperty]
        public List<DocumentoClinico> List { get; set; } = new List<DocumentoClinico>();
        [BindProperty]
        public DocumentoClinico Row { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            this.Select = new SelDocumentoClinico();
            foreach (var key in Request.Query.Keys) DogManager.setPropertyValue(this.Select, key, Request.Query[key]); // carica parametri QueryString
            this.List = new List<DocumentoClinico>();
            //carico eventuali parametri presenti in TempData
            foreach (var item in TempData.Keys) ViewData[item] = TempData[item];
            return View("~/Views/SIO/HealthData/DocumentoClinico/Index.cshtml", this);  //passo il Controller alla vista, come Model
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(SelDocumentoClinico selobj)
        {
            if (selobj != null) { this.Select = selobj; }
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO
            if (!TryValidateModel(this.Select))
            {
                ModelState.AddModelError(string.Empty, "Verifica valore dei campi.");
                return View("~/Views/SIO/HealthData/DocumentoClinico/Index.cshtml", this);
            }
            if (!this.Select.TryValidateInt(ModelState)) {
                return View("~/Views/SIO/HealthData/DocumentoClinico/Index.cshtml", this);
            }
            //carica lista
            try { this.List = ErpContext.Instance.DogFactory.GetDog(dogId).List<DocumentoClinico>(this.Select); }
            catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: List: " + ex.Message); }
            this.StatusMessage = "Lista caricata!";
            return View("~/Views/SIO/HealthData/DocumentoClinico/Index.cshtml", this);
        }

        [HttpPost]
        public IActionResult ReadForEdit([FromBody] ModelParam parms)  
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = "EDIT";  //prefisso da applicare a id e name nei tag, se uso lo stesso @model più volte nella stessa pagina eg: <xx id="EDIT_IdPatient" name="EDIT.IdPatient" ..>
            DocumentoClinico obj = this.ReadForEditModel<DocumentoClinico>(parms);
            return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Save([FromBody] ModelObject dataObj)
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = "EDIT";  //prefisso da applicare a id e name nei tag, se uso lo stesso @model più volte nella stessa pagina eg: <xx id="EDIT_IdPatient" name="EDIT.IdPatient" ..>
            DocumentoClinico obj = this.SaveModel<DocumentoClinico>(dataObj);
            if (!TryValidateModel(obj))
            {
                return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialEdit.cshtml", obj);
            }
            this.StatusMessage = "Record aggiornato!";
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult ReadForDelete([FromBody] ModelParam parms)  
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = "DELETE";  //prefisso da applicare a id e name nei tag, se uso lo stesso @model più volte nella stessa pagina eg: <xx id="EDIT_IdPatient" name="EDIT.IdPatient" ..>
            DocumentoClinico obj = this.ReadForDeleteModel<DocumentoClinico>(parms);
            return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialDelete.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Delete([FromBody] ModelObject dataObj)
        {
            ViewData.TemplateInfo.HtmlFieldPrefix = "DELETE";  //prefisso da applicare a id e name nei tag, se uso lo stesso @model più volte nella stessa pagina eg: <xx id="EDIT_IdPatient" name="EDIT.IdPatient" ..>
            DocumentoClinico obj = this.DeleteModel<DocumentoClinico>(dataObj);
            if (ModelState.ErrorCount > 0)
            {
                return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialDelete.cshtml", obj);
            }
            this.StatusMessage = "Record cancellato!";
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialDelete.cshtml", obj);
        }
    }
}
