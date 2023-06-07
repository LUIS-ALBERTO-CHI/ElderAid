using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;

namespace FwaEu.MediCare.Patients
{
    public class PatientEntity : IPerson
    {
        public int Id { get; set; }

        public int? BuildingId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoomName { get; set; }
        public bool? IsActive { get; set; }
        public override string ToString()
        {
            return this.ToFullNameString();
        }
    }


    public class PatientEntityClassMap : ClassMap<PatientEntity>
    {
        public PatientEntityClassMap()
        {
            Not.LazyLoad();
            ReadOnly();

            Id(entity => entity.Id).GeneratedBy.Identity();
            Map(entity => entity.BuildingId);
            Map(entity => entity.LastName);
            Map(entity => entity.FirstName);
            Map(entity => entity.RoomName);
            Map(entity => entity.IsActive);
        }
    }


    public class PatientEntityRepository : DefaultRepository<PatientEntity, int>
    {
    }
}
