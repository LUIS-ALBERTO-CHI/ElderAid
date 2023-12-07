import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';

class OrganizationConfiguration extends AbstractConfiguration {
	constructor() {
		super();
	}

	getResources(locale) {
		return [import(`@/MediCare/Organizations/Content/organizations-global-messages.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['organizationTitle']);
	}

	getGroupText(resourcesManager) {
		return resourcesManager.getResource(['organizationGroupTitle']);
	}
}

export default {
	configurationKey: 'OrganizationEntity',
	icon: "fa-duotone fa-sitemap",
	getConfiguration: function () {
		return new OrganizationConfiguration();
	},
	async isAccessibleAsync() {
		const authenticationService = (await import("@/Fwamework/Authentication/Services/authentication-service")).default;
		const currentUserService = (await import("@/Fwamework/Users/Services/current-user-service")).default;

		const isCurrentUserAuthenticated = await authenticationService.isAuthenticatedAsync();
		if (!isCurrentUserAuthenticated)
			return false;

		let currentUser = await currentUserService.getAsync();
		if (currentUser?.parts?.adminState?.isAdmin) {
			return true;
		}
		return false;
	}
};