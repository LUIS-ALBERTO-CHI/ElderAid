<template>
    <div class="stock-consumption-page-container">
        <patient-info-component />
        <div v-if="stockConsumptions.some(stockConsumption => 'article' in stockConsumption)"
             v-for="(stock, index) in stockConsumptions" :key="index">
            <div class="stock-consumption-item">
                <span class="stock-consumption-item-title">{{stock.article.title}}</span>
                <span>{{stock.quantity}} {{ stock.article.invoicingUnit }}</span>
                <span>{{$d(new Date(stock.updatedOn))}}  à {{new Intl.DateTimeFormat('default', { hour: '2-digit', minute: '2-digit' }).format(new Date(stock.updatedOn))}} par {{stock.updatedBy}}</span>
            </div>
        </div>
    </div>
</template>
<script>

    import Button from 'primevue/button';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import StockConsumptionMasterDataService from '@/MediCare/StockConsumption/Services/stock-consumption-master-data-service'
    import ArticlesMasterDataService from "@/MediCare/Referencials/Services/articles-master-data-service";

    export default {
        components: {
            PatientInfoComponent,
            Button
        },
        data() {
            return {
                consumptionStock: [{
                    name: "ADAPTRIC pensements 7.6x7.6 stériles sach 10 pce",
                    quantity: "4 boîtes",
                    date: "23/03/2023 à 9:23:33",
                    orderedBy: "Julien SABLET",
                },
                {
                    name: "ADAPTRIC pensements 7.6x7.6 stériles sach 10 pce",
                    quantity: "3 boîtes",
                    date: "23/03/2023 à 9:23:33",
                    orderedBy: "Claire CHAGAL",
                }],
                stockConsumptions: [],
            };
        },
        async created() {
            this.stockConsumptions = await StockConsumptionMasterDataService.getAllAsync();
            this.fillStockConsumption();
        },
        methods: {
            async fillStockConsumption() {
                const stockConsumptionsArticleIds = this.stockConsumptions.map(x => x.articleId);
                const articles = await ArticlesMasterDataService.getByIdsAsync(stockConsumptionsArticleIds);
                console.log(articles)
                this.stockConsumptions.forEach(stockConsumption => {
                    const article = articles.find(article => article.id === stockConsumption.articleId)
                    stockConsumption.article = article
                })
            }
        },
        computed: {
        },

    }
</script>
<style type="text/css" scoped src="./Content/stock-consumption-page.css">
</style>