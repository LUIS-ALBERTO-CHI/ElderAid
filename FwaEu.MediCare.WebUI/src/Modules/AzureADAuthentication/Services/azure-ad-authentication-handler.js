import { ClientAuthErrorMessage, InteractionRequiredAuthError, PublicClientApplication, BrowserAuthErrorMessage } from "@azure/msal-browser";
import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
import { AuthenticationHandler } from "@/Fwamework/Authentication/Services/authentication-service";

import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";
import { LocalStorage } from "@/Fwamework/Storage/Services/local-storage-store";
import { defineAsyncComponent } from "vue";


const AzureADActiveAccountKey = 'AzureADActiveAccount';
export const AuthenticationHandlerKey = 'AzureAd'

class AzureADAuthenticationHandler extends AuthenticationHandler {

	constructor() {
		super(AuthenticationHandlerKey);
		this.msalInstance = new PublicClientApplication(
			Configuration.azureADAuthentication.msalConfig
		);
		window.msalInstance = this.msalInstance;
	}

	createLoginComponentAsync = async function () {
		return defineAsyncComponent(() => import('@/Modules/AzureADAuthentication/Components/LoginFormComponent.vue'));
	}

	/**@type {PublicClientApplication} */
	msalInstance = null;


	async loginAsync() {
		let result = null;
		try {
			result = await this.msalInstance.loginPopup({ prompt: 'select_account' });
		} catch (ex) {
			if (ex.errorCode === BrowserAuthErrorMessage.interactionInProgress.code) {

				//HACK: Sometimes, force cache refresh if interaction in progress is set during login
				this.msalInstance.browserStorage.temporaryCacheStorage.windowStorage.clear();
				result = await this.msalInstance.loginPopup({ prompt: 'select_account' });
			} else {
				//NOTE: Microsoft authentication popup will handle all user errors, other promise rejections will be only notifications to the app about abort action
				return null;
			}
		}

		this.setActiveAccount(result.account);

		//Try to load current user in order to fully identify the logged on user
		const currentUser = await CurrentUserService.getAsync(true);
		if (!currentUser) {
			this.setActiveAccount(null);
			return null;
		}

		return { token: result.idToken };
	}

	async isAuthenticatedAsync() {
		return !!this.getActiveAccount();
	}

	async renewTokenAsync() {
		return { token: await this.getCurrentTokenAsync(true) };
	}

	async getCurrentTokenAsync(forceRefresh = false) {

		const account = this.getActiveAccount();
		if (!account)
			return null;
		try {
			let authenticationResult = await this.msalInstance.acquireTokenSilent({ account, forceRefresh: forceRefresh, scopes: ["profile"] });
			return authenticationResult.idToken;
		} catch (ex) {
			if (ex.errorCode === ClientAuthErrorMessage.tokenRefreshRequired.code && !forceRefresh) {
				return await this.getCurrentTokenAsync(true);
			} else if (ex instanceof InteractionRequiredAuthError) {
				await this.logoutAsync();
				return null;
			}
			throw ex;
		}
	}

	async logoutAsync() {
		const account = this.getActiveAccount();

		this.setActiveAccount(null);
		await this.msalInstance.logoutRedirect({ account, prompt: 'none' });
		return true;
	}

	/** @param {import('@azure/msal-common/dist').AccountInfo} accountInfo */
	setActiveAccount(accountInfo) {
		this.msalInstance.setActiveAccount(accountInfo);
		if (accountInfo)
			LocalStorage.setValue(AzureADActiveAccountKey, accountInfo.homeAccountId);
		else LocalStorage.removeValue(AzureADActiveAccountKey);
	}

	/** @returns {import('@azure/msal-common/dist').AccountInfo}  */
	getActiveAccount() {
		if (!this.msalInstance.getActiveAccount()) {

			const activeAccount = LocalStorage.getValue(AzureADActiveAccountKey);
			if (activeAccount) {
				const account = this.msalInstance.getAccountByHomeId(activeAccount);

				this.setActiveAccount(account);
				return account;
			}
			else return null;
		}

		return this.msalInstance.getActiveAccount();
	}
}

export default new AzureADAuthenticationHandler();