<template>
    <div class="search-patient-container ">
        <span class="p-input-icon-right">
            <i @click="removeSearch" class="fa fa-solid fa-close" :style="searchPatient.length == 0 ? 'opacity: 0.5;' : ''" />
            <InputText ref="searchInput" v-model="searchPatient" class="search-input" placeholder="Rechercher un patient" />
        </span>
        <Dropdown v-show="buildings.length > 1" v-model="selectedBuilding" :options="buildingOptions" />
        <div v-show="filteredPatients.length > 0" class="patient-list">
            <div v-for="patient in filteredPatients" :key="patient.firstname">
                <div @click="goToPatientPage(patient)" :class="[patient.isActive ? 'patient-item' : 'patient-item patient-item-inactive']">
                    <div class="name-patient-area">
                        <span>{{cuttedName(patient)}}</span>
                        <i v-show="!patient.isActive" class="fa-solid fa-circle patient-state" />
                    </div>
                    <span><i class="fa fa-solid fa-bed" style="margin-right: 10px;"></i>{{patient.roomName}}</span>
                </div>
            </div>
        </div>
        <div v-show="filteredPatients.length == 0" class="patient-not-found">
            <i class="fa-solid fa-heart-pulse icon-not-found"></i>
            <span>Aucun patient trouvé</span>
            <span>{{ selectedBuilding == "Tous les secteurs" ? 'Contactez la pharmacie': 'Vérifiez votre filtre de secteur ou contactez la pharmacie'}}</span>
        </div>
        <span class="display-patients-text" @click="changeDisplayInactive">{{displayInactivePatients ? 'Exclure les patients inactifs' : 'Inclure les patients inactifs'}}</span>
    </div>
</template>
<!-- eslint-disable @fwaeu/custom-rules/no-local-storage -->

<script>
    import patientsData from './patients.json';

    import Dropdown from 'primevue/dropdown';
    import InputText from 'primevue/inputtext';
    import PatientsMasterDataService from "@/MediCare/Patients/Services/patients-master-data-service";
    import BuildingsMasterDataService from "@/MediCare/Referencials/Services/buildings-master-data-service";





    export default {
        inject: ["deviceInfo"],
        components: {
            Dropdown,
            InputText
        },
        data() {
            return {
                patients: [],
                searchPatient: "",
                buildingOptions: ["Tous les secteurs"],
                selectedBuilding: "Tous les secteurs",
                displayInactivePatients: false,
                buildings: [],
            };
        },
        async created() {
            this.focusSearchBar();
            this.patients = await PatientsMasterDataService.getAllAsync();
            this.buildings = await BuildingsMasterDataService.getAllAsync();
            this.buildingOptions = this.buildingOptions.concat(this.buildings);
            this.selectedBuilding = this.buildingOptions[0]
            if (localStorage.getItem("searchPatient")) {
                this.searchPatient = localStorage.getItem("searchPatient");
            }
        },
        methods: {
            changeDisplayInactive() {
                this.displayInactivePatients = !this.displayInactivePatients;
            },
            goToPatientPage(patient) {
            const args = { cancelNavigation: false, selectedPatient: patient };
            this.$emit("selectedPatient", args);
            if (!args.cancelNavigation) {
                this.$router.push({ name: "Patient", params: { id: patient.id }});
            }
            },
            removeSearch() {
                this.searchPatient = "";
                this.focusSearchBar();
            },
            focusSearchBar() {
                this.$nextTick(() => {
                    this.$refs.searchInput.$el.focus();
                });
            },
            cuttedName(patient) {
                return patient.fullName.length > 20 ? patient.fullName.substring(0, 20) + "..." : patient.fullName;
            },
        },
        computed: {
            filteredPatients() {
                var patients = this.patients.filter(patient => {
                    return patient.fullName.toLowerCase().includes(this.searchPatient.toLowerCase()) ||
                        patient.roomName.toLowerCase().includes(this.searchPatient.toLowerCase());
                });
                // remove all inactive patients if displayInactivePatients is true
                if (!this.displayInactivePatients) {
                    patients = patients.filter(patient => {
                        return patient.isActive;
                    });
                }

                // filter by building
                if (this.selectedBuilding.id != null) {
                    patients = patients.filter(patient => {
                        return patient.buildingId == this.selectedBuilding.id;
                    });
                }
                return patients;
            },
        },
        beforeUnmount() {
            // keep research in local storage
            localStorage.setItem("searchPatient", this.searchPatient);
        }
    }
</script>
<style type="text/css" scoped src="./Content/search-patient-page.css">
</style>