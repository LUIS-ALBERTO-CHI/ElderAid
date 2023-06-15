import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const masterDataService = new MasterDataService('Patients', ['id'], false);

export default masterDataService;

export const PatientDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "fullName", desc: false }]
});
