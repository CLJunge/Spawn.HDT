using System.Globalization;
using System.Windows.Controls;

namespace Spawn.HDT.DustUtility.UI.Components
{
    public class NumericValidationRule : ValidationRule
    {
        #region Validate
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult retVal = ValidationResult.ValidResult;

            if (!DustUtilityPlugin.NumericRegex.IsMatch(value.ToString()))
            {
                retVal = new ValidationResult(false, "Not a valid number!");
            }
            else { }

            return retVal;
        }
        #endregion
    }
}