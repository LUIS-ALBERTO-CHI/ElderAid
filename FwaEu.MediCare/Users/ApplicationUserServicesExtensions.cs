using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Fwamework.Users.WebApi;
using FwaEu.Modules.Users.HistoryPart;
using FwaEu.Modules.Users.HistoryPart.Services;
using FwaEu.Modules.Users.UserPerimeter;
using FwaEu.MediCare.Users;
using FwaEu.MediCare.Users.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.GenericImporter;
using FwaEu.MediCare.Users.Import;
using FwaEu.Modules.SimpleMasterData;
using FwaEu.Modules.SimpleMasterData.MasterData;
using FwaEu.Modules.SimpleMasterData.GenericAdmin;

namespace FwaEu.MediCare.Users
{
	public static class ApplicationUserServicesExtensions
	{
		public static IServiceCollection AddApplicationUserServices(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			services.Configure<UserEntityClassMapOptions>(options =>
			{
				options.IdentityColumnName = ApplicationUserEntityClassMap.EmailColumnName;
			});

			services.AddTransient<IUserService, ApplicationUserService>();
			services.AddTransient<IPartHandler, HistoryPartHandler>();
			services.AddTransient<IListPartHandler, ListHistoryPartHandler>();
			services.AddTransient<IModelImporter<ApplicationUserImportModel>, ApplicationUserImporter>();

			services.AddTransient<EntityUserMasterDataProvider>();
			services.AddMasterDataProvider<ApplicationUserMasterDataProvider>("Users");

			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<ApplicationUserEntityRepository>();

			services.AddTransient<IUserDetailsService, ApplicationUserDetailsService>();

			services.AddTransient<IListPartHandler, ApplicationUserModelListPartHandler>();
			services.AddTransient<IPartHandler, ApplicationUserModelPartHandler>();

			services.AddTransient<IUserSynchronizationService, UserSynchronizationService>();
			
			return services;
		}
	}
}
