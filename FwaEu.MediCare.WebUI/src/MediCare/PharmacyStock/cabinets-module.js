import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import CabinetsMasterDataService from '@/MediCare/Referencials/Services/cabinets-master-data-service';

export class CabinetsModule extends AbstractModule {
    async onInitAsync() {
        await CabinetsMasterDataService.configureAsync();
    }
}