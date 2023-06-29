<template>
    <div class="protection-page-container">
        <PatientInfoComponent />
        <div class="protection-info-item">
            <div class="alert-content">
                <span @click="goToIncontinenceLevelPage">Niveau d'incontience: légère</span>
                <div v-if="isAlert" :style="{ color: '#f44538'}" class="alert-container">
                    <i class="fa-sharp fa-solid fa-circle-exclamation alert-icon"></i>
                    <span>Le forfait d'incontinence est dépassé</span>
                </div>
                <div v-else :style="{ color: '#00b300'}" class="alert-container">
                    <i class="fa-solid fa-circle-check alert-icon"></i>
                    <span>Le forfait d'incontinence est respecté</span>
                </div>
            </div>
            <i style="font-size: 30px;" class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div style="display: flex; flex-direction: column; margin-top: 20px;">
            <div v-for="(protection, index) in protections" :key="index">
                <ProtectionAccordionTabComponent :protection="protection" />
            </div>
        </div>
        <Button label="Imprimer le protocole"></Button>
        <Button label="Ajouter une protection"></Button>

    </div>

</template>
<!-- eslint-disable @fwaeu/custom-rules/no-local-storage -->
<script>

    import Accordion from 'primevue/accordion';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import Button from 'primevue/button';
    import ProtectionAccordionTabComponent from './ProtectionAccordionTabComponent.vue';


    export default {
        components: {
            Accordion,
            PatientInfoComponent,
            Button,
            ProtectionAccordionTabComponent,
        },
        data() {
            return {
                isAlert: true,
                protections: [{
                    name: "Tena Pants Maxi",
                    amount: "1 pèce: 8h, 1 pièce 16h",
                    date: "De 23/02/2023 à 30/05/2023"
                },
                {
                    name: "Tena Pants Maxi",
                    amount: "1 pèce: 8h, 1 pièce 16h",
                    date: "De 23/02/2023 à 30/05/2023"
                },
                ],
            };
        },
        async created() {
        },
        methods: {
            goToIncontinenceLevelPage() {
                this.$router.push({ name: 'IncontinenceLevel' });
            },
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/protection-page.css">
</style>