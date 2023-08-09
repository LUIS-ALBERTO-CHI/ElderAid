using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.MediCare.Initialization;
using System;

namespace FwaEu.MediCare.Referencials
{
    public class BuildingEntity : IUpdatedOnTracked
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime UpdatedOn { get { return _dateTime; } set { } }
        private static DateTime _dateTime = DateTime.Now;
        public bool IsNew() => Id == 0;
    }

    [ConnectionString("Generic")]
    public class BuildingEntityClassMap : ClassMap<BuildingEntity>
    {
        public BuildingEntityClassMap()
        {
            Table("MDC_Buildings");
           
            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.Name).Column("Name");
            Map(entity => entity.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }


    public class BuildingEntityRepository : DefaultRepository<BuildingEntity, int>, IQueryByIds<BuildingEntity, int>
    {
        public System.Linq.IQueryable<BuildingEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}