import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
import AbstractModule from '@/Fwamework/Core/Services/abstract-module-class';

export class HttpsRedirectionModule extends AbstractModule {
	onInitAsync() {
		const environmentMode = import.meta.env.MODE;
		const isHttpsRedirection = Configuration.application.forceHttpsRedirection;
		if (environmentMode !== 'development' && isHttpsRedirection && location.protocol !== 'https:') {
			location.replace(`https:${location.href.substring(location.protocol.length)}`);
		}
	}
}