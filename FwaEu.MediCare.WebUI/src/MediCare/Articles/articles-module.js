import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import RecentArticlesMasterDataService from './Services/recent-articles-master-data-service';
import ArticlesTypeMasterDataService from './Services/articles-type-master-data-service';

export class ArticlesModule extends AbstractModule {

    async onInitAsync() {
        await RecentArticlesMasterDataService.configureAsync();
        await ArticlesTypeMasterDataService.configureAsync();
    }
}