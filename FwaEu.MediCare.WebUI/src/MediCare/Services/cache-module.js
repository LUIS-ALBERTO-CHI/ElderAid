import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import CachePreloaderService from '@/MediCare/Services/cache-preloader-service';

export class CachePreloaderModule extends AbstractModule {
    async onInitAsync() {
        await CachePreloaderService.loadAllMasterDataAsync();
    }
}