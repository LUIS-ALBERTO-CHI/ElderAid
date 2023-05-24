<template>
    <div class="search-patient-container ">
        <Dropdown class="select-sector" placeholder="Selectionner un secteur"/>
        <span class="p-input-icon-right">
            <i class="fa fa-solid fa-close"/>
            <InputText class="search-input" placeholder="Rechercher un patient"/>
        </span>
        <div class="patient-list">
            <div v-for="patient in patients" :key="patient.firstname">
                <div  :class="[patient.isActive ? 'patient-item' : 'patient-item patient-item-inactive']">
                    <span>{{`${patient.firstname} ${patient.lastname}`}}</span>
                    <div class="room-patient-area">
                        <span><i class="fa fa-solid fa-bed" style="margin-right: 10px;"></i>B302</span>
                        <span v-show="!patient.isActive">Inactif</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import patientsData from './patients.json';

    import Dropdown from 'primevue/dropdown';
    import InputText from 'primevue/inputtext';



    export default {
        inject: ["deviceInfo"],
        components: {
            Dropdown,
            InputText
        },
        data() {
            return {
                patients: []
            };
        },
        async created() {
            this.patients = patientsData;
            console.log(this.patients);
        },
        methods: {
        },
    }
</script>
<style type="text/css" scoped src="./Content/search-patient-page.css">
</style>