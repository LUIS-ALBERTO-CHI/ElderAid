using System;
using System.Linq;
using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;

namespace FwaEu.ElderAid.Referencials
{
    public class DosageFormEntity :IEntity, ICreationAndUpdateTracked
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserEntity CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserEntity UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsNew()
        {
            return this.Id == 0;
        }
    }
    public class DosageFormEntityClassMap : ClassMap<DosageFormEntity>
    {
        public DosageFormEntityClassMap()
        {
            Not.LazyLoad();
            
            Id(entity => entity.Id).GeneratedBy.Identity();
            Map(entity => entity.Name).Not.Nullable().Unique();
            this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
        }
    }

    public class DosageFormEntityRepository : DefaultRepository<DosageFormEntity, int>, IQueryByIds<DosageFormEntity, int>
    {

        public IQueryable<DosageFormEntity> QueryByIds(int[] ids)
        {
            return this.Query().Where(entity => ids.Contains(entity.Id));
        }
    }
}