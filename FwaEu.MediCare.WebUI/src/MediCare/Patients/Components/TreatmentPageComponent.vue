<template>
    <div class="treatment-page-container">
        <patient-info-component />
        <Accordion v-if="patientTreatments.some(treatment => 'article' in treatment)">
            <template v-for="(treatment, index) in patientTreatments" :key="index">
                <AccordionTab>
                    <template #header>
                        <div class="accordion-header">
                            <div class="accordion-top-area">
                                <span class="header-title">
                                    {{ treatment.article.title }}
                                </span>
                                <i v-show="treatment.isBag" class="fa-solid fa-briefcase-medical bag-icon"></i>
                            </div>
                            <span class="header-subtitle">{{treatment.article.groupName}}</span>
                            <span class="header-subtitle">{{treatment.dosageDescription}}</span>
                            <div>
                                <span class="header-subtitle">De {{ $d(new Date(treatment.dateStart))}} à {{ $d(new Date(treatment.dateEnd))}}</span>
                            </div>
                        </div>
                    </template>
                    <OrderComponent :article="treatment.article" :patientOrders="patientOrders"/>
                </AccordionTab>
            </template>
        </Accordion>
    </div>

</template>
<!-- eslint-disable @fwaeu/custom-rules/no-local-storage -->
<script>

    import Button from 'primevue/button';
    import Accordion from 'primevue/accordion';
    import AccordionTab from 'primevue/accordiontab';
    import SelectButton from 'primevue/selectbutton';
    import InputNumber from 'primevue/inputnumber';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import OrderComponent from './OrderComponent.vue';
    import ArticlesMasterDataService from "@/MediCare/Referencials/Services/articles-master-data-service";
    import PatientService from "@/MediCare/Patients/Services/patients-service";
    import DateLiteral from '@/Fwamework/Utils/Components/DateLiteralComponent.vue';
    import OrdersMasterDataService from '@/MediCare/Orders/Services/orders-master-data-service'


    export default {
        components: {
            Button,
            Accordion,
            AccordionTab,
            SelectButton,
            InputNumber,
            PatientInfoComponent,
            OrderComponent,
            DateLiteral
        },
        data() {
            return {
                patient: {},
                isMoreQuandityDisplayed: false,
                moreQuantityDisplayedIndex: -1,
                quantityOptions: [1, 2, 3],
                selectedQuantity: 1,
                selectOptions: ["à l'unité", "boîtes complètes"],
                selectedOption: "à l'unité",
                treatments: [{
                    medicationOrdered: "ADAPTRIC pensements 7.6x7.6 stériles sach 10 pce",
                    initialMedication: "Jelonet gaze coton paraffinée 10x10cm bte 10pce",
                    frequency: "unité/1:matin/Ts les jours",
                    date: "De 23/02/2023 à 30/05/2023",
                    isBag: true
                },
                {
                    medicationOrdered: "ANTIDRY lotion huilde amande 500ml",
                    initialMedication: "ANTIDRY calm lotion 500ml",
                    frequency: "unité/1:matin/Ts les jours",
                    date: "De 23/02/2023 à 30/05/2023",
                    isBag: false
                }],
                showConfirmationIndex: -1,
                patient: {},
                patientTreatments: [],
                patientOrders: []
            };
        },
        async created() {
            this.patient = JSON.parse(localStorage.getItem("patient"));
            var patientTreatments = await PatientService.getMasterDataByPatientId(this.patient.id, 'Treatments')
            this.patientTreatments = patientTreatments.filter(obj => obj.appliedArticleId !== 0)
            this.fillPatientTreatments()
            this.patientOrders = await PatientService.getMasterDataByPatientId(this.patient.id, 'Orders')


        },
        methods: {
            goToTreatmentPage() {
                this.$router.push({ name: "Treatment" });
            },
            displayMoreQuantity(index) {
                this.moreQuantityDisplayedIndex = index;
                this.selectedQuantity = 4;
                this.focusInputNumber(index);
            },
            handleOptionChange(newValue) {
                // TODO: prevent the user to deselect the selected option
            },
            focusInputNumber(index) {
                this.$nextTick(() => {
                    this.$refs.inputNumber[index].$el.querySelector("input").focus();
                });
            },
            showConfirmation(index) {
                this.showConfirmationIndex = index;
            },
            hideConfirmation() {
                this.showConfirmationIndex = -1;
            },
            async fillPatientTreatments() {
                const treatmentArticleIds = this.patientTreatments.map(treatment => treatment.appliedArticleId)
                const articles = await ArticlesMasterDataService.getByIdsAsync(treatmentArticleIds)
                this.patientTreatments.forEach(treatment => {
                    const article = articles.find(article => article.id === treatment.appliedArticleId)
                    treatment.article = article
                })
            }
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/treatment-page.css">
</style>