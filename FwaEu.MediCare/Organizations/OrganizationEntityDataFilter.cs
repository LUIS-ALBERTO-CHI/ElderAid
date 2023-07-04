using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Organizations
{
    public class OrganizationEntityDataFilter : IRepositoryDataFilter<OrganizationEntity, int>
    {
        public Expression<Func<OrganizationEntity, bool>> Accept(RepositoryDataFilterContext<OrganizationEntity, int> context)
        {
            var organizationUserLinkRepository = context.ServiceProvider
               .GetRequiredService<IRepositoryFactory>()
               .Create<OrganizationUserLinkEntityRepository>(context.Session);
            var currentUserService = context.ServiceProvider.GetRequiredService<ICurrentUserService>();

            if (currentUserService != null)
            {
                var organizationsUserIds = organizationUserLinkRepository.Query().Where(entity => entity.User == currentUserService.User.Entity).Select(x => x.Organization.Id).ToList();
                return entity => currentUserService.User.Entity.IsAdmin || organizationsUserIds.Contains(entity.Id);
            }
            return null;
        }

        public async Task<bool> AcceptAsync(OrganizationEntity entity, RepositoryDataFilterContext<OrganizationEntity, int> context)
        {
            var organizationUserLinkRepository = context.ServiceProvider
               .GetRequiredService<IRepositoryFactory>()
               .Create<OrganizationUserLinkEntityRepository>(context.Session);
            var currentUserService = context.ServiceProvider.GetRequiredService<ICurrentUserService>();

            if (currentUserService != null)
            {
                var organizationsUserIds = await organizationUserLinkRepository.Query().Where(entity => entity.User == currentUserService.User.Entity).Select(x => x.Organization.Id).ToListAsync();
                return currentUserService.User.Entity.IsAdmin || organizationsUserIds.Contains(entity.Id);
            }
            return true;
        }

    }
}
