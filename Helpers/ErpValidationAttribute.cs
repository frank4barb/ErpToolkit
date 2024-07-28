using Google.Api;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

// VALIDATE FIELD AT SERVER SIDE


namespace ErpToolkit.Helpers
{
    //https://medium.com/@aykutalpturkay/short-blogs-how-to-write-custom-validation-attributes-in-net-c-7295d8d9a33e
    // usage
    //-----
    //public class CreateOpinionDto
    //{
    //    [ErpMustNotContainsAttribute(new[] { "bad", "word" })]
    //    public required string Opinion { get; set; }
    //}
    // and
    //----
    //[HttpPost]
    //public async Task<IActionResult> Create([FromBody] CreateOpinionDto dto)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest("request not validated.");
    //    }

    //    var op = new Opinion
    //    {
    //        Content = dto.Opinion
    //    };

    //    _context.Opinions.Add(op);
    //    await _context.SaveChangesAsync();

    //    return Ok(op);
    //}
    public class ErpMustNotContainsAttribute : ValidationAttribute
    {
        public ErpMustNotContainsAttribute(string[] words) => Words = words;
        public string[] Words { get; set; }
        public string GetErrorMessage() => "This includes bad words.";

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var targetWord = value?.ToString();

            if (targetWord != null && Words.Any(s => targetWord.Contains(s)))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }

    //==================================================================================================================

    // Erp: Attributo DOG per salvare le proprietà SQL del campo

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ErpDogFieldAttribute : Attribute
    {
        public string SqlFieldName { get; set; }  // eg: AV_CODICE
        public string SqlFieldProperties { get; set; }  // eg: prop() xref() xdup(ATTIVITA.AV__ICODE[AV__ICODE] {AV_CODICE=' '}) multbxref()
        public string SqlFieldNameExt { get; set; }  // AY_CODE

        public ErpDogFieldAttribute(string sqlFieldName)
        {
            SqlFieldName = sqlFieldName;
        }
    }


    //==================================================================================================================


    // Erp: Matricola
    public class ErpMatricola : ValidationAttribute
    {
        public ErpMatricola() { }
        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var targetWord = value?.ToString();

            if (targetWord != null && targetWord.Length > 8) return new ValidationResult("La Matricola deve contenere 8 caratteri");
            if (targetWord != null && targetWord.Length > 2 && targetWord.StartsWith("sb"))
            {
                if (targetWord != null && !targetWord.Substring(2).ToCharArray().Any(s => "0123456789".Contains(s))) return new ValidationResult("La Matricola deve contenere solo cifre dopo sb");
            }
            else
            {
                if (targetWord != null && !targetWord.ToCharArray().Any(s => "0123456789".Contains(s))) return new ValidationResult("La Matricola deve contenere solo cifre");
            }
            if (targetWord != null && targetWord.Length < 8) return new ValidationResult("La Matricola deve contenere 8 caratteri");


