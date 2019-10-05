using AutoMapper;
using MCS.DTO;
using MCS.Models;
using MCS.Repositories;
using MCS.Services;
using MCS.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Tests.ViewModel
{
	[TestClass]
	public class MainViewModelTests
	{
		[TestMethod]
		public void Test()
		{
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

			var mapperMock = new Mock<IMapper>(MockBehavior.Strict);

			List<PersonForListDto> dto = new List<PersonForListDto>
			{
				new PersonForListDto
				{
					Id = 1,
					ApartmentNumber = "1",
					BirthDate = DateTime.Now.AddYears(-20),
					FirstName = "John",
					HouseNumber = "2",
					LastName = "Doe",
					PhoneNumber = "123123",
					PostalCode = "123-12",
					StreetName = "Street",
					Age = "1",
					On = 1
				}
			};

			mapperMock.Setup(m => m.Map<IEnumerable<PersonForListDto>>(people)).Returns(dto);

			var personRepositoryMock = new Mock<IPersonRepository>(MockBehavior.Strict);

			personRepositoryMock.Setup(m => m.Get()).Returns(people);

			var messageBoxServiceMock = new Mock<IMessageBoxService>(MockBehavior.Strict);
			var messageStringManagerMock = new Mock<IMessageStringManager>(MockBehavior.Strict);

			var target = new MainViewModel(
				mapperMock.Object,
				personRepositoryMock.Object,
				messageBoxServiceMock.Object,
				messageStringManagerMock.Object);

			target.AddNewPersonRowCommand.Execute(null);

			Assert.AreEqual(2, target.People.Count());
			Assert.AreEqual(2, target.People[1].On);
			Assert.IsTrue(target.People[1].IsNew);
			Assert.IsFalse(target.IsBusy);

			mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(people), Times.Once);
			personRepositoryMock.Verify(m => m.Get(), Times.Once);
		}
	}
}
