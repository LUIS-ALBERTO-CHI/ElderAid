using FwaEu.ElderAid.Organizations;
using FwaEu.ElderAid.Users.UsersOrganizations;
using FwaEu.Fwamework;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.UserPerimeter;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Users.UsersOrganizations
{
	public class UserOrganizationUserPerimeterProvider
		: EntityUserPerimeterProviderBase
			<
			UserOrganizationPerimeterEntity, UserOrganizationPerimeterEntityRepository,
			OrganizationEntity, int, OrganizationEntityRepository
			>
	{
		public UserOrganizationUserPerimeterProvider(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		protected override Expression<Func<UserOrganizationPerimeterEntity, int>> SelectReferenceEntityId()
		{
			return entity => entity.Organization.Id;
		}

		protected override Expression<Func<UserOrganizationPerimeterEntity, bool>> WhereContainsReferenceEntities(int[] ids)
		{
			return entity => ids.Contains(entity.Organization.Id);
		}

		protected override Expression<Func<UserOrganizationPerimeterEntity, bool>> WhereFullAccess()
		{
			return entity => entity.Organization == null;
		}

		protected override Expression<Func<UserOrganizationPerimeterEntity, bool>> WhereNotFullAccess()
		{
			return entity => entity.Organization != null;
		}

		protected override UserOrganizationPerimeterEntity CreatePerimeterEntity(OrganizationEntity referenceEntity)
		{
			return new UserOrganizationPerimeterEntity()
			{
				Organization = referenceEntity,
			};
		}
	}
}
