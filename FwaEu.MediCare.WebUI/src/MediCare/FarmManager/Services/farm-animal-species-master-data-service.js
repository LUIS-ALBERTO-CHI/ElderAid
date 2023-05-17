import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import DataSourceOptionsFactory from "@/Modules/MasterDataDevExtreme/Services/data-source-options-factory";

const masterDataService = new MasterDataService('FarmAnimalSpecies', ['id']);

export default masterDataService;
export const FarmAnimalSpeciesDataSourceOptions = DataSourceOptionsFactory.create(masterDataService);
