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
		private Dictionary<string, string> errors;

		public string this[string columnName]
		{
			get
			{
				

				switch (columnName)
				{
					case nameof(this.FirstName):
						return this.ValidColumn(nameof(this.FirstName), this.FirstName);
					case nameof(this.LastName):
						return this.ValidColumn(nameof(this.LastName), this.LastName);
					case nameof(this.StreetName):
						return this.ValidColumn(nameof(this.StreetName), this.StreetName);
					case nameof(this.HouseNumber):
						return this.ValidColumn(nameof(this.HouseNumber), this.HouseNumber);
					case nameof(this.PostalCode):
						return this.ValidColumn(nameof(this.PostalCode), this.PostalCode);
					case nameof(this.PhoneNumber):
						return this.ValidColumn(nameof(this.PhoneNumber), this.PhoneNumber);
					case nameof(this.BirthDate):
						return this.ValidColumn(nameof(this.BirthDate), this.BirthDate.HasValue ? this.BirthDate.Value.ToString() : null);
					default:
						return null;
				}
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
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string StreetName { get; set; }
		public string HouseNumber { get; set; }
		public string ApartmentNumber { get; set; }
		public string PostalCode { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime? BirthDate { get; set; }
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

		public string Error => throw new NotImplementedException();

		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

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

		private string ValidColumn(string paramName, string paramValue)
		{
			if (String.IsNullOrEmpty(paramValue))
			{
				string err = "Data cannot be empty";

				this.AddError(paramName, err);

				return err;
			}
			this.RemoveError(paramName);
			return null;
		}
	}
}
