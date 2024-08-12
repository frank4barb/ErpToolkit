
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using ErpToolkit.Helpers.Db;
using System.Data;
using System.Text;
using static Google.Rpc.Help.Types;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Encodings.Web;


// VALIDATE FIELD AT SERVER SIDE


namespace ErpToolkit.Helpers
{

    // Struttura scelta
    public class Choice
    {
        public string label { get; set; }
        public string value { get; set; }
    }

    //*****************************************************************************************************************************************************
    //
    // AUTOCOMPLETE
    //
    // Carica dinamicamente le scelte dell'autocomplete in base a query eseguite lato server o client

    // usage
    //-----
    //public class .......
    //{
    //  [Display(Name = "Id Tipo Attivita", ShortName = "", Description = "Codice della classe generale di attività predefinita", Prompt = "")]
    //  [ErpDogField("AV_ID_TIPO_ATTIVITA", SqlFieldNameOLD = "AV_ID_TIPO_ATTIVITA", SqlFieldProperties = "prop() xref(TIPO_ATTIVITA.TA__ICODE) xdup() multbxref()")]
    //  [DefaultValue("")]
    //  [DataType(DataType.Text)]
    //  [AutocompleteServer("Attivita", "AutocompleteTipoAttivita", "PreLoadTipoAttivita", 3)]
    //  public List<string> AvIdTipoAttivita { get; set; } = new List<string>() { "15002", "009801" };
    //}
    //-------------
    //public class ....Controller : Controller
    //{
    //  [HttpGet]
    //  public JsonResult AutocompleteGetAllTipoAttivita()
    //  {
    //      List<Choice> list = new List<Choice>();
    //      try
    //      {
    //          string sql = "select ??????? as label, ???????? as value from ??????? where ?????????? ";
    //          DataTable dt = ErpContext.Instance.getSQLSERVERHelper("#connectionString_SQLSLocal").execQuery(sql);
    //          list = SQLSERVERHelper.ConvertDataTable<Choice>(dt, "");
    //          return Json(list);
    //      }
    //      catch (Exception ex)
    //      {
    //          return Json(new { error = "Problemi in accesso al DB: GetCities: " + ex.Message });
    //      }
    //  }
    //  [HttpGet]
    //  public JsonResult AutocompleteTipoAttivita(string term)
    //  {
    //      List<Choice> list = new List<Choice>();
    //      try
    //      {
    //          string sql = "select ??????? as label, ???????? as value from ??????? where ?????????? and upper(???????) like '%" + term.ToUpper() + "%'";
    //          DataTable dt = ErpContext.Instance.getSQLSERVERHelper("#connectionString_SQLSLocal").execQuery(sql);
    //          list = SQLSERVERHelper.ConvertDataTable<Choice>(dt, "");
    //          return Json(list);
    //      }
    //      catch (Exception ex)
    //      {
    //          return Json(new { error = "Problemi in accesso al DB: GetCities: " + ex.Message });
    //      }
    //  }
    //  [HttpPost]
    //  public JsonResult PreLoadTipoAttivita([FromBody] List<string> values)
    //  {
    //      List<Choice> list = new List<Choice>();
    //      try
    //      {
    //          string sql = "select ??????? as label, ???????? as value from ??????? where ?????????? and ??__ICODE in ('" + string.Join("', '", values.ToArray()) + "')";
    //          DataTable dt = ErpContext.Instance.getSQLSERVERHelper("#connectionString_SQLSLocal").execQuery(sql);
    //          list = SQLSERVERHelper.ConvertDataTable<Choice>(dt, "");
    //            return Json(list);
    //      }
    //      catch (Exception ex)
    //      {
    //          return Json(new { error = "Problemi in accesso al DB: GetCities: " + ex.Message });
    //      }
    //  }
    //  .......
    //}
    //-------------
    //<html>
    //<head>
    //  <link href = "https://cdn.jsdelivr.net/npm/bootstrap@5.1.0/dist/css/bootstrap.min.css" rel="stylesheet">
    //  <script src = "https://code.jquery.com/jquery-3.6.0.min.js" ></ script >
    //</head>
    //<body>
    //    <div>
    //          <h2>Autocomplete Example</h2>
    //          @Html.LabelFor(model => model.AvIdTipoAttivita, htmlAttributes: new { @class = "control-label" })
    //          <input asp-for="AvIdTipoAttivita" class="form-control" />
    //          @Html.ValidationMessageFor(model => model.AvIdTipoAttivita, "", new { @class = "text-danger" })
    //    </ div >
    //</ body >
    //</ html >

