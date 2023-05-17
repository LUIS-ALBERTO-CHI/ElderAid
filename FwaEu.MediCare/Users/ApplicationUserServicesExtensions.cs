using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Fwamework.Users.WebApi;
using FwaEu.Modules.Users.HistoryPart.Services;
using FwaEu.MediCare.Users.MasterData;
using Microsoft.Extensions.DependencyInjection;

using FwaEu.Modules.GenericImporter;
using FwaEu.MediCare.Users.Import;
using FwaEu.Modules.SimpleMasterData.MasterData;

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

			services.AddMasterDataProvider<ApplicationUserMasterDataProvider>("Users")
		    .AddRealtedEntity<UserEntity>()
			.AddRealtedEntity<ApplicationUserEntity>();

			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<ApplicationUserEntityRepository>();

			services.AddTransient<IUserDetailsService, ApplicationUserDetailsService>();

			services.AddTransient<IListPartHandler, ApplicationUserModelListPartHandler>();
			services.AddTransient<IPartHandler, ApplicationUserModelPartHandler>();

			return services;
		}
	}
}
