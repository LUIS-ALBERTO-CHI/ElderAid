using FluentNHibernate.Mapping;
using FwaEu.ElderAid.Organizations;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Users.UserPerimeter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Users.UsersOrganizations
{
	public class UserOrganizationPerimeterEntity : IPerimeterEntity<int>
	{
		public const string ProviderKey = "UsersOrganizations";

		public int Id { get; protected set; }

		public UserEntity User { get; set; }
		public OrganizationEntity Organization { get; set; }

		public int GetReferenceEntityId()
		{
			return this.Organization.Id;
		}

		public bool IsNew()
		{
			return this.Id == 0;
		}
	}

	public class UserOrganizationPerimeterEntityClassMap : ClassMap<UserOrganizationPerimeterEntity>
	{
		public UserOrganizationPerimeterEntityClassMap()
		{
			Not.LazyLoad();
			Id(entity => entity.Id).GeneratedBy.Identity();
			References(entity => entity.User).Not.Nullable().UniqueKey("UQ_User_Organization");
			References(entity => entity.Organization).UniqueKey("UQ_User_Organization");
		}
	}

	public class UserOrganizationPerimeterEntityRepository : DefaultRepository<UserOrganizationPerimeterEntity, int>
	{
	}
}
