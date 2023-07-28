<template>
    <div>
        <div class="orders-container">
            <div v-if="orders" class="periodic-orders-container">
                <div @click="goToPeriodicOrdersPage(patient.id)" v-for="patient in patientsFiltered" :key="patient.id"
                    class="periodic-orders-item">
                    <div class="header">
                        <div class="patient-info">
                            <i class="fa-sharp fa-solid fa-circle-exclamation alert-periodic-icon"/>
                            <span>{{ patient.fullName }}</span>
                        </div>
                        <div class="room-info">
                            <i class="fa-solid fa-bed" />
                            <span>{{ patient.roomName }}</span>
                        </div>
                    </div>
                    <div class="quantity-info">
                        <span>{{ getTotalQuantity(patient.id) }} produits á valider</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import PatientService from "@/MediCare/Patients/Services/patients-service";
import OrderMasterDataService from "../Services/orders-master-data-service";

export default {
    data() {
        return {
            orders: null,
            patientsData: {},
        };
    },
    async created() {
        this.orders = await OrderMasterDataService.getAllAsync();
        await this.orderPatientData();
    },
    methods: {
                async orderPatientData() {
            for (const order of this.orders) {
                if (order.patientId != null && order.patientId > 0) {
                    if (!this.patientsData[order.patientId]) {
                        const patient = await PatientService.getPatientById(order.patientId);
                        this.patientsData[order.patientId] = patient;
                    }
                }
            }
        },
            getTotalQuantity(patientId) {
            const ordersForPatient = this.orders.filter(order => order.patientId === patientId);
            const totalQuantity = ordersForPatient.reduce((total, order) => total + order.quantity, 0);
            return totalQuantity;
        },
                goToPeriodicOrdersPage(patientId) {
            this.$router.push({
                name: "PeriodicOrders",
                params: { id: patientId },
            });
        },
    },
    computed: {
            patientsFiltered() {
            return Object.values(this.patientsData);
        },
    }
};
</script>
<style type="text/css" scoped src="./Content/orders-page.css"></style>