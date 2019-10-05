using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
	public class PersonForListDto : INotifyPropertyChanged, IDataErrorInfo
	{
		private bool isNew;
		private bool isEdited;
		private bool isDeleted;

		public string this[string columnName]
		{
			get
			{
				string err = "Data cannot be null";

				switch (columnName)
				{
					case nameof(this.FirstName):
						if (String.IsNullOrEmpty(this.FirstName))
						{
							this.AddError(nameof(this.FirstName), err);
							return err;
						}
						this.RemoveError(nameof(this.FirstName));
						return null;
					default:
						return null;
						break;
				}
			}
		}

		private Dictionary<string, string> errors { get; set; }

		private void AddError(string paramName, string error)
		{
			if (this.errors == null)
			{
				this.errors = new Dictionary<string, string>();
			}

			if (!this.errors.ContainsKey(paramName))
			{
				this.errors.Add(paramName, error);
			}
		}

		private void RemoveError(string paramName)
		{
			if (this.errors != null && this.errors.Count > 0)
			{
				this.errors.Remove(paramName);
			}
		}


		public bool HasError
		{
			get
			{
				return this.errors != null && this.errors.Count > 0;
			}
		}

		public int Id { get; set; }
		//public string FirstName { get; set; }
		public string LastName { get; set; }
		public string StreetName { get; set; }
		public string HouseNumber { get; set; }
		public string ApartmentNumber { get; set; }
		public string PostalCode { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime? BirthDate { get; set; }
		public string Age { get; set; }

		private string firstName;
		public string FirstName
		{
			get => firstName;
			set
			{
				firstName = value;
				this.RaisePropertyChanged(nameof(this.FirstName));
			}
		}

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

		public string Error => throw new NotImplementedException();

		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
