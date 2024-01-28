using Microsoft.Extensions.DependencyInjection;
using FwaEu.Modules.MasterData;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework;
using FwaEu.ElderAid.Articles.Services;
using FwaEu.ElderAid.Articles.MasterData;
using FwaEu.ElderAid.Orders;

namespace FwaEu.ElderAid.Articles
{
    public static class ArticlesExtensions
    {
        public static IServiceCollection AddApplicationArticles(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();

            repositoryRegister.Add<ArticleEntityRepository>();
            services.AddTransient<IArticleService, ArticleService>();

            services.AddMasterDataProvider<RecentArticlesMasterDataProvider>("RecentArticles");
            return services;
        }
    }
}