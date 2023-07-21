<template>
    <div class="treatment-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <Accordion v-if="patientTreatments && patientTreatments.some(treatment => 'article' in treatment)">
            <template v-for="(treatment, index) in patientTreatments" :key="index">
                <AccordionTab :disabled="isArticleNotFound(treatment)">
                    <template #header>
                        <div v-if="treatment.article != null" class="accordion-header">
                            <div class="accordion-top-area">
                                <span class="header-title">
                                    {{ treatment.article.title }}
                                </span>
                                    <i v-show="treatment.article.isGalenicDosageForm" class="fa-solid fa-briefcase-medical bag-icon"></i>
                            </div>
                            <span class="header-subtitle">{{treatment.article.groupName}}</span>
                            <span class="header-subtitle">{{treatment.dosageDescription}}</span>
                            <div>
                                <span class="header-subtitle">De {{ $d(new Date(treatment.dateStart))}} à {{ $d(new Date(treatment.dateEnd))}}</span>
                            </div>
                        </div>
                        <div v-else class="accordion-header">
                            <div class="accordion-top-area">
                                <span class="header-title">
                                    {{ treatment.alternativeArticleDescription }}
                                </span>
                            </div>
                        </div>
                    </template>
                    <OrderComponent v-if="treatment.article" :article="treatment.article" :patientOrders="patientOrders" />
                </AccordionTab>
            </template>
        </Accordion>
        <EmptyListComponent v-show="patientTreatments != null && patientTreatments.length < 1" />
    </div>

</template>

<script>

    import Button from 'primevue/button';
    import Accordion from 'primevue/accordion';
    import AccordionTab from 'primevue/accordiontab';
    import SelectButton from 'primevue/selectbutton';
    import InputNumber from 'primevue/inputnumber';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import OrderComponent from './OrderComponent.vue';
    import ArticlesMasterDataService from "@/MediCare/Referencials/Services/articles-master-data-service";
    import DateLiteral from '@/Fwamework/Utils/Components/DateLiteralComponent.vue';
    import PatientService, { usePatient } from "@/MediCare/Patients/Services/patients-service";
    import EmptyListComponent from '@/MediCare/Components/EmptyListComponent.vue'

    export default {
        components: {
            Button,
            Accordion,
            AccordionTab,
            SelectButton,
            InputNumber,
            PatientInfoComponent,
            OrderComponent,
            DateLiteral,
            EmptyListComponent
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
                patientOrders: []
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();
            this.patientTreatments = await PatientService.getMasterDataByPatientId(this.patient.id, 'Treatments')
            if (this.$route.params.treatmentType) {
                this.patientTreatments = this.patientTreatments.filter(treatment => treatment.treatmentType === this.$route.params.treatmentType)
            }
            this.fillPatientTreatments()
            this.patientOrders = await PatientService.getMasterDataByPatientId(this.patient.id, 'Orders')
        },
        methods: {
            goToTreatmentPage() {
                this.$router.push({ name: "Treatment" });
            },
            handleOptionChange(newValue) {
                // TODO: prevent the user to deselect the selected option
            },
            focusInputNumber(index) {
                this.$nextTick(() => {
                    this.$refs.inputNumber[index].$el.querySelector("input").focus();
                });
            },
            async fillPatientTreatments() {
                let treatmentArticleIds = this.patientTreatments.map(treatment => treatment.appliedArticleId)
                treatmentArticleIds = treatmentArticleIds.concat(this.patientTreatments.map(treatment => treatment.prescribedArticleId))
                const articles = await ArticlesMasterDataService.getByIdsAsync(treatmentArticleIds)
                this.patientTreatments.forEach(treatment => {
                    if (treatment.appliedArticleId !== 0 || treatment.appliedArticleId !== null) {
                        const article = articles.find(article => article.id === treatment.appliedArticleId)
                        treatment.article = article
                    } else if (treatment.prescribedArticleId !== 0 || treatment.prescribedArticleId !== null) {
                        const article = articles.find(article => article.id === treatment.prescribedArticleId)
                        treatment.article = article
                    } else {
                        treatment.article = null
                    }
                })
            },
            isArticleNotFound(treatment) {
                if (treatment.appliedArticleId === 0 || treatment.appliedArticleId === null &&
                    treatment.prescribedArticleId === 0 || treatment.prescribedArticleId === null) {
                    return true
                } else {
                    return false
                }
            }
        },
    }
</script>
<style type="text/css" scoped src="./Content/treatment-page.css">
</style>