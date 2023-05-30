<template>
    <div class="patient-orders-page-container">
        <patient-info-component />
        <div class="patient-orders">
            <div v-for="(patientOrder, index) in patientOrders" :key="patientOrder.name">
                <div @click="displayButton(index)" class="patient-order-item" :class="getStateColor(patientOrder)">
                    <span class="patient-order-title">{{patientOrder.name}}</span>
                    <span class="patient-order-subtitle">{{patientOrder.quantity}}</span>
                    <div class="patient-order-item-bottom-container">
                        <span class="patient-order-subtitle">{{patientOrder.date}}</span>
                        <span class="patient-order-subtitle">{{patientOrder.state}}</span>

                    </div>
                    <div class="button-order-item-container" v-show="selectedItemIndex === index">
                        <Button severity="danger" style="height: 50px !important;" label="Annuler la commande" />
                        <Button style="height: 50px !important;" label="Commander à nouveau" />
                    </div>
                </div>
            </div>
        </div>
        <span class="display-more-order-text">Plus de commandes</span>
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
                patientOrders: [{
                    name: "ADAPTRIC pensements 7.6x7.6 stériles sach 10 pce",
                    quantity: "4 boîtes",
                    date: "23/03/2023 à 9:23:33",
                    state: "En attente"
                },
                {
                    name: "ADAPTRIC pensements 7.6x7.6 stériles sach 10 pce",
                    quantity: "3 boîtes",
                    date: "23/03/2023 à 9:23:33",
                    state: "Livrée"
                }],
                selectedItemIndex: -1,
            };
        },
        async created() {
            var patient = localStorage.getItem("patient");
            this.patient = JSON.parse(patient);
        },
        methods: {
            displayButton(index) {
                this.selectedItemIndex = index;
            },
            getStateColor(patientOrder) {
                if (patientOrder.state === "En attente") {
                    return "patient-order-item-waiting";
                }
                else if (patientOrder.state === "Livrée") {
                    return "patient-order-item-delivered";
                }
                else
                    return "patient-order-item-waiting";
            }
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-orders-page.css">
</style>