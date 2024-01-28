import MenuService from '@/Fwamework/Menu/Services/menu-service';
import { I18n } from '@/Fwamework/Culture/Services/localization-service';

export const MenuType = 'administration';

class AdministrationMenuService extends MenuService {
	constructor() {
		super(MenuType);
	}

	localizeMenuItems(menuItems) {
		super.localizeMenuItems(menuItems);
		for (var menuItem of menuItems) {
			if (!menuItem.groupText) {
				menuItem.groupIndex = -1;
				menuItem.groupText = I18n.t("defaultGroupText");
			}
		}
	}
}

export default new AdministrationMenuService();