import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import ArticlesMasterDataService from './Services/articles-master-data-service';
import ArticlesTypeMasterDataService from './Services/articles-type-master-data-service';

export class ArticlesModule extends AbstractModule {

    async onInitAsync() {
        await ArticlesMasterDataService.configureAsync();
        await ArticlesTypeMasterDataService.configureAsync();
    }
}