<template>
    <div class="patient-page-container">
        <patient-info-component :patient="patient" />
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
    import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';


    export default {
        components: {
            Button,
            PatientInfoComponent
        },
        data() {
            return {
                patient: new AsyncLazy(async () => {
                    return await PatientService.getPatientById(this.$route.params.id);
                }),
                patientTreatments: [],
            };
        },
        async created() {
            this.patient = await this.patient.getValueAsync();
            this.patientTreatments = await PatientService.getMasterDataByPatientId(this.patient.id, 'Treatments')
        },
        methods: {
            goToTreatmentPage() {
                this.$router.push({ name: "Treatment", params: { id: this.patient.id } });
            },
            async getCurrentPatientAsync() {
                return await this.patient.getValueAsync();
            },
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-page.css">
</style>