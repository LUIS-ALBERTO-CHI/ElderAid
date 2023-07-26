<template>
    <div>
        <div v-show="showPage" class="page-articles">
            <span>Articles en stock</span>
            <span class="p-input-icon-right">
                <i @click="removeSearch" class="fa fa-solid fa-close remove-icon"
                    :style="searchValue.length === 0 ? 'opacity: 0.5;' : ''" />
                <InputText ref="searchInput" v-model="searchValue" class="search-input" placeholder="Rechercher un article">
                </InputText>
                <i @click="goToScanCode" class="fa-sharp fa-regular fa-qrcode qr-code-icon"></i>
            </span>
            <div class="vignette-list">
                <div v-for="stockPharmacy in filteredArticles" :key="stockPharmacy.id">
                    <div @click="goToArticleDetails(stockPharmacy)" class="vignette-item">
                        <span>{{ stockPharmacy.article.title }}, {{ stockPharmacy.article.unit }}</span>
                        <span>{{ stockPharmacy.article.countInBox }}</span>
                    </div>
                </div>
                <span @click="loadMoreArticlesAsync" class="load-more-text">Plus d'articles</span>
            </div>
            <div v-show="filteredArticles.length === 0" class="article-not-found">
                <i class="fa-solid fa-box-open icon-not-found"></i>
                <span>Aucun article trouvé</span>
            </div>
        </div>
        <div v-if="showScanner">
            <ScannerComponent @codeScanned="handleCodeScanned" @cancelScan="handleCancelScan"></ScannerComponent>
        </div>
    </div>
</template>

<script>
import InputText from 'primevue/inputtext';
import CabinetsMasterDataService from "@/MediCare/Referencials/Services/cabinets-master-data-service";
import ScannerComponent from '@/MediCare/Components/ScanCodeComponent.vue';
import ArticlesInStockService from '@/MediCare/PharmacyStock/Services/search-articles-in-stock-service';
import { useRoute, useRouter } from 'vue-router';
import { ref, watch } from "vue";
import { watchDebounced } from '@vueuse/core'
import OnlineService from '@/fwamework/OnlineStatus/Services/online-service';
import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
import ArticlesService from '@/MediCare/Referencials/Services/articles-service';
import ArticlesMasterDataService from "@/MediCare/Referencials/Services/articles-master-data-service";


export default {
    components: {
        InputText,
        ScannerComponent
    },
        setup() {
        const route = useRoute();
        const cabinetId = route.params.id;
        const searchValue = ref(route.query.searchMode ?? "");
        const filteredArticles = ref([]);
        const stockPharmacy = ref([]);
        const cabinet = ref(cabinetId);
        const currentPage = ref(0);
        const nextPage = ref(0);
        const pageSize = ref(30);
        const performSearch = async () => {
            const value = searchValue.value.toLowerCase().trim();
            if (!value) {
                filteredArticles.value = stockPharmacy.value;
            } else if (value.length >= 3) {
                const response = await ArticlesInStockService.getAllAsync(cabinet.value, value, nextPage.value, pageSize.value);
                filteredArticles.value = await ArticlesService.fillArticlesAsync(response);
            } else {
                filteredArticles.value = [];
            }
        };

        const watchSearchValue = watchDebounced([searchValue], performSearch, { debounce: 500 });
        const watchResetPage = watch([searchValue], () => currentPage.value == 0);

        return {
            searchValue,
            filteredArticles,
            watchSearchValue,
            stockPharmacy,
            performSearch,
            currentPage,
            watchResetPage,
            nextPage,
            pageSize,
            cabinet
        };
    },
    data() {
        return {
            cabinetName: '',
            showScanner: false,
        };
    },
    async created() {
        this.focusSearchBar();
        await this.getCurrentCabinetAsync();
        this.loadInitialArticles();
        this.stockPharmacy = await ArticlesMasterDataService.getAllAsync();
    },
    methods: {
            async loadMoreArticlesAsync() {
            if (OnlineService.isOnline()) {
                const nextPage = this.currentPage + 1;
                this.currentPage = nextPage;
                await this.performSearch();
            } else {
                NotificationService.showError("La connexion avec le serveur a été perdue. Retentez plus tard")
            }
        },
        removeSearch() {
            this.searchValue = "";
            this.focusSearchBar();
        },
        focusSearchBar() {
            this.$nextTick(() => {
                this.$refs.searchInput.$el.focus();
            });
        },
        goToArticleDetails(stockPharmacy) {
            const selectedArticleData = {
                title: stockPharmacy.article.title,
                unit: stockPharmacy.article.unit,
                countInBox: stockPharmacy.article.countInBox,
                articleType: stockPharmacy.article.articleType,
                groupName: stockPharmacy.article.groupName
            };
            this.$router.push({ name: 'Articles', query: { selectedArticle: JSON.stringify(selectedArticleData) } });
        },
        goToScanCode() {
            this.showScanner = true;
        },
        async getCurrentCabinetAsync() {
            const cabinetId = this.$route.params.id;
            const cabinet = await CabinetsMasterDataService.getAsync(cabinetId);
            this.cabinetName = cabinet.name;
            return cabinet;
        },
        handleCodeScanned(data) {
            this.searchValue = data.qrCodeText;
            this.showScanner = false;
        },
        handleCancelScan() {
            this.showScanner = false;
        },
        loadInitialArticles() {
            this.performSearch();
        },
    },
    computed: {
        showPage() {
            return !this.showScanner
        }
    }
};
</script>
<style type="text/css" scoped src="./Content/articles.css"></style>