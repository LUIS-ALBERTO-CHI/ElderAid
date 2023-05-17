import { I18n } from "@/Fwamework/Culture/Services/localization-service"
import { defineAsyncComponent } from "vue";

export default {
	type: "pivot",
	createComponent: () => defineAsyncComponent(() => import("@/Modules/ReportDisplay/Components/ReportPivotDisplayComponent.vue")),
	getDescription() {
		return {
			label: I18n.t("pivotLabel"),
			icon: "fad fa-file-excel"
		};
	}
}

