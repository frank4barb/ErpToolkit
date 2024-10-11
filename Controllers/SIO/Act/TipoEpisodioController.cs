using ErpToolkit.Helpers;
using ErpToolkit.Helpers.Db;
using ErpToolkit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ErpToolkit.Models.SIO.Act;

namespace ErpToolkit.Controllers.SIO.Act
{
    public class TipoEpisodioController : ControllerErp
    {
        //private static NLog.ILogger _logger;
        public TipoEpisodioController()
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
                string sql = "select TE_CODICE + ' - ' + TE_DESCRIZIONE as label, TE__ICODE as value from TIPO_EPISODIO where TE__DELETED='N' ";
                return Json(DogHelper.ExecQuery<Choice>(dogId, sql));
            }
            catch (Exception ex) { return Json(new { error = "Problemi in accesso al DB: AutocompleteGetAll TipoEpisodio: " + ex.Message }); }
        }
        [HttpGet]
        public JsonResult AutocompleteGetSelect(string term)
        {
            try
            {
                string sql = "select TE_CODICE + ' - ' + TE_DESCRIZIONE as label, TE__ICODE as value from TIPO_EPISODIO where TE__DELETED='N' and upper(' ' + TE_CODICE + ' - ' + TE_DESCRIZIONE + ' ') like '%" + term.ToUpper() + "%'";
                return Json(DogHelper.ExecQuery<Choice>(dogId, sql));
            }
            catch (Exception ex)  { return Json(new { error = "Problemi in accesso al DB: AutocompleteGetSelect TipoEpisodio: " + ex.Message }); }
        }
        [HttpPost]
        public JsonResult AutocompletePreLoad([FromBody] List<string> values)
        {
            try
            {
                string sql = "select TE_CODICE + ' - ' + TE_DESCRIZIONE as label, TE__ICODE as value from TIPO_EPISODIO where TE__DELETED='N' and TE__ICODE in ('" + string.Join("', '", values.ToArray()) + "')";
                return Json(DogHelper.ExecQuery<Choice>(dogId, sql));
            }
            catch (Exception ex) { return Json(new { error = "Problemi in accesso al DB: AutocompletePreLoad TipoEpisodio: " + ex.Message }); }
        }
        [BindProperty]
        public SelTipoEpisodio Select { get; set; }
        [BindProperty]
        public List<TipoEpisodio> List { get; set; }
        [BindProperty]
        public TipoEpisodio Row { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            this.Select = new SelTipoEpisodio();
            foreach (var key in Request.Query.Keys) DogHelper.setPropertyValue(this.Select, key, Request.Query[key]); // carica parametri QueryString
            this.List = new List<TipoEpisodio>();
            //carico eventuali parametri presenti in TempData
            foreach (var item in TempData.Keys) ViewData[item] = TempData[item];
            return View("~/Views/SIO/Act/TipoEpisodio/Index.cshtml", this);  //passo il Controller alla vista, come Model
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
                return View("~/Views/SIO/Act/TipoEpisodio/Index.cshtml", this);
            }
            if (!this.Select.TryValidateInt(ModelState)) {
                return View("~/Views/SIO/Act/TipoEpisodio/Index.cshtml", this);
            }
            //carica lista
            try { this.List = DogHelper.List<TipoEpisodio>(dogId, this.Select); }
            catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: List: " + ex.Message); }
            this.StatusMessage = "Lista caricata!";
            return View("~/Views/SIO/Act/TipoEpisodio/Index.cshtml", this);
        }

        [HttpPost]
        public IActionResult Edit([FromBody] ModalParams parms)  
        {
            TipoEpisodio obj = new TipoEpisodio();
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (parms != null && !String.IsNullOrWhiteSpace(parms.Id))
            {
                try { obj = DogHelper.Row<TipoEpisodio>(dogId, parms.Id); }
                catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: Row: " + ex.Message); }
            }
            return PartialView("~/Views/SIO/Act/TipoEpisodio/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Save([FromBody] TipoEpisodio obj)
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
                return PartialView("~/Views/SIO/Act/TipoEpisodio/_PartialEdit.cshtml", obj);
            }
            if (!obj.TryValidateInt(ModelState))
            {
                return PartialView("~/Views/SIO/Act/TipoEpisodio/_PartialEdit.cshtml", obj);
            }
            // salva e ricarica la pagina
            //StatusMessage = "Obj data was saved!";
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/Act/TipoEpisodio/_PartialEdit.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Alert([FromBody] ModalParams parms)  
        {
            TipoEpisodio obj = new TipoEpisodio();
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (parms != null && !String.IsNullOrWhiteSpace(parms.Id))
            {
                try { obj = DogHelper.Row<TipoEpisodio>(dogId, parms.Id); }
                catch (Exception ex) { ModelState.AddModelError(string.Empty, "Problemi in accesso al DB: Row: " + ex.Message); }
            }
            return PartialView("~/Views/SIO/Act/TipoEpisodio/_PartialDelete.cshtml", obj);
        }
        [HttpPost]
        public IActionResult Delete([FromBody] TipoEpisodio obj)
        {
            ModelState.Clear(); //FORZA RICONVALIDA MODELLO 
            if (String.IsNullOrWhiteSpace(obj.Te1Icode))
            {
                ModelState.AddModelError(string.Empty, "Identificativo nullo");
                return PartialView("~/Views/SIO/Act/TipoEpisodio/_PartialDelete.cshtml", obj);
            }
            //---GESTISCE AZIONI CLICK PULSANTE
            ViewData["IsModalACTION"] = "CLOSE";
            ViewData["IsPageACTION"] = "RELOAD";
            ViewData["IsPageREDIRECT"] = "";
            //---
            return PartialView("~/Views/SIO/Act/TipoEpisodio/_PartialDelete.cshtml", obj);
        }
    }
}
