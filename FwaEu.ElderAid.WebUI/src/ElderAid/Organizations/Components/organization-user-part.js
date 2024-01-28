import OrganizationsMasterDataService from "@/ElderAid/Organizations/Services/organizations-master-data-service";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import { defineAsyncComponent } from "vue";

export default {
	partName: "organizations",
	component: defineAsyncComponent(() => import("./OrganizationUserPartComponent.vue")),

	initializeAsync: function (user, context) {
		let userPart = user.parts[this.partName];
		if (context.isNew) {
			userPart = createNewUserPart();
		}
		return [{
			showForAdmin: false,
			readOnly: context.currentUser.id === user.id,
			data: userPart,
			title: I18n.t('organizationTitle'),
			fetchDataAsync: async () => await OrganizationsMasterDataService.getAllAsync()
		}];
	},

	fillAsync: async function (user, component, context) {


		const hasPermission = user.id !== context.currentUser.id;
		if (hasPermission) {
			const currentUserService = (await import("@/Fwamework/Users/Services/current-user-service")).default;

			
			let currentUser = await currentUserService.getAsync();
			if (currentUser?.parts?.adminState?.isAdmin) {
				user.parts[this.partName] = { selectedIds: component.modelValue.data.selectedIds };
			}
		}
	}
}


function createNewUserPart() {
	return { selectedIds: [] };
}