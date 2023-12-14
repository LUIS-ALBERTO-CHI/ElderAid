import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

const masterDataService = new MasterDataService('Cabinets', ['id'], false);

export default masterDataService;

export const CabinetsDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "name", desc: false }]
});
