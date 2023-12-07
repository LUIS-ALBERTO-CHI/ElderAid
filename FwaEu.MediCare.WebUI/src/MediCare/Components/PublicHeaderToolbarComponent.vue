<template>
	<!-- <vue-use-online>
	</vue-use-online> -->
	<div v-show="isBreadcrumbsEnabled" class="header-container">
		<breadcrumbs />
		<img @click="goToHome" class="logo" alt="" src="../Content/logo.png" />
	</div>
</template>

<script>
import Breadcrumbs from "@/Fwamework/Breadcrumbs/Components/BreadcrumbsComponent.vue";

import VueUseOnline from "@/Fwamework/OnlineStatus/Components/OnlineStatusIndicator.vue";
import OnlineService from '@/Fwamework/OnlineStatus/Services/online-service';
import OfflineDataSynchronizationService from "@/MediCare/OfflineDataSynchronization/Services/indexed-db-service";

import OrdersService from '@/MediCare/Orders/Services/orders-service';

export default {
	components: {
		Breadcrumbs,
		VueUseOnline
	},
	data() {
		return {
			isOnline: OnlineService.isOnline()
		};
	},
	async created() {
	},
	async mounted() {
		OnlineService.onOnline(this.handleOnlineEventAsync);
		OnlineService.onOffline(this.handleOfflineEvent);
	},
	methods: {
		async handleOnlineEventAsync() {
			console.log('Internet is online. Notifying application...');
			const indixedDbService = new OfflineDataSynchronizationService('orders');
			const orders = await indixedDbService.readObjectStore();
			if (orders.length > 0)
				await OrdersService.saveAsync(orders).then(async (result) => {

					// Perform actions to notify your application about the online status
					const indixedDbService = new OfflineDataSynchronizationService('orders');
					await indixedDbService.clearObjectStore();

				});

		},
		handleOfflineEvent() {
			console.log('Internet is offline');
		},
		goToHome() {
			this.$router.push('/');
		}
	},
	computed: {
		isBreadcrumbsEnabled() {
			return this.$route.path !== '/Login';
		},
	},
	watch: {

		isOnline() {
		}

	}
}
</script>
<style>
.header-container {
	display: flex;
	flex-direction: row;
	justify-content: space-between;
	width: 100%;
}

.logo {
	width: auto;
	height: 50px;
}
</style>