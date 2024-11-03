
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ErpToolkit.Helpers.Db;


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

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public int MinChars { get; set; } = 2; // Numero di caratteri predefinito
 
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var property = For.Metadata.ContainerType.GetProperty(For.Name);
            var attributeServer = property.GetCustomAttributes(typeof(AutocompleteServerAttribute), false).FirstOrDefault() as AutocompleteServerAttribute;
            var attributeClient = property.GetCustomAttributes(typeof(AutocompleteClientAttribute), false).FirstOrDefault() as AutocompleteClientAttribute;
            var attributeErpDogField = property.GetCustomAttributes(typeof(ErpDogFieldAttribute), false).FirstOrDefault() as ErpDogFieldAttribute;
            var attributeErpDogField_Xref = attributeErpDogField?.Xref ?? "";

            //calcola restrizioni visibilità pagina
            //-------------------------------------
            DogManager.FieldAttr attrField = UtilHelper.fieldAttrTagHelper(For.Name, attributeErpDogField_Xref, ViewContext);


            if (attributeServer != null)
            {
                MinChars = 3;

                var visibleInputName = For.Name + "Label";
                //%%//var preSelectedValues = For.ModelExplorer.Model as List<string>;
                var preSelectedValues = new List<string>();
                if (For.ModelExplorer.Model is string) preSelectedValues = new List<string>() { For.ModelExplorer.Model as string };
                else preSelectedValues = For.ModelExplorer.Model as List<string>;

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
                output.Attributes.SetAttribute("data-readonly", attrField.Readonly);  // Readonly field value
                output.Attributes.SetAttribute("data-visible", attrField.Visible);  // Visible field value
                output.Attributes.SetAttribute("data-selected-items-div-id", divId); // Aggiungi l'ID del div degli elementi selezionati
                output.Attributes.SetAttribute("value", ""); //pulisco valore campo

                // Aggiungi il wrapper per l'input e l'icona
                output.PreElement.AppendHtml($"<div id='{For.Name}AutocompleteWrapper' class='autocomplete-wrapper'>");
                output.PostElement.AppendHtml($"<div class='autocomplete-icon'><i class='bi bi-search' aria-hidden='true'></i></div></div>");
                //--

                output.PostElement.AppendHtml(selectedItemsDiv);
                output.PostElement.AppendHtml($"<div id='{For.Name}AutocompleteResults' class='autocomplete-results' style='display: none;'></div>"); // Aggiungi l'ID del div dei risultati dell'autocomplete
            }
            else if (attributeClient != null)
            {
                MinChars = 1;

                var visibleInputName = For.Name + "Label";
                //%%//var preSelectedValues = For.ModelExplorer.Model as List<string>;
                var preSelectedValues = new List<string>();
                if (For.ModelExplorer.Model is string) preSelectedValues = new List<string>() { For.ModelExplorer.Model as string };
                else preSelectedValues = For.ModelExplorer.Model as List<string>;
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
                output.Attributes.SetAttribute("data-readonly", attrField.Readonly);  // Readonly field value
                output.Attributes.SetAttribute("data-visible", attrField.Visible);  // Visible field value
                output.Attributes.SetAttribute("data-selected-items-div-id", divId); // Aggiungi l'ID del div degli elementi selezionati
                output.Attributes.SetAttribute("value", ""); //pulisco valore campo

                // Aggiungi il wrapper per l'input e l'icona
                output.PreElement.AppendHtml($"<div id='{For.Name}AutocompleteWrapper' class='autocomplete-wrapper'>");
                output.PostElement.AppendHtml($"<div class='autocomplete-icon'><i class='bi bi-search' aria-hidden='true'></i></div></div>");
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

                if (dateRange.StartDate != default && dateRange.EndDate != default && dateRange.StartDate > dateRange.EndDate)
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

    //[HtmlTargetElement("input", Attributes = "asp-for")]
    //public class DateRangeTagHelper : TagHelper
    //{
    //    [HtmlAttributeName("asp-for")]
    //    public ModelExpression For { get; set; }

    //    public override void Process(TagHelperContext context, TagHelperOutput output)
    //    {
    //        if (For.Metadata.ContainerType.GetProperty(For.Name).GetCustomAttributes(typeof(DateRangeAttribute), false).Length > 0)
    //        {
    //            var daterangeAttribute = (DateRangeAttribute)For.Metadata.ContainerType.GetProperty(For.Name).GetCustomAttributes(typeof(DateRangeAttribute), false)[0];
    //            string options = daterangeAttribute.Options;
    //            string displayName = For.Metadata.DisplayName ?? For.Name;
    //            string startDateLabel = $"{displayName}: Inizio";
    //            string endDateLabel = $"{displayName}: Fine";
    //            string format = options == "DateTime" ? "dd/MM/yyyy HH:mm" : "dd/MM/yyyy";  // future use: attualmente non implementato

    //            string startDateId = $"{For.Name}.StartDate";
    //            string endDateId = $"{For.Name}.EndDate";

    //            string content = $@"
    //            <div class='row'>
    //                <div class='col-md-6'>
    //                    <label for='{startDateId}'>{startDateLabel}</label>
    //                    <input class='form-control' type='date' data-val='true' data-val-length='Inserire massimo 10 caratteri' data-val-length-max='10' 
    //                                            id='{startDateId}' name='{startDateId}' value=''>
    //                    <input name='__Invariant' type='hidden' value='{startDateId}'>
    //                </div>
    //                <div class='col-md-6'>
    //                    <label for='{endDateId}'>{endDateLabel}</label>
    //                    <input class='form-control' type='date' data-val='true' data-val-length='Inserire massimo 10 caratteri' data-val-length-max='10' 
    //                                            id='{endDateId}' name='{endDateId}' value=''>
    //                    <input name='__Invariant' type='hidden' value='{endDateId}'>
    //                </div>
    //            </div>";

    //            output.Attributes.SetAttribute("type", "hidden");
    //            output.Attributes.SetAttribute("value", "");
    //            output.PostElement.AppendHtml(content);

    //        }
    //    }
    //}









    [HtmlTargetElement("input", Attributes = "asp-for")]
    public class DateRangeTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var property = For.Metadata.ContainerType.GetProperty(For.Name);
            var attributeErpDogField = property.GetCustomAttributes(typeof(ErpDogFieldAttribute), false).FirstOrDefault() as ErpDogFieldAttribute;
            var attributeErpDogField_Xref = attributeErpDogField?.Xref ?? "";

            if (property.GetCustomAttributes(typeof(DateRangeAttribute), false).Length > 0)
            {
                var daterangeAttribute = (DateRangeAttribute)property.GetCustomAttributes(typeof(DateRangeAttribute), false)[0];
                string options = daterangeAttribute.Options;
                string displayName = For.Metadata.DisplayName ?? For.Name;
                string startDateLabel = $"{displayName}: Inizio";
                string endDateLabel = $"{displayName}: Fine";
                //string format = options == "DateTime" ? "dd-MM-yyyy HH:mm" : "dd-MM-yyyy";  // future use: attualmente non implementato
                string format = options == "DateTime" ? "yyyy-MM-ddTHH:mm" : "yyyy-MM-dd";  // Formato ISO 8601 ( i browser si aspettano automaticamente un formato ISO 8601 (yyyy-MM-dd) per gli input di tipo date)

                string startDateId = $"{For.Name}.StartDate";
                string endDateId = $"{For.Name}.EndDate";


                // Recupera i valori dal modello
                DateTime? startDateValue = For.Model?.GetType().GetProperty("StartDate")?.GetValue(For.Model) as DateTime?;
                DateTime? endDateValue = For.Model?.GetType().GetProperty("EndDate")?.GetValue(For.Model) as DateTime?;
                if (startDateValue != null && (DateTime)startDateValue == default) startDateValue = null;
                if (endDateValue != null && (DateTime)endDateValue == default) endDateValue = null;

                // Formatta i valori per il campo di input
                string startDateFormatted = startDateValue?.ToString(format) ?? "";
                string endDateFormatted = endDateValue?.ToString(format) ?? "";


                //calcola restrizioni visibilità pagina
                //-------------------------------------
                DogManager.FieldAttr attrField = UtilHelper.fieldAttrTagHelper(For.Name, attributeErpDogField_Xref, ViewContext);


                string content = $@"
                <div class='row'>
                    <div class='col-md-6'>
                        <label for='{startDateId}'>{startDateLabel}</label>
                        <input class='form-control' type='date' data-val='true' id='{startDateId}' name='{startDateId}' value='{startDateFormatted}' {(attrField.Readonly == 'Y' ? "readonly" : "")}>
                        <input name='__Invariant' type='hidden' value='{startDateId}'>
                    </div>
                    <div class='col-md-6'>
                        <label for='{endDateId}'>{endDateLabel}</label>
                        <input class='form-control' type='date' data-val='true' id='{endDateId}' name='{endDateId}' value='{endDateFormatted}' {(attrField.Readonly == 'Y' ? "readonly" : "")}>
                        <input name='__Invariant' type='hidden' value='{endDateId}'>
                    </div>
                </div>";



                if (attrField.Visible == 'N')
                {
                    // Se Visible è N, nascondiamo l'intero controllo
                    output.SuppressOutput();
                }
                else
                {
                    output.Attributes.SetAttribute("type", "hidden");
                    output.Attributes.SetAttribute("value", "");
                    output.PostElement.AppendHtml(content);
                }
            }
        }
    }


    //*****************************************************************************************************************************************************
    //
    // SCELTA SINGOLA O MULTIPLA
    //


    //public class YourViewModel
    //{
    //    [MultipleChoices(new[] { "A", "B", "C" }, maxSelections: 3, labelProviderAction: "GetLabels")]
    //    public List<string> EpClasseEpisodioMultiplo { get; set; } = new List<string>();
    //
    //    [MultipleChoices(new[] { "Choice 1", "Choice 2", "Choice 3" }, MaxSelections = 1)]
    //    public string EpClasseEpisodioSingolo { get; set; }
    //}


    //@model YourViewModel
    //<form>
    //    <switch-group asp-for="Model.EpClasseEpisodioMultiplo" readonly= "N" visible= "Y" >
    //    </switch-group>

    //    <button type = "submit" class="btn btn-primary">Submit</button>
    //</form>


    //1.Attributo Personalizzato
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class MultipleChoicesAttribute : Attribute
    {
        public string[] Choices { get; }
        public int MaxSelections { get; set; } = 1;  // disable controll if MaxSelections < 1
        public string LabelContoller { get; set; } = "Home";
        public string LabelAction { get; set; } = "GetLabels";
        public string LabelClassName { get; set; } = "";

        public MultipleChoicesAttribute(string[] choices, int maxSelections = 1, string labelContoller = "Home", string labelAction = "GetLabels", string labelClassName = "")
        {
            Choices = choices;
            MaxSelections = maxSelections; 
            LabelContoller = labelContoller;
            LabelAction = labelAction;
            LabelClassName = labelClassName;
        }
    }

    [HtmlTargetElement("input", Attributes = "asp-for")]
    public class SwitchGroupMultipleChoicesTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SwitchGroupMultipleChoicesTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //----------------------------------

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var property = For.ModelExplorer.Container.ModelType.GetProperty(For.Name);
            var multipleChoicesAttribute = property.GetCustomAttributes(typeof(MultipleChoicesAttribute), false).FirstOrDefault() as MultipleChoicesAttribute;
            var attributeErpDogField = property.GetCustomAttributes(typeof(ErpDogFieldAttribute), false).FirstOrDefault() as ErpDogFieldAttribute;
            var attributeErpDogField_Xref = attributeErpDogField?.Xref ?? "";

            if (multipleChoicesAttribute != null)
            {

                //calcola restrizioni visibilità pagina
                //-------------------------------------
                DogManager.FieldAttr attrField = UtilHelper.fieldAttrTagHelper(For.Name, attributeErpDogField_Xref, ViewContext);
                //-------------------------------------


                if (attrField.Visible == 'N')
                {
                    output.SuppressOutput();

                    // Aggiungi codice JavaScript per rimuovere la label associata
                    string script = $@"
                                <script>
                                    document.addEventListener('DOMContentLoaded', function() {{
                                        var label = document.querySelector('label[for=""{For.Name}""]');
                                        if (label) {{
                                            label.style.display = 'none';
                                        }}
                                    }});
                                </script>";
                    output.PostElement.AppendHtml(script);

                    return;
                }



                var choices = multipleChoicesAttribute.Choices;
                var maxSelections = multipleChoicesAttribute.MaxSelections; // disable controll if maxSelections < 1 
                var isMultiple = maxSelections != 1;
                var name = For.Name;
                var readonlyAttr = attrField.Readonly == 'Y' ? "disabled" : "";
                var labels = choices;

                // Se è specificata l'azione per ottenere le label, esegue la chiamata al controller
                if (!string.IsNullOrEmpty(multipleChoicesAttribute.LabelClassName))
                {
                    var labelResponse = GetLabelsFromController(multipleChoicesAttribute.LabelClassName, multipleChoicesAttribute.LabelAction, multipleChoicesAttribute.LabelClassName);

                    if (!string.IsNullOrEmpty(labelResponse.Item2))
                    {
                        // Gestione dell'errore
                        output.Content.SetHtmlContent($"<div class='alert alert-danger'>Errore: {labelResponse.Item1}</div>");
                        return;
                    }
                    var labelChoices = labelResponse.Item1;
                    labels = choices
                                .Select(choice => labelChoices.FirstOrDefault(item => item.value == choice)?.label)
                                .ToArray() ?? choices;
                }

                // Determina i valori pre-selezionati
                var selectedValues = new HashSet<string>();

                if (isMultiple && For.Model is IEnumerable<string> modelList)
                {
                    selectedValues = new HashSet<string>(modelList);
                }
                else if (For.Model is string modelValue)
                {
                    selectedValues.Add(modelValue);
                }

                // HTML per il gruppo di switch
                var content = new StringBuilder();
                content.AppendLine("<div class='switch-group'>");

                for (int i = 0; i < choices.Length; i++)
                {
                    //if (i > 0 && i % 6 == 0)
                    //{
                    //    content.AppendLine("<div class='w-100'></div>"); // Line break after 6 items
                    //}

                    string id = $"{name}_{i}";
                    string value = choices[i].Trim();
                    string inputType = isMultiple ? "checkbox" : "radio";
                    string checkedAttr = selectedValues.Contains(value) ? "checked" : "";

                    content.AppendLine($@"
                        <div class='form-check form-switch d-inline-block mb-2'>
                            <input class='form-check-input' type='{inputType}' name='{name}' id='{id}' value='{value}' {checkedAttr} {readonlyAttr} onchange='handleMaxSelections(""{name}"", {maxSelections})'>
                            <label class='form-check-label' for='{id}'>{value}</label> &nbsp; &nbsp; 
                        </div>");
                }

                content.AppendLine("</div>");

                // JavaScript per gestire il numero massimo di selezioni
                if (isMultiple && maxSelections > 1)
                {
                    content.AppendLine($@"
                        <script>
                            document.addEventListener('DOMContentLoaded', function() {{
                                handleMaxSelections('{name}', {maxSelections});
                            }});
                        </script>");
                }

                output.SuppressOutput();  // elimino il tag input e sostituisco con tag radio o checkbox
                output.PostElement.AppendHtml(content.ToString());

            }

        }

        private Tuple<List<Choice>?, string?> GetLabelsFromController(string controllerName, string actionName, string labelType)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var url = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{controllerName}/{actionName}?labelType={labelType}";

                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    var result = response.Content.ReadAsStringAsync().Result;

                    // Prova a deserializzare prima come un dizionario (per catturare eventuali errori)
                    var errorData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(result);

                    if (errorData != null && errorData.ContainsKey("error"))
                    {
                        // C'è un errore, restituisci il messaggio di errore
                        return new Tuple<List<Choice>?, string?>(null, $"Errore nella chiamata a {actionName}: {errorData["error"]}");
                    }

                    // Altrimenti, prova a deserializzare come una lista di scelte
                    var responseData = System.Text.Json.JsonSerializer.Deserialize<List<Choice>>(result);

                    if (responseData == null)
                    {
                        throw new Exception("La deserializzazione ha restituito un oggetto nullo.");
                    }

                    //var labels = responseData.Select(choice => choice.label).ToArray();
                    return new Tuple<List<Choice>?, string?>(responseData, null);
                }
            }
            catch (Exception ex)
            {
                return new Tuple<List<Choice>?, string?>(null, $"Errore nella chiamata a {actionName}: {ex.Message}");
            }
        }
    }

    //*****************************************************************************************************************************************************
    //
    // QUILL EDITOR
    //

    // ?????????????????? da verificare ????????????????????????????????????????


    //Step 1: Aggiungere Quill al progetto
    //Puoi includere Quill usando un CDN.Aggiungi questo nel tuo layout _Layout.cshtml:

    //html
    //Copia codice
    //<link href="https://cdn.quilljs.com/1.3.6/quill.snow.css" rel="stylesheet">
    //<script src = "https://cdn.quilljs.com/1.3.6/quill.min.js" ></ script >

    //Step 2: Utilizzare il TagHelper nella Vista
    //Ecco come puoi usare il TagHelper nella tua vista:

    //html
    //Copia codice
    //<input asp-for="Descrizione" />
    //<input asp-for="Commenti" />

    //Step 3: Modello e Salvataggio
    //(Nel modello, puoi definire la proprietà come string)
    //
    //public class YourViewModel
    //{
    //    [QuillEditor(Height = "500px", MaxLength = 5000, AllowImages = true, AllowCopyPaste = false)]
    //    public string Descrizione { get; set; }
    //
    //    [QuillEditor(Height = "300px", MaxLength = 10000, AllowImages = false, AllowCopyPaste = true)]
    //    public string Commenti { get; set; }
    //}
    //Quill salva il contenuto formattato come HTML, quindi puoi memorizzarlo direttamente in una stringa nel database.



    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class QuillEditorAttribute : Attribute
    {
        public string Height { get; set; } = "300px";                           // Altezza predefinita
        public int MaxLength { get; set; } = 10000;                             // Lunghezza massima predefinita
        public bool AllowImages { get; set; } = true;                           // Possibilità di inserire immagini
        public string[] ToolbarOptions { get; set; } = new string[]             // Opzioni di formattazione predefinite
        {
            "bold", "italic", "underline", "strike", "blockquote",
            "code-block", "header", "list", "script", "indent", "direction",
            "size", "color", "background", "font", "align", "link", "image", "video"
        };
        public bool AllowCopyPaste { get; set; } = true;                        // Opzione per abilitare/disabilitare copia-incolla

        public QuillEditorAttribute() { }
    }


    [HtmlTargetElement("input", Attributes = "asp-for")]
    public class QuillEditorTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var propertyInfo = For.Metadata.ContainerType.GetProperty(For.Name);
            var quillEditorAttr = propertyInfo?.GetCustomAttribute<QuillEditorAttribute>();

            if (quillEditorAttr == null)
            {
                return;
            }

            var name = For.Name;
            var uniqueEditorId = name.Replace(".", "_"); // Sostituisci i punti con underscore per creare un ID unico
            var value = For.Model?.ToString() ?? string.Empty;
            var height = quillEditorAttr.Height;
            var maxLength = quillEditorAttr.MaxLength;
            var allowImages = quillEditorAttr.AllowImages ? "true" : "false";
            var toolbarOptions = string.Join(", ", quillEditorAttr.ToolbarOptions.Select(o => $"'{o}'"));
            var allowCopyPaste = quillEditorAttr.AllowCopyPaste ? "true" : "false";

            output.TagName = "div";
            output.Attributes.SetAttribute("id", uniqueEditorId);
            output.Attributes.SetAttribute("style", $"height: {height};");

            output.PostElement.SetHtmlContent($@"
            <script>
                var quill_{uniqueEditorId} = new Quill('#{uniqueEditorId}', {{
                    theme: 'snow',
                    modules: {{
                        toolbar: [{toolbarOptions}],
                        imageDrop: {allowImages},
                    }},
                    readOnly: false
                }});

                quill_{uniqueEditorId}.on('text-change', function(delta, oldDelta, source) {{
                    var text = quill_{uniqueEditorId}.getText();
                    if (text.length > {maxLength}) {{
                        quill_{uniqueEditorId}.deleteText({maxLength}, text.length);
                    }}
                    document.querySelector('input[name=""{name}""]').value = quill_{uniqueEditorId}.root.innerHTML;
                }});

                // Gestione del copia-incolla
                if ({allowCopyPaste} === false) {{
                    quill_{uniqueEditorId}.root.addEventListener('copy', function(e) {{
                        e.preventDefault();
                    }});
                    quill_{uniqueEditorId}.root.addEventListener('paste', function(e) {{
                        e.preventDefault();
                    }});
                    quill_{uniqueEditorId}.root.addEventListener('cut', function(e) {{
                        e.preventDefault();
                    }});
                }}

                quill_{uniqueEditorId}.root.innerHTML = `{value}`;
            </script>
            <input type='hidden' name='{name}' value='{value}' />
        ");
        }
    }






    //*****************************************************************************************************************************************************
    //
    // PROPRIETA' CON DIFFERENTI ATTRIBUTI DataType
    //
    // TagHelper generico che si applica a tutti gli elementi input, indipendentemente dal tipo di DataType.
    // Questo TagHelper può essere configurato per modificare l'output dell'elemento HTML in base ai valori di Visible e Readonly.

    //usage
    //DataType(DataType.Text)] , [DataType(DataType.Date)] , [DataType(DataType.Time)] , [DataType(DataType.EmailAddress)] , [DataType(DataType.PhoneNumber)] , [DataType(DataType.Text)] , [DataType(DataType.Currency)] , [DataType(DataType.Duration)] e [DataType(DataType.MultilineText)]
    //[Required]
    //public string? EpNote  { get; set; }


    [HtmlTargetElement("input", Attributes = "asp-for")]
    public class GenericDataTypeTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var property = For.Metadata.ContainerType.GetProperty(For.Name);
            var attributeErpDogField = property.GetCustomAttributes(typeof(ErpDogFieldAttribute), false).FirstOrDefault() as ErpDogFieldAttribute;
            var attributeErpDogField_Xref = attributeErpDogField?.Xref ?? "";

            if (property != null)
            {

                //calcola restrizioni visibilità pagina
                //-------------------------------------
                DogManager.FieldAttr attrField = UtilHelper.fieldAttrTagHelper(For.Name, attributeErpDogField_Xref, ViewContext);
                //-------------------------------------

                var dataTypeAttribute = property.GetCustomAttributes(typeof(DataTypeAttribute), false).FirstOrDefault() as DataTypeAttribute;

                if (dataTypeAttribute != null)
                {
                    if (attrField.Visible == 'N')
                    {
                        // Nascondi il controllo se Visible è "N"
                        output.SuppressOutput();

                        // Aggiungi codice JavaScript per rimuovere la label associata
                        string script = $@"
                                <script>
                                    document.addEventListener('DOMContentLoaded', function() {{
                                        var label = document.querySelector('label[for=""{For.Name}""]');
                                        if (label) {{
                                            label.style.display = 'none';
                                        }}
                                    }});
                                </script>";
                        output.PostElement.AppendHtml(script);

                    }
                    else
                    {
                        // Imposta il tipo di input HTML in base a DataType
                        string inputType = dataTypeAttribute.DataType switch
                        {
                            DataType.Text => "text",
                            DataType.Date => "date",
                            DataType.Time => "time",
                            DataType.DateTime => "date",
                            DataType.EmailAddress => "email",
                            DataType.PhoneNumber => "tel",
                            DataType.Currency => "text", // Non c'è un tipo specifico per la valuta in HTML5
                            DataType.Duration => "text", // Puoi personalizzare questo a seconda delle esigenze
                            DataType.MultilineText => "textarea", // Per i multiline, utilizzeremo un <textarea>
                            _ => "text" // Default
                        };

                        output.Attributes.SetAttribute("type", inputType);

                        if (attrField.Readonly == 'Y')
                        {
                            // Imposta l'attributo readonly se necessario
                            output.Attributes.SetAttribute("readonly", "readonly");
                        }

                        // Gestione speciale per textarea (multiline text), ecc..
                        if (inputType == "textarea")
                        {
                            output.TagName = "textarea";
                            output.Content.SetContent(For.Model?.ToString() ?? "");
                            output.Attributes.RemoveAll("type"); // Rimuove il tipo poiché non è necessario per <textarea>
                        }
                    }
                }
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
