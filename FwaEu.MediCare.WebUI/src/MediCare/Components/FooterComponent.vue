<template>
	<footer class="footer">
		<div>
			<div class="logout-container">
				<a v-if="!showLogoutLink" @click="goToLoginFront">Se connecter</a>
				<a v-else @click="logoutAsync">Se déconnecter</a>
			</div>

			{{ getVersionLabel() }}
			-
			Copyright © 2023, <a href="https://www.fwa.eu" target="_blank">FWA</a>
			<div class="logos">
				<a href="https://www.fwa.eu" target="_blank">
					<img class="logo" alt="" src="../Content/logo-fwa.png" />
				</a>
				<img class="logo" alt="" src="../Content/logo.png" />
			</div>
		</div>
	</footer>
</template>

<script>
	import ApplicationInfoService from '@/Fwamework/Core/Services/application-info-service';
    import { loadMessagesAsync } from '@/Fwamework/Culture/Services/single-file-component-localization';
    import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';

    export default {
        created: showLoadingPanel(async function () {
            await loadMessagesAsync(this, import.meta.glob('./Content/footer-messages.*.json'));
        }),
        methods: {
            getVersionLabel() {
                return this.$t("versionMask", [ApplicationInfoService.get()?.version]);
            }
        }
    }
</script>

<style type="text/css" src="../Content/footer.css"></style>
