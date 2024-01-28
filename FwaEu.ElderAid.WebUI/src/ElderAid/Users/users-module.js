import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import UsersPartConfigurator from '@/ElderAid/Users/Services/users-parts-configurator';
import UserSettingsPartConfigurator from '@/ElderAid/Users/Services/user-settings-parts-configurator';
import UserColumnCustomizerConfigurator from '@/ElderAid/Users/Services/user-columns-customizer-configurator';

export class ApplicationUsersModule extends AbstractModule {
	onInitAsync() {
		UsersPartConfigurator.configure();
		UserSettingsPartConfigurator.configure();
		UserColumnCustomizerConfigurator.configure();
	}
}