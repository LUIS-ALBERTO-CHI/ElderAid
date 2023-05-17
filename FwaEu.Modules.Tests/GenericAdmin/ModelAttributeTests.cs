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
	public class ModelAttributeTests
	{
		private static ServiceProvider BuildServiceProvider()
		{
			var services = new ServiceCollection();

			services.AddFwameworkModuleGenericAdmin();

			services.AddTransient<IGenericAdminModelConfiguration, ModelAttributeMockGenericAdminModelConfiguration>();

			return services.BuildServiceProvider();
		}

		[TestMethod]
		public void ModelAttributeConfiguration_GetProperties()
		{
			using (var serviceProvider = BuildServiceProvider())
			{
				var configuration = serviceProvider.GetServices<IGenericAdminModelConfiguration>()
					.FirstOrDefault(c => c.Key == "MockByModelAttribute");
				var properties = configuration.GetProperties().ToDictionary(p => p.Name);

				Assert.IsTrue(properties[nameof(ModelAttributeMockModel.Id)].IsKey);

				var nameProperty = properties[nameof(ModelAttributeMockModel.Name)];
				Assert.AreEqual(true, nameProperty.ExtendedProperties.GetValueOrDefault("IsRequired"));
				Assert.AreEqual(100, nameProperty.ExtendedProperties.GetValueOrDefault("MaxLength"));

				Assert.IsNotNull(properties[nameof(ModelAttributeMockModel.CityId)].ExtendedProperties);
			}
		}
	}
}
