using Microsoft.Extensions.DependencyInjection;
using FwaEu.Modules.MasterData;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework;
using FwaEu.MediCare.Articles.Services;
using FwaEu.MediCare.Articles.MasterData;
using FwaEu.MediCare.Orders;

namespace FwaEu.MediCare.Articles
{
    public static class ArticlesExtensions
    {
        public static IServiceCollection AddApplicationArticles(this IServiceCollection services, ApplicationInitializationContext context)
        {
            var repositoryRegister = context.ServiceStore.Get<IRepositoryRegister>();

            repositoryRegister.Add<ArticleEntityRepository>();
            services.AddTransient<IArticleService, ArticleService>();

            services.AddMasterDataProvider<RecentArticlesMasterDataProvider>("RecentArticles");
            var articleImagesSection = context.Configuration.GetSection("Application:Articles");
            services.Configure<ArticleImagesOptions>(articleImagesSection);
            return services;
        }
    }
}