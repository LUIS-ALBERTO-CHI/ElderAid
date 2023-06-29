using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;

namespace FwaEu.MediCare.Referencials
{
    public class CabinetEntity : IUpdatedOnTracked
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime UpdatedOn { get; set; }
        public bool IsNew() => Id == 0;
    }

    public class CabinetEntityClassMap : ClassMap<CabinetEntity>
    {
        public CabinetEntityClassMap()
        {
            Table("MDC_Cabinets");

            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.Name).Column("Name");
        }
    }

    public class CabinetEntityRepository : DefaultRepository<CabinetEntity, int>, IQueryByIds<CabinetEntity, int>
    {
        public System.Linq.IQueryable<CabinetEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}