<template>
	<footer class="footer">
		<div>
			<div class="logout-container">
				<a v-if="!showLogoutLink" @click="goToLoginFront">Conectarse</a>
				<a v-else @click="logoutAsync">Desconectarse</a>
			</div>

			<div class="logos">
				<img class="logo" alt="" src="../Content/logo2.png" />
			</div>
		</div>
	</footer>
</template>

<script>
	import ApplicationInfoService from '@/Fwamework/Core/Services/application-info-service';
	import { loadMessagesAsync } from '@/Fwamework/Culture/Services/single-file-component-localization';
	import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';
	import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
	import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";
	import UserFormatterService from "@/Fwamework/Users/Services/user-formatter-service";
	import UserService from '@/Fwamework/Users/Services/user-service';
	import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';

	export default {
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('./Content/footer-messages.*.json'));
			const $this = this;
			CurrentUserService.getAsync().then(async user => {

				$this.showLogoutLink = !!user;

			});
			await this.loadUser();
		}),
		data() {
			return {
				administrationLink: `${Configuration.application.publicUrl}admin.html`,
				showAdministrationLink: false,
				showLogoutLink: false,
				userName: null,
			}
		},
		watch: {
			async $route() {
				await this.loadUser();
			}
		},

		methods: {
			async loadUser() {
				let userId = this.$route.query?.customerId;
				if (userId) {
					const user = await UserService.getUserByIdAsync(userId);
					this.userName = UserFormatterService.getUserFullName(user);
				} else {
					this.userName = null;
				}
			},

			getVersionLabel() {
				return this.$t("versionMask", [ApplicationInfoService.get()?.version]);
			},
			goToLoginFront() {
				this.$router.push("/Login")
			},
			async logoutAsync() {
				AuthenticationService.logoutAsync().then(() => {
					this.$router.push("/Login")
				});
			},
		}
	}
</script>

<style type="text/css" src="../Content/footer.css"></style>
