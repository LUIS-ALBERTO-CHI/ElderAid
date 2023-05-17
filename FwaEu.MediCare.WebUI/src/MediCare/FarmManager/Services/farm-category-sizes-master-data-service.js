import EnumMasterDataService from '@/Fwamework/EnumMasterData/Services/enum-master-data-service';
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const FarmCategorySizeMasterDataService = new EnumMasterDataService('FwaEu.MediCare.FarmManager.FarmCategorySize');

export default FarmCategorySizeMasterDataService;
export const FarmCategorySizeDataSourceOptions = DataSourceOptionsFactory.create(FarmCategorySizeMasterDataService);