            return ValidationResult.Success;
        }
    }
    // Erp: Codice Sanitario
    public class ErpSanitario : ValidationAttribute
    {
        public ErpSanitario() { }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var targetWord = value?.ToString();
            if (targetWord != null && targetWord.Length != 8) return new ValidationResult("Il Codice Sanitario deve contenere 8 cifre");
            if (targetWord != null && !targetWord.ToCharArray().Any(s => "0123456789".Contains(s))) return new ValidationResult("Il Codice Sanitario deve contenere 8 cifre");
            return ValidationResult.Success;
        }
    }
    // Erp: ControlloDataInizio
    public class ErpControlloDataInizio : ValidationAttribute
    {
        public ErpControlloDataInizio(string nomeCampoDataFine) => NomeCampoDataFine = nomeCampoDataFine;
        public string NomeCampoDataFine { get; set; }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success; // Abort if no data
            if (validationContext.ObjectInstance == null) return ValidationResult.Success; // Abort if no data
            FieldInfo? extField = validationContext.ObjectType.GetField(NomeCampoDataFine, BindingFlags.Public | BindingFlags.Instance);
            if (extField == null) return ValidationResult.Success; // Abort if no data
            object? extFieldValue = extField.GetValue(value);
            if (extFieldValue == null) return ValidationResult.Success; // Abort if no data

            DateTime StartDate = Convert.ToDateTime(value);  
            DateTime EndDate = Convert.ToDateTime(extFieldValue);  
            if (StartDate > EndDate)
                return new ValidationResult("La data di inizio non può precedere la data di fine");

            return ValidationResult.Success;
        }
    }
    // Erp: ControlloDataInizio
    public class ErpControlloDataFine : ValidationAttribute
    {
        public ErpControlloDataFine(string nomeCampoDataInizio) => NomeCampoDataInizio = nomeCampoDataInizio;
        public string NomeCampoDataInizio { get; set; }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success; // Abort if no data
            if (validationContext.ObjectInstance == null) return ValidationResult.Success; // Abort if no data
            FieldInfo? extField = validationContext.ObjectType.GetField(NomeCampoDataInizio, BindingFlags.Public | BindingFlags.Instance);
            if (extField == null) return ValidationResult.Success; // Abort if no data
            object? extFieldValue = extField.GetValue(value);
            if (extFieldValue == null) return ValidationResult.Success; // Abort if no data

            DateTime StartDate = Convert.ToDateTime(extFieldValue);  
            DateTime EndDate = Convert.ToDateTime(value);  
            if (StartDate > EndDate)
                return new ValidationResult("La data di inizio non può precedere la data di fine");

            return ValidationResult.Success;
        }
    }


    //==================================================================================================================
    //==================================================================================================================

    // Proviamo a costruire un nuovo tipo di dato Autocomplete con grafica dedicata lato client (eg: DataType.Date)

    /// <summary>
    ///     Allows for clarification of the <see cref="DataType" /> represented by a given
    ///     property (such as <see cref="System.ComponentModel.DataAnnotations.DataType.PhoneNumber" />
    ///     or <see cref="System.ComponentModel.DataAnnotations.DataType.Url" />)
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class ErpDataTypeAttribute : ValidationAttribute
    {
        private static readonly string[] _dataTypeStrings = System.ComponentModel.DataAnnotations.DataType.GetNames<DataType>();

        /// <summary>
        ///     Constructor that accepts a data type enumeration
        /// </summary>
        /// <param name="dataType">The <see cref="DataType" /> enum value indicating the type to apply.</param>
        public ErpDataTypeAttribute(DataType dataType) 
        {
            DataType = dataType;

            // Set some DisplayFormat for a few specific data types
            switch (dataType)
            {
                case DataType.Date:
                    DisplayFormat = new DisplayFormatAttribute();
                    DisplayFormat.DataFormatString = "{0:d}";
                    DisplayFormat.ApplyFormatInEditMode = true;
                    break;
                case DataType.Time:
                    DisplayFormat = new DisplayFormatAttribute();
                    DisplayFormat.DataFormatString = "{0:t}";
                    DisplayFormat.ApplyFormatInEditMode = true;
                    break;
                case DataType.Currency:
                    DisplayFormat = new DisplayFormatAttribute();
                    DisplayFormat.DataFormatString = "{0:C}";

                    // Don't set ApplyFormatInEditMode for currencies because the currency
                    // symbol can't be parsed
                    break;
            }
        }

        /// <summary>
        ///     Constructor that accepts the string name of a custom data type
        /// </summary>
        /// <param name="customDataType">The string name of the custom data type.</param>
        public ErpDataTypeAttribute(string customDataType)
            : this(DataType.Custom)
        {
            CustomDataType = customDataType;
        }

        /// <summary>
        ///     Gets the DataType. If it equals DataType.Custom, <see cref="CustomDataType" /> should also be retrieved.
        /// </summary>
        public DataType DataType { get; }

        /// <summary>
        ///     Gets the string representing a custom data type. Returns a non-null value only if <see cref="DataType" /> is
        ///     DataType.Custom.
        /// </summary>
        public string? CustomDataType { get; }

        /// <summary>
        ///     Gets the default display format that gets used along with this DataType.
        /// </summary>
        public DisplayFormatAttribute? DisplayFormat { get; protected set; }

        /// <summary>
        ///     Return the name of the data type, either using the <see cref="DataType" /> enum or <see cref="CustomDataType" />
        ///     string
        /// </summary>
        /// <returns>The name of the data type enum</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public virtual string GetDataTypeName()
        {
            EnsureValidDataType();

            if (DataType == DataType.Custom)
            {
                // If it's a custom type string, use it as the template name
                return CustomDataType!;
            }
            // If it's an enum, turn it into a string
            // Use the cached array with enum string values instead of ToString() as the latter is too slow
            return _dataTypeStrings[(int)DataType];
        }

        /// <summary>
        ///     Override of <see cref="ValidationAttribute.IsValid(object)" />
        /// </summary>
        /// <remarks>This override always returns <c>true</c>.  Subclasses should override this to provide the correct result.</remarks>
        /// <param name="value">The value to validate</param>
        /// <returns>Unconditionally returns <c>true</c></returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        public override bool IsValid(object? value)
        {
            EnsureValidDataType();

            return true;
        }

        /// <summary>
        ///     Throws an exception if this attribute is not correctly formed
        /// </summary>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        private void EnsureValidDataType()
        {
            if (DataType == DataType.Custom && string.IsNullOrWhiteSpace(CustomDataType))
            {
                throw new InvalidOperationException("ErpDataTypeAttribute: EmptyDataTypeString");
            }
        }
    }

    //==================================================================================================================
    //==================================================================================================================


}
