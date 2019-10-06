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
		public void CalculateAge_BirthNull_Return0()
		{
			DateTime? birth = null;

			var target = new DateTimeService();

			int result = target.CalculateAge(birth);

			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void CalculateAge_Equal3Years_Return2()
		{
			DateTime birth = DateTime.Now.AddYears(-3);

			var target = new DateTimeService();

			int result = target.CalculateAge(birth);

			Assert.AreEqual(2, result);
		}

		[TestMethod]
		public void CalculateAge_MoreThan3_YearsReturn3()
		{
			DateTime birth = DateTime.Now.AddYears(-3).AddDays(-1);

			var target = new DateTimeService();

			int result = target.CalculateAge(birth);

			Assert.AreEqual(3, result);
		}
	}
}
