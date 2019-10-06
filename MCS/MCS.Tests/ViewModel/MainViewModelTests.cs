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
		private List<Person> people;
		private List<PersonForListDto> dto;
		private Mock<IMapper> mapperMock;
		private Mock<IPersonRepository> personRepositoryMock;
		private Mock<IMessageBoxService> messageBoxServiceMock;
		private Mock<IMessageStringManager> messageStringManagerMock;
		private string errorMsg;

		[TestMethod]
		public void AddNewPersonRowCommand_Success()
		{
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
		public void DeletePersonRowCommand_DeleteExistingPerson_ChangeIsDeletedToTrue()
		{
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
		public void DeletePersonRowCommand_DeleteNewPerson_RemoveFromPeople()
		{
			this.dto.Add(new PersonForListDto
			{
				Id = 2,
				On = 2,
				IsNew = true
			});

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
		public void SaveChangesCommand_IsValidFalse_FunctionNotExecute()
		{
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
		public void SaveChangesCommand_IsValidTrue_FunctionExecute()
		{
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

			this.mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(this.people), Times.Exactly(2));
			this.mapperMock.Verify(m => m.Map<IList<Person>>(this.dto), Times.Once);
			this.personRepositoryMock.Verify(m => m.Save(this.people), Times.Once);
			this.personRepositoryMock.Verify(m => m.Get(), Times.Once);
		}

		[TestMethod]
		public void SaveChangesCommand_SaveThrowException()
		{
			this.personRepositoryMock.Setup(m => m.Save(people)).Throws(new Exception("xxx"));

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

			this.mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(this.people), Times.Once);
			this.mapperMock.Verify(m => m.Map<IList<Person>>(this.dto), Times.Once);
			this.personRepositoryMock.Verify(m => m.Save(this.people), Times.Once);
			this.personRepositoryMock.Verify(m => m.Get(), Times.Once);
			this.messageBoxServiceMock.Verify(m => m.ShowErrorMsgBox(this.errorMsg), Times.Once);
			this.messageStringManagerMock.Verify(p => p.ErrorMessageBoxContentMessage, Times.Once);
		}

		[TestMethod]
		public void DiscardChangesCommand_Success()
		{
			var target = new MainViewModel(
				this.mapperMock.Object,
				this.personRepositoryMock.Object,
				this.messageBoxServiceMock.Object,
				this.messageStringManagerMock.Object);

			target.DiscardChangesCommand.Execute(null);

			Assert.AreEqual(1, target.People.Count());
			Assert.AreEqual(1, target.People[0].Id);
			Assert.IsFalse(target.IsBusy);
			Assert.IsFalse(target.IsValid);
			Assert.IsFalse(target.DiscardChangesButtonIsEnabled);

			mapperMock.Verify(m => m.Map<IEnumerable<PersonForListDto>>(people), Times.Exactly(2));
			personRepositoryMock.Verify(m => m.Get(), Times.Exactly(2));
		}

		[TestInitialize]
		public void Init()
		{
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

			this.dto = new List<PersonForListDto>
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

			this.mapperMock = new Mock<IMapper>(MockBehavior.Strict);
			this.personRepositoryMock = new Mock<IPersonRepository>(MockBehavior.Strict);
			this.messageBoxServiceMock = new Mock<IMessageBoxService>(MockBehavior.Strict);
			this.messageStringManagerMock = new Mock<IMessageStringManager>(MockBehavior.Strict);

			this.mapperMock.Setup(m => m.Map<IList<Person>>(dto)).Returns(people);
			this.mapperMock.Setup(m => m.Map<IEnumerable<PersonForListDto>>(people)).Returns(dto);

			this.personRepositoryMock.Setup(m => m.Get()).Returns(people);
			this.personRepositoryMock.Setup(m => m.Save(this.people));

			this.errorMsg = "err";

			messageBoxServiceMock.Setup(m => m.ShowErrorMsgBox(errorMsg));

			messageStringManagerMock.SetupGet(p => p.ErrorMessageBoxContentMessage).Returns(errorMsg);
		}
	}
}
