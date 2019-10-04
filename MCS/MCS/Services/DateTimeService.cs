using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Services
{
	public class DateTimeService : IDateTimeService
	{
		public int CalculateAge(DateTime birth)
		{
			var age = DateTime.Today.Year - birth.Year;

			if (birth.AddYears(age) > DateTime.Today)
			{
				age--;
			}

			return age;
		}
	}
}
