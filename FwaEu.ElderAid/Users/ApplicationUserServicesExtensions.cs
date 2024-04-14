using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Modules.MasterData;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using FwaEu.Fwamework.Users.WebApi;
using FwaEu.Modules.Users.HistoryPart;
using FwaEu.Modules.Users.HistoryPart.Services;
using FwaEu.Modules.Users.UserPerimeter;
using FwaEu.ElderAid.Users.MasterData;
using FwaEu.ElderAid.Users.UserGroups;
using Microsoft.Extensions.DependencyInjection;
using System;
using FwaEu.ElderAid.Users.UsersOrganizations;
using FwaEu.ElderAid.Organizations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.GenericImporter;
using FwaEu.ElderAid.Users.Import;
using FwaEu.Modules.SimpleMasterData;
using FwaEu.Modules.SimpleMasterData.MasterData;
using FwaEu.Modules.SimpleMasterData.GenericAdmin;
using FwaEu.Modules.SearchEngine;
using FwaEu.ElderAid.Users.SearchEngine;
using FwaEu.ElderAid.Articles.MasterData;
using FwaEu.ElderAid.Organizations.MasterData;

namespace FwaEu.ElderAid.Users
{
	public static class ApplicationUserServicesExtensions
	{
		public static IServiceCollection AddApplicationUserServices(this IServiceCollection services,
			ApplicationInitializationContext context)
		{
			services.Configure<UserEntityClassMapOptions>(options =>
			{
				options.IdentityColumnName = ApplicationUserEntityClassMap.IdentityColumnName;
			});

			services.AddTransient<IUserService, ApplicationUserService>();
			services.AddTransient<IPartHandler, HistoryPartHandler>();
			services.AddTransient<IListPartHandler, ListHistoryPartHandler>();
			services.AddTransient<IModelImporter<ApplicationUserImportModel>, ApplicationUserImporter>();

			services.AddTransient<EntityUserMasterDataProvider>();

			services.AddMasterDataProvider<ApplicationUserMasterDataProvider>("Users")
		    .AddRelatedEntity<UserEntity>()
			.AddRelatedEntity<ApplicationUserEntity>();

			var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();
			repositoryRegister.Add<UserGroupPerimeterEntityRepository>();
			repositoryRegister.Add<UserOrganizationPerimeterEntityRepository>();
			repositoryRegister.Add<ApplicationUserEntityRepository>();

			services.For<UserGroupEntity>(context)
				.AddRepository<UserGroupEntityRepository>()
					.AddMasterDataProviderFactory()
					.AddGenericAdminModelConfiguration();

			repositoryRegister.Add<OrganizationUserLinkEntityRepository>();
			services.AddMasterDataProvider<UserOrganizationsMasterDataProvider>("UsersOrganizations");

			services.AddUserPerimeterProvider<UserGroupUserPerimeterProvider>(UserGroupPerimeterEntity.ProviderKey);
			services.AddUserPerimeterProvider<UserOrganizationUserPerimeterProvider>(UserOrganizationPerimeterEntity.ProviderKey);

			services.AddTransient<IPermissionProviderFactory, DefaultPermissionProviderFactory<UserGroupPermissionProvider>>();
			services.AddTransient<IUserDetailsService, ApplicationUserDetailsService>();

			services.AddTransient<IListPartHandler, ApplicationUserModelListPartHandler>();
			services.AddTransient<IPartHandler, ApplicationUserModelPartHandler>();

			services.AddSearchEngineResultProvider<UserSearchEngineResultProvider>("User");
			services.AddSearchEngineResultProvider<UserIdSearchEngineResultProvider>("UserId");

			return services;
		}
	}
}
