using FwaEu.Modules.MasterData;
using System;
using FwaEu.Fwamework.Globalization;
using System.Globalization;
using System.Linq.Expressions;
using FwaEu.Fwamework.Data.Database.Sessions;

namespace FwaEu.MediCare.Referencials.MasterData
{
    public class IncontinenceLevelMasterDataProvider : EntityMasterDataProvider<IncontinenceLevelEntity, int, IncontinenceLevelEntityMasterDataModel, IncontinenceLevelEntityRepository>
    {
        public IncontinenceLevelMasterDataProvider(MainSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<IncontinenceLevelEntity, IncontinenceLevelEntityMasterDataModel>>
            CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new IncontinenceLevelEntityMasterDataModel(entity.Id, (int)entity.Level, entity.Year, entity.Amount);
        }
    }

    public class IncontinenceLevelEntityMasterDataModel
    {
        public IncontinenceLevelEntityMasterDataModel(int id, int level, int year, double amount)
        {
            Id = id;
            Level = level;
            Year = year;
            Amount = amount;
        }

        public int Id { get; }
        public int Level { get; }
        public int Year { get; }
        public double Amount { get; }
    }
}