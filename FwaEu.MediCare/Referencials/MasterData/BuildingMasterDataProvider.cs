using FwaEu.Modules.MasterData;
using System;
using FwaEu.Fwamework.Globalization;
using FwaEu.MediCare.GenericRepositorySession;
using System.Globalization;
using System.Linq.Expressions;

namespace FwaEu.MediCare.Referencials.MasterData
{
    public class BuildingMasterDataProvider : EntityMasterDataProvider<BuildingEntity, int, BuildingEntityMasterDataModel, BuildingEntityRepository>
    {
        public BuildingMasterDataProvider(GenericSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<BuildingEntity, BuildingEntityMasterDataModel>>
            CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new BuildingEntityMasterDataModel(entity.Id, entity.Name);
        }

        protected override Expression<Func<BuildingEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Name.Contains(search);
        }
    }

    public class BuildingEntityMasterDataModel
    {
        public BuildingEntityMasterDataModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}