using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.Permissions.ByRole;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FwaEu.MediCare.Organizations
{
    public class OrganizationUserLinkEntity : IEntity, IUpdateTracked
    {
        public int Id { get; set; }
        public OrganizationEntity Organization { get; set; }
        public UserEntity User { get; set; }

        public UserEntity UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public bool IsNew()
        {
            return Id == 0;
        }
    }
    public class OrganizationUserLinkEntityClassMap : ClassMap<OrganizationUserLinkEntity>
    {
        public OrganizationUserLinkEntityClassMap()
        {
            Not.LazyLoad();
            Id(entity => entity.Id).GeneratedBy.Identity();
            References(entity => entity.Organization).Not.Nullable().UniqueKey("UQ_OrganizationUser");
            References(entity => entity.User).Not.Nullable().UniqueKey("UQ_OrganizationUser");
            this.AddUpdateTrackedPropertiesIntoMapping();
        }
    }
    public class OrganizationUserLinkEntityRepository : DefaultRepository<OrganizationUserLinkEntity, int>, IQueryByIds<OrganizationUserLinkEntity, int>
    {
        public IQueryable<OrganizationUserLinkEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }

        public IQueryable<OrganizationUserLinkEntity> QueryByUserId(int userId)
        {
            return this.Query().Where(ur => ur.User.Id == userId);
        }

        protected override IEnumerable<IRepositoryDataFilter<OrganizationUserLinkEntity, int>> CreateDataFilters(
            RepositoryDataFilterContext<OrganizationUserLinkEntity, int> context)
        {
            yield return new OrganizationUserLinkEntityDataFilter();
        }
    }
}