using AutoMapper;
using GalaSoft.MvvmLight;
using MCS.DTO;
using MCS.Models;
using System;
using System.Collections.ObjectModel;

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

		public ObservableCollection<PersonForListDto> People { get; set; }

		public MainViewModel(IMapper mapper)
		{
			this.mapper = mapper;

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
	}
}