<template>
    <div class="page-home">
        <div class="flex-section justify-content-center" v-if="isSingleOrganization">
            <span class="organization-text">Organisation 1</span>
        </div>
        <Dropdown v-else v-model="selectedOrganization" :options="OrganizationsOptions" />
        <div class="vignette-list">
            <div class="vignette-item">
                <div @click="goToPatientPage" class="vignette-main-info">
                    <i class="fa-regular fa-user fa-fw vignette-icon" style="color: #94a595;" />
                    <span v-show="patients.length > 0" class="vignette-text">{{patients.length }} patients</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon" />
            </div>
            <div @click="goToOrdersPage" class="vignette-item">
                <div class="vignette-main-info">
                    <i class="fa-regular fa-cart-plus fa-fw vignette-icon" style="color: #bda6a0;" />
                    <span class="vignette-text">Commandes</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon" />
            </div>
            <div class="vignette-item">
                <div class="vignette-main-info">
                    <i class="fa-regular fa-inbox vignette-icon fa-fw" style="color: #d9c4b3;" />
                    <span class="vignette-text">Stock pharmacie</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon" />
            </div>
            <div class="vignette-item">
                <div class="vignette-main-info">
                    <i class="fa-regular fa-truck vignette-icon fa-fw" style="color: #a5ae9d;" />
                    <span class="vignette-text">Livraison</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon" />
            </div>
            <div @click="goToProfilPage" class="vignette-item">
                <div class="vignette-main-info">
                    <i class="fa-regular fa-gear vignette-icon fa-fw" style="color: #bb8a7c;" />
                    <span class="vignette-text">Mon profil</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon" />
            </div>
        </div>
        <footer>
            <application-footer-component />
        </footer>
    </div>
</template>
<script>
    import LocalizationMixing from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
    import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
    const path = Configuration.application.customResourcesPath;
    import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
    import BuildingsMasterDataService from "@/MediCare/Referencials/Services/buildings-master-data-service";
    import PatientsMasterDataService from "@/MediCare/Patients/Services/patients-master-data-service";
    import OrdersMasterDataService from "@/MediCare/Orders/Services/orders-master-data-service";
    import ArticlesMasterDataService from "@/MediCare/Referencials/Services/articles-master-data-service";
    import Dropdown from 'primevue/dropdown';




    export default {
        inject: ["deviceInfo"],
        mixins: [LocalizationMixing],
        components: {
            Dropdown
        },
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
                selectedOrganization: 'Organisation 1',
                OrganizationsOptions: ['Organisation 1', 'Organisation 2', 'Organisation 3'],
                isSingleOrganization: false,
                patients: [],
            };
        },
        async created() {
            this.isCurrentUserAuthenticated = await AuthenticationService.isAuthenticatedAsync();

            // NOTE : To be removed
            const buildings = await BuildingsMasterDataService.getAllAsync();
            const patients = await PatientsMasterDataService.getAllAsync();
            this.patients = patients;
            // const orders = await OrdersMasterDataService.getAllAsync();
            // const articles = await ArticlesMasterDataService.getAllAsync();
            // console.log(articles);
            // console.log(orders);
            // console.log(buildings);
            // console.log(patients);
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
                localStorage.removeItem("searchPatient")
            },
            goToProfilPage() {
                this.$router.push("/UserSettings")
            },
            goToOrdersPage() {
                // this.$router.push("/Orders")
            },
        },
    }
</script>