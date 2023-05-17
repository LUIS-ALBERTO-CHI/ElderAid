using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FwaEu.Modules.GenericAdmin.WebApi
{
	public class ConfigurationModel
	{
		public PropertyModel[] Properties { get; set; }
		public ModelsDataSourceModel Models { get; set; }
		public AuthorizedActionsModel AuthorizedActions { get; set; }

		private static DataSourceModel CreateDataSourceModel(
			IEnumerable<IDataSourceModelFactory> dataSourceModelFactories, IDataSource dataSource)
		{
			return dataSourceModelFactories.Select(factory => factory.Create(dataSource))
				.First(model => model != null);
		}

		public static async Task<ConfigurationModel> FromConfigurationAsync(IGenericAdminModelConfiguration configuration, 
			IServiceProvider serviceProvider)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}

			var dataSourceModelFactories = serviceProvider.GetServices<IDataSourceModelFactory>();

			var loadDataResult = await configuration.GetModelsAsync();

			var authorizedActions = configuration.GetAuthorizedActions();

			return new ConfigurationModel()
			{
				Properties = configuration.GetProperties().Select(p =>
				{
					var propertyModel = new PropertyModel()
					{
						Name = p.Name,
						Type = p.CustomInnerTypeName ?? p.InnerType.Name,
						ExtendedProperties = p.ExtendedProperties,
					};

					Property.Copy(p, propertyModel);
					return propertyModel;
				}
				)
				.ToArray(),

				Models = new ModelsDataSourceModel()
				{
					DataSource = CreateDataSourceModel(dataSourceModelFactories, loadDataResult.Value),
				},

				AuthorizedActions = new AuthorizedActionsModel()
				{
					AllowCreate = authorizedActions.AllowCreate,
					AllowDelete = authorizedActions.AllowDelete,
					AllowUpdate = authorizedActions.AllowUpdate,
				}
			};
		}
	}

	public class PropertyModel : IProperty
	{
		public bool IsKey { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }

		public bool IsEditable { get; set; } = true;
		public Dictionary<string, object> ExtendedProperties { get; set; }
	}

	public class AuthorizedActionsModel
	{
		public bool AllowCreate { get; set; }
		public bool AllowUpdate { get; set; }
		public bool AllowDelete { get; set; }
	}
}