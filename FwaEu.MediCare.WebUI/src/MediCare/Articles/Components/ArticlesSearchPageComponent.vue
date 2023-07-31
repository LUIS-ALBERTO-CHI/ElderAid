<template>
    <div>
        <div v-show="showPage" class="articles-search-page">
            <patient-info-component v-if="patient" :patient="patient" />
            <span class="command-title">Commander un article:</span>
            <span class="p-input-icon-right">
                <i @click="removeSearch" class="fa fa-solid fa-close remove-icon"
                   :style="searchValue.length === 0 ? 'opacity: 0.5;' : ''" />
                <InputText ref="searchInput" v-model="searchValue" class="search-input"
                           placeholder="Rechercher un articles" />
                <i @click="goToScanCode" class="fa-sharp fa-regular fa-qrcode qr-code-icon"></i>
            </span>
            <Dropdown v-model="selectedArticleType" :options="articlesType" optionValue="id" optionLabel="text"
                      placeholder="Tous" />
            <div class="article-list">
                <div v-for="article in filteredArticles" :key="article.id">
                    <div class="article-item" @click="goToArticlePage(article)">
                        <span style="width: 80%;">{{ article.title }}</span>
                        <div class="icons-container">
                            <i v-show="article.isFavorite" class="fa-solid fa-heart favorite-icon"></i>
                            <i v-show="article.isHistory" class="fa-solid fa-clock-rotate-left history-icon"></i>
                        </div>
                    </div>
                </div>
                <span @click="loadMoreArticlesAsync" class="load-more-text">Plus d'articles</span>
            </div>
        </div>
        <div v-if="showScanner">
            <ScannerComponent @codeScanned="handleCodeScanned" @cancelScan="handleCancelScan"></ScannerComponent>
        </div>
    </div>
</template>
<script>
    import PatientInfoComponent from "@/MediCare/Patients/Components/PatientInfoComponent.vue";
    import ScannerComponent from "@/MediCare/Components/ScanCodeComponent.vue";
    import InputText from "primevue/inputtext";
    import ArticlesMasterDataService from "@/MediCare/Referencials/Services/articles-master-data-service";
    import Dropdown from "primevue/dropdown";
    import PatientService, { usePatient } from "@/MediCare/Patients/Services/patients-service";
    import ArticlesService from "@/MediCare/Referencials/Services/articles-service";
    import ArticlesTypeMasterDataService from "@/MediCare/Referencials/Services/articles-type-master-data-service";
    import { useRoute } from 'vue-router';
    import { ref, watch } from "vue";
    import { watchDebounced } from '@vueuse/core'
    import OnlineService from '@/fwamework/OnlineStatus/Services/online-service';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';

    export default {
        components: {
            InputText,
            Dropdown,
            PatientInfoComponent,
            ScannerComponent,
        },

        setup() {
            const route = useRoute();
            const searchValue = ref(route.query.searchMode ?? "");
            const filteredArticles = ref([]);
            const selectedArticleType = ref(null);
            const articles = ref([]);
            const currentPage = ref(0);
            const nextPage = ref(0);
            const pageSize = ref(30);
            const performSearch = async () => {
                const value = searchValue.value.toLowerCase().trim();
                if (!value) {
                    filteredArticles.value = articles.value;
                } else if (value.length >= 3) {
                    const response = await ArticlesService.getAllBySearchAsync(searchValue.value, selectedArticleType.value, nextPage.value, pageSize.value);
                    filteredArticles.value = response.articles;
                } else {
                    filteredArticles.value = [];
                }
                if (selectedArticleType.value) {
                    filteredArticles.value = filteredArticles.value.filter(
                        (article) => article.articleType == selectedArticleType.value
                    );
                }
            };

            const { patientLazy, getCurrentPatientAsync } = usePatient();
            const watchSearchValue = watchDebounced([searchValue, selectedArticleType], performSearch, { debounce: 500 });
            const watchResetPage = watch([searchValue, selectedArticleType], () => currentPage.value == 0);

            return {
                patientLazy,
                getCurrentPatientAsync,
                searchValue,
                filteredArticles,
                selectedArticleType,
                watchSearchValue,
                articles,
                performSearch,
                currentPage,
                watchResetPage,
                nextPage,
                pageSize
            };
        },
        data() {
            return {
                patient: null,
                showScanner: false,
                articlesType: [],
                selectedArticle: null,
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();
            this.focusSearchBar();
            this.articles = await ArticlesMasterDataService.getAllAsync();
            this.articlesType = [{ id: null, text: "Tous" }, ...(await ArticlesTypeMasterDataService.getAllAsync()),];
            this.loadInitialArticles();
        },
        methods: {
            async loadMoreArticlesAsync() {
                if (OnlineService.isOnline()) {
                    const nextPage = this.currentPage + 1;
                    this.currentPage = nextPage;
                    this.performSearch();
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
            goToScanCode() {
                this.showScanner = true;
            },
            handleCodeScanned(data) {
                this.searchValue = data.qrCodeText;
                this.showScanner = false;
            },
            handleCancelScan() {
                this.showScanner = false;
            },
            goToArticlePage(article) {
                this.selectedArticle = article;
                this.$router.push({
                    name: "OrderArticle",
                    params: { articleId: article.id }
                });
            },
            loadInitialArticles() {
                this.performSearch();
            },
        },
        computed: {
            showPage() {
                return !this.showScanner;
            },
        },
    };
</script>

<style type="text/css" scoped src="./Content//articles-search.css"></style>
