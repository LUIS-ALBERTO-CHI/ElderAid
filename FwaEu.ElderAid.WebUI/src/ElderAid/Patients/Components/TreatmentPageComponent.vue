<template>
    <div class="treatment-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <Accordion v-if="patientTreatments && patientTreatments.some(treatment => 'prescribedArticle' in treatment)">
            <template v-for="(treatment, index) in patientTreatments" :key="index">
                <AccordionTab :disabled="isArticleNotFound(treatment)">
                    <template #header>
                        <div class="accordion-header">
                            <div class="accordion-top-area">
                                <span class="header-title">
                                    {{ treatment.prescribedArticle.title }}
                                </span>
                                <span v-show="treatment.prescribedArticle.isGalenicDosageForm" 
                                >Bolsa</span>
                            </div>
                            <span v-show="treatment.appliedArticle != null" class="header-subtitle">{{treatment.appliedArticle?.title }}</span>
                            <span class="header-subtitle">{{treatment.dosageDescription}}</span>
                            <span class="header-subtitle">{{isGoodEndDate(treatment.dateEnd, treatment.dateStart) ? `De ${$d(new Date(treatment.dateStart))} à ${$d(new Date(treatment.dateEnd))}` : $d(new Date(treatment.dateStart))}}</span>
                        </div>
                    </template>
                    <OrderComponent v-if="getArticleToOrder(treatment)" :article="getArticleToOrder(treatment)" :patientOrders="patientOrders" :patientId="patient.id" />
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
    import RecentArticlesMasterDataService from "@/ElderAid/Articles/Services/recent-articles-master-data-service";
	import DateLiteral from '@UILibrary/Fwamework/Utils/Components/DateLiteralComponent.vue';
    import PatientService, { usePatient } from "@/ElderAid/Patients/Services/patients-service";
    import EmptyListComponent from '@/ElderAid/Components/EmptyListComponent.vue'

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
                this.patientTreatments = this.patientTreatments.filter(treatment => (treatment.treatmentType === this.$route.params.treatmentType)
                && (treatment.articleType === "Medicine"))
            } else {
                this.patientTreatments = this.patientTreatments.filter(treatment => treatment.articleType === "CareEquipment")
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
                const articles = await RecentArticlesMasterDataService.getByIdsAsync(treatmentArticleIds)
                this.patientTreatments.forEach(treatment => {
                    treatment.prescribedArticle = articles.find(article => article.id === treatment.prescribedArticleId)
                    if (treatment.appliedArticleId !== 0 || treatment.appliedArticleId !== null)
                        treatment.appliedArticle = articles.find(article => article.id === treatment.appliedArticleId)
                })
            },
            isArticleNotFound(treatment) {
                if ( treatment.prescribedArticleId === 0 || treatment.prescribedArticleId === null) {
                    return true
                } else {
                    return false
                }
            },
            isGoodEndDate(dateEnd, dateStart) {
                if (dateEnd === null || dateEnd < dateStart) {
                    return false
                } else {
                    return true
                }
            },
            getArticleToOrder(treatment) {
                return treatment.appliedArticle && treatment.appliedArticle.pharmaCode != '9999999' ? treatment.appliedArticle : treatment.prescribedArticle;
            }
        }
    }
</script>
<style type="text/css" scoped src="./Content/treatment-page.css">
</style>