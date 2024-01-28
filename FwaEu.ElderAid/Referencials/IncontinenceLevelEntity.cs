using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using FwaEu.ElderAid.Patients;
using System;
using System.Linq;

namespace FwaEu.ElderAid.Referencials
{


    public class IncontinenceLevelEntity : ICreationAndUpdateTracked
    {
        public int Id { get; set; }

        public IncontinenceLevel Level { get; set; }
        public int Year { get; set; }
        public double Amount { get; set; }

        public UserEntity CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserEntity UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public bool IsNew() => Id == 0;
    }

    public class IncontinenceLevelEntityClassMap : ClassMap<IncontinenceLevelEntity>
    {
        public IncontinenceLevelEntityClassMap()
        {
            Not.LazyLoad();

            Id(entity => entity.Id).GeneratedBy.Identity();
            Map(entity => entity.Level);
            Map(entity => entity.Year);
            Map(entity => entity.Amount);

            this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
        }
    }

    public class IncontinenceLevelEntityRepository : DefaultRepository<IncontinenceLevelEntity, int>, IQueryByIds<IncontinenceLevelEntity, int>
    {
        public IQueryable<IncontinenceLevelEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}