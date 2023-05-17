import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateRoles } from '@/Modules/Roles/permissions-by-role-permissions';

class RoleConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	getResources(locale) {
		return [import(`@/Modules/Roles/Content/roles-global-messages.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['roleTitle']);
	}

	getGroupText(resourcesManager) {
		return resourcesManager.getResource(['usersAndPermissions']);
	}
}

export default {
	configurationKey: 'RoleEntity',
	icon: "fas fa-user-tag",
	getConfiguration: function () {
		return new RoleConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateRoles);
	}
};