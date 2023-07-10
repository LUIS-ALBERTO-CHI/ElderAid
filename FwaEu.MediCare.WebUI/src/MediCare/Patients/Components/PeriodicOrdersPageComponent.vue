<template>
    <div class="periodic-orders-page-container">
        <patient-info-component />
        <div v-if="periodicOrders.some(periodicOrders => 'article' in periodicOrders)"
             v-for="(periodicOrder, index) in periodicOrders" :key="index">
            <div class="periodic-order-item">
                <span class="periodic-order-item-title">{{periodicOrder.article.title}}</span>
                <span>{{periodicOrder.dosageDescription}}</span>
                <span>{{ quantityNeeded(periodicOrder) }}</span>
                <InputNumber v-model="periodicOrder.periodicQuantity" ref="inputNumber" showButtons buttonLayout="horizontal" style="width: 60%; align-self: center;"
                             decrementButtonClassName="p-button-secondary" incrementButtonClassName="p-button-secondary"
                             incrementButtonIcon="fa fa-solid fa-plus" decrementButtonIcon="fa fa-solid fa-minus" />
            </div>
        </div>
        <div class="periodic-orders-validation-container">
            <span>Dernière validation par Alexandre DUPONT le 16/06/23 à 14:23</span>
            <Button @click="onSubmit" label="Valider" style="height: 40px !important;"></Button>
        </div>
    </div>
</template>
<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import InputNumber from 'primevue/inputnumber';
    import PatientService from "@/MediCare/Patients/Services/patients-service";
    import ArticlesMasterDataService from "@/MediCare/Referencials/Services/articles-master-data-service";
    import ViewContextService from "@/MediCare/ViewContext/Services/view-context-service";
    import OrderService from "@/MediCare/Orders/Services/orders-service";
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';


    export default {
        components: {
            PatientInfoComponent,
            Button,
            InputNumber
        },
        data() {
            return {
                periodicOrders: [],
                patient: {},
                organization: {},
            };
        },
        async created() {
            this.organization = ViewContextService.get();
            this.patient = JSON.parse(localStorage.getItem('patient'));
            this.periodicOrders = await PatientService.getMasterDataByPatientId(this.patient.id, 'Protections')
            this.fillPeriodicOrders();
        },
        methods: {
            async fillPeriodicOrders() {
                const periodicOrdersArticleIds = this.periodicOrders.map(x => x.articleId);
                const articles = await ArticlesMasterDataService.getByIdsAsync(periodicOrdersArticleIds);
                this.periodicOrders.forEach(periodicOrder => {
                    const article = articles.find(article => article.id === periodicOrder.articleId)
                    periodicOrder.article = article
                    periodicOrder.periodicQuantity = Math.ceil((this.organization.orderPeriodicityDays * periodicOrder.quantityPerDay) / article.countInBox);
                    periodicOrder.defaultPeriodicQuantity = Math.ceil((this.organization.orderPeriodicityDays * periodicOrder.quantityPerDay) / article.countInBox);
                })
            },
            quantityNeeded(periodicOrder) {
                return `Besoin de ${periodicOrder.quantityPerDay * this.organization.periodicityOrderActivationDaysNumber} ${periodicOrder.article.invoicingUnit}
                pour ${this.organization.periodicityOrderActivationDaysNumber} prochains jours`
            },
            async onSubmit() {
                var periodicOrders = this.periodicOrders.map(x => {
                    return {
                        articleId: x.articleId,
                        quantity: x.periodicQuantity,
                        defaultQuantity: x.defaultPeriodicQuantity
                    }
                })

                var model = {
                    articles: periodicOrders,
                    patientId: this.patient.id,
                }

                try {
                    OrderService.validatePeriodicOrderAsync(model);
                    NotificationService.showConfirmation('Commandes périodiques validées')
                } catch (error) {
                    NotificationService.showError('Une erreur est survenue lors de la validation des commandes périodiques')
                }

            }
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/periodic-orders-page.css">
</style>