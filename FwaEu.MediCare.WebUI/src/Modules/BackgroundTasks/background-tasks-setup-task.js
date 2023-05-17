import { defineAsyncComponent } from "vue";

export default {
	taskName: "BackgroundTasks",
	createComponent: () => defineAsyncComponent(() => import("@/Modules/BackgroundTasks/Components/BackgroundTasksSetupTaskResultComponent.vue"))
}