import EnumMasterDataService from "@/Fwamework/EnumMasterData/Services/enum-master-data-service";
import dataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const articlesTypeMasterDataService = new EnumMasterDataService('Fwa.MediCare.Articles.ArticleType');

export default articlesTypeMasterDataService;
export const articlesTypeDataSourceOptions = dataSourceOptionsFactory.create(articlesTypeMasterDataService);