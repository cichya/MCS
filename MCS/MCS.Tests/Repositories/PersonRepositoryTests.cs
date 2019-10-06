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
		private const string userProfilePath = "xxx";

		private Mock<IPathProvider> pathProviderMock;
		private Mock<IXmlService> xmlServiceMock;
		private string filePath;
		private List<Person> people;

		[TestMethod]
		public void Get_FileNotExists_ReturnNull()
		{
			this.xmlServiceMock.Setup(m => m.CheckFileExists(this.filePath)).Returns(false);
			
			var target = new PersonRepository(this.pathProviderMock.Object, this.xmlServiceMock.Object);

			IList<Person> result = target.Get();

			Assert.IsNull(result);

			this.pathProviderMock.Verify(m => m.GetUserProfilePath(), Times.Once);
			this.xmlServiceMock.Verify(m => m.CheckFileExists(this.filePath), Times.Once);
			this.xmlServiceMock.Verify(m => m.CreateXmlFile(this.filePath), Times.Once);
			this.xmlServiceMock.Verify(m => m.LoadXml(this.filePath), Times.Never);
		}

		[TestMethod]
		public void Get_FileExists_ReturnList()
		{
			this.xmlServiceMock.Setup(m => m.CheckFileExists(filePath)).Returns(true);

			var target = new PersonRepository(this.pathProviderMock.Object, this.xmlServiceMock.Object);

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

			this.pathProviderMock.Verify(m => m.GetUserProfilePath(), Times.Once);
			this.xmlServiceMock.Verify(m => m.CheckFileExists(this.filePath), Times.Once);
			this.xmlServiceMock.Verify(m => m.CreateXmlFile(this.filePath), Times.Never);
			this.xmlServiceMock.Verify(m => m.LoadXml(this.filePath), Times.Once);
		}

		[TestMethod]
		public void Save_FileNotExists_CreateFileExecute()
		{
			this.xmlServiceMock.Setup(m => m.CheckFileExists(this.filePath)).Returns(false);

			var target = new PersonRepository(this.pathProviderMock.Object, this.xmlServiceMock.Object);

			target.Save(people);

			this.pathProviderMock.Verify(m => m.GetUserProfilePath(), Times.Once);
			this.xmlServiceMock.Verify(m => m.CheckFileExists(this.filePath), Times.Once);
			this.xmlServiceMock.Verify(m => m.CreateXmlFile(this.filePath), Times.Once);
			this.xmlServiceMock.Verify(m => m.SaveXml(this.filePath, this.people), Times.Once);
		}

		[TestMethod]
		public void Save_FileExists_CreateFileNotExecute()
		{
			this.xmlServiceMock.Setup(m => m.CheckFileExists(this.filePath)).Returns(true);

			var target = new PersonRepository(this.pathProviderMock.Object, this.xmlServiceMock.Object);

			target.Save(people);

			this.pathProviderMock.Verify(m => m.GetUserProfilePath(), Times.Once);
			this.xmlServiceMock.Verify(m => m.CheckFileExists(this.filePath), Times.Once);
			this.xmlServiceMock.Verify(m => m.CreateXmlFile(this.filePath), Times.Never);
			this.xmlServiceMock.Verify(m => m.SaveXml(this.filePath, this.people), Times.Once);
		}

		[TestInitialize]
		public void Init()
		{
			this.pathProviderMock = new Mock<IPathProvider>(MockBehavior.Strict);
			this.xmlServiceMock = new Mock<IXmlService>(MockBehavior.Strict);

			this.filePath = Path.Combine(userProfilePath, "people_db.xml");

			this.pathProviderMock.Setup(m => m.GetUserProfilePath()).Returns(userProfilePath);

			this.xmlServiceMock.Setup(m => m.CreateXmlFile(this.filePath));

			this.people = new List<Person>
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

			this.xmlServiceMock.Setup(m => m.LoadXml(this.filePath)).Returns(this.people);

			this.xmlServiceMock.Setup(m => m.SaveXml(this.filePath, this.people));
		}
	}
}
