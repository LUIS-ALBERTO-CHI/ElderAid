import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import { I18n } from "@/Fwamework/Culture/Services/localization-service";
import { CanAdministrateReports } from "@/Modules/ReportAdmin/report-admin-permissions";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";

export class ReportAdminModule extends AbstractModule {
	onInitAsync() {
	}

	async getMenuItemsAsync(menuType) {
		let menuItems = [];

		if (menuType === "administration" && await hasPermissionAsync(CanAdministrateReports)) {
			menuItems.push({
				textKey: "reportAdminMenuItemText",
				path: { name: "ReportAdmin" },
				icon: "fas fa-tools",
				descriptionKey: "reportAdminMenuItemDescription",
				groupText: I18n.t('reports'),
			});
		}
		return menuItems;
	}
}
