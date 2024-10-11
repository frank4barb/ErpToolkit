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
                string sql = "select DC__ICODE + ' - ' + DC_NOTE as label, DC__ICODE as value from DOCUMENTO_CLINICO where DC__DELETED='N' and upper(' ' + DC__ICODE + ' - ' + DC_NOTE + ' ') like '%" + term.ToUpper() + "%'";
                return Json(DogHelper.ExecQuery<Choice>(dogId, sql));
            }
            catch (Exception ex)  { return Json(new { error = "Problemi in accesso al DB: AutocompleteGetSelect DocumentoClinico: " + ex.Message }); }
        }
        [HttpPost]
        public JsonResult AutocompletePreLoad([FromBody] List<string> values)
        {
            try
            {
                string sql = "select DC__ICODE + ' - ' + DC_NOTE as label, DC__ICODE as value from DOCUMENTO_CLINICO where DC__DELETED='N' and DC__ICODE in ('" + string.Join("', '", values.ToArray()) + "')";
                return Json(DogHelper.ExecQuery<Choice>(dogId, sql));
            }
            catch (Exception ex) { return Json(new { error = "Problemi in accesso al DB: AutocompletePreLoad DocumentoClinico: " + ex.Message }); }
        }
        [BindProperty]
        public SelDocumentoClinico Select { get; set; }
        [BindProperty]
        public List<DocumentoClinico> List { get; set; }
        [BindProperty]
        public DocumentoClinico Row { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            this.Select = new SelDocumentoClinico();
            foreach (var key in Request.Query.Keys) DogHelper.setPropertyValue(this.Select, key, Request.Query[key]); // carica parametri QueryString
            this.List = new List<DocumentoClinico>();
            //carico eventuali parametri presenti in TempData
            foreach (var item in TempData.Keys) ViewData[item] = TempData[item];
            return View("~/Views/SIO/HealthData/DocumentoClinico/Index.cshtml", this);  //passo il Controller alla vista, come Model
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index()
        {
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
            try { this.List = DogHelper.List<DocumentoClinico>(dogId, this.Select); }
            catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: List: " + ex.Message); }
            this.StatusMessage = "Lista caricata!";
            return View("~/Views/SIO/HealthData/DocumentoClinico/Index.cshtml", this);
        }

        [HttpPost]
        public IActionResult Edit([FromBody] ModalParams parms)  
        {
            DocumentoClinico obj = new DocumentoClinico();
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (parms != null && !String.IsNullOrWhiteSpace(parms.Id))
            {
                try { obj = DogHelper.Row<DocumentoClinico>(dogId, parms.Id); }
                catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: Row: " + ex.Message); }
            }
            return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Save([FromBody] DocumentoClinico obj)
        {
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (!TryValidateModel(obj))
            {
                ModelState.AddModelError(string.Empty, "Verifica valore dei campi: "+
                    string.Join(", ",
                        ModelState.Where(ms => ms.Value.Errors.Any())
                                              .Select(kvp => kvp.Key)
                                              .ToArray()
                    )
                );
                return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialEdit.cshtml", obj);
            }
            if (!obj.TryValidateInt(ModelState))
            {
                return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialEdit.cshtml", obj);
            }
            // salva e ricarica la pagina
            //StatusMessage = "Obj data was saved!";
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Alert([FromBody] ModalParams parms)  
        {
            DocumentoClinico obj = new DocumentoClinico();
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (parms != null && !String.IsNullOrWhiteSpace(parms.Id))
            {
                try { obj = DogHelper.Row<DocumentoClinico>(dogId, parms.Id); }
                catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: Row: " + ex.Message); }
            }
            return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialDelete.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Delete([FromBody] DocumentoClinico obj)
        {
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (String.IsNullOrWhiteSpace(obj.Dc1Icode))
            {
                ModelState.AddModelError(string.Empty, "Identificativo nullo");
                return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialDelete.cshtml", obj);
            }
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/HealthData/DocumentoClinico/_PartialDelete.cshtml", obj);
        }
    }
}
