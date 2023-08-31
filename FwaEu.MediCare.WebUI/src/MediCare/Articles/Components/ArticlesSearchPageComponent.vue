<template>
    <div>
        <div v-show="showPage" class="articles-search-page">
            <patient-info-component v-if="patient" :patient="patient" />
            <span class="command-title">Commander un article :</span>
            <span class="p-input-icon-right">
                <i @click="removeSearch" class="fa fa-solid fa-close remove-icon"
                   :style="searchValue.length === 0 ? 'opacity: 0.5;' : ''" />
                <InputText ref="searchInput" v-model="searchValue" class="search-input"
                           placeholder="Rechercher un article" />
                <i @click="goToScanCode" class="fa-sharp fa-regular fa-qrcode qr-code-icon"></i>
            </span>
            <Dropdown v-model="selectedArticleType" :options="articlesType" optionValue="id" optionLabel="text"
                      placeholder="Tous" />
            <div v-show="!isSearchActive" class="article-not-found">
                <i class="fa-solid fa-heart-pulse icon-not-found"></i>
                <span>Saisir un mot dans le champ de recherche pour afficher les articles</span>
            </div>
            <ProgressSpinner v-show="isLoading" />
            <div v-if="!isSearchResultEmpty" class="article-list">
                <div v-for="article in filteredArticles" :key="article.id">
                    <div class="article-item" @click="goToArticlePage(article)">
                        <span style="width: 80%;">{{ article.title }}</span>
                        <div class="icons-container">
                            <i v-show="article.isFavorite" class="fa-solid fa-heart favorite-icon"></i>
                            <i v-show="article.isHistory" class="fa-solid fa-clock-rotate-left history-icon"></i>
                        </div>
                    </div>
                </div>
            </div>
            <div v-else class="article-not-found">
                <i class="fa-solid fa-heart-pulse icon-not-found"></i>
                <span>Aucun article trouvé</span>
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
    import RecentArticlesMasterDataService from "../Services/recent-articles-master-data-service";
    import Dropdown from "primevue/dropdown";
    import { usePatient } from "@/MediCare/Patients/Services/patients-service";
    import ArticlesService from "../Services/articles-service";
    import ArticlesTypeMasterDataService from "../Services/articles-type-master-data-service";
    import { useRoute } from 'vue-router';
    import { ref, watch } from "vue";
    import { watchDebounced } from '@vueuse/core'
    import OnlineService from '@/fwamework/OnlineStatus/Services/online-service';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import ProgressSpinner from 'primevue/progressspinner';

    export default {
        components: {
            InputText,
            Dropdown,
            PatientInfoComponent,
            ScannerComponent,
            ProgressSpinner
        },
        setup() {
            const route = useRoute();
            const searchValue = ref(route.query.searchMode ?? "");
            const filteredArticles = ref([]);
            const selectedArticleType = ref(null);
            const articles = ref([]);
            // we don't know if there is pagination on this page so we let the variable and logic for the moment
            const currentPage = ref(0);
            const nextPage = ref(0);
            const pageSize = ref(30);
            const isSearchActive = ref(false);
            const isSearchResultEmpty = ref(false);
            const isLoading = ref(false);
            const performSearch = async () => {
                const value = searchValue.value.toLowerCase().trim();

                if (value.length >= 3) {
                    isSearchActive.value = true;
                    isLoading.value = true;
                    isSearchResultEmpty.value = false;
                    const response = await ArticlesService.getAllBySearchAsync(searchValue.value, selectedArticleType.value, nextPage.value, pageSize.value).then(result => {
                        isLoading.value = false;
                        return result;
                    });
                    filteredArticles.value = response;
                    if (response.length == 0) {
                        isSearchResultEmpty.value = true;
                    } else {
                        isSearchResultEmpty.value = false;
                    }
                } else {
                    filteredArticles.value = [];
                    isSearchActive.value = false;
                    isSearchResultEmpty.value = false;
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
                pageSize,
                isSearchActive,
                isSearchResultEmpty,
                isLoading
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
            this.articles = await RecentArticlesMasterDataService.getAllAsync();
            this.articlesType = [{ id: null, text: "Tous" }, ...(await ArticlesTypeMasterDataService.getAllAsync()),];
            this.loadInitialArticles();
            if (this.$route.query.articleFilterType)
                this.selectedArticleType = this.articlesType.find(x => x.id == this.$route.query.articleFilterType).id;
        },
        methods: {
            async loadMoreArticlesAsync() {
                if (OnlineService.isOnline()) {
                    const nextPage = this.currentPage + 1;
                    this.currentPage = nextPage;
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
                if (this.$route.name === "SearchArticleFromProtection") {
                    this.$router.push({ name: "AddPosology", params: { id: this.patient.id, articleId: article.id } });
                } else if (this.$route.name === "SearchArticleForEMSFromOrder") {
                    this.$router.push({ name: "OrderArticleForEmsFromOrder", params: { articleId: article.id } });
                }
                else {
                    this.$router.push({ name: "OrderArticle", params: { articleId: article.id } });
                }
            },
            loadInitialArticles() {
                this.performSearch();
            }
        },
        computed: {
            showPage() {
                return !this.showScanner;
            }
        }
    };
</script>

<style type="text/css" scoped src="./Content//articles-search.css"></style>
