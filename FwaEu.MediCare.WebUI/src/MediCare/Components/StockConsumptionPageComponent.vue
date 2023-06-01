<template>
    <div class="stock-consumption-page-container">
        <patient-info-component />
        <div v-for="(consumption, index) in consumptionStock" :key="index">
            <div class="stock-consumption-item">
                <span>{{consumption.name}}</span>
                <span>{{consumption.quantity}}</span>
                <span>{{consumption.date}} par {{consumption.orderedBy}}</span>
            </div>
        </div>
    </div>
</template>
<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';



    export default {
        components: {
            PatientInfoComponent,
            Button
        },
        data() {
            return {
                consumptionStock: [{
                    name: "ADAPTRIC pensements 7.6x7.6 stériles sach 10 pce",
                    quantity: "4 boîtes",
                    date: "23/03/2023 à 9:23:33",
                    orderedBy: "Julien SABLET",
                },
                {
                    name: "ADAPTRIC pensements 7.6x7.6 stériles sach 10 pce",
                    quantity: "3 boîtes",
                    date: "23/03/2023 à 9:23:33",
                    orderedBy: "Claire CHAGAL",
                }],
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
<style type="text/css" scoped src="./Content/stock-consumption-page.css">
</style>