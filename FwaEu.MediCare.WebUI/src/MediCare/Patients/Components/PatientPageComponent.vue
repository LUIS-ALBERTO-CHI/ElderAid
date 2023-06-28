<template>
    <div class="patient-page-container">
        <patient-info-component />
        <div @click="goToMedicationsPage" class="patient-info-item">
            <span>Médicaments</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToTreatmentPage" class="patient-info-item">
            <span>4 matériels de soin</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToProtectionPage" class="patient-info-item">
            <span>2 protections</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToPatientOrdersPage" class="patient-info-item">
            <span>8 commandes, dont 4 en cours</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToStockConsumptionPage" class="patient-info-item">
            <span>Consommation du stock pharmacie</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div  class="patient-info-item">
            <div class="periodic-container">
                <i class="fa-sharp fa-solid fa-circle-exclamation alert-periodic-icon"></i>
                <span>2 commandes périodiques à valider</span>
            </div>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <Button @click="goToOrderOtherProductPage" style="margin-top: 20px; width: 100%;" label="Commander un autre produit" />
    </div>
</template>
<!-- eslint-disable @fwaeu/custom-rules/no-local-storage -->
<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';


    export default {
        components: {
            Button,
            PatientInfoComponent
        },
        data() {
            return {
                patient: {},
            };
        },
        async created() {
            var patient = localStorage.getItem("patient");
            this.patient = JSON.parse(patient);
        },
        methods: {
            goToTreatmentPage() {
                this.$router.push({ name: "Treatment" });
            },
            goToMedicationsPage() {
                this.$router.push({ name: "PatientMedications" });
            },
            goToPatientOrdersPage() {
                this.$router.push({ name: "PatientOrders" });
            },
            goToStockConsumptionPage() {
                this.$router.push({ name: "StockConsumption" });
            },
            goToOrderOtherProductPage() {
                this.$router.push({ name: "OrderOtherProduct" });
            },
            goToProtectionPage() {
                this.$router.push({ name: "Protection" });
            }
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-page.css">
</style>