<template>
    <div class="order-article-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <div class="article-title-container">
            <span style="width: 90%;" class="command-title">{{ article.title }}</span>
            <i class="fa-solid fa-heart"></i>
        </div>
        <div class="article-label-container">
            <i class="fa-solid fa-clock-rotate-left history-icon"></i>
            <span>Utilisé par le patient en mars 2022</span>
        </div>
        <div class="article-container">
            <div class="article-info-container">
                <div class="article-label-container">
                    <span style="font-weight: bold;">{{ article.unit }}</span>
                    <span>{{ article.price }}</span>
                </div>
                <div class="article-label-container">
                    <i class="fa-solid fa-money-bill-1"></i>
                    <span>{{ article.leftAtChargeExplanation }}, reste à charge</span>
                </div>
                <div class="article-label-container">
                    <i class="fa-regular fa-box"></i>
                    <span>{{ article.unit }} de {{ article.countInBox }} {{ article.invoicingUnit }}</span>
                </div>
            </div>
            <img class="article-image" :src="article.imageURLs" />
        </div>

        <OrderComponent v-if="!isOrderSubmitted" :article="article" :patientOrders="patientOrders" />
        <div v-else class="order-submitted-container">
            <span>Commande réalisée avec succès !</span>
            <span>Votre prochaine action : </span>
            <Button style="height: 45px !important; width: 100%; display: flex; justify-content: space-between; align-items: center;">
                <span style="text-align: center;">Voir les commandes en cours pour {{ patient.fullName }}</span>
                <i class="fa fa-solid fa-angle-right"></i>
            </Button>
            <Button style="height: 45px !important; width: 100%; display: flex; justify-content: space-between; align-items: center;">
                <span style="text-align: center;">Commander un autre article pour {{ patient.fullName }}</span>
                <i class="fa fa-solid fa-angle-right"></i>
            </Button>
            <Button style="height: 45px !important; width: 100%; display: flex; justify-content: space-between; align-items: center;">
                <span style="text-align: center;">Consulter la fiche du patient {{ patient.fullName }}</span>
                <i class="fa fa-solid fa-angle-right"></i>
            </Button>
            <Button style="height: 45px !important; width: 100%; display: flex; justify-content: space-between; align-items: center;">
                <span style="text-align: center;">Revenir à l'accueil {{ patient.fullName }}</span>
                <i class="fa fa-solid fa-angle-right"></i>
            </Button>
        </div>
    </div>
</template>
<script>

import PatientInfoComponent from './PatientInfoComponent.vue';
import OrderComponent from './OrderComponent.vue';
import Button from 'primevue/button';
import ArticlesMasterDataService from '@/MediCare/Referencials/Services/articles-master-data-service';
import PatientService, { usePatient } from "@/MediCare/Patients/Services/patients-service";



export default {
    components: {
        PatientInfoComponent,
        OrderComponent,
        Button
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
            article: {},
            isOrderSubmitted: false,
            patientOrders: [],
        };
    },
    async created() {
        this.patient = await this.patientLazy.getValueAsync();
        const articleId = this.$route.params.articleId;
        if (articleId) {
            const [article] = await ArticlesMasterDataService.getByIdsAsync([articleId]);
            this.article = article;
        }
        if (this.patient)
            this.patientOrders = await PatientService.getMasterDataByPatientId(this.patient.id, 'Orders')
    },
    methods: {

    },
    computed: {

    },

}
</script>
<style type="text/css" scoped src="./Content/order-article-page.css"></style>