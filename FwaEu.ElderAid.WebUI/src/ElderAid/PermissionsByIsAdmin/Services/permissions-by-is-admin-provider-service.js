import PermissionsMasterDataService from "@/Fwamework/Permissions/Services/permissions-master-data-service";
import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";

export default {
	getCurrentUserPermissionsAsync: async function () {
		let currentUser = await CurrentUserService.getAsync();

		if (currentUser?.parts?.adminState?.isAdmin) {
			let permissions = await PermissionsMasterDataService.getAllAsync();
			return permissions.map(md => md.invariantId);
		}
		return [];
	}
}