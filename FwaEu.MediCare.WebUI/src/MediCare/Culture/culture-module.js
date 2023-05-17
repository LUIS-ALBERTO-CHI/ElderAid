import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";

export class ApplicationCultureModule extends AbstractModule {
	async onInitAsync(vueConfig) {
		const localizationService = (await import('@/Fwamework/Culture/Services/localization-service')).default;
		const i18n = (await import('@/Fwamework/Culture/Services/localization-service')).I18n;
		await localizationService.configureAsync(vueConfig);
		i18n.locale = 'fr';//NOTE: Force the current locale to 'en' and don't use setCurrentLanguage in order to
	}
	getGlobalMessagesAsync(locale) {
		return [import(`@/MediCare/Users/Content/users-global-messages.${locale}.json`)];
	}
}

