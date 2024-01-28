import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { CanAdministrateRoles } from '@/Modules/Roles/permissions-by-role-permissions';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';

class RolePermissionConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	getResources(locale) {
		return [import(`@/Modules/Roles/Content/role-permission-common.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['rolePermissionLinksTitle']);
	}

	getDescription(resourcesManager) {
		return resourcesManager.getResource(['rolePermissionLinksDescription']);
	}

	getGroupText(resourcesManager) {
		return resourcesManager.getResource(['usersAndPermissions']);
	}
}

export default {
	configurationKey: 'RolePermissionEntity',
	icon: "fas fa-users-cog",
	getConfiguration: function () {
		return new RolePermissionConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateRoles);
	}
};