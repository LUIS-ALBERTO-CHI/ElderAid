<template>
    <div class="patient-orders-page-container">
        <patient-info-component />
        <div style="display: flex; flex-direction: column;">
            <div v-for="(order, index) in orders" :key="index">
                <AccordionOrderComponent :order="order" :isPatientUnique="true">
                    <div class="button-order-item-container">
                        <Button v-show="!isOrderDelivered(order)" severity="danger" style="height: 50px !important;"
                            label="Annuler la commande" />
                        <Button style="height: 50px !important;" label="Commander à nouveau" @click="orderAgainAsync()" />
                    </div>
                </AccordionOrderComponent>
            </div>
        </div>
        <span class="display-more-order-text">Plus de commandes</span>
    </div>
</template>
<script>

import Button from 'primevue/button';
import PatientInfoComponent from './PatientInfoComponent.vue';
import AccordionOrderComponent from '@/MediCare/Orders/Components/AccordionOrderComponent.vue'

import OrdersService from '@/MediCare/Orders/Services/orders-service';

export default {
    components: {
        PatientInfoComponent,
        Button,
        AccordionOrderComponent
    },
    data() {
        return {
            orders: [
                {
                    patientName: "Jean Dupont",
                    nurseName: "Claire Dupont",
                    medicationName: "ADAPATRIC 10 mg, comprimé pelliculé sécable",
                    date: "12/12/2020",
                    box: "4 boîtes",
                    isDelivered: true,
                    room: "A506",
                },
                {
                    patientName: "",
                    nurseName: "Claire Dupont",
                    medicationName: "ANTIDRY lotion huilde amande 500ml",
                    date: "12/12/2020",
                    box: "4 boîtes",
                    isDelivered: false,
                    room: "A809",
                }
            ]
        };
    },
    async created() {
    },
    methods: {
        isOrderDelivered(patientOrder) {
            return patientOrder.isDelivered;
        },
        async orderAgainAsync() {
            const model = { patientId: 2, articleId: 2, quantity: 1, createdOn: new Date() };

            OrdersService.saveAsync(model);
        }
    },
    computed: {
    },

}
</script>
<style type="text/css" scoped src="./Content/patient-orders-page.css"></style>