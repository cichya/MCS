using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using MCS.Models;

namespace MCS.Services
{
	public class XmlService : IXmlService
	{
		public bool CheckFileExists(string filePath)
		{
			return File.Exists(filePath);
		}

		public void CreateXmlFile(string path)
		{
			var xmlFile = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

			xmlFile.Add(new XElement("ArrayOfPerson"));

			xmlFile.Save(path);
		}

		public IList<Person> LoadXml(string filePath)
		{
			var xmlStr = File.ReadAllText(filePath);

			using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr)))
			{
				XmlRootAttribute xRoot = new XmlRootAttribute();
				xRoot.ElementName = "ArrayOfPerson";

				XmlSerializer serializer = new XmlSerializer(typeof(List<Person>), xRoot);
				return ((List<Person>)serializer.Deserialize(ms));
			}
		}

		public void SaveXml(string filePath, IList<Person> people)
		{
			using (var stringWriter = new StringWriter())
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true }))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Person>));

					xmlSerializer.Serialize(xmlWriter, people);

					File.WriteAllText(filePath, stringWriter.ToString(), Encoding.UTF8);
				}
			}
		}
	}
}
