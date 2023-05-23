<template>
    <div class="page-home">
       
    </div>
</template>
<script>
    import LocalizationMixing from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';

    import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
    const path = Configuration.application.customResourcesPath;
    import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';

    export default {
        inject: ["deviceInfo"],
        mixins: [LocalizationMixing],
        i18n: {
            messages: {
                getMessagesAsync(locale) {
                    return import(`@/MediCare/Components/Content/home-page-messages.${locale}.json`);
                }
            }
        },
        data() {
            return {
                isCurrentUserAuthenticated: false,
            };
        },
        async created() {
            this.isCurrentUserAuthenticated = await AuthenticationService.isAuthenticatedAsync();
        },
        methods: {
            goToLoginFront() {
                this.$router.push("/Login")
            },
            async logoutAsync() {
                AuthenticationService.logoutAsync().then(() => {
                    this.$router.push("/Login")
                });
            },
            goToPatientPage() {
                this.$router.push("/SearchPatient")
            }
        },
    }
</script>