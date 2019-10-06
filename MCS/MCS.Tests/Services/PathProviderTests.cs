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
	public class PathProviderTests
	{
		[TestMethod]
		public void PathProvider_Success()
		{
			var target = new PathProvider(null);

			string result = target.GetUserProfilePath();

			Assert.IsNotNull(result);
		}
	}
}
