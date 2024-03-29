using System;
using System.Globalization;
using System.Linq.Expressions;
using FwaEu.ElderAid.GenericRepositorySession;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;

namespace FwaEu.ElderAid.Referencials.MasterData
{
    public class CabinetMasterDataProvider : EntityMasterDataProvider<CabinetEntity, int, CabinetEntityMasterDataModel, CabinetEntityRepository>
    {
        public CabinetMasterDataProvider(GenericSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<CabinetEntity, CabinetEntityMasterDataModel>>
        CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new CabinetEntityMasterDataModel(entity.Id, entity.Name);
        }

        protected override Expression<Func<CabinetEntity, bool>> CreateSearchExpression(string search,
        CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Name.Contains(search);
        }
    }

    public class CabinetEntityMasterDataModel
    {
        public CabinetEntityMasterDataModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}