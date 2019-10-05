using MCS.Models;
using MCS.Repositories;
using MCS.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Tests.Repositories
{
	[TestClass]
	public class PersonRepositoryTests
	{
		[TestMethod]
		public void Get_File_Not_Exists_Return_Null_Test()
		{
			string userProfilePath = "xxx";

			string filePath = Path.Combine(userProfilePath, "people_db.xml");

			var pathProviderMock = new Mock<IPathProvider>(MockBehavior.Strict);

			pathProviderMock.Setup(m => m.GetUserProfilePath()).Returns(userProfilePath);

			var xmlServiceMock = new Mock<IXmlService>(MockBehavior.Strict);

			xmlServiceMock.Setup(m => m.CheckFileExists(filePath)).Returns(false);
			
			xmlServiceMock.Setup(m => m.CreateXmlFile(filePath));

			var target = new PersonRepository(pathProviderMock.Object, xmlServiceMock.Object);

			IList<Person> result = target.Get();

			Assert.IsNull(result);

			pathProviderMock.Verify(m => m.GetUserProfilePath(), Times.Once);
			xmlServiceMock.Verify(m => m.CheckFileExists(filePath), Times.Once);
			xmlServiceMock.Verify(m => m.CreateXmlFile(filePath), Times.Once);
			xmlServiceMock.Verify(m => m.LoadXml(filePath), Times.Never);
		}

		[TestMethod]
		public void Get_File_Exists_Return_List_Test()
		{
			string userProfilePath = "xxx";

			string filePath = Path.Combine(userProfilePath, "people_db.xml");

			var pathProviderMock = new Mock<IPathProvider>(MockBehavior.Strict);

			pathProviderMock.Setup(m => m.GetUserProfilePath()).Returns(userProfilePath);

			var xmlServiceMock = new Mock<IXmlService>(MockBehavior.Strict);

			xmlServiceMock.Setup(m => m.CheckFileExists(filePath)).Returns(true);

			List<Person> people = new List<Person>
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

			xmlServiceMock.Setup(m => m.LoadXml(filePath)).Returns(people);

			var target = new PersonRepository(pathProviderMock.Object, xmlServiceMock.Object);

			IList<Person> result = target.Get();

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

			pathProviderMock.Verify(m => m.GetUserProfilePath(), Times.Once);
			xmlServiceMock.Verify(m => m.CheckFileExists(filePath), Times.Once);
			xmlServiceMock.Verify(m => m.CreateXmlFile(filePath), Times.Never);
			xmlServiceMock.Verify(m => m.LoadXml(filePath), Times.Once);
		}

		[TestMethod]
		public void Save_File_Not_Exists_CreateFile_Execute_Test()
		{
			string userProfilePath = "xxx";

			string filePath = Path.Combine(userProfilePath, "people_db.xml");

			var pathProviderMock = new Mock<IPathProvider>(MockBehavior.Strict);

			pathProviderMock.Setup(m => m.GetUserProfilePath()).Returns(userProfilePath);

			var xmlServiceMock = new Mock<IXmlService>(MockBehavior.Strict);

			xmlServiceMock.Setup(m => m.CheckFileExists(filePath)).Returns(false);

			xmlServiceMock.Setup(m => m.CreateXmlFile(filePath));

			List<Person> people = new List<Person>
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

			xmlServiceMock.Setup(m => m.SaveXml(filePath, people));

			var target = new PersonRepository(pathProviderMock.Object, xmlServiceMock.Object);

			target.Save(people);

			pathProviderMock.Verify(m => m.GetUserProfilePath(), Times.Once);
			xmlServiceMock.Verify(m => m.CheckFileExists(filePath), Times.Once);
			xmlServiceMock.Verify(m => m.CreateXmlFile(filePath), Times.Once);
			xmlServiceMock.Verify(m => m.SaveXml(filePath, people), Times.Once);
		}

		[TestMethod]
		public void Save_File_Exists_CreateFile_Not_Execute_Test()
		{
			string userProfilePath = "xxx";

			string filePath = Path.Combine(userProfilePath, "people_db.xml");

			var pathProviderMock = new Mock<IPathProvider>(MockBehavior.Strict);

			pathProviderMock.Setup(m => m.GetUserProfilePath()).Returns(userProfilePath);

			var xmlServiceMock = new Mock<IXmlService>(MockBehavior.Strict);

			xmlServiceMock.Setup(m => m.CheckFileExists(filePath)).Returns(true);

			xmlServiceMock.Setup(m => m.CreateXmlFile(filePath));

			List<Person> people = new List<Person>
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

			xmlServiceMock.Setup(m => m.SaveXml(filePath, people));

			var target = new PersonRepository(pathProviderMock.Object, xmlServiceMock.Object);

			target.Save(people);

			pathProviderMock.Verify(m => m.GetUserProfilePath(), Times.Once);
			xmlServiceMock.Verify(m => m.CheckFileExists(filePath), Times.Once);
			xmlServiceMock.Verify(m => m.CreateXmlFile(filePath), Times.Never);
			xmlServiceMock.Verify(m => m.SaveXml(filePath, people), Times.Once);
		}
	}
}
