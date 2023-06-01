<template>
    <div class="patient-container">
        <div class="patient-main-info-container">
            <div class="patient-area">
                <span style="font-weight: bold;">{{patient.firstname}} {{patient.lastname}}</span>
                <i class="fa-solid fa-circle " :class="patient.isActive ? 'active-patient-text' : 'inactive-patient-text'" />
            </div>
            <span><i class="fa fa-solid fa-bed" style="margin-right: 10px;"></i>{{patient.roomNumber}}</span>
        </div>
        <span>Mise à jour: 29/10/22 à 9:12</span>
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

    .patient-area {
        display: flex;
        flex-direction: row;
        align-items: center;
        column-gap: 10px;
    }

    .active-patient-text {
        color: #22C55E
    }

    .inactive-patient-text {
        color: #484349;
        opacity: 0.5;
    }
</style>