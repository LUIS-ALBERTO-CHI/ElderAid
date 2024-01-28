import EnumMasterDataService from "@/Fwamework/EnumMasterData/Services/enum-master-data-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";
const ArticlesTypeMasterDataService = new EnumMasterDataService('FwaEu.ElderAid.Articles.ArticleType');

export default ArticlesTypeMasterDataService;
export const articlesTypeDataSourceOptions = DataSourceOptionsFactory.create(ArticlesTypeMasterDataService);