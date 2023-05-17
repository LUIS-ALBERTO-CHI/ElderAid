import { I18n } from "@/Fwamework/Culture/Services/localization-service"
import { defineAsyncComponent } from "vue";

export default {
	type: "grid",
	createComponent: () => defineAsyncComponent(() => import("@/Modules/ReportDisplay/Components/ReportGridDisplayComponent.vue")),
	getDescription() {
		return {
			label: I18n.t("gridLabel"),
			icon: "fad fa-table"
		};
	},
	isDefault: true,
}

