import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import OnlineService from '@/fwamework/OnlineStatus/Services/online-service';

export class OnlineStatusModule extends AbstractModule {
	async onInitAsync() {
		OnlineService.initialize();
	}
}