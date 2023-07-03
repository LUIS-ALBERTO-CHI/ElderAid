﻿using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.MediCare.Initialization;
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

    public class PatientEntity : IUpdatedOnTracked
    {
        // NOTE: PAT KID PAtient
        public int Id { get; set; }
        public int? BuildingId { get; set; }
        public string FullName { get; set; }
        public IncontinenceLevel IncontinenceLevel { get; set; }
        public DateTime? IncontinenceStartDate { get; set; }
        // NOTE: Location
        public string RoomName { get; set; }
        // NOTE: PAT Deleted
        public bool? IsActive { get; set; }
        // NOTE: PAT Created
        public DateTime UpdatedOn { get { return _dateTime; } set { } }
        private static DateTime _dateTime = DateTime.Now;
        public bool IsNew() => Id == 0;
    }

    [ConnectionString("Generic")]
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
            Map(entity => entity.IncontinenceLevel).Column("IncontinenceLevel");
            Map(entity => entity.IncontinenceStartDate).Column("IncontinenceStartDate");
            Map(entity => entity.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }

    public class PatientEntityRepository : DefaultRepository<PatientEntity, int>, IQueryByIds<PatientEntity, int>
    {
        public IQueryable<PatientEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}