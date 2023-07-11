<template>
    <div class="patient-orders-page-container">
        <patient-info-component :patient="patient"/>
        <div style="display: flex; flex-direction: column;">
            <div v-for="(order, index) in patientOrders" :key="index">
                <AccordionOrderComponent :order="order" :patient="patient">
                    <div class="button-order-item-container">
                        <Button v-show="!isOrderDelivered(order)" severity="danger" style="height: 50px !important;"
                                label="Annuler la commande" />
                        <Button style="height: 50px !important;" label="Commander à nouveau" @click="orderAgainAsync(order)" />
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
    import PatientService from "@/MediCare/Patients/Services/patients-service";
    import OrdersService from '@/MediCare/Orders/Services/orders-service';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import MasterDataManagerService from "@/Fwamework/MasterData/Services/master-data-manager-service";
    import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';


    export default {
        components: {
            PatientInfoComponent,
            Button,
            AccordionOrderComponent
        },
        data() {
            return {
                patient: new AsyncLazy(async () => {
                    return await PatientService.getPatientById(this.$route.params.id);
                }),
                patientOrders: [],
            };
        },
        async created() {
            this.patient = await this.patient.getValueAsync();
            this.patientOrders = await PatientService.getMasterDataByPatientId(this.patient.id, 'Orders')

        },
        methods: {
            isOrderDelivered(patientOrder) {
                return patientOrder.isDelivered;
            },
            async orderAgainAsync(order) {
                const modelOrder = [{
                    patientId: order.patientId,
                    articleId: order.articleId,
                    quantity: order.quantity
                }];

                try {
                    await OrdersService.saveAsync(modelOrder)
                    await MasterDataManagerService.clearCacheAsync();
                    NotificationService.showConfirmation('Vous avez commander à nouveau la commande')
                } catch (error) {
                    NotificationService.showError('Une erreur est survenue lors de la commande')
                }
            },
            async getCurrentPatientAsync() {
                return await this.patient.getValueAsync();
            },
        },
        computed: {
        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-orders-page.css"></style>