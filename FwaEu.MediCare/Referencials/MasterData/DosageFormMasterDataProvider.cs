using System;
using System.Globalization;
using System.Linq.Expressions;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;

namespace FwaEu.MediCare.Referencials.MasterData
{
    public class DosageFormMasterDataProvider : EntityMasterDataProvider<DosageFormEntity, int, DosageFormEntityMasterDataModel, DosageFormEntityRepository> 
    {
        public DosageFormMasterDataProvider(MainSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<DosageFormEntity, DosageFormEntityMasterDataModel>>
        CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new DosageFormEntityMasterDataModel(entity.Id, entity.Name);
        }

        protected override Expression<Func<DosageFormEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Name.Contains(search);
        }
    }


    public class DosageFormEntityMasterDataModel
    {
        public DosageFormEntityMasterDataModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }

}