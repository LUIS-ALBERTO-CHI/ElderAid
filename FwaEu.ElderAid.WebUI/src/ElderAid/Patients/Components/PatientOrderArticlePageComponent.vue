<template>
    <div v-if="!isLoading" class="order-article-page-container">
        <patient-info-component v-if="patient != undefined" :patient="patient" />
        <div class="article-title-container">
            <span style="width: 90%;" class="command-title">{{ article.title }}</span>
            <i class="fa-solid fa-heart"></i>
        </div>
        <div v-if="!isGalleryDisplayed && patientOrders" class="article-label-container">
            <i class="fa-solid fa-clock-rotate-left history-icon"></i>
            <span>{{ articleLastOrderForPatient() }}</span>
        </div>
        <div v-show="!isGalleryDisplayed" class="article-container">
            <div class="article-info-container">
                <div class="article-label-container">
                    <span style="font-weight: bold;">$</span> <!--PRECIO DEL PRODUCTO --> 
                    <span>{{ article.price }} </span>
                </div>
                <div v-show="article.leftAtChargeExplanation" class="article-label-container">
                    <i class="fa-solid fa-money-bill-1"></i>
                    <span><!--{{ article.leftAtChargeExplanation }},--> Gastos de bolsillo</span>
                </div>
                <div class="article-label-container">
                    <i class="fa-regular fa-box"></i>
                    <span>{{ article.countInBox }} {{ article.invoicingUnit }} <!--{{ article.unit }}--></span> <!--Unidades que se deben de tomar o aplicar-->
                </div>
            </div>
            <img v-if="images?.length" @click="displayGallery" class="article-image" :src="getImageUrl(this.images[0].imageType, true)" />
        </div>
        <div v-show="isGalleryDisplayed" class="gallery-area-container">
            <div @click="displayGallery" class="gallery-return-back-container">
                <i class="fa-solid fa-arrow-left history-icon"></i>
                <span>Regresar</span>
            </div>
            <div class="galleria-container">
                <Galleria :value="gallery" :numVisible="5" containerStyle="max-width: 640px" :showThumbnails="false"
                          :showIndicators="true" :showItemNavigators="true">
                    <template #item="slotProps">
                        <img :src="slotProps.item.itemImageSrc" alt="Image" style="width: 100%; display: block" />
                    </template>
                </Galleria>
            </div>
        </div>
        <AddPosologyComponent v-if="isAddPosologyPage && patient" :article="article" :patient="patient" />
        <div v-else>
            <OrderComponent v-if="!isOrderSubmitted" :article="article" :patientOrders="patientOrders" :patientId="getPatientId()" />
            <div v-else class="order-submitted-container">
                <span>¡Pedido completado con éxito!</span>
                <span>Próxima acción : </span>
                <Button style="height: 45px !important; width: 100%; display: flex; justify-content: space-between; align-items: center;">
                    <span style="text-align: center;">Ver órdenes abiertas para {{ patient.fullName }}</span>
                    <i class="fa fa-solid fa-angle-right"></i>
                </Button>
                <Button style="height: 45px !important; width: 100%; display: flex; justify-content: space-between; align-items: center;">
                    <span style="text-align: center;">Pedir otro artúcilo por {{ patient.fullName }}</span>
                    <i class="fa fa-solid fa-angle-right"></i>
                </Button>
                <Button style="height: 45px !important; width: 100%; display: flex; justify-content: space-between; align-items: center;">
                    <span style="text-align: center;">Consultar ficha del paciente {{ patient.fullName }}</span>
                    <i class="fa fa-solid fa-angle-right"></i>
                </Button>
                <Button style="height: 45px !important; width: 100%; display: flex; justify-content: space-between; align-items: center;">
                    <span style="text-align: center;">Regresar al inicio{{ patient.fullName }}</span>
                    <i class="fa fa-solid fa-angle-right"></i>
                </Button>
            </div>
        </div>
    </div>
    <div v-else>
        <ProgressSpinner v-if="isLoading" style="position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%);" />
        <span v-if="isLoading" style="position: absolute; top: 60%; left: 50%; transform: translateX(-50%); font-size: 16px;">Carga...</span>
    </div>
</template>
<script>

    import PatientInfoComponent from './PatientInfoComponent.vue';
    import OrderComponent from './OrderComponent.vue';
    import Button from 'primevue/button';
    import RecentArticlesMasterDataService from '@/ElderAid/Articles/Services/recent-articles-master-data-service';
    import PatientService, { usePatient } from "@/ElderAid/Patients/Services/patients-service";
    import Galleria from 'primevue/galleria';
    import AddPosologyComponent from '@/ElderAid/Patients/Components/AddPosologyComponent.vue'
    import ArticleService from '@/ElderAid/Articles/Services/articles-service'
    import ProgressSpinner from 'primevue/progressspinner';

    export default {
        components: {
            PatientInfoComponent,
            OrderComponent,
            Button,
            Galleria,
            AddPosologyComponent,
            ProgressSpinner,
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
                gallery: [],
                isGalleryDisplayed: false,
                images: null,
                isLoading: true
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();

            const articleId = this.$route.params.articleId;
            if (articleId) {
                const [article] = await RecentArticlesMasterDataService.getByIdsAsync([articleId]);
                if (article != null) {
                    this.article = article;
                }
                else {
                    [this.article] = await ArticleService.getByIdsAsync([this.$route.params.articleId])
                }
            }
            if (this.patient)
                this.patientOrders = await PatientService.getMasterDataByPatientId(this.patient.id, 'Orders').then((orders) => {
                    return orders.filter(order => order.state != 'Cancelled');
                });
            this.images = await ArticleService.getArticlesImageAsync(this.article.pharmaCode);
            this.loadGallery();
            this.isLoading = false;
        },
        methods: {
            loadGallery() {
                for (var i = 0; i != this.images.length; i++) {
                    this.gallery.push({
                        itemImageSrc: this.getImageUrl(this.images[i].imageType, false),
                    })
                }
            },
            getImageUrl(imageType, thumbnail) {

                const imageSize = thumbnail ? "T" : "F";

                var urlImage = "https://documedis.hcisolutions.ch/2020-01/api/products/image/" + imageType + "/Pharmacode/" + this.article.pharmaCode + "/" + imageSize;

                return urlImage;
            },
            displayGallery() {
                this.isGalleryDisplayed = !this.isGalleryDisplayed;
            },
            getPatientId() {
                return parseInt(this.$route.params.id);
            },
            articleLastOrderForPatient() {
                const orders = this.patientOrders.filter(order => order.articleId == this.article.id)
                const order = orders.sort((a, b) => new Date(b.updatedOn) - new Date(a.updatedOn));
                if (order[0])
                    return "Último pedido " + new Date(order[0].updatedOn).toLocaleDateString('fr-FR');
                else
					return "No hay pedidos recientes de este artículo"
            }
        },
        computed: {
            isAddPosologyPage() {
                return this.$route.name == "AddPosology";
            }
        },

    }
</script>
<style type="text/css" scoped src="./Content/order-article-page.css"></style>