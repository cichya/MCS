using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MCS.Services
{
	public class MessageBoxService : IMessageBoxService
	{
		public void ShowErrorMsgBox(string message)
		{
			MessageBox.Show(message, Languages.Lang.ErrorMessageBoxContentText, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
