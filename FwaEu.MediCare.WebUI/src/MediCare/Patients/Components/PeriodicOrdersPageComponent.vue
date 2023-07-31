<template>
    <div class="periodic-orders-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <div v-if="periodicOrders && periodicOrders.some(periodicOrders => 'article' in periodicOrders)"
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
        <div v-if="periodicOrders && periodicOrders.some(periodicOrders => 'article' in periodicOrders)" class="periodic-orders-validation-container">
            <span>Dernière validation par Alexandre DUPONT le 16/06/23 à 14:23</span>
            <Button @click="onSubmit" label="Valider" style="height: 40px !important;"></Button>
        </div>
        <empty-list-component v-show="periodicOrders != null && periodicOrders.length < 1" />
    </div>
</template>
<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import InputNumber from 'primevue/inputnumber';

    import PatientService, { usePatient } from "@/MediCare/Patients/Services/patients-service";
    import ArticlesMasterDataService from "@/MediCare/Articles/Services/articles-master-data-service";
    import ViewContextService from "@/MediCare/ViewContext/Services/view-context-service";
    import OrderService from "@/MediCare/Orders/Services/orders-service";
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import EmptyListComponent from '@/MediCare/Components/EmptyListComponent.vue'
    import PeriodicOrdersMasterDataService from '@/MediCare/Orders/Services/periodic-orders-master-data-service';



    export default {
        components: {
            PatientInfoComponent,
            Button,
            InputNumber,
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
                periodicOrders: null,
                patient: null,
                organization: {},
                periodicOrderValidations: null,
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();
            this.organization = ViewContextService.get();
            this.periodicOrders = await PatientService.getMasterDataByPatientId(this.patient.id, 'Protections')
            this.periodicOrderValidations = await PatientService.getMasterDataByPatientId(this.patient.id, 'PeriodicOrderValidations')
            this.fillPeriodicOrders();
        },
        methods: {
            async fillPeriodicOrders() {
                const periodicOrdersArticleIds = this.periodicOrders.map(x => x.articleId);
                const articles = await ArticlesMasterDataService.getByIdsAsync(periodicOrdersArticleIds);
                this.periodicOrders.forEach(periodicOrder => {
                    const article = articles.find(article => article.id === periodicOrder.articleId)
                    periodicOrder.article = article
                    periodicOrder.periodicQuantity = this.getPeriodicQuantity(periodicOrder, article)
                    periodicOrder.defaultPeriodicQuantity = this.getPeriodicQuantity(periodicOrder, article)
                    this.getPeriodicQuantity(periodicOrder, article)
                })
            },
            quantityNeeded(periodicOrder) {
                return `Besoin de ${periodicOrder.quantityPerDay * this.organization.periodicityOrderActivationDaysNumber} ${periodicOrder.article.invoicingUnit}
                pour ${this.organization.periodicityOrderActivationDaysNumber} prochains jours`
            },
            getPeriodicQuantity(periodicOrder, article) {
                const filteredPeriodicOrderValidations = this.periodicOrderValidations.filter(x => x.articleId == periodicOrder.articleId);
                let periodicQuantity = null;
                for (var i = 0; i < filteredPeriodicOrderValidations.length; i++) {
                    if (filteredPeriodicOrderValidations[i].orderedOn == null) {
                        periodicQuantity = filteredPeriodicOrderValidations[i].quantity;
                        return periodicQuantity;
                    }
                }
                if (periodicQuantity == null) {
                    return Math.ceil((this.organization.orderPeriodicityDays * periodicOrder.quantityPerDay) / article.countInBox);
                } 
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

            },
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/periodic-orders-page.css">
</style>