using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Users;
using FwaEu.MediCare.Initialization;
using FwaEu.MediCare.Users;
using FwaEu.TestTools.InMemoryDatabase.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Tests.Users
{
	[TestClass]
	public class ApplicationUserServiceTests
	{
		private static ServiceCollection CreateServiceCollection()
		{
			var services = new ServiceCollection();
			services.AddFwameworkUsersServices();
			services.AddApplicationUserServices(new ApplicationInitializationContextDummy());
			return services;
		}

	}
}
