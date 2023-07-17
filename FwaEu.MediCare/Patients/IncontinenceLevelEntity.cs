using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using FwaEu.MediCare.Organizations;
using System;
using System.Linq;

namespace FwaEu.MediCare.Patients
{
    public enum IncontinenceLevel
    {
        Light = 0,
        Medium = 1,
        Severe = 2,
        Total = 3,
        Double = 4
    }

    public class IncontinenceLevelEntity : ICreationAndUpdateTracked
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public OrganizationEntity Organization { get; set; }

        public IncontinenceLevel Level { get; set; }
        public int Year { get; set; }
        public double Sum { get; set; }

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
            References(entity => entity.Organization).Not.Nullable();
            Map(entity => entity.PatientId);
            Map(entity => entity.Level);
            Map(entity => entity.Year);
            Map(entity => entity.Sum);

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