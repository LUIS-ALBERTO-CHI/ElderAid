import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";

import CurrentUserPermissionsService from "@/Fwamework/Permissions/Services/current-user-permissions-service";
import PermissionsByIsAdminProviderService from "./Services/permissions-by-is-admin-provider-service";

export class PermissionsByIsAdminModule extends AbstractModule {

	onApplicationCreated() {
		CurrentUserPermissionsService.setPermissionsProvider(PermissionsByIsAdminProviderService);
	}
}