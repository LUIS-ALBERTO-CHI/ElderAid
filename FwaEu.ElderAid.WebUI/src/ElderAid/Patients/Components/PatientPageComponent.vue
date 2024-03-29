<template>
    <div v-if="contentLoaded" class="patient-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <div @click="goToMedicationsPage" class="patient-info-item">
            <span>Medicamentos</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToTreatmentPage" class="patient-info-item">
            <span>{{ patientTreatments.length }} tratamientos</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToProtectionPage" class="patient-info-item">
            <span>{{ protections.length }} cuidados</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToPatientOrdersPage" class="patient-info-item">
            <span>{{ patientsOrders.length }} pedidos, {{ patientsOrders.filter(x => x.state == "Pending").length }} en curso</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToStockConsumptionPage" class="patient-info-item">
            <span>Consumo de stock de farmacia</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div @click="goToPeriodicOrdersPage" class="patient-info-item">
            <div class="periodic-container">
                <i v-if="isPeriodicAlertEnabled()" class="fa-sharp fa-solid fa-circle-exclamation alert-periodic-icon"></i>
                <span>Pedidos periódicos a validar</span>
            </div>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <Button @click="goToSearchArticlePage" style="margin-top: 20px; width: 100%;" label="Pedir otro producto" />
    </div>
</template>

<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import PatientService, { usePatient } from "@/ElderAid/Patients/Services/patients-service";
    import ViewContextService from "@/ElderAid/ViewContext/Services/view-context-service";


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
                patientTreatments: null,
                patientsOrders: null,
                contentLoaded: false,
                protections: null,
                organization: {},
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();
            this.patientTreatments = await PatientService.getMasterDataByPatientId(this.patient.id, 'Treatments').then(x => {
                return x.filter(x => x.articleType == "CareEquipment")
            })
            this.patientsOrders = await PatientService.getMasterDataByPatientId(this.patient.id, 'Orders')
			this.protections = await PatientService.getMasterDataByPatientId(this.patient.id, 'Protections')
            this.organization = ViewContextService.get();

            if (this.patientTreatments != null && this.patientsOrders != null && this.protections != null) {
                this.contentLoaded = true;
            }
        },
        methods: {
            goToTreatmentPage() {
                this.$router.push({ name: "Treatments", params: { id: this.patient.id } });
            },
            goToMedicationsPage() {
                this.$router.push({ name: "PatientMedications", params: { id: this.patient.id } });
            },
            goToPatientOrdersPage() {
                this.$router.push({ name: "PatientOrders", params: { id: this.patient.id } });
            },
            goToStockConsumptionPage() {
                this.$router.push({ name: "StockConsumption", params: { id: this.patient.id } });
            },
            goToSearchArticlePage() {
                this.$router.push({ name: "SearchArticle" });
            },
            goToProtectionPage() {
                this.$router.push({ name: "Protection" });
            },
            goToPeriodicOrdersPage() {
                this.$router.push({ name: "PeriodicOrder", params: { id: this.patient.id } });
            },
            isPeriodicAlertEnabled() {
                const periodicAlertDate = new Date(this.organization.nextPeriodicityOrder);
                periodicAlertDate.setDate(periodicAlertDate.getDate() - this.organization.periodicityOrderActivationDaysNumber);
                return new Date() > periodicAlertDate
            }
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-page.css">
</style>