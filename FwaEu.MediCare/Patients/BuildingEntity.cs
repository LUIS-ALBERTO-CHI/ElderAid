using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;

namespace FwaEu.MediCare.Patients
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
            Not.LazyLoad();
            ReadOnly();

            Id(entity => entity.Id).GeneratedBy.Identity();
            Map(entity => entity.Name).Not.Nullable();
        }
    }


    public class BuildingEntityRepository : DefaultRepository<BuildingEntity, int>
    {
    }
}
