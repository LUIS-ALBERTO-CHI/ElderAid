<template>
    <div class="treatment-page-container">
        <patient-info-component />
        <Accordion>
            <template v-for="(treatment, index) in treatments" :key="index">
                <AccordionTab>
                    <template #header>
                        <div class="accordion-header">
                            <span class="header-title">
                                {{ treatment.medicationOrdered }}
                            </span>
                            <span class="header-subtitle">{{treatment.initialMedication}}</span>
                            <span class="header-subtitle">{{treatment.frequency}}</span>
                            <span class="header-subtitle">{{treatment.date}}</span>
                        </div>
                    </template>
                    <div class="accordion-content">
                        <span>Commander une quantité :</span>
                        <div v-show="moreQuantityDisplayedIndex != index" class="quantity-container">
                            <div style="width: 75%;">
                                <SelectButton @change="handleOptionChange" class="quantity-select-button" v-model="selectedQuantity" :options="quantityOptions" />
                            </div>
                            <i @click="displayMoreQuantity(index)" class="fa fa-solid fa-plus add-icon"></i>
                        </div>
                        <div v-show="moreQuantityDisplayedIndex == index">
                            <InputNumber ref="inputNumber" v-model="selectedQuantity" showButtons buttonLayout="horizontal" style="width: 75%;"
                                         decrementButtonClassName="p-button-secondary" incrementButtonClassName="p-button-secondary"
                                         incrementButtonIcon="fa fa-solid fa-plus" decrementButtonIcon="fa fa-solid fa-minus" />

                        </div>
                        <SelectButton class="custom-select-button" v-model="selectedOption" :options="selectOptions" />
                        <div class="confirmation-container" v-if="showConfirmationIndex === index">
                            <span>Etes vous sûre de commander?</span>
                            <div class="confirmaton-button-container">
                                <Button label="OUI" outlined style="border: none !important; height: 30px !important;" />
                                <Button @click="hideConfirmation" label="NON" outlined style="border: none !important; height: 30px !important;" />
                            </div>
                        </div>
                        <Button v-else @click="showConfirmation(index)" style="height: 35px !important;" label="Commander" />
                    </div>
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

    export default {
        components: {
            Button,
            Accordion,
            AccordionTab,
            SelectButton,
            InputNumber,
            PatientInfoComponent
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
                    date: "De 23/02/2023 à 30/05/2023"
                },
                {
                    medicationOrdered: "ANTIDRY lotion huilde amande 500ml",
                    initialMedication: "ANTIDRY calm lotion 500ml",
                    frequency: "unité/1:matin/Ts les jours",
                    date: "De 23/02/2023 à 30/05/2023"
                }],
                showConfirmationIndex: -1
            };
        },
        async created() {
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
            }
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/treatment-page.css">
</style>