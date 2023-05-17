import HttpService from '@/Fwamework/Core/Services/http-service';
import Router from '@/Fwamework/Routing/Services/vue-router-service';


let authenticationServiceLoader = null;
function getAuthenticationServiceAsync() {
	if (!authenticationServiceLoader) {
		authenticationServiceLoader = new Promise(function (resolve) {
			import('@/Fwamework/Authentication/Services/authentication-service').then(response => {
				resolve(response.default);
			});
		});
	}
	return authenticationServiceLoader;
}

export default {
	configure() {
		this.configureHttpRequest();
		this.configureHttpResponse();
	},


	configureHttpRequest() {
		HttpService.interceptors.request.use(async config => {
			const authenticationService = await getAuthenticationServiceAsync();
			const token = await authenticationService.getCurrentTokenAsync();
			config.headers.common.Authorization = `Bearer ${token}`;
			return config;
		});
	},

	configureHttpResponse() {
		HttpService.interceptors.response.use(response => response, async (error) => {
			
			if (error.response && error.response?.status === 401) {
				if (error.response.data === 'UnknownUser') {

					error.isHandled = true;
					const i18n = (await import('@/Fwamework/Culture/Services/localization-service')).I18n;
					const notificationService = (await import('@/Fwamework/Notifications/Services/notification-service')).default;
					notificationService.showError(i18n.t("unknownUser"));
				}
				if (!Router.currentRoute.value.meta.allowAnonymous) {

					error.isHandled = true;
					const authenticationService = await getAuthenticationServiceAsync();
					await authenticationService.logoutAsync();
				}
			}
			return Promise.reject(error);
		});
	}

}