import { defineAsyncComponent } from "vue";

export default {
	taskName: "ServerApplicationManagement",
	createComponent: () => defineAsyncComponent(() => import("@/Modules/SetupServerManagement/Components/ApplicationManagementComponent.vue"))
}
