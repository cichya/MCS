using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Helpers
{
	public class CannotCreateFileException : Exception
	{
		public CannotCreateFileException(string message) : base(message)
		{

		}
	}
}
