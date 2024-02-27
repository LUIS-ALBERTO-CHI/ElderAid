<template>
    <div class="patient-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <div @click="goToTreatmentPage('Fixe')" class="patient-info-item">
            <span>{{ patientTreatments.filter(x => x.treatmentType == "Fixe").length }} tratamiento(s) fijo(s) en curso</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToTreatmentPage('Reserve')" class="patient-info-item">
            <span>{{ patientTreatments.filter(x => x.treatmentType == "Reserve").length }} tratamientos de reserva en curso</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToTreatmentPage('Erased')" class="patient-info-item">
            <span>{{ patientTreatments.filter(x => x.treatmentType == "Erased").length }} tratamientos borrados</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <Button @click="goToSearchArticlePage()" style="margin-top: 20px;" label="Pedir otro producto" />
    </div>
</template>

<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import PatientService, { usePatient } from "@/ElderAid/Patients/Services/patients-service";
    import { medicineType } from '@/ElderAid/Articles/article-filter-types'


    export default {
        components: {
            Button,
            PatientInfoComponent
        },
        setup() {
            const { patientLazy, getCurrentPatientAsync } = usePatient();
            return {
                patientLazy,
                getCurrentPatientAsync
            }
        },
        data() {
            return {
                patient: null,
                patientTreatments: [],
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();
            var patientTreatments = await PatientService.getMasterDataByPatientId(this.patient.id, 'Treatments')
            this.patientTreatments = patientTreatments.filter(obj => obj.articleType == "Medicine")
        },
        methods: {
            goToTreatmentPage(treatmentType) {
                if (treatmentType == "Fixe")
                    this.$router.push({ name: "TreatmentsFixe", params: { id: this.patient.id, treatmentType: treatmentType } });
                else if (treatmentType == "Erased")
                    this.$router.push({ name: "TreatmentsErased", params: { id: this.patient.id, treatmentType: treatmentType } });
                else
                    this.$router.push({ name: "TreatmentsReserve", params: { id: this.patient.id, treatmentType: treatmentType } });
            },
            goToSearchArticlePage() {
                this.$router.push({ name: "SearchArticle", query: { articleFilterType: medicineType } });
            },
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-page.css">
</style>