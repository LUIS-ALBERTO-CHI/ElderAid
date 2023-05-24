<template>
    <div class="page-home">
        <div class="vignette-list">
            <div class="vignette-item">
                <div @click="goToPatientPage" class="vignette-main-info">
                    <div class="vignette-icon-area" style="background-color: #94a595;">
                        <i class="fa-regular fa-user vignette-icon" />
                    </div>
                    <span class="vignette-text">130 patients</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon"/>
            </div>
            <div class="vignette-item">
                <div class="vignette-main-info">
                    <div class="vignette-icon-area" style="background-color: #bda6a0;">
                        <i class="fa-regular fa-cart-plus vignette-icon" />
                    </div>
                    <span class="vignette-text">Commandes</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon"/>
            </div>
            <div class="vignette-item">
                <div class="vignette-main-info">
                    <div class="vignette-icon-area" style="background-color: #d9c4b3;">
                        <i class="fa-regular fa-inbox vignette-icon" />
                    </div>
                    <span class="vignette-text">Stock pharmacie</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon"/>
            </div>
            <div class="vignette-item">
                <div class="vignette-main-info">
                    <div class="vignette-icon-area" style="background-color: #a5ae9d;">
                        <i class="fa-regular fa-truck vignette-icon" />
                    </div>
                    <span class="vignette-text">Livraison</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon"/>
            </div>
            <div class="vignette-item">
                <div class="vignette-main-info">
                    <div class="vignette-icon-area" style="background-color: #bb8a7c;">
                        <i class="fa-regular fa-gear vignette-icon" />
                    </div>
                    <span class="vignette-text">Mon profil</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon"/>
            </div>

        
        </div>
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