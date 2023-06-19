﻿
using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using System;

namespace FwaEu.MediCare.Patients
{
    public class PatientEntity 
    {
        // NOTE: PAT KID PAtient
        public int Id { get; set; }

        public int? BuildingId { get; set; }

        public string FullName { get; set; }

        // NOTE: Location
        public string RoomName { get; set; }
        // NOTE: PAT Deleted
        public bool? IsActive { get; set; }
        // NOTE: PAT Created
        public DateTime? UpdatedOn { get; set; }
    }


    public class PatientEntityClassMap : ClassMap<PatientEntity>
    {
        public PatientEntityClassMap()
        {
            Table("MDC_Patients");
            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.BuildingId).Column("BuildingId");
            Map(entity => entity.FullName).Column("FullName");
            Map(entity => entity.RoomName).Column("RoomName");
            Map(entity => entity.IsActive).Column("IsActive");
            Map(entity => entity.UpdatedOn).Column("UpdatedOn");
        }
    }

    public class PatientEntityRepository : DefaultRepository<PatientEntity, int>
    {
    }
}
