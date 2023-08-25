using FwaEu.Modules.MasterData;
using System;
using FwaEu.Fwamework.Globalization;
using System.Globalization;
using System.Linq.Expressions;
using FwaEu.MediCare.GenericRepositorySession;

namespace FwaEu.MediCare.Patients.MasterData
{
    public class PatientMasterDataProvider : EntityMasterDataProvider<PatientEntity, int, PatientEntityMasterDataModel, PatientEntityRepository>
    {
        public PatientMasterDataProvider(GenericSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<PatientEntity, PatientEntityMasterDataModel>>
            CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new PatientEntityMasterDataModel(entity.Id, entity.IncontinenceLevel, entity.BuildingId, entity.FullName, entity.RoomName, entity.IsActive, entity.IncontinenceStartDate, entity.UpdatedOn);
        }

        protected override Expression<Func<PatientEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.FullName.Contains(search);
        }
    }

    public class PatientEntityMasterDataModel
    {
        public PatientEntityMasterDataModel(int id, IncontinenceLevel incontinenceLevel, int? buildingId, string fullName, string roomName, bool? isActive, DateTime? incontinenceStartDate, DateTime? updatedOn)
        {
            var rand = new Random();
            var value = rand.Next(0, 2);
            Id = id;
            IncontinenceLevel = incontinenceLevel;
            BuildingId = buildingId;
            FullName = fullName;
            RoomName = roomName;
            IsActive = isActive;
            IncontinenceStartDate = incontinenceStartDate;
            UpdatedOn = updatedOn;
            IsIncontinenceLevelOverPassed = Convert.ToBoolean(value);

        }

        public int Id { get; }
        public int? BuildingId { get; }
        public string FullName { get; }
        public string RoomName { get; }
        public bool? IsActive { get; }
        public IncontinenceLevel IncontinenceLevel { get; set; }
        public bool IsIncontinenceLevelOverPassed { get; set; }
        public DateTime? IncontinenceStartDate { get; set; }
        public DateTime? UpdatedOn { get; }
    }
}