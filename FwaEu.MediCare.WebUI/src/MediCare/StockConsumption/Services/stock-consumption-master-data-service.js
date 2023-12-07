import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

const masterDataService = new MasterDataService('StockConsumptions', ['id'], false);

export default masterDataService;

export const OrderDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "id", desc: false }]
});
