using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Models;
using MCS.Services;

namespace MCS.Repositories
{
	public class PersonRepository : IPersonRepository
	{
		private const string dbFileName = "people_db.xml";

		private readonly IPathProvider pathProvider;
		private readonly IXmlService xmlService;

		public PersonRepository(IPathProvider pathProvider, IXmlService xmlService)
		{
			this.pathProvider = pathProvider;
			this.xmlService = xmlService;
		}

		public IList<Person> Get()
		{
			string userProfilePath = this.pathProvider.GetUserProfilePath();

			string filePath = Path.Combine(userProfilePath, dbFileName);

			if (!this.xmlService.CheckFileExists(filePath))
			{
				this.xmlService.CreateXmlFile(filePath);

				return null;
			}

			return this.xmlService.LoadXml(filePath);
		}

		public void Save(IList<Person> people)
		{
			string userProfilePath = this.pathProvider.GetUserProfilePath();

			string filePath = Path.Combine(userProfilePath, dbFileName);

			if (!this.xmlService.CheckFileExists(filePath))
			{
				this.xmlService.CreateXmlFile(filePath);
			}

			this.xmlService.SaveXml(filePath, people);
		}
	}
}
