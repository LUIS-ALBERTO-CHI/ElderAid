import EnumMasterDataService from "@/Fwamework/EnumMasterData/Services/enum-master-data-service";
import dataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const incontinenceLevelMasterDataService = new EnumMasterDataService('FwaEu.MediCare.Patients.IncontinenceLevel');

export default incontinenceLevelMasterDataService;
export const incontinenceLevelDataSourceOptions = dataSourceOptionsFactory.create(incontinenceLevelMasterDataService);