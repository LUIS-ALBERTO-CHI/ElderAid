<template>
    <div class="patient-page-container">
        <div class="patient-main-info-container">
            <span style="font-weight: bold;">{{patient.firstname}} {{patient.lastname}}</span>
            <div class="room-patient-area">
                <span><i class="fa fa-solid fa-bed" style="margin-right: 10px;"></i>{{patient.roomNumber}}</span>
                <span>{{patient.isActive ? 'Actif': 'Inactif'}}</span>
            </div>
        </div>
        <span>Dernière mise à jour : 29/10/2000 à 9:12:33</span>
        <div @click="goToTreatmentPage" class="patient-info-item">
            <span>15 traitements fixes en cours</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div class="patient-info-item">
            <span>4 traitements de réserve en cours</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div class="patient-info-item">
            <span>4 matériels de soin</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div class="patient-info-item">
            <span>8 commandes, dont 4 en cours</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div class="patient-info-item">
            <span>3 traitements effacés</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div class="patient-info-item">
            <span>Consommation du stock pharmacie</span>
            <i class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <Button style="margin-top: 20px;" label="Commander un autre produit" />
    </div>
</template>
<!-- eslint-disable @fwaeu/custom-rules/no-local-storage -->
<script>

    import Button from 'primevue/button';


    export default {
        components: {
            Button
        },
        data() {
            return {
                patient: {},
            };
        },
        async created() {
            var patient = localStorage.getItem("patient");
            this.patient = JSON.parse(patient);
        },
        methods: {
            goToTreatmentPage() {
                this.$router.push({ name: "Treatment" });
            }
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-page.css">
</style>