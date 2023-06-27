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
            return entity => new PatientEntityMasterDataModel(entity.Id, entity.BuildingId, entity.FullName, entity.RoomName, entity.IsActive, entity.UpdatedOn);
        }

        protected override Expression<Func<PatientEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.FullName.Contains(search);
        }
    }

    public class PatientEntityMasterDataModel
    {
        public PatientEntityMasterDataModel(int id, int? buildingId, string fullName, string roomName, bool? isActive, DateTime? updatedOn)
        {
            Id = id;
            BuildingId = buildingId;
            FullName = fullName;
            RoomName = roomName;
            IsActive = isActive;
            UpdatedOn = updatedOn;
        }

        public int Id { get; }
        public int? BuildingId { get; }
        public string FullName { get; }
        public string RoomName { get; }
        public bool? IsActive { get; }
        public DateTime? UpdatedOn { get; }
    }
}