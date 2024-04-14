import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import UserOrganizationMasterDataService from "@/ElderAid/Organizations/Services/organizations-master-data-service"

export class UserOrganizationsModule extends AbstractModule {
	async onInitAsync() {
		await UserOrganizationMasterDataService.configureAsync();
	}
}