import { defineAsyncComponent } from 'vue';

export default {
	key: "FarmRegionHeaderItem",
	component: defineAsyncComponent(() => import('@/MediCare/FarmManager/Components/FarmRegionHeaderComponent.vue')),
	smallModeContentComponent: defineAsyncComponent(() => import('@/MediCare/FarmManager/Components/FarmRegionHeaderSmallModeContentComponent.vue')),

	async fetchDataAsync() {
		const regionsMasterData = (await import('@/MediCare/FarmManager/Services/Regions/farm-regions-master-data-service')).default;
		const data = {
			regions: await regionsMasterData.getAllAsync()
		};
		return data;
	}
}