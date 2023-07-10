<template>
    <div class="patient-page-container">
        <patient-info-component />
        <div @click="goToTreatmentPage" class="patient-info-item">
            <span>{{ patientTreatments.filter(x => x.treatmentType == "Fixe").length }} traitements fixes en cours</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToTreatmentPage" class="patient-info-item">
            <span>{{ patientTreatments.filter(x => x.treatmentType == "Reserve").length }} traitements de réserve en cours</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToTreatmentPage" class="patient-info-item">
            <span>{{ patientTreatments.filter(x => x.treatmentType == "Erased").length }} traitements effacés</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <Button style="margin-top: 20px;" label="Commander un autre produit" />
    </div>
</template>
<!-- eslint-disable @fwaeu/custom-rules/no-local-storage -->
<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import PatientService from "@/MediCare/Patients/Services/patients-service";


    export default {
        components: {
            Button,
            PatientInfoComponent
        },
        data() {
            return {
                patient: {},
                patientTreatments: [],
            };
        },
        async created() {
            var patient = localStorage.getItem("patient");
            this.patient = JSON.parse(patient);

            this.patientTreatments = await PatientService.getMasterDataByPatientId(this.patient.id, 'Treatments')

        },
        methods: {
            goToTreatmentPage() {
                this.$router.push({ name: "Treatment" });
            },
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-page.css">
</style>