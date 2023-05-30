<template>
    <div class="patient-container">
        <div class="patient-main-info-container">
            <span style="font-weight: bold;">{{patient.firstname}} {{patient.lastname}}</span>
            <div class="room-patient-area">
                <span><i class="fa fa-solid fa-bed" style="margin-right: 10px;"></i>{{patient.roomNumber}}</span>
                <span :class="patient.isActive ? 'active-patient-text' : 'inactive-patient-text'">{{patient.isActive ? 'Actif': 'Inactif'}}</span>
            </div>
        </div>
        <span>Dernière mise à jour : 29/10/2000 à 9:12:33</span>
    </div>

</template>
<!-- eslint-disable @fwaeu/custom-rules/no-local-storage -->
<script>



    export default {
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
        },
        computed: {

        },

    }
</script>
<style type="text/css">
    .patient-container {
        display: flex;
        flex-direction: column;
        row-gap: 10px;
    }

    .patient-main-info-container {
        display: flex;
        justify-content: space-between;
        color: var(--alt-secondary-text-color);
    }

    .room-patient-area {
        display: flex;
        flex-direction: column;
        align-items: center;
        row-gap: 5px;
    }

    .active-patient-text {
        font-weight: bold;
        color: #22C55E
    }

    .inactive-patient-text {
        font-weight: bold;
        color: #EF4444;
    }
</style>