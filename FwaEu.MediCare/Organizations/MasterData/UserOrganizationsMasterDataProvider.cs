using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Users;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace FwaEu.MediCare.Organizations.MasterData
{
	public class UserOrganizationsMasterDataProvider : EntityMasterDataProvider<OrganizationUserLinkEntity, int, UserOrganizationMasterDataModel, OrganizationUserLinkEntityRepository>
	{

        public UserOrganizationsMasterDataProvider(
            MainSessionContext sessionContext,
            ICulturesService culturesService)
            : base(sessionContext, culturesService)
        {
        }
        protected override Expression<Func<OrganizationUserLinkEntity, UserOrganizationMasterDataModel>> CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new UserOrganizationMasterDataModel(entity.Id, entity.Organization.InvariantId, entity.Organization.Name, entity.Organization.DatabaseName, entity.User.Id);
        }
        protected override Expression<Func<OrganizationUserLinkEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Organization.Name.Contains(search) || entity.Organization.InvariantId.Contains(search);
        }
    }

	public class UserOrganizationMasterDataModel
    {
		public UserOrganizationMasterDataModel(int id, string invariantId, string name, string databaseName, int userId)
		{
			this.Id = id;
            this.InvariantId = invariantId;
			this.Name = name;
            this.DataBaseName = databaseName;
            this.UserId = userId;
        }

		public int Id { get; }
		public string InvariantId { get; }
        public string Name { get; }
        public string DataBaseName { get; }
        public int UserId { get; }
    }
}
