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

namespace FwaEu.ElderAid.Organizations.MasterData
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
            return entity => new UserOrganizationMasterDataModel(entity.Id, entity.User.Id, entity.Organization.Id);
        }
        protected override Expression<Func<OrganizationUserLinkEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Organization.Name.Contains(search) || entity.Organization.InvariantId.Contains(search);
        }
    }

	public class UserOrganizationMasterDataModel
    {
		public UserOrganizationMasterDataModel(int id, int userId, int organizationId)
		{
			this.Id = id;
            this.UserId = userId;
            this.OrganizationId = organizationId;
        }

		public int Id { get; }
        public int OrganizationId { get; }
        public int UserId { get; }
    }
}
