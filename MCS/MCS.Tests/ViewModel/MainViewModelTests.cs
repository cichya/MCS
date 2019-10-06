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
using System.Windows.Controls;

namespace MCS.Tests.ViewModel
{
	[TestClass]
	public class MainViewModelTests
	{
		[TestMethod]
		public void AddNewPersonRowCommand_Test()
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
			Assert.AreEqual(2, target.People[1].Id);
			Assert.IsTrue(target.People[1].IsNew);
			Assert.IsFalse(target.IsBusy);
			Assert.IsFalse(target.IsValid);
			Assert.IsTrue(target.DiscardChangesButtonIsEnabled);

			mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(people), Times.Once);
			personRepositoryMock.Verify(m => m.Get(), Times.Once);
		}

		[TestMethod]
		public void DeletePersonRowCommand_Delete_Existing_Person_Change_IsDeleted_To_True_Test()
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

			target.DeletePersonRowCommand.Execute(1);

			Assert.AreEqual(1, target.People.Count());
			Assert.AreEqual(1, target.People[0].Id);
			Assert.IsTrue(target.People[0].IsDeleted);
			Assert.IsFalse(target.IsBusy);

			mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(people), Times.Once);
			personRepositoryMock.Verify(m => m.Get(), Times.Once);
		}

		[TestMethod]
		public void DeletePersonRowCommand_Delete_New_Person_Remove_From_People_Test()
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
				},
				new PersonForListDto
				{
					Id = 2,
					On = 2,
					IsNew = true
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

			target.DeletePersonRowCommand.Execute(2);

			Assert.AreEqual(1, target.People.Count());
			Assert.AreEqual(1, target.People[0].Id);
			Assert.IsFalse(target.People[0].IsDeleted);
			Assert.IsFalse(target.IsBusy);

			mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(people), Times.Once);
			personRepositoryMock.Verify(m => m.Get(), Times.Once);
		}

		[TestMethod]
		public void SaveChangesCommand_Is_Valid_False_Function_Not_Execute_Test()
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

			mapperMock.Setup(m => m.Map<IList<Person>>(dto)).Returns(people);

			var personRepositoryMock = new Mock<IPersonRepository>(MockBehavior.Strict);

			personRepositoryMock.Setup(m => m.Get()).Returns(people);

			personRepositoryMock.Setup(m => m.Save(people));

			var messageBoxServiceMock = new Mock<IMessageBoxService>(MockBehavior.Strict);
			var messageStringManagerMock = new Mock<IMessageStringManager>(MockBehavior.Strict);

			var target = new MainViewModel(
				mapperMock.Object,
				personRepositoryMock.Object,
				messageBoxServiceMock.Object,
				messageStringManagerMock.Object);

			target.IsValid = false;

			target.DiscardChangesButtonIsEnabled = true;

			target.SaveChangesCommand.Execute(null);

			Assert.AreEqual(1, target.People.Count());
			Assert.AreEqual(1, target.People[0].Id);
			Assert.IsFalse(target.People[0].IsDeleted);
			Assert.IsFalse(target.IsBusy);
			Assert.IsTrue(target.DiscardChangesButtonIsEnabled);
			Assert.IsFalse(target.IsValid);

			mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(people), Times.Once);
			mapperMock.Verify(m => m.Map<IList<Person>>(dto), Times.Never);
			personRepositoryMock.Verify(m => m.Save(people), Times.Never);
			personRepositoryMock.Verify(m => m.Get(), Times.Once);
		}

		[TestMethod]
		public void SaveChangesCommand_Is_Valid_True_Function_Execute_Test()
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

			mapperMock.Setup(m => m.Map<IList<Person>>(dto)).Returns(people);

			var personRepositoryMock = new Mock<IPersonRepository>(MockBehavior.Strict);

			personRepositoryMock.Setup(m => m.Get()).Returns(people);

			personRepositoryMock.Setup(m => m.Save(people));

			var messageBoxServiceMock = new Mock<IMessageBoxService>(MockBehavior.Strict);
			var messageStringManagerMock = new Mock<IMessageStringManager>(MockBehavior.Strict);

			var target = new MainViewModel(
				mapperMock.Object,
				personRepositoryMock.Object,
				messageBoxServiceMock.Object,
				messageStringManagerMock.Object);

			target.IsValid = true;
			target.DiscardChangesButtonIsEnabled = true;

			target.SaveChangesCommand.Execute(null);

			Assert.AreEqual(1, target.People.Count());
			Assert.AreEqual(1, target.People[0].Id);
			Assert.IsFalse(target.People[0].IsDeleted);
			Assert.IsFalse(target.IsBusy);
			Assert.IsFalse(target.DiscardChangesButtonIsEnabled);
			Assert.IsFalse(target.IsValid);

			mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(people), Times.Exactly(2));
			mapperMock.Verify(m => m.Map<IList<Person>>(dto), Times.Once);
			personRepositoryMock.Verify(m => m.Save(people), Times.Once);
			personRepositoryMock.Verify(m => m.Get(), Times.Once);
		}

		[TestMethod]
		public void SaveChangesCommand_Save_Throw_Exception_Test()
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

			mapperMock.Setup(m => m.Map<IList<Person>>(dto)).Returns(people);

			var personRepositoryMock = new Mock<IPersonRepository>(MockBehavior.Strict);

			personRepositoryMock.Setup(m => m.Get()).Returns(people);

			personRepositoryMock.Setup(m => m.Save(people)).Throws(new Exception("xxx"));

			string errorMsg = "err";

			var messageBoxServiceMock = new Mock<IMessageBoxService>(MockBehavior.Strict);

			messageBoxServiceMock.Setup(m => m.ShowErrorMsgBox(errorMsg));

			var messageStringManagerMock = new Mock<IMessageStringManager>(MockBehavior.Strict);

			messageStringManagerMock.SetupGet(p => p.ErrorMessageBoxContentMessage).Returns(errorMsg);

			var target = new MainViewModel(
				mapperMock.Object,
				personRepositoryMock.Object,
				messageBoxServiceMock.Object,
				messageStringManagerMock.Object);

			target.IsValid = true;
			target.DiscardChangesButtonIsEnabled = true;

			target.SaveChangesCommand.Execute(null);

			Assert.AreEqual(1, target.People.Count());
			Assert.AreEqual(1, target.People[0].Id);
			Assert.IsFalse(target.People[0].IsDeleted);
			Assert.IsFalse(target.IsBusy);
			Assert.IsTrue(target.DiscardChangesButtonIsEnabled);
			Assert.IsTrue(target.IsValid);

			mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(people), Times.Once);
			mapperMock.Verify(m => m.Map<IList<Person>>(dto), Times.Once);
			personRepositoryMock.Verify(m => m.Save(people), Times.Once);
			personRepositoryMock.Verify(m => m.Get(), Times.Once);
			messageBoxServiceMock.Verify(m => m.ShowErrorMsgBox(errorMsg), Times.Once);
			messageStringManagerMock.Verify(p => p.ErrorMessageBoxContentMessage, Times.Once);
		}

		[TestMethod]
		public void DiscardChangesCommand_Test()
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

			target.DiscardChangesCommand.Execute(null);

			Assert.AreEqual(1, target.People.Count());
			Assert.AreEqual(1, target.People[0].Id);
			Assert.IsFalse(target.IsBusy);
			Assert.IsFalse(target.IsValid);
			Assert.IsFalse(target.DiscardChangesButtonIsEnabled);

			mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(people), Times.Exactly(2));
			personRepositoryMock.Verify(m => m.Get(), Times.Exactly(2));
		}
	}
}
