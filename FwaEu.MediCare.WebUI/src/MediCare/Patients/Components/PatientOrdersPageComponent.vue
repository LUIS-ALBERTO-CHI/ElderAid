<template>
    <div class="patient-orders-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <div style="display: flex; flex-direction: column;">
            <div v-if="patient" v-for="(order, index) in patientOrders" :key="index">
                <AccordionOrderComponent :order="order">
                    <div v-if="isOrderingIndex != index">
                        <div v-if="!isCancelConfirmationDisplayed" class="button-order-item-container">
                            <Button v-show="isOrderPending(order)" @click="showConfirmation" severity="danger" style="height: 50px !important;"
                                    label="Annuler la commande" />
                            <Button style="height: 50px !important;" label="Commander à nouveau" @click="showOrderComponent(index)" />
                        </div>
                        <div v-else class="cancel-confirmation-container">
                            <span>Etes vous sûr d'annuler la commande ?</span>
                            <div class="confirmaton-button-container">
                                <Button @click="cancelOrder(order.id)" label="OUI" outlined class="button-confirmation " />
                                <Button @click="showConfirmation()" label="NON" outlined class="button-confirmation" />
                            </div>
                        </div>
                    </div>
                    <div v-else>
                        <OrderComponent :article="getArticleInfo(order.articleId)" :patientOrders="patientOrders" @order-done="orderSubmitted" :patientId="patient.id" />
                    </div>

                </AccordionOrderComponent>
            </div>
        </div>
        <empty-list-component v-show="patientOrders != null && patientOrders.length < 1" />
        <span v-show="!isEndOfPagination" @click="getMoreOrders()" class="load-more-text">Charger d'autres commandes</span>
    </div>
</template>
<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import AccordionOrderComponent from '@/MediCare/Orders/Components/AccordionOrderComponent.vue'
    import PatientService, { usePatient } from "@/MediCare/Patients/Services/patients-service";
    import ArticlesMasterDataService from "@/MediCare/Articles/Services/articles-master-data-service";
    import OrderComponent from './OrderComponent.vue';
    import EmptyListComponent from '@/MediCare/Components/EmptyListComponent.vue'
    import OrderService from '@/MediCare/Orders/Services/orders-service'
    import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
    import OnlineService from '@/fwamework/OnlineStatus/Services/online-service';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import OrderMasterDataService from '@/MediCare/Orders/Services/orders-master-data-service';



    export default {
        components: {
            PatientInfoComponent,
            Button,
            AccordionOrderComponent,
            OrderComponent,
            EmptyListComponent
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
                patient: null,
                patientOrders: null,
                isOrderingIndex: null,
                articles: [],
                actualPage: 0,
                isEndOfPagination: false,
                isCancelConfirmationDisplayed: false
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();
            this.fillPatientOrders()
            this.articles = await ArticlesMasterDataService.getAllAsync()
        },
        methods: {
            isOrderPending(patientOrder) {
                if (patientOrder.state == 'Pending')
                    return true;
                return false;
            },
            getArticleInfo(articleId) {
                return this.articles.find(x => x.id == articleId)
            },
            showOrderComponent(index) {
                this.isOrderingIndex = index;
            },
            async getMoreOrders() {
                if (OnlineService.isOnline()) {
                    var model = {
                        patientId: this.patient.id,
                        page: this.actualPage++,
                        pageSize: Configuration.paginationSize.orders,
                    }
                    var orders = await OrderService.getAllAsync(model)
                    if (orders.length < Configuration.paginationSize.orders)
                        this.isEndOfPagination = true;
                    this.patientOrders = this.patientOrders.concat(orders)
                } else {
                    NotificationService.showError("La connexion avec le serveur a été perdue. Retentez plus tard")
                }

            },
            async orderSubmitted() {
                this.fillPatientOrders()
                this.isOrderingIndex = null;
            },
            async cancelOrder(id) {
                try {
                    await OrderService.cancelOrderAsync(id).then(() => {
                        NotificationService.showConfirmation("La commande a bien été annulée")
                        this.fillPatientOrders()
                    })
                } catch (error) {
                    NotificationService.showError("Une erreur est survenue lors de l'annulation de la commande")
                }
                this.isCancelConfirmationDisplayed = !this.isCancelConfirmationDisplayed;
            },
            showConfirmation() {
                this.isCancelConfirmationDisplayed = !this.isCancelConfirmationDisplayed;
            },
            async fillPatientOrders() {
                await OrderMasterDataService.clearCacheAsync();
                this.patientOrders = await PatientService.getMasterDataByPatientId(this.patient.id, 'Orders')
            }
        },
        computed: {
        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-orders-page.css"></style>