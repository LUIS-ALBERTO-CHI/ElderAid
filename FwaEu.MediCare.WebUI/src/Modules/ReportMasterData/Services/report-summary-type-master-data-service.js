import EnumMasterDataService from '@/Fwamework/EnumMasterData/Services/enum-master-data-service';
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const ReportSummaryTypeMasterDataService = new EnumMasterDataService('FwaEu.Modules.ReportsMasterDataByEntities.ReportSummaryType');

export default ReportSummaryTypeMasterDataService;
export const ReportSummaryTypeDataSourceOptions = DataSourceOptionsFactory.create(ReportSummaryTypeMasterDataService);
