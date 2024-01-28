import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

const masterDataService = new MasterDataService('Protections', ['id'], false);

export default masterDataService;

export const ProtectionDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "dosageDescription", desc: false }]
});