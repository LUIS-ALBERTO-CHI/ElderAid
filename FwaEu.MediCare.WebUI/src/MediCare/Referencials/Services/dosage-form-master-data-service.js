import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const masterDataService = new MasterDataService('DosageForms', ['id'], false);

export default masterDataService;

export const DosageFormDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "name", desc: false }]
});