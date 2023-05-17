import { defineAsyncComponent } from "vue";

export default {
	taskName: "ListTables",
	createComponent: () => defineAsyncComponent(() => import("@/Fwamework/SetupData/Components/ListTablesComponent.vue"))
}
