import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';
import CachePreloaderService from '@/MediCare/Cache/Services/cache-preloader-service';
import Router from '@/Fwamework/Routing/Services/vue-router-service';

export class CachePreloaderModule extends AbstractModule {
    async onInitAsync() {
        Router.beforeResolve(async (to, from, next) => {
            
            const authenticationService = (await import('@/Fwamework/Authentication/Services/authentication-service')).default;
            if (await authenticationService.isAuthenticatedAsync()) {

                await CachePreloaderService.loadAllMasterDataAsync();
            }
            next();
        });
    }
}