<template>
    <div class="patient-orders-page-container">
        <patient-info-component v-if="patient" :patient="patient"/>
        <div style="display: flex; flex-direction: column;">
            <div v-if="patient" v-for="(order, index) in patientOrders" :key="index">
                <AccordionOrderComponent :order="order">
                    <div v-if="isOrderingIndex != index" class="button-order-item-container">
                        <Button v-show="!isOrderDelivered(order)" severity="danger" style="height: 50px !important;"
                                label="Annuler la commande" />
                        <Button style="height: 50px !important;" label="Commander à nouveau" @click="showOrderComponent(index)" />
                    </div>
                    <div v-else>
                        <OrderComponent :article="getArticleInfo(order.articleId)" :patientOrders="patientOrders"/>
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
    import PatientService, { usePatient } from "@/MediCare/Patients/Services/patients-service";
    import ArticlesMasterDataService from "@/MediCare/Referencials/Services/articles-master-data-service";
    import OrderComponent from './OrderComponent.vue';



    export default {
        components: {
            PatientInfoComponent,
            Button,
            AccordionOrderComponent,
            OrderComponent
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
                patientOrders: [],
                isOrderingIndex: null,
                articles: [],
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();
            this.patientOrders = await PatientService.getMasterDataByPatientId(this.patient.id, 'Orders')
            this.articles = await ArticlesMasterDataService.getAllAsync()
        },
        methods: {
            isOrderDelivered(patientOrder) {
                return patientOrder.isDelivered;
            },
            getArticleInfo(articleId) {
                return this.articles.find(x => x.id == articleId)
            },
            showOrderComponent(index) {
                this.isOrderingIndex = index;
            }
        },
        computed: {
        },

    }
</script>
<style type="text/css" scoped src="./Content/patient-orders-page.css"></style>