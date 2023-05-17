<template>
	<footer class="footer">
		{{ getVersionLabel() }}
		-
		Copyright © 2023, <a href="https://www.fwa.eu" target="_blank">FWA</a>
		<div class="logos">
			<a href="https://www.fwa.eu" target="_blank">
				<img class="logo" alt="" src="../Content/logo-fwa.png" />
			</a>
			<img class="logo" alt="" src="../Content/logo-company.png" />
		</div>
	</footer>
</template>

<script>
	import ApplicationInfoService from '@/Fwamework/Core/Services/application-info-service';
    import { loadMessagesAsync } from '@/Fwamework/Culture/Services/single-file-component-localization';
    import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';

    export default {
        created: showLoadingPanel(async function () {
            await loadMessagesAsync(this, (locale) => import(`./Content/footer-messages.${locale}.json`));
        }),
        methods: {
            getVersionLabel() {
                return this.$t("versionMask", [ApplicationInfoService.get()?.version]);
            }
        }
    }
</script>

<style type="text/css" src="../Content/footer.css"></style>
