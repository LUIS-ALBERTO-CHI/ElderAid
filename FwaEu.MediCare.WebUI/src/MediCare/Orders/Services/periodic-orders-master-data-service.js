import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const masterDataService = new MasterDataService('PeriodicOrderValidations', ['id'], false);

export default masterDataService;

export const PeriodicOrderDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "id", desc: false }]
});
