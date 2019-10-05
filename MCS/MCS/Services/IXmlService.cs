using MCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Services
{
	public interface IXmlService
	{
		IList<Person> LoadXml(string filePath);
		void SaveXml(string filePath, IList<Person> people);
		bool CheckFileExists(string filePath);
		void CreateXmlFile(string path);
	}
}
