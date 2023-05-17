import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import LocalizationService from "@/Fwamework/Culture/Services/localization-service";
import DotNetTypeConverterService from "@/Fwamework/DotNetTypeConversion/Services/dot-net-type-converter-service";
import DotNetTypesToDevExtremeConverterService from '@/Fwamework/DevExtreme/Services/dot-net-types-to-devextreme-converter-service.js';
import GenericAdminLocalizableStringDictionaryConverter from "@/Modules/GenericAdmin/Services/generic-admin-localizable-string-dictionary-converter";
import GenericAdminLocalizableStringDictionaryToDevExtremeConverter from '@/Modules/GenericAdmin/Services/generic-admin-localizable-string-dictionary-to-devextreme-converter';
import ColumnsCustomizerService from "@/Modules/GenericAdmin/Services/columns-customizer-service";
import MasterDataColumnCustomizer from "@/Modules/GenericAdminMasterData/Services/master-data-column-customizer";
import LocalizableColumnCustomizer from "@/Modules/GenericAdmin/Services/localizable-string-column-customizer";
import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';

export class GenericAdminModule extends AbstractModule {
	async onInitAsync() {
		DotNetTypeConverterService.register(GenericAdminLocalizableStringDictionaryConverter);
		DotNetTypesToDevExtremeConverterService.register(GenericAdminLocalizableStringDictionaryToDevExtremeConverter);
		ColumnsCustomizerService.registerCustomCustomizer(new MasterDataColumnCustomizer());
		ColumnsCustomizerService.registerCustomCustomizer(new LocalizableColumnCustomizer());
	}

	async getMenuItemsAsync(menuType) {
		let menuItems = [];

		if (menuType === "administration") {
			const currentLocale = LocalizationService.getCurrentLanguage();
			const genericAdminConfigurations = GenericAdminConfigurationService.getAll();

			for (let genericAdminConfiguration of genericAdminConfigurations) { //NOTE: Adapt code with issue https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/4602
				if (typeof genericAdminConfiguration.isAccessibleAsync === 'undefined'
					|| typeof genericAdminConfiguration.isAccessibleAsync === 'function'
					&& await genericAdminConfiguration.isAccessibleAsync()) {
					const configuration = genericAdminConfiguration.getConfiguration();
					const resourcesManager = await configuration.getOrInitResourcesManagerAsync(currentLocale);

					menuItems.push({
						groupIndex: configuration.getGroupIndex(),
						groupText: configuration.getGroupText(resourcesManager),
						visibleIndex: configuration.getVisibleIndex(),
						text: configuration.getExportFileName(resourcesManager),
						path: { name: 'GenericAdmin', params: { configurationKey: genericAdminConfiguration.configurationKey } },
						icon: typeof genericAdminConfiguration.icon !== 'undefined' ? genericAdminConfiguration.icon : 'smalliconslayout ',
						descriptionText: configuration.getDescription(resourcesManager),
					});
				}
			}
		}
		return menuItems;
	}
}