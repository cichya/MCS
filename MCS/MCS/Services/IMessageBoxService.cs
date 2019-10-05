using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Services
{
	public interface IMessageBoxService
	{
		void ShowErrorMsgBox(string message);
	}
}
