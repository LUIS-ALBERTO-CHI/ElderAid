import EnumMasterDataService from "@/Fwamework/EnumMasterData/Services/enum-master-data-service";
import dataSourceOptionsFactory from "@UILibrary/Modules/MasterData/Services/data-source-options-factory";

const incontinenceLevelMasterDataService = new EnumMasterDataService('FwaEu.ElderAid.Patients.IncontinenceLevel');

export default incontinenceLevelMasterDataService;
export const incontinenceLevelDataSourceOptions = dataSourceOptionsFactory.create(incontinenceLevelMasterDataService);