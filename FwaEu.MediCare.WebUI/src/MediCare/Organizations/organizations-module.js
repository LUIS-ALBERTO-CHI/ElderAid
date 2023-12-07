import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import OrganizationsMasterDataService from "@/MediCare/Organizations/Services/organizations-master-data-service";
import UserOrganizationsMasterDataService from "@/MediCare/Organizations/Services/organizations-user-master-data-service";

import GenericAdminConfigurationService from '@/Modules/GenericAdmin/Services/generic-admin-configuration-service';
import OrganizationsConfiguration from '@/MediCare/Organizations/Services/organizations-configuration';

export class OrganizationsModule extends AbstractModule {
	async onInitAsync() {
		await OrganizationsMasterDataService.configureAsync();
		await UserOrganizationsMasterDataService.configureAsync();

		GenericAdminConfigurationService.register(OrganizationsConfiguration);		
	}
}
