import { HeaderItem } from "@/Modules/Header/Services/header-item";
import HeaderService from "@/Modules/Header/Services/header-service";
import RouterService from "@/Fwamework/Routing/Services/vue-router-service";
import FarmManagerRoutes from "@/MediCare/FarmManager/routes";
import FarmRegionHeaderItemConfiguration from "./farm-region-header-configuration";

export default {
	async configureAsync() {
		const farmManagerRouteNames = FarmManagerRoutes.map(r => r.name);
		const isHeaderVisible = farmManagerRouteNames.includes(RouterService.currentRoute.value?.name);

		HeaderService.register(new HeaderItem(FarmRegionHeaderItemConfiguration, isHeaderVisible));
		RouterService.afterEach((to, from) => {
			const isHeaderVisible = farmManagerRouteNames.includes(RouterService.currentRoute.value?.name);
			HeaderService.setVisibility(FarmRegionHeaderItemConfiguration.key, isHeaderVisible);
		});
	}
}