using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Services
{
	public class PathProvider : IPathProvider
	{
		private readonly IMessageStringManager messageStringManager;

		public PathProvider(IMessageStringManager messageStringManager)
		{
			this.messageStringManager = messageStringManager;
		}

		public string GetUserProfilePath()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

			if (String.IsNullOrEmpty(path))
			{
				throw new InvalidOperationException(this.messageStringManager.CannotFindCurrentUserProfilePathExceptionMessage);
			}

			return path;
		}
	}
}
