import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const masterDataService = new MasterDataService('ReportCategories', ['invariantId']);

export default masterDataService;
export const ReportCategoriesDataSourceOptions = DataSourceOptionsFactory.create(masterDataService);