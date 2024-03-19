<template>
    <div class="stock-consumption-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <div v-if="stockConsumptions && stockConsumptions.some(stockConsumption => 'article' in stockConsumption)"
             v-for="(stock, index) in stockConsumptions" :key="index">
            <div class="stock-consumption-item">
                <span class="stock-consumption-item-title">{{ stock.article?.title }}</span>
                <span>{{ stock.quantity }} {{ stock.article?.invoicingUnit }}</span>
                <span>
                    {{$d(new Date(stock.updatedOn))}}  a las {{ new Intl.DateTimeFormat('default', { hour: '2-digit', minute: '2-digit' }).format(new Date(stock.updatedOn)) }} por {{ stock.updatedBy }}
                </span>
            </div>
        </div>
        <empty-list-component v-show="stockConsumptions != null && stockConsumptions.length < 1" />
        <span v-show="!isEndOfPagination" @click="getMoreStocks()" class="load-more-text">Carga más</span>
    </div>
</template>

<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import StockConsumptionMasterDataService from '@/ElderAid/StockConsumption/Services/stock-consumption-master-data-service'
    import RecentArticlesMasterDataService from "@/ElderAid/Articles/Services/recent-articles-master-data-service";
    import { usePatient } from "@/ElderAid/Patients/Services/patients-service";
    import EmptyListComponent from '@/ElderAid/Components/EmptyListComponent.vue'
    import StockConsumptionService from '@/ElderAid/StockConsumption/Services/stock-consumption-service'
    import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
    import OnlineService from '@/Fwamework/OnlineStatus/Services/online-service';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';

    export default {
        components: {
            PatientInfoComponent,
            Button,
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
                stockConsumptions: null,
                actualPage: 0,
                isEndOfPagination: false
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();
            this.stockConsumptions = (await StockConsumptionMasterDataService.getAllAsync()).filter(x => x.patientId === this.patient.id)
            this.fillStockConsumption();
        },
        methods: {
            async fillStockConsumption() {
                const stockConsumptionsArticleIds = this.stockConsumptions.filter(x => !x.article).map(x => x.articleId);
                const articles = await RecentArticlesMasterDataService.getByIdsAsync(stockConsumptionsArticleIds);
                this.stockConsumptions.forEach(stockConsumption => {
                    if (!stockConsumption.article) {
                        const article = articles.find(article => article.id === stockConsumption.articleId)
                        stockConsumption.article = article
                    }
                })
            },
            async getMoreStocks() {
                if (OnlineService.isOnline()) {
                    var model = {
                        patientId: this.patient.id,
                        page: this.actualPage++,
                        pageSize: Configuration.paginationSize.stockConsumptions,
                    }

                    var stockConsumptions = await StockConsumptionService.getAllAsync(model)
                    if (stockConsumptions.length < Configuration.paginationSize.stockConsumptions)
                        this.isEndOfPagination = true;

                    this.stockConsumptions = this.stockConsumptions.concat(stockConsumptions)
                    this.fillStockConsumption();

                } else {
                    NotificationService.showError("Se perdió la conexión con el servidor. Vuelva a intentarlo más tarde")
                }
            }
        },
        computed: {
        },
    }
</script>
<style type="text/css" scoped src="./Content/stock-consumption-page.css">
</style>