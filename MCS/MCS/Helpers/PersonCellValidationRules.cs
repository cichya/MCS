using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MCS.Helpers
{
	public class PersonCellValidationRules : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (string.IsNullOrEmpty((string)value))
			{
				return new ValidationResult(false, "Data cannot be empty");
			}

			return new ValidationResult(true, null);
		}
	}
}
