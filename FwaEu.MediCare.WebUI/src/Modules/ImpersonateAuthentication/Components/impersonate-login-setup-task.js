import { defineAsyncComponent } from "vue";

export default {
	taskName: "Impersonate",
	createComponent: () => defineAsyncComponent(() => import("@/Modules/ImpersonateAuthentication/Components/ImpersonateLoginComponent.vue"))
}
