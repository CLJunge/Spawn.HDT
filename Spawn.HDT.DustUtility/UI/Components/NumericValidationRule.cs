#region Using
using System;
using System.Globalization;
using System.Windows.Controls;
#endregion

namespace Spawn.HDT.DustUtility.UI.Components
{
    public class NumericValidationRule : ValidationRule
    {
        #region Constants
        private const string ErrorMessage = "Invalid value! Enter a value between 1 and 3600 (seconds).";
        #endregion

        #region Validate
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult retVal = ValidationResult.ValidResult;

            if (!DustUtilityPlugin.NumericRegex.IsMatch(value.ToString()))
            {
                retVal = new ValidationResult(false, ErrorMessage);
            }
            else
            {
                try
                {
                    int nValue = Convert.ToInt32(value.ToString());

                    if (nValue < 1 || nValue > 3600)
                    {
                        retVal = new ValidationResult(false, ErrorMessage);
                    }
                }
                catch
                {
                    retVal = new ValidationResult(false, ErrorMessage);
                }
            }

            return retVal;
        }
        #endregion
    }
}