
using FwaEu.Fwamework.Users;
using System;

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
        public DateTime? UpdatedOn { get; set; }
        public override string ToString()
        {
            return this.ToFullNameString();
        }
    }


}
