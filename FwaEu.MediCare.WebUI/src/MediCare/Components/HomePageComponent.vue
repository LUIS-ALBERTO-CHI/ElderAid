<template>
    <div class="page-home">
        <div class="flex-section justify-content-center" v-if="isSingleOrganization">
            <span class="organization-text">Organisation 1</span>
        </div>
        <Dropdown v-else v-model="selectedOrganization" :options="OrganizationsOptions" @change="refreshMasterDataByDatabaseInvariantId" />
        <div class="vignette-list">
            <div class="vignette-item">
                <div @click="goToPatientPage" class="vignette-main-info">
                    <i class="fa-regular fa-user fa-fw vignette-icon" style="color: #94a595;" />
                    <span v-show="patientsActive.length > 0" class="vignette-text">{{patientsActive.length }} patients</span>
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

    import ViewContextService, { ViewContextModel } from '@/MediCare/ViewContext/Services/view-context-service';
    import UserOrganizationsMasterDataService from "@/MediCare/Organizations/Services/organizations-user-master-data-service";
    import OrganizationsMasterDataService from "@/MediCare/Organizations/Services/organizations-master-data-service";
    

    import MasterDataManagerService from "@/Fwamework/MasterData/Services/master-data-manager-service";

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
            const $this = this;
            return {
                isCurrentUserAuthenticated: false,
                selectedOrganization: 'Organisation 1',
                OrganizationsOptions: [],
                isSingleOrganization: false,
                patientsActive: [],
                currentDatabase: ViewContextService.get()?.id,
                viewContextChangeOff: ViewContextService.onChanged((viewContext) => {
                    $this.currentDatabase = viewContext.id;
                })
            };
        },
        async created() {
            this.isCurrentUserAuthenticated = await AuthenticationService.isAuthenticatedAsync();

            // NOTE : organizations contient la liste de toutes les organisations existantes dans l'entit� OrganizationEntity
            // userOrganizations contient la liste de toutes les organizations qu'il ont �t� affect� a l'utilisateur courant
            // Si l'utilisateur est admin on lui affiche la liste de toutes les organizations sinon on va lui afficher que ceux qu'il les appartient
            const userOrganizations = await UserOrganizationsMasterDataService.getAllAsync();
            const organizations = await OrganizationsMasterDataService.getAllAsync();

            ViewContextService.set(new ViewContextModel(organizations[0]));

            //NOTE: Loading data only when the currentdatabase invariantId is avlaible
            if (this.currentDatabase != null) {
                const patients = await PatientsMasterDataService.getAllAsync();
                console.log(patients[0]);
                this.patientsActive = patients.filter(x => x.isActive);
            }

            this.OrganizationsOptions = organizations.map(x => x.name);
            this.selectedOrganization = this.OrganizationsOptions[0];
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
                this.$router.push("/Orders")
            },
            async refreshMasterDataByDatabaseInvariantId(e) {
                // NOTE : Update the ViewContext to save the selected database
                const organizations = await OrganizationsMasterDataService.getAllAsync();
                ViewContextService.set(new ViewContextModel( organizations[1]));

                // NOTE : refraichir toutes les masterdata
                await MasterDataManagerService.clearCacheAsync();

                // NOTE: Rafra�chir les
                const patients = await PatientsMasterDataService.getAllAsync();
            }
        }
    }
</script>