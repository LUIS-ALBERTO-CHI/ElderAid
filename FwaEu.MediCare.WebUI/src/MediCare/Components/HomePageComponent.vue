<template>
    <div class="page-home">
        <div class="flex-section justify-content-center" v-if="isSingleOrganization">
            <span class="organization-text" v-if="organizations.length > 0">{{ organizations[0].name }}</span>
            <span class="organization-text" v-else>Vous n'êtes affecté à aucun EMS (base de données)</span>
        </div>
        <div v-else class="change-organization-container">
            <span @click="goToOrganizationSelectionPage">{{ organization?.name }}</span>
            <i class="fa-solid fa-pen-to-square change-organization-icon "></i>
        </div>
        <div v-if="this.patientsActive.length > 0" class="vignette-list">
            <div class="vignette-item">
                <div @click="goToPatientPage" class="vignette-main-info">
                    <i class="fa-regular fa-user fa-fw vignette-icon" style="color: #94a595;" />
                    <span v-show="patientsActive.length > 0" class="vignette-text">
                        {{ patientsActive.length }}
                        patients
                    </span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon" />
            </div>
            <div @click="goToOrdersPage" class="vignette-item">
                <div class="vignette-main-info">
                    <i class="fa-regular fa-cart-plus fa-fw vignette-icon" style="color: #bda6a0;" />
                    <span class="vignette-text">Commandes unitaires</span>
                </div>
                <i class="fa-regular fa-angle-right chevron-icon" />
            </div>
            <div @click="goToPeriodicPage" class="vignette-item">
                <div class="vignette-main-info">
                    <i class="fa-regular fa-repeat vignette-icon fa-fw" style="color: #d8b291;" />
                    <div style="display: flex; flex-direction: column;">
                        <span class="vignette-text">Commandes périodiques</span>
                        <span class="vignette-text-subtitle">{{ getNumberOfPatientToValidate() }}</span>
                    </div>
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

import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';

import OrganizationsMasterDataService from "@/MediCare/Organizations/Services/organizations-master-data-service";
import CabinetsMasterDataService from "@/MediCare/Referencials/Services/cabinets-master-data-service";

import PatientsMasterDataService from '@/MediCare/Patients/Services/patients-master-data-service';
import ViewContextService from "@/MediCare/ViewContext/Services/view-context-service";

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
            selectedOrganization: null,
            isSingleOrganization: false,
            patientsActive: [],
            organizations: [],
            startLoadTime: 0,
            cabinets: [],
            organization: null,
        };
    },
    created: showLoadingPanel(async function () {
        const patients = await PatientsMasterDataService.getAllAsync();
        this.organizations = await OrganizationsMasterDataService.getAllAsync();

        this.cabinets = await CabinetsMasterDataService.getAllAsync();
        if (this.organizations.length <= 1) {
            this.isSingleOrganization = true;
        }
        this.organization = ViewContextService.get();
        this.patientsActive = patients.filter(x => x.isActive);
    }),
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
        goToCabinetsPage() {
            if (this.cabinets.length == 1) {
                this.$router.push("/Cabinet/" + this.cabinets[0].id);
            } else {
                this.$router.push("/stockPharmacy")
            }
        },
        goToPeriodicPage() {
            this.$router.push("/PeriodicOrders")
        },
        goToOrganizationSelectionPage() {
            this.$router.push("/OrganizationSelection")
        },
        getNumberOfPatientToValidate() {
            const patientsToValidate = this.patientsActive.filter(patient => patient.incontinenceLevel != 0).length;
            return `${patientsToValidate} ${patientsToValidate > 1 ? 'patients' : 'patient'} à valider`
        }
    }
}
</script>