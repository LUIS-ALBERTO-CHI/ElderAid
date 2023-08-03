import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const masterDataService = new MasterDataService('ProtectionDosages', ['id'], false);

export default masterDataService;

export const ProtectionDosagesDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "name", desc: false }]
});