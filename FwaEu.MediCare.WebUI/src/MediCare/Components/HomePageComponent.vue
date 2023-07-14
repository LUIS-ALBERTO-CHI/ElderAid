<template>
    <div class="page-home">
        <div class="flex-section justify-content-center" v-if="isSingleOrganization">
            <span class="organization-text">{{ this.organizations[0].name }}</span>
        </div>
        <Dropdown v-else v-model="selectedOrganization" :options="organizationsOptions"
            @change="refreshMasterDataByDatabaseInvariantId" optionLabel="name" />
        <div v-if="this.patientsActive.length > 0" class="vignette-list">
            <div class="vignette-item">
                <div @click="goToPatientPage" class="vignette-main-info">
                    <i class="fa-regular fa-user fa-fw vignette-icon" style="color: #94a595;" />
                    <span v-show="patientsActive.length > 0" class="vignette-text">{{ patientsActive.length }}
                        patients</span>
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
            <div @click="goToCabinetsPage" class="vignette-item">
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
import Dropdown from 'primevue/dropdown';
import ViewContextService, { ViewContextModel } from '@/MediCare/ViewContext/Services/view-context-service';
import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";
import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';

import PatientsMasterDataService from "@/MediCare/Patients/Services/patients-master-data-service";
import ArticlesMasterDataService from "@/MediCare/Referencials/Services/articles-master-data-service";
import OrganizationsMasterDataService from "@/MediCare/Organizations/Services/organizations-master-data-service";
import OrdersMasterDataService from "@/MediCare/Orders/Services/orders-master-data-service";
import BuildingsMasterDataService from "@/MediCare/Referencials/Services/buildings-master-data-service";
import UserOrganizationsMasterDataService from "@/MediCare/Organizations/Services/organizations-user-master-data-service";
import CabinetsMasterDataService from "@/MediCare/Referencials/Services/cabinets-master-data-service";
import DosageFormMasterDataService from "@/MediCare/Referencials/Services/dosage-form-master-data-service";
import ProtectionsMasterDataService from "@/MediCare/Referencials/Services/protections-master-data-service";
import TreatmentsMasterDataService from "@/MediCare/Referencials/Services/treatments-master-data-service";
import StockConsumptionMasterDataService from "@/MediCare/StockConsumption/Services/stock-consumption-master-data-service";
import ArticlesTypeMasterDataService from "@/MediCare/Referencials/Services/articles-type-master-data-service";

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
            selectedOrganization: null,
            organizationsOptions: [],
            isSingleOrganization: false,
            patientsActive: [],
            currentDatabase: ViewContextService.get()?.id,
            viewContextChangeOff: ViewContextService.onChanged((viewContext) => {
                $this.currentDatabase = viewContext.id;
            }),
            isUserAdmin: false,
            organizations: [],
            organizationsLink: [],
            startLoadTime: 0

        };
    },
    async created() {
        this.isCurrentUserAuthenticated = await AuthenticationService.isAuthenticatedAsync();
        const currentUser = await CurrentUserService.getAsync();
        this.isUserAdmin = currentUser.parts.adminState.isAdmin;

        this.organizations = await OrganizationsMasterDataService.getAllAsync();

        if (this.organizations.length == 1) {
            this.isSingleOrganization = true;
        } else {
            this.organizationsOptions = this.organizations
            this.selectedOrganization = this.organizationsOptions[0];
            ViewContextService.set(new ViewContextModel(this.organizations[0]));
        }
        await this.loadAllMasterDataAsync(false);
    },
    methods: {
        loadAllMasterDataAsync: showLoadingPanel(async function (onlyEms) {

            this.startLoadTime = new Date().getTime();

            //NOTE: Loading data only when the currentdatabase invariantId is avlaible
            if (this.currentDatabase != null) {
                const patients = PatientsMasterDataService.getAllAsync();
                const articles = ArticlesMasterDataService.getAllAsync();
                const orders = OrdersMasterDataService.getAllAsync();
                const buildings = BuildingsMasterDataService.getAllAsync();
                const userOrganizations = UserOrganizationsMasterDataService.getAllAsync();
                const cabinets = CabinetsMasterDataService.getAllAsync();
                const protections = ProtectionsMasterDataService.getAllAsync();
                const treatments = TreatmentsMasterDataService.getAllAsync();
                const stockConsuptions = StockConsumptionMasterDataService.getAllAsync();
                this.patientsActive = patients.filter(x => x.isActive);
            }

            if (!onlyEms) {
                const dosageForms = DosageFormMasterDataService.getAllAsync();
                const articlesType = ArticlesTypeMasterDataService.getAllAsync();
            }

            const loadingTime = new Date().getTime() - this.startLoadTime;
            if (loadingTime < 5000) {
                await new Promise(resolve => setTimeout(resolve, 5000 - loadingTime));
            }
        }),
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
        goToCabinetsPage() {
            this.$router.push("/stockPharmacy")
        },
        async refreshMasterDataByDatabaseInvariantId(e) {

            // NOTE : Update the ViewContext to save the selected database
            // const organizations = await OrganizationsMasterDataService.getAllAsync();
            ViewContextService.set(new ViewContextModel(e.value));

            // NOTE : refraichir toutes les masterdata
            await MasterDataManagerService.clearCacheAsync();

            await this.loadAllMasterDataAsync(true);
        }
    }
}
</script>