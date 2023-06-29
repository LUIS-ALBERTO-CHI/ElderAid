<template>
    <div class="incontinence-level-page-container">
        <PatientInfoComponent />
        <div class="incontinence-info-container">
            <span class="incontinence-info-label-title">Niveau d'incontinence : Légère</span>
            <span>Forfait journalier : 4 CHF</span>
            <span>Protocole journalier saisi : 3,75 CHF</span>
        </div>
        <div class="incontinence-info-container">
            <span class="incontinence-info-label-title">Analyse de consommation à date</span>
            <span>Entre 1/01/2023 et 14/06/2023</span>
            <Chart type="bar" :data="chartData" :options="chartOptions" />
            <span>Date virtuelle sans dépassement : 30/05/2023</span>
        </div>
        <Button v-if="!isIncontinenceLevelChange" @click="changeIncontinenceLevel"
                label="Changer de niveau d'incontinence" />
        <div v-else class="incontinence-change-container">
            <span class="incontinence-info-label-title">Changer de niveau d'incontinence</span>
            <Dropdown v-model="selectedIncontinence" :options="incontinenceOptions" />
            <Button v-if="!isIncontinenceLevelChange" @click="changeIncontinenceLevel"
                    label="Confirmer" />
        </div>
    </div>
</template>
<!-- eslint-disable @fwaeu/custom-rules/no-local-storage -->
<script>
import PatientInfoComponent from './PatientInfoComponent.vue';
import Dropdown from 'primevue/dropdown';
import Button from 'primevue/button';
import Chart from 'primevue/chart';
import 'chartjs-plugin-datalabels';


export default {
    components: {
        PatientInfoComponent,
        Dropdown,
        Button,
        Chart
        // ChartComponent
    },
    data() {
        return {
            selectedIncontinence: "Légère",
            incontinenceOptions: ["Légère", "Moyenne", "Sévère", "Totale", "Doublée"],
            isIncontinenceLevelChange: false,
            chartData: {
                labels: ['Forfait', 'Consommé', 'Dépassement'],
                datasets: [
                    {
                        label: 'Analyse de consommation',
                        backgroundColor: ['#66BB6A', '#FFA726', '#e06666'],
                        data: [600, 640, -40]
                    },
                ]
            },
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