<template>
	<div class="login-page">
		<img @click="goToHome" class="logo-login" alt="" src="../Content/elder-aid-logo.png" />
		<page-container type="form">
			<h3 class="login-box-content">{{ $t('welcomeMessage', { applicationName: applicationName }) }}</h3>
			<div class="login-box-container">
				<h3>{{ $t('title') }}</h3>
				<box class="login-box">
					<div v-for="(l, index) in loginComponents">
						<component :is="l.loginComponent" />
						<div class="block" v-if="userPartsComponents.length">

							<div :key="index"
								v-for="(c, index) in userPartsComponents.filter(upc => upc.authenticationHandlerKey == l.handlerKey)">
								<component :is="c.component" />
							</div>
						</div>
						<hr class="login-separator" v-if="index < loginComponents.length - 1" />
					</div>
				</box>
			</div>
		</page-container>
	</div>
</template>

<script>
import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
import { shallowRef } from "vue";
import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
import UserPartsRegistry from '@/Fwamework/Users/Services/users-parts-registry';

export default {
	data() {
		return {
			loginComponents: null,
			userPartsComponents: [],
			applicationName: Configuration.application.name,
		};
	},
	created: showLoadingPanel(async function () {

				await loadMessagesAsync(this, import.meta.glob('./Content/login-front-page-messages.*.json'));
		this.loginComponents = shallowRef(await AuthenticationService.createLoginComponentAsync());
		const userPartsComponents = [];
		await Promise.all(UserPartsRegistry.getAll()
			.filter(handler => typeof handler.createLoginComponentAsync === 'function')
			.map(async function (userParthandler) {
				const componentInfo = await userParthandler.createLoginComponentAsync();
				componentInfo.authenticationHandlerKey = userParthandler.authenticationHandlerKey;
				if (componentInfo) {
					userPartsComponents.push(componentInfo);
				}
			}));

		this.userPartsComponents = shallowRef(userPartsComponents);
	}),
	components: {
		Box,
		PageContainer
	}
};
</script>
<style type="text/css">
.login-page {
	width: 100%;
}

.login-page .box {
	max-width: 730px;
	margin: 0px auto;
	/*width: 100%;*/
}

.login-box-container {
	text-align: center;
}

.login-box {
	width: 45%;
}

.login-box-content {
	color: #7092be;
	font-weight: bold;
	font-size: 20px;
	text-align: center;
}

.login-separator {
	width: 300px;
	height: 5px;
	filter: blur(1px);
	margin: 15px auto;
}

.logo-login {
	display: flex;
	margin: 0 auto;
	height: 100px;
	width: auto;
}
</style>