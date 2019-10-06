using MCS.Models;
using MCS.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Tests.Services
{
	[TestClass]
	[DeploymentItem(@"Resources\", @"Resources\")]
	public class XmlServiceTests
	{
		private const string filePath = @"Resources\database.xml";
		private const string filePathTmp = @"Resources\databasetmp.xml";

		[TestMethod]
		public void LoadXml_Success()
		{
			var target = new XmlService();

			var result = target.LoadXml(filePath);

			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Count);

			Assert.AreEqual(1, result[0].Id);
			Assert.AreEqual("1", result[0].ApartmentNumber);
			Assert.IsNotNull(result[0].BirthDate);
			Assert.AreEqual("John", result[0].FirstName);
			Assert.AreEqual("2", result[0].HouseNumber);
			Assert.AreEqual("Doe", result[0].LastName);
			Assert.AreEqual("123123", result[0].PhoneNumber);
			Assert.AreEqual("123-12", result[0].PostalCode);
			Assert.AreEqual("Street", result[0].StreetName);
		}

		[TestMethod]
		public void SaveXml_Success()
		{
			var list = new List<Person>
			{
				new Person
				{
					Id = 1,
					ApartmentNumber = "1",
					BirthDate = DateTime.Now.AddYears(-20),
					FirstName = "John",
					HouseNumber = "2",
					LastName = "Doe",
					PhoneNumber = "123123",
					PostalCode = "123-12",
					StreetName = "Street"
				}
			};

			var target = new XmlService();

			target.SaveXml(filePathTmp, list);

			Assert.IsTrue(File.Exists(filePathTmp));
		}

		[TestMethod]
		public void CheckFileExists_FileNotExists_ReturnFalse()
		{
			var target = new XmlService();

			bool result = target.CheckFileExists("xxx");

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void CheckFileExists_FileExists_ReturnTrue()
		{
			var target = new XmlService();

			bool result = target.CheckFileExists(filePath);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void CreateXmlFile_Success()
		{
			var target = new XmlService();

			var path = Path.Combine(Environment.CurrentDirectory, "Resources", "tmp.xml");

			target.CreateXmlFile(path);

			Assert.IsTrue(File.Exists(path));
		}

		[TestInitialize]
		public void Init()
		{
			Assert.IsTrue(File.Exists(filePath));
		}
	}
}
