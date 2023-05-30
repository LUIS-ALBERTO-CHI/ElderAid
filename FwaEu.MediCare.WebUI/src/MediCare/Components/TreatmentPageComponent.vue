<template>
    <div class="treatment-page-container">
        <patient-info-component />
        <Accordion :activeIndex="0">
            <AccordionTab>
                <template #header>
                    <div class="accordion-header">
                        <span class="header-title">
                            ADAPTRIC pensements 7.6x7.6 stériles sach 10
                            pce
                        </span>
                        <span class="header-subtitle">Jelonet gaze coton paraffinée 10x10cm bte 10pce</span>
                        <span class="header-subtitle">unité/1:matin/Ts les jours</span>
                        <span class="header-subtitle">De 23/02/2023 à 30/05/2023</span>
                    </div>
                </template>
                <div class="accordion-content">
                    <span>Commander une quantité :</span>
                    <div v-show="!isMoreQuandityDisplayed" class="quantity-container">
                        <div style="width: 75%;">
                            <SelectButton @change="handleOptionChange" class="quantity-select-button" v-model="selectedQuantity" :options="quantityOptions" />
                        </div>
                        <i @click="displayMoreQuantity" class="fa fa-solid fa-plus add-icon"></i>
                    </div>
                    <div v-show="isMoreQuandityDisplayed">
                        <InputNumber v-model="selectedQuantity" showButtons buttonLayout="horizontal" style="width: 75%"
                                     decrementButtonClassName="p-button-secondary" incrementButtonClassName="p-button-secondary"
                                     incrementButtonIcon="fa fa-solid fa-plus" decrementButtonIcon="fa fa-solid fa-minus" />

                    </div>
                    <SelectButton class="custom-select-button" v-model="selectedOption" :options="selectOptions" />
                    <Button style="height: 35px !important;" label="Commander" />

                </div>
            </AccordionTab>
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
                quantityOptions: [1, 2, 3],
                selectedQuantity: 1,
                selectOptions: ["à l'unité", "boîtes complètes"],
                selectedOption: "à l'unité",
                value1: null
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
            displayMoreQuantity() {
                this.isMoreQuandityDisplayed = !this.isMoreQuandityDisplayed;
                this.selectedQuantity = 4;
            },
            handleOptionChange(newValue) {
                // TODO: prevent the user to deselect the selected option
            },
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/treatment-page.css">
</style>