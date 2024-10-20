using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using ErpToolkit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ErpToolkit.Models.SIO.HealthData;

namespace ErpToolkit.Controllers.SIO.HealthData
{
    public class StatoSaluteController : ControllerErp
    {
        //private static NLog.ILogger _logger;
        public StatoSaluteController()
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
                string sql = $"select SS__ICODE + {DogManager.addParam(" - ", ref parameters)} + SS_NOTE as label, SS__ICODE as value from STATO_SALUTE where SS__DELETED = {DogManager.addParam("N", ref parameters)} and upper({DogManager.addParam(" ", ref parameters)} + SS__ICODE + {DogManager.addParam(" - ", ref parameters)} + SS_NOTE + {DogManager.addParam(" ", ref parameters)}) like {DogManager.addParam("%" + term.ToUpper() + "%", ref parameters)} ";
                return Json(ErpContext.Instance.DogFactory.GetDog(dogId).ExecuteQuery<Choice>(sql, parameters));
            }
            catch (Exception ex)  { return Json(new { error = "Problemi in accesso al DB: AutocompleteGetSelect StatoSalute: " + ex.Message }); }
        }
        [HttpPost]
        public JsonResult AutocompletePreLoad([FromBody] List<string> values)
        {
            try
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                string sql = $"select SS__ICODE + {DogManager.addParam(" - ", ref parameters)} + SS_NOTE as label, SS__ICODE as value from STATO_SALUTE where SS__DELETED = {DogManager.addParam("N", ref parameters)} and SS__ICODE in (" + string.Join(", ", DogManager.addListParam(values.ToList<object>(), ref parameters)) + ")";
                return Json(ErpContext.Instance.DogFactory.GetDog(dogId).ExecuteQuery<Choice>(sql, parameters));
            }
            catch (Exception ex) { return Json(new { error = "Problemi in accesso al DB: AutocompletePreLoad StatoSalute: " + ex.Message }); }
        }
        [BindProperty]
        public SelStatoSalute Select { get; set; }
        [BindProperty]
        public List<StatoSalute> List { get; set; } = new List<StatoSalute>();
        [BindProperty]
        public StatoSalute Row { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            this.Select = new SelStatoSalute();
            foreach (var key in Request.Query.Keys) DogManager.setPropertyValue(this.Select, key, Request.Query[key]); // carica parametri QueryString
            this.List = new List<StatoSalute>();
            //carico eventuali parametri presenti in TempData
            foreach (var item in TempData.Keys) ViewData[item] = TempData[item];
            return View("~/Views/SIO/HealthData/StatoSalute/Index.cshtml", this);  //passo il Controller alla vista, come Model
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(SelStatoSalute selobj)
        {
            if (selobj != null) { this.Select = selobj; }
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO
            if (!TryValidateModel(this.Select))
            {
                ModelState.AddModelError(string.Empty, "Verifica valore dei campi.");
                return View("~/Views/SIO/HealthData/StatoSalute/Index.cshtml", this);
            }
            if (!this.Select.TryValidateInt(ModelState)) {
                return View("~/Views/SIO/HealthData/StatoSalute/Index.cshtml", this);
            }
            //carica lista
            try { this.List = ErpContext.Instance.DogFactory.GetDog(dogId).List<StatoSalute>(this.Select); }
            catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: List: " + ex.Message); }
            this.StatusMessage = "Lista caricata!";
            return View("~/Views/SIO/HealthData/StatoSalute/Index.cshtml", this);
        }

        [HttpPost]
        public IActionResult Edit([FromBody] ModelParam parms)  
        {
            StatoSalute obj = new StatoSalute();
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (parms != null && !String.IsNullOrWhiteSpace(parms.Id))
            {
                try { obj = ErpContext.Instance.DogFactory.GetDog(dogId).Row<StatoSalute>(parms.Id); }
                catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: Row: " + ex.Message); }
            }
            return PartialView("~/Views/SIO/HealthData/StatoSalute/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Save([FromBody] ModelObject dataObj)
        {
            if (dataObj == null || dataObj.data == null)
            {
                ModelState.AddModelError(string.Empty, "Oggetto StatoSalute non valido. null");
                return PartialView("~/Views/SIO/HealthData/StatoSalute/_PartialEdit.cshtml", null);
            }
            StatoSalute obj = System.Text.Json.JsonSerializer.Deserialize<StatoSalute>((System.Text.Json.JsonElement)dataObj.data);
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (!TryValidateModel(obj))
            {
                ModelState.AddModelError(string.Empty, "Verifica valore dei campi: "+
                    string.Join(", ",
                        ModelState.Where(ms => ms.Value.Errors.Any()).Select(kvp => kvp.Key).ToArray()
                    )
                );
                return PartialView("~/Views/SIO/HealthData/StatoSalute/_PartialEdit.cshtml", obj);
            }
            if (!obj.TryValidateInt(ModelState))
            {
                return PartialView("~/Views/SIO/HealthData/StatoSalute/_PartialEdit.cshtml", obj);
            }
            // salva e ricarica la pagina
            try { DogManager.DogResult objResult = ErpContext.Instance.DogFactory.GetDog(dogId).Mnt<StatoSalute>(obj); }
            catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: Mnt: " + ex.Message); }
            this.StatusMessage = "Record aggiornato!";
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/HealthData/StatoSalute/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Alert([FromBody] ModelParam parms)  
        {
            StatoSalute obj = new StatoSalute();
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (parms != null && !String.IsNullOrWhiteSpace(parms.Id))
            {
                try { obj = ErpContext.Instance.DogFactory.GetDog(dogId).Row<StatoSalute>(parms.Id); }
                catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: Row: " + ex.Message); }
            }
            return PartialView("~/Views/SIO/HealthData/StatoSalute/_PartialDelete.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Delete([FromBody] StatoSalute obj)
        {
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (String.IsNullOrWhiteSpace(obj.Ss1Icode))
            {
                ModelState.AddModelError(string.Empty, "Identificativo nullo");
                return PartialView("~/Views/SIO/HealthData/StatoSalute/_PartialDelete.cshtml", obj);
            }
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/HealthData/StatoSalute/_PartialDelete.cshtml", obj);
        }
    }
}
