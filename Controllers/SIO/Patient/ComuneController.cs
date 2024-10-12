using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using ErpToolkit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ErpToolkit.Models.SIO.Patient;

namespace ErpToolkit.Controllers.SIO.Patient
{
    public class ComuneController : ControllerErp
    {
        //private static NLog.ILogger _logger;
        public ComuneController()
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
                string sql = $"select CM_CODICE + {DogManager.addParam(" - ", ref parameters)} + CM_NOME as label, CM__ICODE as value from COMUNE where CM__DELETED = {DogManager.addParam("N", ref parameters)} ";
                return Json(ErpContext.Instance.DogFactory.GetDog(dogId).ExecuteQuery<Choice>(sql, parameters));
            }
            catch (Exception ex) { return Json(new { error = "Problemi in accesso al DB: AutocompleteGetAll Comune: " + ex.Message }); }
        }
        [HttpGet]
        public JsonResult AutocompleteGetSelect(string term)
        {
            try
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                string sql = $"select CM_CODICE + {DogManager.addParam(" - ", ref parameters)} + CM_NOME as label, CM__ICODE as value from COMUNE where CM__DELETED = {DogManager.addParam("N", ref parameters)} and upper({DogManager.addParam(" ", ref parameters)} + CM_CODICE + {DogManager.addParam(" - ", ref parameters)} + CM_NOME + {DogManager.addParam(" ", ref parameters)}) like {DogManager.addParam("%" + term.ToUpper() + "%", ref parameters)} ";
                return Json(ErpContext.Instance.DogFactory.GetDog(dogId).ExecuteQuery<Choice>(sql, parameters));
            }
            catch (Exception ex)  { return Json(new { error = "Problemi in accesso al DB: AutocompleteGetSelect Comune: " + ex.Message }); }
        }
        [HttpPost]
        public JsonResult AutocompletePreLoad([FromBody] List<string> values)
        {
            try
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                string sql = $"select CM_CODICE + {DogManager.addParam(" - ", ref parameters)} + CM_NOME as label, CM__ICODE as value from COMUNE where CM__DELETED = {DogManager.addParam("N", ref parameters)} and CM__ICODE in (" + string.Join(", ", DogManager.addListParam(values.ToList<object>(), ref parameters)) + ")";
                return Json(ErpContext.Instance.DogFactory.GetDog(dogId).ExecuteQuery<Choice>(sql, parameters));
            }
            catch (Exception ex) { return Json(new { error = "Problemi in accesso al DB: AutocompletePreLoad Comune: " + ex.Message }); }
        }
        [BindProperty]
        public SelComune Select { get; set; }
        [BindProperty]
        public List<Comune> List { get; set; }
        [BindProperty]
        public Comune Row { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            this.Select = new SelComune();
            foreach (var key in Request.Query.Keys) DogManager.setPropertyValue(this.Select, key, Request.Query[key]); // carica parametri QueryString
            this.List = new List<Comune>();
            //carico eventuali parametri presenti in TempData
            foreach (var item in TempData.Keys) ViewData[item] = TempData[item];
            return View("~/Views/SIO/Patient/Comune/Index.cshtml", this);  //passo il Controller alla vista, come Model
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
                return View("~/Views/SIO/Patient/Comune/Index.cshtml", this);
            }
            if (!this.Select.TryValidateInt(ModelState)) {
                return View("~/Views/SIO/Patient/Comune/Index.cshtml", this);
            }
            //carica lista
            try { this.List = ErpContext.Instance.DogFactory.GetDog(dogId).List<Comune>(this.Select); }
            catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: List: " + ex.Message); }
            this.StatusMessage = "Lista caricata!";
            return View("~/Views/SIO/Patient/Comune/Index.cshtml", this);
        }

        [HttpPost]
        public IActionResult Edit([FromBody] ModalParams parms)  
        {
            Comune obj = new Comune();
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (parms != null && !String.IsNullOrWhiteSpace(parms.Id))
            {
                try { obj = ErpContext.Instance.DogFactory.GetDog(dogId).Row<Comune>(parms.Id); }
                catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: Row: " + ex.Message); }
            }
            return PartialView("~/Views/SIO/Patient/Comune/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Save([FromBody] Comune obj)
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
                return PartialView("~/Views/SIO/Patient/Comune/_PartialEdit.cshtml", obj);
            }
            if (!obj.TryValidateInt(ModelState))
            {
                return PartialView("~/Views/SIO/Patient/Comune/_PartialEdit.cshtml", obj);
            }
            // salva e ricarica la pagina
            //StatusMessage = "Obj data was saved!";
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/Patient/Comune/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Alert([FromBody] ModalParams parms)  
        {
            Comune obj = new Comune();
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (parms != null && !String.IsNullOrWhiteSpace(parms.Id))
            {
                try { obj = ErpContext.Instance.DogFactory.GetDog(dogId).Row<Comune>(parms.Id); }
                catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: Row: " + ex.Message); }
            }
            return PartialView("~/Views/SIO/Patient/Comune/_PartialDelete.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Delete([FromBody] Comune obj)
        {
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (String.IsNullOrWhiteSpace(obj.Cm1Icode))
            {
                ModelState.AddModelError(string.Empty, "Identificativo nullo");
                return PartialView("~/Views/SIO/Patient/Comune/_PartialDelete.cshtml", obj);
            }
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/Patient/Comune/_PartialDelete.cshtml", obj);
        }
    }
}
