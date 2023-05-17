using FwaEu.Modules.GenericAdmin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	[TestClass]
	public class ExportTests
	{
		private static ServiceProvider BuildServiceProvider()
		{
			var services = new ServiceCollection();

			services.AddFwameworkModuleGenericAdmin();

			services.AddTransient<IGenericAdminModelConfiguration, MockGenericAdminModelConfiguration>();

			return services.BuildServiceProvider();
		}

		[TestMethod]
		public void FindTestExportByType_ShouldFind()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configurations = serviceProvider.GetServices<IGenericAdminModelConfiguration>();

				Assert.IsNotNull(configurations
					.FirstOrDefault(em => em is MockGenericAdminModelConfiguration));
			}
		}

		[TestMethod]
		public void FindTestExportByKey_ShouldFind()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configurations = serviceProvider.GetServices<IGenericAdminModelConfiguration>();

				Assert.IsNotNull(configurations.FirstOrDefault(c => c.Key == "Mock"));
			}
		}

		[TestMethod]
		public void FindTestExportByKey_ShouldNotFind()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configurations = serviceProvider.GetServices<IGenericAdminModelConfiguration>();

				Assert.IsNull(configurations.FirstOrDefault(c => c.Key == "Mock" + DateTime.Now.Ticks.ToString()));
			}
		}
	}
}
