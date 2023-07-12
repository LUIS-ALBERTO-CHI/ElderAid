import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import ArticlesMasterDataService from '../Referencials/Services/articles-master-data-service';
import articlesTypeDataSourceOptions from '../Referencials/Services/articles-type-master-data-service';

export class ArticlesModule extends AbstractModule {

    async onInitAsync() {
        await ArticlesMasterDataService.configureAsync();
        await articlesTypeDataSourceOptions.configureAsync();
    }
}