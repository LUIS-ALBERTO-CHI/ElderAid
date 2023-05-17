import { defineAsyncComponent } from "vue";

export default {
	taskName: "SmtpOptions",
	createComponent: () => defineAsyncComponent(() => import("@/Modules/SetupMail/Components/SmtpSetupTaskResultComponent.vue"))
}