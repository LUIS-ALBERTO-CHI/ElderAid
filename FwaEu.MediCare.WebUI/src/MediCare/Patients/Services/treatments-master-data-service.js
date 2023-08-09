import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const masterDataService = new MasterDataService('Treatments', ['id'], false);

export default masterDataService;

export const ArticleDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "dosageDescription", desc: false }]
});