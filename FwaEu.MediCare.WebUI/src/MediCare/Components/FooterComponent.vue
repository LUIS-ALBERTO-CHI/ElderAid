<template>
	<footer class="footer">
		<div>
			{{ getVersionLabel() }}
			-
			Copyright © 2021, <a href="https://www.fwa.eu" target="_blank">FWA</a>
			<span v-if="showAdministrationLink"> - <a :href="administrationLink">Administration</a></span>
			<div class="logos">
				<a href="https://www.fwa.eu" target="_blank">
					<img class="logo" alt="" src="../Content/logo-fwa.png" />
				</a>
				<img class="logo company-logo" alt="" src="../Content/logo-cites-educatives.svg" />
			</div>
		</div>
	</footer>
</template>

<script>
	import LocalizationMixing from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import ApplicationInfoService from '@/Fwamework/Core/Services/application-info-service';
	import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
	import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";
	import UserFormatterService from "@/Fwamework/Users/Services/user-formatter-service";
	import UserService from '@/Fwamework/Users/Services/user-service';

	export default {
		mixins: [LocalizationMixing],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`@/MediCare/Components/Content/footer-messages.${locale}.json`);
				}
			}
		},
		data() {
			return {
				administrationLink: `${Configuration.application.publicUrl}admin.html`,
				showAdministrationLink: false,
				showLogoutLink: false,
				userName: null,
				saleOrderLink: null
			}
		},
		async created() {
			const $this = this;
			CurrentUserService.getAsync().then(async user => {

				$this.showLogoutLink = !!user;

			});
			await this.loadUser();
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
			}
		}
	}
</script>

<style type="text/css" src="../Content/footer.css"></style>