    //Per ottenere un comportamento di autocomplete che sia completamente determinato dalla presenza di un attributo sul modello, senza modificare direttamente la vista,
    //puoi seguire un approccio basato sull'uso di Tag Helpers insieme a riflessione per generare dinamicamente lo script di autocomplete.


    //1. Creare l'Attributo Personalizzato
    //   Definisci un attributo personalizzato che contenga le informazioni necessarie per l'autocomplete.
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class AutocompleteServerAttribute : Attribute
    {
        public string Controller { get; }
        public string Action { get; }
        public string PreloadAction { get; }
        public int MaxSelections { get; } = 0;

        public AutocompleteServerAttribute(string controller, string action, string preloadAction, int maxSelections = 0)
        {
            Controller = controller;
            Action = action;
            PreloadAction = preloadAction;
            MaxSelections = maxSelections;
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class AutocompleteClientAttribute : Attribute
    {
        public string Controller { get; }
        public string Action { get; }
        public int MaxSelections { get; set; } = 0;

        public AutocompleteClientAttribute(string controller, string action, int maxSelections = 0)
        {
            Controller = controller;
            Action = action;
            MaxSelections = maxSelections;
        }
    }


    /************************************
        //2. Creare un Tag Helper
        //  Crea un Tag Helper che controlla la presenza dell'attributo personalizzato e genera il codice JavaScript per l'autocomplete.
        //[HtmlTargetElement("input", Attributes = "asp-for")]
        //[HtmlTargetElement("editor", TagStructure = TagStructure.WithoutEndTag, Attributes = ForAttributeName)]
        [HtmlTargetElement("input", Attributes = "asp-for")]
        public class AutocompleteTagHelper : TagHelper
        {
            [HtmlAttributeName("asp-for")]
            public ModelExpression For { get; set; }

            public override void Process(TagHelperContext context, TagHelperOutput output)
            {
                var property = For.Metadata.ContainerType.GetProperty(For.Name);
                var attributeServer = property.GetCustomAttributes(typeof(AutocompleteServerAttribute), false).FirstOrDefault() as AutocompleteServerAttribute;
                var attributeClient = property.GetCustomAttributes(typeof(AutocompleteClientAttribute), false).FirstOrDefault() as AutocompleteClientAttribute;

                if (attributeServer != null)
                {
                    var visibleInputName = For.Name + "Label";
                    var preSelectedValues = For.ModelExplorer.Model as List<string>;
                    var divId = $"{For.Name}SelectedItems";

                    var script = $@"
                    <script>
                        $(document).ready(function() {{
                            var input = $('#{visibleInputName}');
                            var resultsDiv = $('#autocompleteResults');
                            var selectedItemsDiv = $('#{divId}');
                            var maxSelections = {attributeServer.MaxSelections};
                            resultsDiv.hide();

                            var preSelected = {JsonConvert.SerializeObject(preSelectedValues)};
                            if (preSelected) {{
                                $.ajax({{
                                    url: '/{attributeServer.Controller}/{attributeServer.PreloadAction}',
                                    type: 'POST',
                                    contentType: 'application/json',
                                    data: JSON.stringify(preSelected),
                                    success: function(data) {{
                                        data.forEach(function(item, index) {{
                                            addSelectedItem(item.label, item.value, index);
                                        }});
                                    }}
                                }});
                            }}

                            input.on('input', function() {{
                                var term = $(this).val();
                                if (term.length >= 2) {{
                                    $.get('/{attributeServer.Controller}/{attributeServer.Action}?term=' + term, function(data) {{
                                        resultsDiv.empty();
                                        if (data.error) {{
                                            var validationMessage = $('[data-valmsg-for=""{For.Name}""]');
                                            validationMessage.text(data.error);
                                            validationMessage.show();
                                        }} else if (data.length) {{
                                            resultsDiv.show();
                                            data.forEach(function(item) {{
                                                resultsDiv.append('<div class=""autocomplete-item"" data-value=""' + item.value + '"" data-label=""' + item.label + '"">' + item.label + '</div>');
                                            }});
                                            adjustResultsDivWidth();
                                            $('.autocomplete-item').on('click', function() {{
                                                var label = $(this).data('label');
                                                var value = $(this).data('value');
                                                addSelectedItem(label, value, selectedItemsDiv.children().length);
                                                input.val('');
                                                resultsDiv.hide();
                                            }});
                                            var validationMessage = $('[data-valmsg-for=""{For.Name}""]');
                                            validationMessage.hide();
                                        }} else {{
                                            resultsDiv.hide();
                                        }}
                                    }});
                                }} else {{
                                    resultsDiv.hide();
                                }}
                            }});

                            function adjustResultsDivWidth() {{
                                resultsDiv.css('width', input.outerWidth() + 'px');
                            }}
                            input.on('focus', function() {{
                                adjustResultsDivWidth();
                            }});

                            input.on('blur', function() {{
                                input.val(''); resultsDiv.hide();
                            }});

                            $(document).on('click', '.remove-item', function() {{
                                $(this).parent().remove();
                                toggleInputVisibility();
                            }});

                            function addSelectedItem(label, value, index) {{
                                var itemDiv = $('<div class=""selected-item"" data-value=""' + value + '"">' + label + ' <span class=""remove-item"">&times;</span></div>');
                                var inputField = $('<input type=""hidden"" name=""{For.Name}[' + index + ']"" value=""' + value + '"" />');
                                itemDiv.append(inputField);
                                selectedItemsDiv.append(itemDiv);
                                toggleInputVisibility();
                            }}

                            function toggleInputVisibility() {{
                                var selectedCount = selectedItemsDiv.children().length;
                                if (maxSelections > 0 && selectedCount >= maxSelections) {{
                                    input.hide();
                                }} else {{
                                    input.show();
                                }}
                            }}

                            // Initial toggle in case there are pre-selected items
                            toggleInputVisibility();
                        }});
                    </script>
                    <div id='autocompleteResults' class='autocomplete-results' style='display: none;'></div>";

                    var selectedItemsDiv = $@"<div id='{divId}' class='selected-items'></div>";

                    output.Attributes.SetAttribute("id", visibleInputName);
                    output.Attributes.SetAttribute("name", visibleInputName);
                    output.Attributes.SetAttribute("value", "");

                    output.PostElement.AppendHtml(selectedItemsDiv);
                    output.PostElement.AppendHtml(script);
                }
                else if (attributeClient != null)
                {
                    var visibleInputName = For.Name + "Label";
                    var preSelectedValues = For.ModelExplorer.Model as List<string>;
                    var divId = $"{For.Name}SelectedItems";

                    var script = $@"
                    <script>
                        $(document).ready(function() {{
                            var input = $('#{visibleInputName}');
                            var resultsDiv = $('#autocompleteResults');
                            var selectedItemsDiv = $('#{divId}');
                            var maxSelections = {attributeClient.MaxSelections};
                            resultsDiv.hide();

                            var allChoices = [];

                            // Fetch all possible choices once
                            $.get('/{attributeClient.Controller}/{attributeClient.Action}', function(data) {{
                                allChoices = data;

                                // Process pre-selected values after all choices are loaded
                                var preSelected = {JsonConvert.SerializeObject(preSelectedValues)};
                                if (preSelected) {{
                                    preSelected.forEach(function(value) {{
                                        var item = allChoices.find(c => c.value === value);
                                        if (item) {{
                                            addSelectedItem(item.label, item.value);
                                        }}
                                    }});
                                    toggleInputVisibility();
                                }}
                            }});

                            input.on('input', function() {{
                                var term = $(this).val().toLowerCase();
                                resultsDiv.empty();
                                if (term.length >= 2) {{
                                    var filtered = allChoices.filter(c => c.label.toLowerCase().includes(term));
                                    if (filtered.length) {{
                                        resultsDiv.show();
                                        filtered.forEach(function(item) {{
                                            resultsDiv.append('<div class=""autocomplete-item"" data-value=""' + item.value + '"" data-label=""' + item.label + '"">' + item.label + '</div>');
                                        }});
                                        adjustResultsDivWidth();
                                    }} else {{
                                        resultsDiv.hide();
                                    }}
                                }} else {{
                                    resultsDiv.hide();
                                }}
                            }});

                            function adjustResultsDivWidth() {{
                                resultsDiv.css('width', input.outerWidth() + 'px');
                            }}
                            input.on('focus', function() {{
                                adjustResultsDivWidth();
                            }});

                            $(document).on('click', '.autocomplete-item', function() {{
                                var label = $(this).data('label');
                                var value = $(this).data('value');
                                addSelectedItem(label, value);
                                input.val('');
                                resultsDiv.hide();
                            }});

                            $(document).on('click', '.remove-item', function() {{
                                $(this).parent().remove();
                                toggleInputVisibility();
                            }});


                            var isSelectingItem = false;
                            $(document).on('mousedown', '.autocomplete-item', function() {{
                                isSelectingItem = true;
                            }});
                            $(document).on('mouseup', '.autocomplete-item', function() {{
                                isSelectingItem = false;
                            }});
                            input.on('blur', function() {{
                                input.val('');
                                setTimeout(function() {{
                                    if (!isSelectingItem) {{
                                        resultsDiv.hide();
                                    }}
                                }}, 100);
                            }});


                            function addSelectedItem(label, value) {{
                                var itemDiv = $('<div class=""selected-item"" data-value=""' + value + '"">' + label + ' <span class=""remove-item"">&times;</span></div>');
                                var inputField = $('<input type=""hidden"" name=""{For.Name}"" value=""' + value + '"" />');
                                itemDiv.append(inputField);
                                selectedItemsDiv.append(itemDiv);
                                toggleInputVisibility();
                            }}

                            function toggleInputVisibility() {{
                                var selectedCount = selectedItemsDiv.children().length;
                                if (maxSelections > 0 && selectedCount >= maxSelections) {{
                                    input.hide();
                                }} else {{
                                    input.show();
                                }}
                            }}

                            // Initial toggle in case there are pre-selected items
                            toggleInputVisibility();
                        }});
                    </script>
                    <div id='autocompleteResults' class='autocomplete-results' style='display: none;'></div>";

                    var selectedItemsDiv = $@"<div id='{divId}' class='selected-items'></div>";

                    output.Attributes.SetAttribute("id", visibleInputName);
                    output.Attributes.SetAttribute("name", visibleInputName);
                    output.Attributes.SetAttribute("value", "");

                    output.PostElement.AppendHtml(selectedItemsDiv);
                    output.PostElement.AppendHtml(script);
                }

            }
    ******************************/



    [HtmlTargetElement("input", Attributes = "asp-for")]
    public class MultiSelectAutocompleteTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public int MinChars { get; set; } = 2; // Numero di caratteri predefinito
 
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var property = For.Metadata.ContainerType.GetProperty(For.Name);
            var attributeServer = property.GetCustomAttributes(typeof(AutocompleteServerAttribute), false).FirstOrDefault() as AutocompleteServerAttribute;
            var attributeClient = property.GetCustomAttributes(typeof(AutocompleteClientAttribute), false).FirstOrDefault() as AutocompleteClientAttribute;

            if (attributeServer != null)
            {
                MinChars = 3;

                var visibleInputName = For.Name + "Label";
                var preSelectedValues = For.ModelExplorer.Model as List<string>;
                var divId = $"{For.Name}SelectedItems";

                var preSelectedValuesJson = preSelectedValues != null ? "{ \"preSelected\": " + JsonConvert.SerializeObject(preSelectedValues) + " }" : "{ \"preSelected\": [] }";

                var selectedItemsDiv = $@"<div id='{divId}' class='selected-items'></div>";

                output.Attributes.SetAttribute("class", "autocomplete-input form-control");
                output.Attributes.SetAttribute("autocomplete", "off");
                output.Attributes.SetAttribute("data-max-selections", attributeServer.MaxSelections);
                output.Attributes.SetAttribute("data-controller", attributeServer.Controller);
                output.Attributes.SetAttribute("data-action", attributeServer.Action);
                output.Attributes.SetAttribute("data-preload-action", attributeServer.PreloadAction);
                output.Attributes.SetAttribute("data-pre-selected", preSelectedValuesJson);
                output.Attributes.SetAttribute("data-name", For.Name);
                output.Attributes.SetAttribute("data-min-chars", MinChars);
                output.Attributes.SetAttribute("data-mode", "autocompleteServer");  // Modalità di autocomplete
                output.Attributes.SetAttribute("data-selected-items-div-id", divId); // Aggiungi l'ID del div degli elementi selezionati
                output.Attributes.SetAttribute("value", ""); //pulisco valore campo

                // Aggiungi il wrapper per l'input e l'icona
                output.PreElement.AppendHtml("<div class='autocomplete-wrapper'>");
                output.PostElement.AppendHtml("<span class='autocomplete-icon'><i class='bi bi-search' aria-hidden='true'></i></span></div>");
                //--

                output.PostElement.AppendHtml(selectedItemsDiv);
                output.PostElement.AppendHtml($"<div id='{For.Name}AutocompleteResults' class='autocomplete-results' style='display: none;'></div>"); // Aggiungi l'ID del div dei risultati dell'autocomplete
            }
            else if (attributeClient != null)
            {
                MinChars = 1;

                var visibleInputName = For.Name + "Label";
                var preSelectedValues = For.ModelExplorer.Model as List<string>;
                var divId = $"{For.Name}SelectedItems";

                var preSelectedValuesJson = preSelectedValues != null ? "{ \"preSelected\": "+JsonConvert.SerializeObject(preSelectedValues)+" }" : "{ \"preSelected\": [] }";
                //var encodedPreSelectedValuesJson = HtmlEncoder.Default.Encode(preSelectedValuesJson);


                var selectedItemsDiv = $@"<div id='{divId}' class='selected-items'></div>";

                output.Attributes.SetAttribute("class", "autocomplete-input form-control");
                output.Attributes.SetAttribute("autocomplete", "off");
                output.Attributes.SetAttribute("data-max-selections", attributeClient.MaxSelections);
                output.Attributes.SetAttribute("data-controller", attributeClient.Controller);
                output.Attributes.SetAttribute("data-action", attributeClient.Action);
                output.Attributes.SetAttribute("data-pre-selected", preSelectedValuesJson);
                output.Attributes.SetAttribute("data-name", For.Name);
                output.Attributes.SetAttribute("data-min-chars", MinChars);
                output.Attributes.SetAttribute("data-mode", "autocompleteClient");  // Modalità di autocomplete
                output.Attributes.SetAttribute("data-selected-items-div-id", divId); // Aggiungi l'ID del div degli elementi selezionati
                output.Attributes.SetAttribute("value", ""); //pulisco valore campo

                // Aggiungi il wrapper per l'input e l'icona
                output.PreElement.AppendHtml("<div class='autocomplete-wrapper'>");
                output.PostElement.AppendHtml("<span class='autocomplete-icon'><i class='bi bi-search' aria-hidden='true'></i></span></div>");
                //--

                output.PostElement.AppendHtml(selectedItemsDiv);
                output.PostElement.AppendHtml($"<div id='{For.Name}AutocompleteResults' class='autocomplete-results' style='display: none;'></div>"); // Aggiungi l'ID del div dei risultati dell'autocomplete
            }
        }



    }


