using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MCS.DTO;
using MCS.Models;
using System;
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
		private readonly IMapper mapper;

		private RelayCommand addNewPersonRowCommand;
		private RelayCommand<DataGridCellEditEndingEventArgs> editPersonRowCommand;

		private bool addNewPersonRowCommandCanExecute;
		private bool editPersonRowCommandCanExecute;

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
			if (!editedPerson.IsEdited)
			{
				var person = this.People.FirstOrDefault(x => x.Id == editedPerson.Id);

				if (person != null)
				{
					person.IsEdited = true;
				}
			}
		}

		private void InitializeCanExecutes()
		{
			this.addNewPersonRowCommandCanExecute = true;
			this.editPersonRowCommandCanExecute = true;
		}
	}
}