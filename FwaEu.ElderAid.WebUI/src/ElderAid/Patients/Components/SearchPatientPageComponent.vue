<template>
    <div class="search-patient-container ">
        <span class="p-input-icon-right">
            <i @click="removeSearch" class="fa fa-solid fa-close" :style="searchPatient.length == 0 ? 'opacity: 0.5;' : ''" />
            <InputText ref="searchInput" v-model="searchPatient" class="search-input" placeholder="Buscar un paciente" />
        </span>
        <Dropdown v-show="buildings.length > 1" v-model="selectedBuilding" :options="buildingOptions" />
        <!-- <span class="display-patients-text" @click="changeDisplayInactive">{{displayInactivePatients ? 'Excluir pacientes inactivos' : 'Incluir pacientes inactivos'}}</span> -->
        <div class="include-patients-switch-item">
            <span class="display-patients-text" > Mostrar pacientes inactivos</span>
            <InputSwitch  @click="changeDisplayInactive"  v-model="displayInactivePatients"  class="my-input-switch"/>
        </div>    
        <div v-show="filteredPatients.length > 0" class="patient-list">
            <div v-for="patient in filteredPatients" :key="patient.firstname">
                <div @click="onPatientClick(patient)" :class="[patient.isActive ? 'patient-item' : 'patient-item patient-item-inactive']">
                    <div class="name-patient-area">
                        <span>{{cuttedName(patient)}}</span>
                        <i v-show="!patient.isActive" class="fa-solid fa-circle patient-state" />
                    </div>
                    <span><i class="fa fa-solid fa-bed" style="margin-right: 10px; color: #0088F6"></i>{{patient.roomName}}</span>
                </div>
            </div>
        </div>
        <div v-show="filteredPatients.length == 0" class="patient-not-found">
            <i class="fa-solid fa-heart-pulse icon-not-found"></i>
            <span>No se encontraron pacientes</span>
            <span>{{ selectedBuilding == "Todos los sectores" ? 'contacta con la farmacia': 'Compruebe su filtro de red o contacte con la farmacia.'}}</span>
        </div>
    </div>
</template>


<script>
    import Dropdown from 'primevue/dropdown';
    
import InputSwitch from 'primevue/inputswitch';

    import InputText from 'primevue/inputtext';
    import PatientsMasterDataService from "@/ElderAid/Patients/Services/patients-master-data-service";
    import BuildingsMasterDataService from "@/ElderAid/Referencials/Services/buildings-master-data-service";

    export default {
        inject: ["deviceInfo"],
        components: {
            Dropdown,
            InputText,
            InputSwitch,
            
        },
        data() {
            return {
                patients: [],
                searchPatient: "",
                buildingOptions: ["Todos los sectores"],
                selectedBuilding: "Todos los sectores",
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
            onPatientClick(patient) {
                const args = { cancelNavigation: false, selectedPatient: patient };
                this.$emit("selectedPatient", args);
                if (!args.cancelNavigation) {
                    if (this.$route.name == "SearchPatientFromOrder") {
                        this.$router.push({ name: "SearchArticleFromOrder", params: { id: patient.id } });
                    }
                    else if (this.$route.name == "SearchPatientFromOrderWithArticleId") {
                        this.$router.push({ name: "OrderArticleFromOrderWithArticleId", params: { id: patient.id, articleId: this.$route.params.articleId } });
                    }
                    else
                        this.$router.push({ name: "Patient", params: { id: patient.id } });
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
            }
        },
        computed: {
            filteredPatients() {
                let patients = this.patients.filter(patient => {
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
            }
        },
        beforeUnmount() {
            // keep research in local storage
            localStorage.setItem("searchPatient", this.searchPatient);
        }
    }
</script>
<style type="text/css" scoped src="./Content/search-patient-page.css">
</style>