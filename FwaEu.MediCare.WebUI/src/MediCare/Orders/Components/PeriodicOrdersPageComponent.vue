<template>
    <div>
        <div class="orders-container">
            <span class="p-input-icon-right">
                <i @click="removeSearch" class="fa fa-solid fa-close"
                   :style="searchOrders.length == 0 ? 'opacity: 0.5;' : ''" />
                <InputText ref="searchInput" v-model="searchOrders" class="search-input"
                           placeholder="Rechercher une commande" />
            </span>
            <Dropdown v-show="buildings.length > 1" v-model="selectedBuilding" :options="buildingOptions" class="select-sector" />

            <Dropdown v-model="selectedOrderType" :options="ordersTypeOptions" class="select-sector" />
            <div v-if="periodicOrders"
                 class="periodic-orders-container">
                <div v-for="patient in filteredPatients" @click="goToPeriodicOrdersPage(patient.id)" :key="patient.id"
                     class="periodic-orders-item">
                    <div class="header">
                        <div class="patient-info">
                            <i class="fa-sharp fa-solid "
                               :class="isPeriodicOrderValidated(patient) ? 'fa-check check-icon' : 'fa-circle-exclamation alert-periodic-icon'" />
                            <span>{{ patient.fullName }}</span>
                        </div>
                        <div class="room-info">
                            <i class="fa-solid fa-bed" />
                            <span>{{ patient.roomName }}</span>
                        </div>
                    </div>
                    <div class="quantity-info">
                        <span v-if="isPeriodicOrderValidated(patient)">{{ getQuantityPeriodicOrderValidated(patient) }}</span>
                        <span v-else>{{ getQuantityPeriodicOrderNotValidated(patient) }}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import InputText from 'primevue/inputtext';
    import Dropdown from 'primevue/dropdown';

    import PeriodicOrdersMasterDataService from '../Services/periodic-orders-master-data-service';
    import PatientsMasterDataService from "@/MediCare/Patients/Services/patients-master-data-service";

    import BuildingsMasterDataService from "@/MediCare/Referencials/Services/buildings-master-data-service";
    import ProtectionMasterDataService from "@/MediCare/Patients/Services/protections-master-data-service"

    export default {
        components: {
            InputText,
            Dropdown,
        },
        data() {
            return {
                ordersTypeOptions: ["Tous les états des commandes", "Patients validés", "Patients à valider"],
                selectedOrderType: "Tous les états des commandes",
                periodicOrders: null,
                patients: [],
                searchOrders: "",
                filteredPatients: [],
                buildingOptions: ["Tous les secteurs"],
                selectedBuilding: "Tous les secteurs",
                buildings: [],
                protections: [],
            };
        },
        async created() {
            this.buildings = await BuildingsMasterDataService.getAllAsync();
            this.periodicOrders = await PeriodicOrdersMasterDataService.getAllAsync();
            this.protections = await ProtectionMasterDataService.getAllAsync();
            this.patients = await PatientsMasterDataService.getAllAsync().then((patients) => {
                return patients.filter(patient => patient.isActive = true)
            });
            this.buildings = await BuildingsMasterDataService.getAllAsync();
            this.buildingOptions = this.buildingOptions.concat(this.buildings);
            this.selectedBuilding = this.buildingOptions[0]
        },
        methods: {
            isPeriodicOrderValidated(patient) {
                return patient.incontinenceLevel == 'None'
            },
            removeSearch() {
                this.searchOrders = "";
                this.focusSearchBar();
            },
            focusSearchBar() {
                this.$nextTick(() => {
                    this.$refs.searchInput.$el.focus();
                });
            },
            goToPeriodicOrdersPage(patientId) {
                this.$router.push({
                    name: "PeriodicOrder",
                    params: { id: patientId },
                });
            },
            getQuantityPeriodicOrderNotValidated(patient) {
                const patientProtections = this.protections.filter(protection => protection.patientId === patient.id);
                if (patientProtections.length > 0)
                    return `${patientProtections.length} protection${patientProtections.length > 1 ? 's' : ''}`
                else
                    return "Aucune protection"
            },
            getQuantityPeriodicOrderValidated(patient) {
                const periodicOrdersOrdered = this.periodicOrders.filter(periodicOrder => periodicOrder.patientId === patient.id && periodicOrder.orederedOn != null);
                return `${periodicOrdersOrdered.length} protection${periodicOrdersOrdered.length > 1 ? 's' : ''}`
            }
        },
        computed: {
            filteredPatients() {
                return this.patients.filter(patient => {
                    return (
                        (patient.fullName.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                            patient.roomName.toLowerCase().includes(this.searchOrders.toLowerCase())) &&
                        (
                            this.selectedOrderType === "Tous les états des commandes" ||
                            (this.selectedOrderType === "Patients validés" && patient.incontinenceLevel == 'None') ||
                            (this.selectedOrderType === "Patients à valider" && patient.incontinenceLevel != 'None')
                        )
                        && (this.selectedBuilding === "Tous les secteurs" || patient.buildingId === this.selectedBuilding.id));
                });
            }

        }
    };
</script>
<style type="text/css" scoped src="./Content/orders-page.css"></style>