    //*****************************************************************************************************************************************************
    //
    // INTERVALLO DI DATE
    //
    // Carica dinamicamente la coppia di date Inizio e Fine da applicare ai filtri di selezione

    //usage
    //[DateRange]
    //[Required]
    //public DateRange Intervallo_di_date { get; set; }


    // Modello
    public class DateRange
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }

    //Attributi

    public class DateRangeAttribute : ValidationAttribute
    {
        public string Options { get; set; } = ""; // contiene le opzioni di verifica separate da spazio. eg: BoundedRange
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateRange = value as DateRange;
            if (dateRange != null)
            {
                // intervallo di date limitato
                if (Options.Contains("BoundedRange") && (dateRange.StartDate == default || dateRange.EndDate == default))  
                {
                    return new ValidationResult("Entrambe le date devono essere compilate.", new[] { validationContext.MemberName });
                }

                if (dateRange.StartDate != default && dateRange.EndDate != default && dateRange.StartDate >= dateRange.EndDate)
                {
                    return new ValidationResult("La data d'inizio deve precedere la data di fine.", new[] { validationContext.MemberName });
                }
            }
            return ValidationResult.Success;
        }
    }

    //TagHelper

    [HtmlTargetElement("label", Attributes = "asp-for")]
    public class EliminaLabel_DateRangeTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var property = AspFor.Metadata.ContainerType.GetProperty(AspFor.Name);
            if (property != null && property.GetCustomAttributes(typeof(DateRangeAttribute), false).Length > 0) output.SuppressOutput();
        }
    }

    [HtmlTargetElement("input", Attributes = "asp-for")]
    public class DateRangeTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AspFor.Metadata.ContainerType.GetProperty(AspFor.Name).GetCustomAttributes(typeof(DateRangeAttribute), false).Length > 0)
            {
                var daterangeAttribute = (DateRangeAttribute)AspFor.Metadata.ContainerType.GetProperty(AspFor.Name).GetCustomAttributes(typeof(DateRangeAttribute), false)[0];
                string options = daterangeAttribute.Options;
                string displayName = AspFor.Metadata.DisplayName ?? AspFor.Name;
                string startDateLabel = $"{displayName}: Inizio";
                string endDateLabel = $"{displayName}: Fine";
                string format = options == "DateTime" ? "dd/MM/yyyy HH:mm" : "dd/MM/yyyy";  // future use: attualmente non implementato

                string startDateId = $"{AspFor.Name}.StartDate";
                string endDateId = $"{AspFor.Name}.EndDate";

                string content = $@"
                <div class='row'>
                    <div class='col-md-6'>
                        <label for='{startDateId}'>{startDateLabel}</label>
                        <input class='form-control' type='date' data-val='true' data-val-length='Inserire massimo 10 caratteri' data-val-length-max='10' 
                                                id='{startDateId}' name='{startDateId}' value=''>
                        <input name='__Invariant' type='hidden' value='{startDateId}'>
                    </div>
                    <div class='col-md-6'>
                        <label for='{endDateId}'>{endDateLabel}</label>
                        <input class='form-control' type='date' data-val='true' data-val-length='Inserire massimo 10 caratteri' data-val-length-max='10' 
                                                id='{endDateId}' name='{endDateId}' value=''>
                        <input name='__Invariant' type='hidden' value='{endDateId}'>
                    </div>
                </div>";

                output.Attributes.SetAttribute("type", "hidden");
                output.Attributes.SetAttribute("value", "");
                output.PostElement.AppendHtml(content);

            }
        }
    }



    //*****************************************************************************************************************************************************
    //*****************************************************************************************************************************************************
    //*****************************************************************************************************************************************************


    //https://blog.techdominator.com/article/using-html-helper-inside-tag-helpers.html

    public class Holder
    {
        public string Name { get; set; }
    }

    public class TemplateRendererTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private IHtmlHelper _htmlHelper;

        public TemplateRendererTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public override async Task ProcessAsync(TagHelperContext context
            , TagHelperOutput output)
        {
            (_htmlHelper as IViewContextAware).Contextualize(ViewContext);

            /*
             * Create some data that are going 
             * to be passed to the view
             */
            _htmlHelper.ViewData["Name"] = "Ali";
            _htmlHelper.ViewBag.AnotherName = "Kamel";
            Holder model = new Holder { Name = "Charles Henry" };

            output.TagName = "div";
            /*
             * model is passed explicitly
             * ViewData and ViewBag are passed implicitly
             */
            output.Content.SetHtmlContent(await _htmlHelper.PartialAsync("Template", model));
        }
    }

    [HtmlTargetElement("template-renderer-new-viewdata")]
    public class TemplateRendererWithNewViewDataTagHelper : TagHelper
    {

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private IHtmlHelper _htmlHelper;

        private IModelMetadataProvider _modelMetadataProvider;
        /*
         * This constructor requests the injection of a IModelMetadataProvider instance
         */
        public TemplateRendererWithNewViewDataTagHelper(IHtmlHelper htmlHelper,
            IModelMetadataProvider metadataProvider)
        {
            _htmlHelper = htmlHelper;
            _modelMetadataProvider = metadataProvider;
        }

        public override async Task ProcessAsync(TagHelperContext context,
            TagHelperOutput output)
        {
            (_htmlHelper as IViewContextAware).Contextualize(ViewContext);
            // Actual instanciation of the new ViewData Dictionary
            ViewDataDictionary viewData = new ViewDataDictionary(_modelMetadataProvider, new ModelStateDictionary());

            Holder model = new Holder { Name = "Joel" };
            viewData["Name"] = "Jeff";

            output.TagName = "div";
            /*
             * model is passed explicitly
             * new ViewData instance needs to be explicitly
             */
            output.Content.SetHtmlContent(
                await _htmlHelper.PartialAsync("TemplateNewViewData", model, viewData));
        }
    }





}
