using MCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Repositories
{
	public interface IPersonRepository
	{
		IList<Person> Get();
		void Save(IList<Person> people);
	}
}
