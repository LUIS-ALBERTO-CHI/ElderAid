import "@fontsource/nunito";
import "@fontsource/roboto";
import FooterComponent from "@/MediCare/Components/FooterComponent.vue";
import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import ApplicationHeaderService from './Services/application-header-service';
import ApplicationSearchService from './Services/application-search-service';

export class ApplicationModule extends AbstractModule {

	async onInitAsync(vueApp) {
		vueApp.component("application-footer-component", FooterComponent);
		await ApplicationHeaderService.configureAsync();
		await ApplicationSearchService.configureAsync();
	}
}
