import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import ArticlesMasterDataService from '../Referencials/Services/articles-master-data-service';

export class ArticlesModule extends AbstractModule {

    async onInitAsync() {
        await ArticlesMasterDataService.configureAsync();
    }
}