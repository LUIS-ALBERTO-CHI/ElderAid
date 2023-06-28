using System;
using System.Linq;
using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;

namespace FwaEu.MediCare.Referencials
{
    public class DosageFormEntity : IUpdatedOnTracked
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedOn { get { return _dateTime; } set { } }
        private static DateTime _dateTime = DateTime.Now;
        public bool IsNew() => Id == 0;
    }
    public class DosageFormEntityClassMap : ClassMap<DosageFormEntity>
    {
        public DosageFormEntityClassMap()
        {
            Not.LazyLoad();
            
            Id(entity => entity.Id).GeneratedBy.Identity();
            Map(entity => entity.Name).Not.Nullable().Unique();
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