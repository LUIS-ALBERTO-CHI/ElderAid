using FwaEu.Modules.GenericAdmin.WebApi;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.Modules.GenericAdmin
{
	public static class GenericAdminExtensions
	{
		public static IServiceCollection AddFwameworkModuleGenericAdmin(this IServiceCollection services)
		{
			services.AddTransient<IDataSourceModelFactory, ArrayDataSourceModelFactory>();
			services.AddTransient<IDataSourceModelFactory, EnumDataSourceModelFactory>();
			return services;
		}
	}
}
