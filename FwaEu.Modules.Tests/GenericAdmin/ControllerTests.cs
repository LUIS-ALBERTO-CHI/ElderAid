using FwaEu.Fwamework.Users;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdmin.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FwaEu.Modules.Tests.GenericAdmin
{
	[TestClass]
	public class ControllerTests
	{
		private static ServiceProvider BuildServiceProvider()
		{
			var services = new ServiceCollection();

			services.AddFwameworkModuleGenericAdmin();

			services.AddSingleton<IGenericAdminModelConfiguration, MockGenericAdminModelConfiguration>();
			services.AddSingleton<IGenericAdminModelConfiguration, UnaccessibleGenericAdminModelConfigurationFake>();
			services.AddSingleton<IGenericAdminModelConfiguration, ModelAttributeMockGenericAdminModelConfiguration>();

			services.AddSingleton<ICurrentUserService, FakeCurrentUserService>();

			return services.BuildServiceProvider();
		}

		[TestMethod]
		public async Task GetConfiguration_IsAccessible()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configurations = serviceProvider.GetServices<IGenericAdminModelConfiguration>();
				var configuration = configurations
					.FirstOrDefault(c => c.Key == "UnaccessibleTest");
				Assert.IsFalse(await configuration.IsAccessibleAsync());

				var result = await new GenericAdminController().GetConfiguration("UnaccessibleTest", configurations, serviceProvider);

				Assert.AreEqual(typeof(ForbidResult), result.Result.GetType(),
					$"This configuration should raise a '{typeof(ForbidResult).FullName}' exception.");
			}
		}

		[TestMethod]
		public async Task GetConfiguration_PropertiesCopy()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configurations = serviceProvider.GetServices<IGenericAdminModelConfiguration>();
				var configuration = serviceProvider.GetServices<IGenericAdminModelConfiguration>()
					.FirstOrDefault(c => c.Key == "Mock");

				var actionResult = await new GenericAdminController().GetConfiguration("Mock", configurations, serviceProvider);
				var controllerModel = (actionResult.Result as OkObjectResult).Value as ConfigurationModel;

				foreach (var property in configuration.GetProperties())
				{
					var modelProperty = controllerModel.Properties.First(p => p.Name == property.Name);

					Assert.AreEqual(property.IsKey, modelProperty.IsKey);
					Assert.AreEqual(property.Name, modelProperty.Name);
					Assert.AreEqual(property.InnerType.Name, modelProperty.Type);
					CollectionAssert.AreEqual(property.ExtendedProperties, modelProperty.ExtendedProperties);
				}
			}
		}

		[TestMethod]
		public async Task GetConfiguration_ModelsDataLoad()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var currentUserService = serviceProvider.GetService<ICurrentUserService>();
				var configurations = serviceProvider.GetServices<IGenericAdminModelConfiguration>();
				var configuration = serviceProvider.GetServices<IGenericAdminModelConfiguration>()
					.FirstOrDefault(c => c.Key == "MockByModelAttribute");

				var authorizedActions = configuration.GetAuthorizedActions();

				var loadDataResult = await configuration.GetModelsAsync();
				Assert.IsFalse(loadDataResult.Value.CouldHaveMoreItems);

				var actionResult = await new GenericAdminController().GetConfiguration("MockByModelAttribute", configurations, serviceProvider);
				var controllerModel = (actionResult.Result as OkObjectResult).Value as ConfigurationModel;

				Assert.IsInstanceOfType(controllerModel.Models.DataSource, typeof(ArrayDataSourceModel));
				Assert.AreEqual(loadDataResult.Value.Items.Count(), controllerModel.Models.DataSource.Items.Length);
			}
		}
	}
}
