import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

class ReportFieldMasterDataService extends MasterDataService {
	constructor() {
		super('ReportFields', ['invariantId']);
	}
}

const masterDataService = new ReportFieldMasterDataService();

export default masterDataService;
export const ReportFieldsDataSourceOptions = DataSourceOptionsFactory.create(masterDataService);