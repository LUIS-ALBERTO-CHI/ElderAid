<template>
    <div class="incontinence-level-page-container" />
    <patient-info-component v-if="patient" :patient="patient" />
    <span class="incontinence-info-label-title">Nivel de incontinencia : {{ $t(''+patientData.incontinenceLevel) }}</span>
    <div class="incontinence-info-container">
        <span>Forfait annuel : {{ patientData.annualFixedPrice }} CHF</span>
        <span>Forfait journalier : {{ patientData.dailyFixedPrice }} CHF</span>
        <span>Protocole journalier saisi : {{ patientData.dailyProtocolEntered }} CHF</span>
    </div>
    <div v-if="new Date(patientData.dateStart) >= new Date(new Date().getFullYear(), 0, 1)"  class="incontinence-info-container">
        <span class="incontinence-info-label-title">Analyse de consommation à date</span>
        <span v-if="patientData.dateStart && patientData.dateEnd">Entre {{ $d(patientData.dateStart, 'short') }} et {{ $d(patientData.dateEnd, 'short') }}</span>
        <Chart type="bar" :data="chartData" :options="chartOptions"   />
        <span v-if="patientData.virtualDateWithoutOverPassed">Date virtuelle sans dépassement : {{ $d(patientData.virtualDateWithoutOverPassed, 'short') }} </span>
        <span v-else>Date virtuelle sans dépassement : Aucune</span>
    </div>
    <Button v-if="isUserCanToChangeIncontinence && !isIncontinenceLevelChange" @click="changeIncontinenceLevel"
            label="Changer de niveau d'incontinence" />
    <div v-else-if="isUserCanToChangeIncontinence" class="incontinence-change-container">
        <span>A partir de <Calendar v-model="startDate" dateFormat="dd/mm/yy" style="width: 34% !important" dateOnly /></span>
        <span class="incontinence-info-label-title">Changer de niveau d'incontinence</span>
        <Dropdown v-model="selectedIncontinence" :options="incontinenceOptions" optionValue="id" optionLabel="text" placeholder="Légère" />
        <Button @click="saveIncontinenceLevelAsync"
                label="Confirmer" />
    </div>
</template>

<script>
import PatientInfoComponent from './PatientInfoComponent.vue';
import Dropdown from 'primevue/dropdown';
import Button from 'primevue/button';
import Chart from 'primevue/chart';
import 'chartjs-plugin-datalabels';
import PatientService, { usePatient } from "@/ElderAid/Patients/Services/patients-service";
import IncontinenceLevelMasterDataService from '@/ElderAid/Patients/Services/incontinence-level-master-data-service';
import Calendar from 'primevue/calendar';
import NotificationService from "@/Fwamework/Notifications/Services/notification-service";
import { hasPermissionAsync } from "@/Fwamework/Permissions/Services/current-user-permissions-service";
import { CanChangeIncontinenceLevel } from "../patients-permissions";

    export default {
        components: {
            PatientInfoComponent,
            Dropdown,
            Button,
            Chart,
            Calendar
        },
        setup() {
            const { patientLazy, getCurrentPatientAsync } = usePatient();
            return {
                patientLazy,
                getCurrentPatientAsync
            }
        },
        data() {
            return {
                isUserCanToChangeIncontinence: false,
                startDate: new Date,
                selectedIncontinence: null,
                incontinenceOptions: [],
                isIncontinenceLevelChange: false,
                chartData: null,
                patient: null,
                patientData: {},
                chartOptions: {
                    responsive: true,
                    maintainAspectRatio: false,
                    indexAxis: 'y',
                    plugins: {
                        legend: {
                            display: false
                        },
                        datalabels: {
                            color: '#000',
                            display: true,
                            align: 'end',
                        }
                    }
                }
            };
        },
        async created() {
            this.isUserCanToChangeIncontinence = await hasPermissionAsync(CanChangeIncontinenceLevel);
            this.incontinenceOptions = await IncontinenceLevelMasterDataService.getAllAsync();
            this.patient = await this.patientLazy.getValueAsync();
            await this.getPatientDataAsync();
        },
        methods: {
            async getPatientDataAsync() {
                this.patientData = await PatientService.getIncontinenceLevelAsync(this.patient.id);
                this.selectedIncontinence = this.patientData?.incontinenceLevel;
                if (this.patientData) {
                    this.chartData = {
                        labels: ['Forfait', 'Consommé', 'Dépassement'],
                        datasets: [
                            {
                                label: 'Analyse de consommation',
                                backgroundColor: ['#66BB6A', '#FFA726', '#e06666'],
                                data: [
                                    this.patientData.fixedPrice,
                                    this.patientData.consumed,
                                    this.patientData.overPassed
                                ]
                            },
                        ]
                    };
                }
            },
            changeIncontinenceLevel() {
                this.isIncontinenceLevelChange = !this.isIncontinenceLevelChange;
            },
            async saveIncontinenceLevelAsync() {
                await PatientService.saveIncontinenceLevelAsync({
                    id: this.patient.id,
                    level: this.selectedIncontinence,
                    dateStart: this.startDate,
                    dateEnd: new Date(this.startDate.getFullYear(), 11, 31, 23, 59, 59)
                }).then(async () => {
                    NotificationService.showConfirmation("Le niveau d'incontinence a été changé");
                    await this.getPatientDataAsync().then(async () => this.$forceUpdate());
                });
            }
        }
    }
</script>
<style type="text/css" scoped src="./Content/incontinence-level-page.css"></style>