import EnumMasterDataService from "@/Fwamework/EnumMasterData/Services/enum-master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const articlesTypeMasterDataService = new EnumMasterDataService('FwaEu.MediCare.Articles.ArticleType');

export default articlesTypeMasterDataService;
export const articlesTypeDataSourceOptions = DataSourceOptionsFactory.create(articlesTypeMasterDataService);