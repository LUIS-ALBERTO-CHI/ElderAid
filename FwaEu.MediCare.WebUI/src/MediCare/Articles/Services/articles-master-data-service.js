import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const masterDataService = new MasterDataService('Articles', ['id'], false);

export default masterDataService;

export const ArticleDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "title", desc: false }]
});
