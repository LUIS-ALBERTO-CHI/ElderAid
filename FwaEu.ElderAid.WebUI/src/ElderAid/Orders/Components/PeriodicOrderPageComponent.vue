<template>
    <div class="periodic-orders-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <div v-if="periodicOrders && periodicOrders.some(periodicOrders => 'article' in periodicOrders)"
             v-for="(periodicOrder, index) in periodicOrders.filter(periodicOrder => periodicOrder.article != null)" :key="index">
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
            <div v-if="validatedById">
                <span>{{showPeriodicOrderValidationUser(validatedById, validatedOn)}}</span>
            </div>
            <Button @click="onSubmit" label="Valider" style="height: 40px !important;"></Button>
        </div>
        <empty-list-component v-show="periodicOrders != null && periodicOrders.length < 1" />
    </div>
</template>
<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from '../../Patients/Components/PatientInfoComponent.vue';
    import InputNumber from 'primevue/inputnumber';

    import PatientService, { usePatient } from "@/ElderAid/Patients/Services/patients-service";
    import RecentArticlesMasterDataService from "@/ElderAid/Articles/Services/recent-articles-master-data-service";
    import ViewContextService from "@/ElderAid/ViewContext/Services/view-context-service";
    import OrderService from "@/ElderAid/Orders/Services/orders-service";
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import EmptyListComponent from '@/ElderAid/Components/EmptyListComponent.vue'
    import PeriodicOrdersMasterDataService from "@/ElderAid/Orders/Services/periodic-orders-master-data-service";
    import UserFormatterService from "@/Fwamework/Users/Services/user-formatter-service";
    import UserService from '@/Fwamework/Users/Services/user-service';

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
                validatedById: null,
                validatedOn: null,
                periodicOrderValidationUserDate: null,
                users: null
            };
        },
        async created() {
            this.users = await UserService.getAllAsync();
            this.patient = await this.patientLazy.getValueAsync();
            this.organization = ViewContextService.get();
            this.periodicOrders = (await PatientService.getMasterDataByPatientId(this.patient.id, 'Protections')).filter(x => new Date(x.dateEnd) > new Date())
            this.periodicOrderValidations = await PatientService.getMasterDataByPatientId(this.patient.id, 'PeriodicOrderValidations')
            this.fillPeriodicOrders();
        },
        methods: {
            async fillPeriodicOrders() {
                const periodicOrdersArticleIds = this.periodicOrders.map(x => x.articleId);
                const articles = await RecentArticlesMasterDataService.getByIdsAsync(periodicOrdersArticleIds);
                this.periodicOrders.forEach(periodicOrder => {
                    const article = articles.find(article => article.id === periodicOrder.articleId);
                    if (article != null) {
                        periodicOrder.article = article
                        periodicOrder.periodicQuantity = this.getPeriodicQuantity(periodicOrder, article)
                        periodicOrder.defaultPeriodicQuantity = this.getPeriodicQuantity(periodicOrder, article)
                    }
                })
            },
            quantityNeeded(periodicOrder) {
                return `Besoin de ${periodicOrder.quantityPerDay * this.organization.orderPeriodicityDays} ${periodicOrder.article.invoicingUnit}
                pour ${this.organization.orderPeriodicityDays} prochains jours`
            },
            getPeriodicQuantity(periodicOrder, article) {
                const filteredPeriodicOrderValidations = this.periodicOrderValidations.filter(x => x.articleId == periodicOrder.articleId);
                let periodicQuantity = null;

                for (var i = 0; i < filteredPeriodicOrderValidations.length; i++) {
                    if (filteredPeriodicOrderValidations[i].orderedOn == null) {
                        periodicQuantity = filteredPeriodicOrderValidations[i].quantity;
                        this.validatedById = filteredPeriodicOrderValidations[i].validatedById;
                        this.validatedOn = filteredPeriodicOrderValidations[i].validatedOn;
                        return periodicQuantity;
                    }
                }
                if (periodicQuantity == null) {
                    return Math.ceil((this.organization.orderPeriodicityDays * periodicOrder.quantityPerDay) / article.countInBox);
                }
            },

            showPeriodicOrderValidationUser(userId, date) {
                const user = this.users.find(u => u.id === userId);
                const userName = UserFormatterService.getUserFullName(user);
                return `Dernière validation par ${userName} le ${new Date(date).toLocaleDateString() } à ${new Intl.DateTimeFormat('default', { hour: '2-digit', minute: '2-digit' }).format(new Date(date))}`;
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
                    await OrderService.validatePeriodicOrderAsync(model).then(() => {
                        NotificationService.showConfirmation('Commandes périodiques validées')
                        PeriodicOrdersMasterDataService.clearCacheAsync();
                        this.$router.push({ name: 'PeriodicOrders' })
                    })
                } catch (error) {
                    NotificationService.showError('Une erreur est survenue lors de la validation des commandes périodiques')
                }
            }
        }
    }
</script>
<style type="text/css" scoped src="./Content/periodic-orders-page.css"></style>