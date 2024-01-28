import "@fontsource/nunito";
import "@fontsource/roboto";
import FooterComponent from "@/ElderAid/Components/FooterComponent.vue";
import CompanyLogoComponent from "@/ElderAid/Components/CompanyLogoComponent.vue";
import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import ApplicationHeaderService from './Services/application-header-service';
import ApplicationSearchService from './Services/application-search-service';
import CustomRouterNavigatorService from './Services/custom-router-navigator-service';
import OrganizationSelectionRedirectService from "./Services/organization-selection-redirect-service";
import IncontinenceLevelMasterDataService from '@/ElderAid/Patients/Services/incontinence-level-master-data-service';


export class ApplicationModule extends AbstractModule {

	async onInitAsync(vueApp) {
		vueApp.component("application-footer-component", FooterComponent);
		vueApp.component("company-logo-component", CompanyLogoComponent);
		await ApplicationHeaderService.configureAsync();
		await ApplicationSearchService.configureAsync();
		await CustomRouterNavigatorService.configureAsync();
		await OrganizationSelectionRedirectService.configureAsync();
		await IncontinenceLevelMasterDataService.configureAsync();
	}
}
