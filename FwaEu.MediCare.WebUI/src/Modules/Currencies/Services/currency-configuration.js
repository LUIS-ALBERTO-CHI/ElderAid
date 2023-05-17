import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateCurrencies } from '@/Modules/Currencies/currencies-permissions';

class CurrencyConfiguration extends AbstractConfiguration {
	constructor() {
		super();

		this.columnsCustomizer.addCustomization('currencyCode', { index: 21, width: 120 });
		this.columnsCustomizer.addCustomization('isInverse', { index: 2000, width: 100 });
	}

	getResources(locale) {
		return [import(`@/Modules/Currencies/Content/currency-common.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['currencies']);
	}
}

export default {
	configurationKey: 'CurrencyEntity',
	icon: "fas fa-sack",
	getConfiguration: function () {
		return new CurrencyConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateCurrencies);
	}
};