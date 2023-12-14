import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

const masterDataService = new MasterDataService('RecentArticles', ['id'], false);

export default masterDataService;

export const ArticleDataSourceOptions = DataSourceOptionsFactory.create(masterDataService, {
	sort: [{ selector: "title", desc: false }]
});
