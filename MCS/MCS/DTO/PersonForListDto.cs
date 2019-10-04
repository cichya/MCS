using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
	public class PersonForListDto : INotifyPropertyChanged
	{
		private bool isNew;
		private bool isEdited;
		private bool isDeleted;

		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string StreetName { get; set; }
		public string HouseNumber { get; set; }
		public string ApartmentNumber { get; set; }
		public string PostalCode { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime BirthDate { get; set; }
		public string Age { get; set; }

		public bool IsNew
		{
			get => isNew;
			set
			{
				isNew = value;
				this.RaisePropertyChanged(nameof(this.IsNew));
			}
		}
		
		public bool IsEdited
		{
			get => isEdited;
			set
			{
				isEdited = value;
				this.RaisePropertyChanged(nameof(this.IsEdited));
			}
		}

		public bool IsDeleted
		{
			get => isDeleted;
			set
			{
				isDeleted = value;
				this.RaisePropertyChanged(nameof(this.IsDeleted));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
