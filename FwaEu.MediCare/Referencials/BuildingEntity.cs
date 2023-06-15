using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.MediCare.MappingTransformer;

namespace FwaEu.MediCare.Referencials
{
    public class BuildingEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class BuildingEntityClassMap : ClassMap<BuildingEntity>
    {
        public BuildingEntityClassMap()
        {
            Table("MEDICARE_EMS.dbo.MDC_Buildings");
           
            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.Name).Column("Name");
        }
    }


    public class BuildingEntityRepository : DefaultRepository<BuildingEntity, int>
    {
    }
}