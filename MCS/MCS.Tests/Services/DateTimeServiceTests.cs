using MCS.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Tests.Services
{
	[TestClass]
	public class DateTimeServiceTests
	{
		[TestMethod]
		public void CalculateAge_Equal_3_Years_Return_2_Test()
		{
			DateTime birth = DateTime.Now.AddYears(-3);

			var target = new DateTimeService();

			int result = target.CalculateAge(birth);

			Assert.AreEqual(2, result);
		}

		[TestMethod]
		public void CalculateAge_More_Than_3_Years_Return_3_Test()
		{
			DateTime birth = DateTime.Now.AddYears(-3).AddDays(-1);

			var target = new DateTimeService();

			int result = target.CalculateAge(birth);

			Assert.AreEqual(3, result);
		}
	}
}
