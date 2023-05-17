import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import { CurrentUserMenuType } from "@/Fwamework/CurrentUserMenu/Services/current-user-menu-service";

export class UserSettingsModule extends AbstractModule {
	
	async getMenuItemsAsync(menuType) {
		const menuItems = [];

		if (menuType === CurrentUserMenuType) {
			menuItems.push({
				textKey: "userSettingsMenuItemText",
				icon: "fad fa-user-cog",
				path: { name: "UserSettings" }
			});
		}
		return menuItems;
	}
}