import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateUserGroups } from '@/Modules/UserGroups/user-groups-permissions';

class UserGroupConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	getResources(locale) {
		return [import(`@/Modules/UserGroups/Content/user-groups-global-messages.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['userGroupsTitle']);
	}

	getGroupText(resourcesManager) {
		return resourcesManager.getResource(['usersAndPermissions']);
	}
}

export default {
	configurationKey: 'UserGroups',
	icon: "fas fa-users",
	getConfiguration: function () {
		return new UserGroupConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateUserGroups);
	}
};