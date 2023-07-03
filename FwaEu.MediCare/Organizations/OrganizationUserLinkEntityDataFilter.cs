using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Users;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Organizations
{
    public class OrganizationUserLinkEntityDataFilter : IRepositoryDataFilter<OrganizationUserLinkEntity, int>
    {
        public Expression<Func<OrganizationUserLinkEntity, bool>> Accept(RepositoryDataFilterContext<OrganizationUserLinkEntity, int> context)
        {
            //var organizationUserLinkRepository = context.ServiceProvider
            //   .GetRequiredService<IRepositoryFactory>()
            //   .Create<OrganizationUserLinkEntityRepository>(context.Session);
            //var currentUserService = context.ServiceProvider.GetRequiredService<ICurrentUserService>();

            //if (currentUserService != null && !currentUserService.User.Entity.IsAdmin)
            //{
            //    return organizationUserLink => organizationUserLinkRepository.Query().Where(entity => entity.User == currentUserService.User).Any(org => org == organizationUserLink);
            //}
            //else if (currentUserService != null && currentUserService.User.Entity.IsAdmin)
            //{
            //    return organizationUserLink => organizationUserLinkRepository.QueryNoPerimeter().Any(org => org == organizationUserLink);
            //}
            return null;
        }
        public async Task<bool> AcceptAsync(OrganizationUserLinkEntity entity, RepositoryDataFilterContext<OrganizationUserLinkEntity, int> context)
        {
            var organizationUserLinkRepository = context.ServiceProvider
               .GetRequiredService<IRepositoryFactory>()
               .Create<OrganizationUserLinkEntityRepository>(context.Session);
            var currentUserService = context.ServiceProvider.GetRequiredService<ICurrentUserService>();

            if (currentUserService != null && !currentUserService.User.Entity.IsAdmin)
            {
                var result = await organizationUserLinkRepository.Query()
                    .Where(e => e.User == currentUserService.User)
                    .AnyAsync(org => org == entity);

                return result;
            }
            else if (currentUserService != null && currentUserService.User.Entity.IsAdmin)
            {
                var result = await organizationUserLinkRepository.QueryNoPerimeter()
                    .AnyAsync(org => org == entity);

                return result;
            }

            return true;
        }

    }
}
