import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { CanAdministrateCurrencies } from '@/Modules/Currencies/currencies-permissions';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';

class ExchangeRateConfiguration extends AbstractConfiguration {
	constructor() {
		super();

		this.columnsCustomizer.addCustomization('baseCurrencyCode', { index: 20, width: 120 });
		this.columnsCustomizer.addCustomization('quotedCurrencyCode', { index: 21, width: 150 });

		this.columnsCustomizer.addCustomization('date', { index: 30, width: 130 });
		this.columnsCustomizer.addCustomization('value', { index: 31 });
		this.columnsCustomizer.addCustomization('isInverse', { index: 2000, width: 100 });
	}

	getResources(locale) {
		return [import(`@/Modules/Currencies/Content/exchange-rate-common.${locale}.json`)];
	}

	getPageTitle(resourcesManager) {
		return resourcesManager.getResource(['exchangeRates']);
	}

	getDescription(resourcesManager) {
		return resourcesManager.getResource(['exchangeRatesDescription']);
	}
}

export default {
	configurationKey: 'ExchangeRateEntity',
	icon: "fas fa-exchange-alt",
	getConfiguration: function () {
		return new ExchangeRateConfiguration();
	},
	async isAccessibleAsync() {
		return await hasPermissionAsync(CanAdministrateCurrencies);
	}
};