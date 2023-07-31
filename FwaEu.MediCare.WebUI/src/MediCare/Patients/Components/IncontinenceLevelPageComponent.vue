<template>
    <div class="incontinence-level-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <div class="incontinence-info-container">
            <span class="incontinence-info-label-title">Niveau d'incontinence : {{ $t(patientData.incontinenceLevel) }}</span>
            <span>Forfait journalier : {{ patientData.dailyFixedPrice }} CHF</span>
            <span>Protocole journalier saisi : {{ patientData.dailyProtocolEntered }} CHF</span>
        </div>
        <div class="incontinence-info-container">
            <span class="incontinence-info-label-title">Analyse de consommation à date</span>
            <span v-if="patientData.dateStart && patientData.dateEnd">Entre {{ $d(patientData.dateStart, 'short') }} et {{ $d(patientData.dateEnd, 'short') }}</span> 
            <Chart type="bar" :data="chartData" :options="chartOptions" />
            <span v-if="patientData.virtualDateWithoutOverPassed">Date virtuelle sans dépassement : {{ $d(patientData.virtualDateWithoutOverPassed, 'short') }} </span> 
        </div>
        <Button v-if="!isIncontinenceLevelChange" @click="changeIncontinenceLevel"
                label="Changer de niveau d'incontinence" />
        <div v-else class="incontinence-change-container">
            <span class="incontinence-info-label-title">Changer de niveau d'incontinence</span>
            <Dropdown v-model="selectedIncontinence" :options="incontinenceOptions" optionValue="id" optionLabel="text" placeholder="Légère"/>
            <Button 
                label="Confirmer" />
        </div>
    </div>
</template>

<script>
import PatientInfoComponent from './PatientInfoComponent.vue';
import Dropdown from 'primevue/dropdown';
import Button from 'primevue/button';
import Chart from 'primevue/chart';
import 'chartjs-plugin-datalabels';
import PatientService, { usePatient } from "@/MediCare/Patients/Services/patients-service";
import incontinenceLevelMasterDataService from '@/MediCare/Patients/Services/incontinence-level-master-data-service';

export default {
    components: {
        PatientInfoComponent,
        Dropdown,
        Button,
        Chart,
        // ChartComponent
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
                        anchor: 'end'
                    }
                }
            }
        };
    },
    async created() {
        this.incontinenceOptions = await incontinenceLevelMasterDataService.getAllAsync();
        this.patient = await this.patientLazy.getValueAsync();
        const patientId = this.$route.params.id;
        this.patientData = await PatientService.getIncontinenceLevelAsync(patientId);

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
    methods: {
        changeIncontinenceLevel() {
            this.isIncontinenceLevelChange = true;
        }
    },
    computed: {

    },

}
</script>
<style type="text/css" scoped src="./Content/incontinence-level-page.css"></style>