<template>
    <div class="patient-page-container">
        <div class="patient-main-info-container">
            <span style="font-weight: bold;">{{patient.firstname}} {{patient.lastname}}</span>
            <div class="room-patient-area">
                <span><i class="fa fa-solid fa-bed" style="margin-right: 10px;"></i>{{patient.roomNumber}}</span>
                <span >Inactif</span>
            </div>
        </div>
        <span>Dernière mise à jour: 29/10/2000 à 9:12:33</span>
        <div class="patient-info-item">
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

<script>

    import Button from 'primevue/button';


    export default {
        inject: ["deviceInfo"],
        components: {
            Button
        },
        data() {
            return {
                patient: {
                    firstname: "Mia",
                    lastname: "Roberts",
                    isActive: false,
                    roomNumber: "B607"
                },
            };
        },
        async created() {

        },
        methods: {

        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-page.css">
</style>