using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MCS.DTO;
using MCS.Helpers;
using MCS.Models;
using MCS.Repositories;
using MCS.Services;
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
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		private readonly IMapper mapper;
		private readonly IPersonRepository personRepository;
		private readonly IMessageBoxService messageBoxService;
		private readonly IMessageStringManager messageStringManager;
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
		private bool isBusy;
		private bool isValid;
		private bool discardChangesButtonIsEnabled;

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

		public bool IsBusy
		{
			get
			{
				return this.isBusy;
			}
			set
			{
				this.isBusy = value;
				this.RaisePropertyChanged(nameof(this.IsBusy));
			}
		}

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

		public MainViewModel(IMapper mapper, IPersonRepository personRepository, IMessageBoxService messageBoxService, IMessageStringManager messageStringManager)
		{
			this.mapper = mapper;
			this.personRepository = personRepository;
			this.messageBoxService = messageBoxService;
			this.messageStringManager = messageStringManager;

			this.InitializeCanExecutes();

			try
			{
				this.Refresh();
			}
			catch (CannotCreateFileException ex)
			{
				Logger.Error(ex);
				this.messageBoxService.ShowErrorMsgBox(ex.Message);
			}
		}

		private void AddNewPersonRow()
		{
			try
			{
				this.IsBusy = true;

				int on = this.People.Count > 0 ? this.People.Last().On + 1 : 1;
				int id = this.People.Count > 0 ? this.People.Max(x => x.Id) + 1 : 1;
				
				this.People.Add(new PersonForListDto
				{
					On = on,
					Id = id,
					IsNew = true
				});

				this.IsValid = false;
				this.DiscardChangesButtonIsEnabled = true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				this.messageBoxService.ShowErrorMsgBox(this.messageStringManager.ErrorMessageBoxContentMessage);
			}
			finally
			{
				this.IsBusy = false;
			}
		}

		private void EditPersonRow(PersonForListDto editedPerson)
		{
			try
			{
				this.IsBusy = true;

				editedPerson.IsEdited = true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				this.messageBoxService.ShowErrorMsgBox(this.messageStringManager.ErrorMessageBoxContentMessage);
			}
			finally
			{
				this.IsBusy = false;
			}	
		}

		private void DeletePersonRow(int id)
		{
			try
			{
				this.IsBusy = true;

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
			catch (Exception ex)
			{
				Logger.Error(ex);
				this.messageBoxService.ShowErrorMsgBox(this.messageStringManager.ErrorMessageBoxContentMessage);
			}
			finally
			{
				this.IsBusy = false;
			}
		}

		private void SaveChanges()
		{
			try
			{
				this.IsBusy = true;

				if (this.IsValid)
				{
					var peopleWithoutDeleted = this.People.Where(x => !x.IsDeleted).ToList();

					var filteredPeople = this.mapper.Map<IList<Person>>(peopleWithoutDeleted);

					this.personRepository.Save(filteredPeople);

					IEnumerable<PersonForListDto> peopleForListDto = this.mapper.Map<IEnumerable<PersonForListDto>>(filteredPeople);

					this.People.Clear();

					for (int i = 0; i < peopleForListDto.Count(); i++)
					{
						PersonForListDto dto = peopleForListDto.ElementAt(i);

						dto.On = i + 1;

						this.People.Add(dto);
					}

					this.DiscardChangesButtonIsEnabled = false;
					this.IsValid = false;
				}
			}
			catch (CannotCreateFileException ex)
			{
				Logger.Error(ex);
				this.messageBoxService.ShowErrorMsgBox(ex.Message);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				this.messageBoxService.ShowErrorMsgBox(this.messageStringManager.ErrorMessageBoxContentMessage);
			}
			finally
			{
				this.IsBusy = false;
			}		
		}

		private void DiscardChanges()
		{
			try
			{
				this.IsBusy = true;

				this.Refresh();

				this.DiscardChangesButtonIsEnabled = false;
				this.IsValid = false;
			}
			catch (CannotCreateFileException ex)
			{
				Logger.Error(ex);
				this.messageBoxService.ShowErrorMsgBox(ex.Message);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				this.messageBoxService.ShowErrorMsgBox(this.messageStringManager.ErrorMessageBoxContentMessage);
			}
			finally
			{
				this.IsBusy = false;
			}
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
				{
					item.PropertyChanged -= People_Item_PropertyChanged;
				}
			}
			if (e.NewItems != null)
			{
				foreach (INotifyPropertyChanged item in e.NewItems)
				{
					item.PropertyChanged += People_Item_PropertyChanged;
				}
			}
		}

		private void People_Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.IsValid = !this.People.Any(x => x.HasError);

			this.DiscardChangesButtonIsEnabled = true;
		}

		private void Refresh()
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

			IEnumerable<Person> peopleFromDb = this.personRepository.Get();

			IEnumerable<PersonForListDto> peopleForListDto = this.mapper.Map<IEnumerable<PersonForListDto>>(peopleFromDb);

			for (int i = 0; i < peopleForListDto.Count(); i++)
			{
				PersonForListDto dto = peopleForListDto.ElementAt(i);

				dto.On = i + 1;

				this.People.Add(dto);
			}
		}
	}
}