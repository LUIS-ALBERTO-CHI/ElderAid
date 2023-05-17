import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const masterDataService = new MasterDataService('FarmActivities', ['id']);

export default masterDataService;
export const FarmActivitiesDataSourceOptions = DataSourceOptionsFactory.create(masterDataService);
