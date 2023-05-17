import { defineAsyncComponent } from "vue";

export default {
	taskName: "ImportFileList",
	createComponent: () => defineAsyncComponent(() => import("@/Fwamework/SetupImport/Components/ImportFilesComponent.vue"))
}
