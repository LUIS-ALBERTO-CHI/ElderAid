<template>
    <div class="periodic-orders-page-container">
        <patient-info-component />
        <div v-for="(periodicOrder, index) in periodicOrders" :key="index">
            <div class="periodic-order-item">
                <span class="periodic-order-item-title">{{periodicOrder.name}}</span>
                <span >{{periodicOrder.dailyQuantity}}</span>
                <span >{{periodicOrder.neededQuantity}}</span>
                <InputNumber v-model="periodicOrder.inputValue"  ref="inputNumber" showButtons buttonLayout="horizontal" style="width: 60%; align-self: center;"
                                         decrementButtonClassName="p-button-secondary" incrementButtonClassName="p-button-secondary"
                                         incrementButtonIcon="fa fa-solid fa-plus" decrementButtonIcon="fa fa-solid fa-minus" />
            </div>
        </div>
        <div class="periodic-orders-validation-container">
            <span>Dernière validation par Alexandre DUPONT le 16/06/23 à 14:23</span>
            <Button label="Valider" style="height: 40px !important;"></Button>
        </div>
    </div>
</template>
<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import InputNumber from 'primevue/inputnumber';



    export default {
        components: {
            PatientInfoComponent,
            Button,
            InputNumber
        },
        data() {
            return {
                periodicOrders: [{
                    name: "TENA PANTS super large 12 pièces",
                    dailyQuantity: "1 pièces: 8h, 1pièces: 16h",
                    neededQuantity: "besoin de 27 pièces pour 7 prochains jours",
                    inputValue: 0,
                },
                {
                    name: "TENA PANTS extra nuit large 12 pièces",
                    dailyQuantity: "1 pièces: 22h",
                    neededQuantity: "besoin de 7 pièces pour 7 prochains jours",
                    inputValue: 0,

                },],
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
<style type="text/css" scoped src="./Content/periodic-orders-page.css">
</style>