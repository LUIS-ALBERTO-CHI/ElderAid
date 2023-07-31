<template>
    <div>
        <div class="orders-container">
            <span class="p-input-icon-right">
                <i @click="removeSearch" class="fa fa-solid fa-close"
                    :style="searchOrders.length == 0 ? 'opacity: 0.5;' : ''" />
                <InputText ref="searchInput" v-model="searchOrders" class="search-input"
                    placeholder="Rechercher une commande" />
            </span>
            <Dropdown v-model="selectedSector" :options="sectorsTypeOptions" class="select-sector" />
            <Dropdown v-model="selectedOrderType" :options="ordersTypeOptions" class="select-sector" />
            <div v-if="periodicOrders && selectedOrderType === 'Patients validés' || selectedOrderType === 'Toutes'" class="periodic-orders-container">
                <div @click="goToPeriodicOrdersPage(patient.id)" v-for="patient in filteredPatients" :key="patient.id" class="periodic-orders-item">
                    <div class="header">
                        <div class="patient-info">
                            <i class="fa-solid fa-check check-icon" />
                            <span>{{ patient.fullName }}</span>
                        </div>
                        <div class="room-info">
                            <i class="fa-solid fa-bed" />
                            <span>{{ patient.roomName }}</span>
                        </div>
                    </div>
                    <div class="quantity-info">
                        <span>{{ getTotalQuantityPeriodic(patient.id) }} produits validés</span>
                    </div>
                </div>
            </div>
            <div v-if="periodicOrders && selectedOrderType === 'Patients à valider' || selectedOrderType === 'Toutes'"
                class="periodic-orders-container">
                <div @click="goToPeriodicOrdersPage(patient.id)" v-for="patient in filteredPatients" :key="patient.id"
                    class="periodic-orders-item">
                    <div class="header">
                        <div class="patient-info">
                            <i class="fa-sharp fa-solid fa-circle-exclamation alert-periodic-icon" />
                            <span>{{ patient.fullName }}</span>
                        </div>
                        <div class="room-info">
                            <i class="fa-solid fa-bed" />
                            <span>{{ patient.roomName }}</span>
                        </div>
                    </div>
                    <div class="quantity-info">
                        <span>{{ getTotalQuantity(patient.id)}} {{ (getTotalQuantity(patient.id) > 0 ? 'produits' : 'produit') }} à valider</span>
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
import PatientService from "@/MediCare/Patients/Services/patients-service";
import OrderMasterDataService from "../Services/orders-master-data-service";
export default {
    components: {
        InputText,
        Dropdown,
    },
    data() {
        return {
            sectorsTypeOptions: ["Toutes", "Secteur A", "Secteur B"],
            selectedSector: "Toutes",
            ordersTypeOptions: ["Toutes", "Patients validés", "Patients à valider"],
            selectedOrderType: "Toutes",
            periodicOrders: null,
            patientsData: {},
            searchOrders: "",
            filteredPatients: []
        };
    },
    async created() {
        this.periodicOrders = await PeriodicOrdersMasterDataService.getAllAsync();
        await this.orderPeriodicPatientData();
    },
    methods: {
        async orderPeriodicPatientData() {
            for (const periodicOrder of this.periodicOrders) {
                if (periodicOrder.patientId != null && periodicOrder.patientId > 0) {
                    if (!this.patientsData[periodicOrder.patientId]) {
                        const patient = await PatientService.getPatientById(periodicOrder.patientId);
                        this.patientsData[periodicOrder.patientId] = patient;
                    }
                }
            }
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
        getValidatedOrdersCount(patientId) {
            return this.periodicOrders.filter(order => order.patientId === patientId && order.isValidated).length;
        },
        getTotalQuantityPeriodic(patientId) {
            const ordersForPatient = this.periodicOrders.filter(order => order.patientId === patientId);
            const totalQuantity = ordersForPatient.reduce((total, order) => total + order.quantity, 0);
            return totalQuantity;
        },
        goToPeriodicOrdersPage(patientId) {
            this.$router.push({
                name: "PatientPeriodicOrders",
                params: { id: patientId },
            });
        },
        getTotalQuantity(patientId) {
        const zeroQuantityOrdersForPatient = this.periodicOrders.filter(order => order.patientId === patientId && order.quantity === 0);
        const totalQuantity = zeroQuantityOrdersForPatient.length;
        return totalQuantity;
        },
    },
    computed: {
        filteredPatients() {
            return Object.values(this.patientsData).filter(patient =>
                (patient?.fullName.toLowerCase().includes(this.searchOrders.toLowerCase().trim()) ||
                    patient?.roomName.toLowerCase().includes(this.searchOrders.toLowerCase().trim())) &&
                (
                    this.selectedOrderType === "Toutes" && this.getTotalQuantity(patient.id) > 0 ||
                    (this.selectedOrderType === "Patients validés" && patient.id != null) ||
                    (this.selectedOrderType === "Patients á valider" && patient.id != null && this.getTotalQuantity(patient.id) > 0)
                )
            );
        }
    }
};
</script>
<style type="text/css" scoped src="./Content/orders-page.css"></style>