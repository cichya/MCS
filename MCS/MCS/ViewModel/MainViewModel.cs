using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MCS.DTO;
using MCS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
		private IList<Person> peopleDb;


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

		public RelayCommand SaveChangesCommand
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

		private bool isValid;

		public bool IsValid
		{
			get
			{
				return this.isValid;
			}
			set
			{
				this.isValid = value;
				this.RaisePropertyChanged(nameof(this.IsValid));
			}
		}

		private bool discardChangesButtonIsEnabled;

		public bool DiscardChangesButtonIsEnabled
		{
			get
			{
				return this.discardChangesButtonIsEnabled;
			}
			set
			{
				this.discardChangesButtonIsEnabled = value;
				this.RaisePropertyChanged(nameof(this.DiscardChangesButtonIsEnabled));
			}
		}

		public MainViewModel(IMapper mapper)
		{
			this.mapper = mapper;

			this.InitializeCanExecutes();

			this.GetDataFromDataBase();

			
		}

		private void AddNewPersonRow()
		{
			int id = this.People.Count > 0 ? this.People.Max(x => x.Id) + 1 : 1;

			this.People.Add(new PersonForListDto
			{
				Id = id,
				Age = "0",
				IsNew = true
			});

			this.IsValid = false;
			this.DiscardChangesButtonIsEnabled = true;
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
			if (this.IsValid)
			{
				var filtered = this.People.Where(x => !x.IsDeleted).ToList();

				this.peopleDb = this.mapper.Map<IList<Person>>(filtered);

				IEnumerable<PersonForListDto> peopleForListDto = this.mapper.Map<IEnumerable<PersonForListDto>>(peopleDb);

				this.People.Clear();

				foreach (var item in peopleForListDto)
				{
					this.People.Add(item);
				}

				this.DiscardChangesButtonIsEnabled = false;
				this.IsValid = false;
			}
		}

		private void DiscardChanges()
		{
			// get data from repository

			this.GetDataFromDataBase();

			this.DiscardChangesButtonIsEnabled = false;
			this.IsValid = false;
		}

		private void InitializeCanExecutes()
		{
			this.addNewPersonRowCommandCanExecute = true;
			this.editPersonRowCommandCanExecute = true;
			this.deletePersonRowCommandCanExecute = true;
			this.saveChangesCommandCanExecute = true;
			this.discardChangesCommandCanExecute = true;
		}

		private void People_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (INotifyPropertyChanged item in e.OldItems)
					item.PropertyChanged -= People_Item_PropertyChanged;
			}
			if (e.NewItems != null)
			{
				foreach (INotifyPropertyChanged item in e.NewItems)
					item.PropertyChanged += People_Item_PropertyChanged;
			}
		}

		private void People_Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.IsValid = !this.People.Any(x => x.HasError);

			this.DiscardChangesButtonIsEnabled = true;
		}

		private void GetDataFromDataBase()
		{
			if (this.People != null)
			{
				this.People.CollectionChanged -= People_CollectionChanged;

				this.People.Clear();
			}
			else
			{
				this.People = new ObservableCollection<PersonForListDto>();
			}

			this.People.CollectionChanged += People_CollectionChanged;

			if (this.peopleDb == null)
			{
				this.peopleDb = new List<Person>();

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

				this.peopleDb.Add(person);
			}

			IEnumerable<PersonForListDto> peopleForListDto = this.mapper.Map<IEnumerable<PersonForListDto>>(peopleDb);

			foreach (var dto in peopleForListDto)
			{
				this.People.Add(dto);
			}
		}
	}
}