import EnumMasterDataService from "@/Fwamework/EnumMasterData/Services/enum-master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const ArticlesTypeMasterDataService = new EnumMasterDataService('FwaEu.MediCare.Articles.ArticleType');

export default ArticlesTypeMasterDataService;
export const articlesTypeDataSourceOptions = DataSourceOptionsFactory.create(ArticlesTypeMasterDataService);