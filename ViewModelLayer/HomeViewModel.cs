using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ViewModelLayer
{

    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            citymodel Cities = new citymodel();
        }
        public int Userid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool IsJava { get; set; }
        public bool IsCsharp { get; set; }
        public bool IsPaython { get; set; }
        public string ImagePath { get; set; }
        public HttpPostedFileBase UserImageFile { get; set; }

        //[Required]
        //[DataType(DataType.Date)]
        //public DateTime? StartDate { get; set; }
        //[Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        //[DateGreaterThanAttribute(ErrorMessage = "Back date entry not allowed")]
        [DisplayName("Estimated end date")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //[DateGreaterThan("StartDate", "Estimated end date must be greater than the start date of the project")]
        //[Required]
        //public DateTime? EndDate { get; set; }
        ///[Required(ErrorMessage = "City req")]
        public int? Cityid { get; set; }
        //multiselect dropdown

        //[MinLength(1)]
        //[StringArrayRequired("Cityids", "Altleast Select a city")]
        [Required(ErrorMessage = "Country is required")]
        public string[] Cityids { get; set; }
        public string selectedIds { get; set; }
        public List<citymodel> Cities { get; set; }
        ////public MultiSelectList Cities { get; set; }
        public List<CountriesDto> Countries { get; set; }
        public List<CountriesDto> States { get; set; }
        //[Required(ErrorMessage = "Country is required")]
        public int? SelectedCountry { get; set; }
        public int? SelectedState { get; set; }
        public string Action { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class CountriesDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public int country_id { get; set; }
    }
    public class citymodel
    {
        public int cityid { get; set; }
        public string cityname { get; set; }
    }


    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    //public class DateGreaterThanAttribute : ValidationAttribute, IClientValidatable
    //{
    //    string otherPropertyName;

    //    public DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
    //        : base(errorMessage)
    //    {
    //        this.otherPropertyName = otherPropertyName;
    //    }

    //    /// <summary>
    //    /// Format the error message filling in the name of the property to validate and the reference one.
    //    /// </summary>
    //    /// <param name="name">The name of the property to validate</param>
    //    /// <returns>The formatted error message</returns>
    //    public override string FormatErrorMessage(string name)
    //    {
    //        // In our case this will return: "Estimated end date must be greater than Start date"
    //        return string.Format(ErrorMessageString, name, otherPropertyName);
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        ValidationResult validationResult = ValidationResult.Success;
    //        try
    //        {
    //            // Using reflection we can get a reference to the other date property, in this example the project start date
    //            var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.otherPropertyName);
    //            var containerType = validationContext.ObjectInstance.GetType();
    //            var field = containerType.GetProperty("StartDate");
    //            //Let's check that otherProperty is of type DateTime as we expect it to be
    //            DateTime referenceProperty = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
    //            if (otherPropertyInfo.PropertyType.Equals(new DateTime().GetType()))
    //            {
    //                DateTime toValidate = (DateTime)value;
    //                //DateTime referenceProperty = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
    //                // if the end date is lower than the start date, than the validationResult will be set to false and return
    //                // a properly formatted error message
    //                if (toValidate.CompareTo(referenceProperty) < 1)
    //                {
    //                    //string message = FormatErrorMessage(validationContext.DisplayName);
    //                    //validationResult = new ValidationResult(message);
    //                    validationResult = new ValidationResult(ErrorMessageString);
    //                }
    //            }
    //            else
    //            {
    //                validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // Do stuff, i.e. log the exception
    //            // Let it go through the upper levels, something bad happened
    //            throw ex;
    //        }

    //        return validationResult;
    //    }


    //    #region IClientValidatable Members

    //    /// <summary>
    //    ///  
    //    /// </summary>
    //    /// <param name="metadata"></param>
    //    /// <param name="context"></param>
    //    /// <returns></returns>
    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    {
    //        //string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
    //        string errorMessage = ErrorMessageString;


    //        // The value we set here are needed by the jQuery adapter
    //        ModelClientValidationRule dateGreaterThanRule = new ModelClientValidationRule();
    //        dateGreaterThanRule.ErrorMessage = errorMessage;
    //        dateGreaterThanRule.ValidationType = "dategreaterthan"; // This is the name the jQuery validator will use
    //        //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
    //        dateGreaterThanRule.ValidationParameters.Add("otherpropertyname", otherPropertyName);
    //        //dateGreaterThanRule.ValidationParameters.Add("cityids", "Cityids");

    //        yield return dateGreaterThanRule;
    //    }

    //    #endregion

    //}

    //public class LengthOnOtherPropertyValueAttribute : ValidationAttribute, IClientValidatable
    //{
    //    private readonly string propertyNameToCheck;
    //    private readonly string propertyValueToCheck;
    //    private readonly int maxLengthOnMatch;
    //    private readonly int maxLength;

    //    public LengthOnOtherPropertyValueAttribute(string propertyNameToCheck, string propertyValueToCheck, int maxLengthOnMatch, int maxLength)
    //    {
    //        this.propertyValueToCheck = propertyValueToCheck;
    //        this.maxLengthOnMatch = maxLengthOnMatch;
    //        this.maxLength = maxLength;
    //        this.propertyNameToCheck = propertyNameToCheck;

    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var propertyName = validationContext.ObjectType.GetProperty(propertyNameToCheck);
    //        if (propertyName == null)
    //            return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "Unknown property {0}", new[] { propertyNameToCheck }));

    //        var propertyValue = propertyName.GetValue(validationContext.ObjectInstance, null) as string;

    //        if (propertyValueToCheck.Equals(propertyValue, StringComparison.InvariantCultureIgnoreCase) && value != null && ((string)value).Length > maxLengthOnMatch)
    //            return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName, maxLengthOnMatch));

    //        if (value != null && ((string)value).Length > maxLength)
    //            return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName, maxLength));

    //        return ValidationResult.Success;
    //    }

    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    {
    //        var rule = new ModelClientValidationRule
    //        {
    //            ErrorMessage = string.Format(ErrorMessageString, metadata.GetDisplayName(), maxLengthOnMatch),
    //            ValidationType = "lengthonotherpropertyvalue"
    //        };
    //        rule.ValidationParameters.Add("nametocheck", propertyNameToCheck);
    //        rule.ValidationParameters.Add("valuetocheck", propertyValueToCheck);
    //        rule.ValidationParameters.Add("maxlengthonmatch", maxLengthOnMatch);
    //        rule.ValidationParameters.Add("maxlength", maxLength);
    //        yield return rule;
    //    }
    //}

    //public class StringArrayRequiredAttribute : ValidationAttribute, IClientValidatable
    //{
    //    string otherPropertyName;

    //    public StringArrayRequiredAttribute(string otherPropertyName, string errorMessage)
    //        : base(errorMessage)
    //    {
    //        this.otherPropertyName = otherPropertyName;
    //    }


    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        string[] array = value as string[];

    //        if (array == null || array.Any(item => string.IsNullOrEmpty(item)))
    //        {
    //            return new ValidationResult(this.ErrorMessage);
    //        }
    //        else
    //        {
    //            return ValidationResult.Success;
    //        }
    //    }

    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    {
    //        string errorMessage = ErrorMessageString;
    //        // The value we set here are needed by the jQuery adapter
    //        ModelClientValidationRule dateGreaterThanRule = new ModelClientValidationRule();
    //        dateGreaterThanRule.ErrorMessage = errorMessage;
    //        dateGreaterThanRule.ValidationType = "stringlist"; // This is the name the jQuery validator will use
    //        //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
    //        dateGreaterThanRule.ValidationParameters.Add("otherpropertyname", otherPropertyName);
    //        //dateGreaterThanRule.ValidationParameters.Add("cityids", "Cityids");

    //        yield return dateGreaterThanRule;
    //    }
    //}
}
