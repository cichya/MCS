using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Services
{
	public class DateTimeService : IDateTimeService
	{
		public int CalculateAge(DateTime? birth)
		{
			if (birth == null)
			{
				return 0;
			}

			var age = DateTime.Today.Year - birth.Value.Year;

			if (birth.Value.AddYears(age) > DateTime.Today)
			{
				age--;
			}

			return age;
		}
	}
}
