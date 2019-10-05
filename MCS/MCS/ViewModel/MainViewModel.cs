using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MCS.DTO;
using MCS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace MCS.ViewModel
{
	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
	/// </para>
	/// <para>
	/// You can also use Blend to data bind with the tool's support.
	/// </para>
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase
	{
		public bool Errors { get; set; }


		//public bool Errors { get { return errors; } set { errors = value; no } }


		private readonly IMapper mapper;

		private RelayCommand addNewPersonRowCommand;
		private RelayCommand<DataGridCellEditEndingEventArgs> editPersonRowCommand;
		private RelayCommand<int> deletePersonRowCommand;
		private RelayCommand saveChangesCommand;
		private RelayCommand discardChangesCommand;

		private bool addNewPersonRowCommandCanExecute;
		private bool editPersonRowCommandCanExecute;
		private bool deletePersonRowCommandCanExecute;
		private bool saveChangesCommandCanExecute;
		private bool discardChangesCommandCanExecute;

		public RelayCommand AddNewPersonRowCommand
		{
			get
			{
				return addNewPersonRowCommand ??
					   (addNewPersonRowCommand =
						   new RelayCommand(AddNewPersonRow, this.addNewPersonRowCommandCanExecute));
			}
		}
  
		public RelayCommand<DataGridCellEditEndingEventArgs> EditPersonRowCommand
		{
			get
			{
				return editPersonRowCommand ??
					   (editPersonRowCommand =
						   new RelayCommand<DataGridCellEditEndingEventArgs>(param => this.EditPersonRow(param.EditingElement.DataContext as PersonForListDto), this.editPersonRowCommandCanExecute));
			}
		}

		public RelayCommand<int> DeletePersonRowCommand
		{
			get
			{
				return deletePersonRowCommand ??
					   (deletePersonRowCommand =
						   new RelayCommand<int>(param => this.DeletePersonRow(param), this.deletePersonRowCommandCanExecute));
			}
		}

		public RelayCommand RelayCommandSaveChangesCommand
		{
			get
			{
				return saveChangesCommand ??
					   (saveChangesCommand =
						   new RelayCommand(this.SaveChanges, this.saveChangesCommandCanExecute));
			}
		}

		public RelayCommand DiscardChangesCommand
		{
			get
			{
				return discardChangesCommand ??
					   (discardChangesCommand =
						   new RelayCommand(this.DiscardChanges, this.discardChangesCommandCanExecute));
			}
		}

		public ObservableCollection<PersonForListDto> People { get; set; }

		public MainViewModel(IMapper mapper)
		{
			this.mapper = mapper;

			this.InitializeCanExecutes();

			this.People = new ObservableCollection<PersonForListDto>();

			Person person = new Person
			{
				Id = 1,
				FirstName = "John",
				LastName = "Kovalsky",
				StreetName = "Wiejska",
				HouseNumber = "1",
				ApartmentNumber = "2",
				PostalCode = "12-123",
				PhoneNumber = "123456789",
				BirthDate = DateTime.Now.AddYears(-30)
			};

			PersonForListDto personForListDto = this.mapper.Map<PersonForListDto>(person);

			this.People.Add(personForListDto);
		}

		private void AddNewPersonRow()
		{
			this.People.Add(new PersonForListDto
			{
				Id = this.People.Max(x => x.Id) + 1,
				Age = "0",
				IsNew = true
			});
		}

		private void EditPersonRow(PersonForListDto editedPerson)
		{
			editedPerson.IsEdited = true;
		}

		private void DeletePersonRow(int id)
		{
			PersonForListDto person = this.People.FirstOrDefault(x => x.Id == id);
			
			if (person != null)
			{
				if (person.IsNew)
				{
					this.People.Remove(person);
				}
				else
				{
					person.IsDeleted = !person.IsDeleted;
				}
			}
		}

		private void SaveChanges()
		{
			IEnumerable<Person> peopleToSave = this.mapper.Map<IEnumerable<Person>>(this.People);

			// save to repository
		}

		private void DiscardChanges()
		{
			// get data from repository
		}

		private void InitializeCanExecutes()
		{
			this.addNewPersonRowCommandCanExecute = true;
			this.editPersonRowCommandCanExecute = true;
			this.deletePersonRowCommandCanExecute = true;
			this.saveChangesCommandCanExecute = true;
			this.discardChangesCommandCanExecute = true;
		}
	}
}