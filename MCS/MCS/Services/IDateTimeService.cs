﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Services
{
	public interface IDateTimeService
	{
		int CalculateAge(DateTime? birth);
	}
}
