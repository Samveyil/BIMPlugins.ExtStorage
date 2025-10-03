using System.Globalization;
using System.Windows.Controls;

namespace BIMPlugins.ExtStorage.ValidationRules
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Обязательное поле.")
                : ValidationResult.ValidResult;
        }
    }
}
