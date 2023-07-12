<template>
    <div>
        <div v-show="showPage" class="articles-search-page">
            <patient-info-component />
            <span class="command-title">Commander un article:</span>
            <span class="p-input-icon-right">
                <i @click="removeSearch" class="fa fa-solid fa-close remove-icon"
                    :style="searchValue.length === 0 ? 'opacity: 0.5;' : ''" />
                <InputText ref="searchInput" v-model="searchValue" class="search-input"
                    placeholder="Rechercher un articles">
                </InputText>
                <i @click="goToScanCode" class="fa-sharp fa-regular fa-qrcode qr-code-icon"></i>
            </span>
            <Dropdown v-model="selectedArticleType" :options="articlesType" optionValue="id" optionLabel="text"
                placeholder="Select an article type" />
            <div class="article-list">
                <div v-for="articles in filteredArticles" :key="articles.id">
                    <div class="article-item" @click="goToArticlePage">
                        <span style="width: 80%;">{{ articles.title }}</span>
                        <div class="icons-container">
                            <i class="fa-solid fa-heart favorite-icon"></i>
                            <i class="fa-solid fa-clock-rotate-left history-icon"></i>
                        </div>
                    </div>
                </div>
            </div>
            <span class="more-articles-text">Plus d'articles</span>
        </div>
        <div v-show="showScanner">
            <ScannerComponent @codeScanned="handleCodeScanned" @cancelScan="handleCancelScan"></ScannerComponent>
        </div>
    </div>
</template>
<script>
import PatientInfoComponent from '../../Patients/Components/PatientInfoComponent.vue';
import ScannerComponent from '../../Components/ScanCodeComponent.vue';
import InputText from 'primevue/inputtext';
import ArticlesMasterDataService from '../../Referencials/Services/articles-master-data-service';
import Dropdown from 'primevue/dropdown';
import { ref } from "vue";

export default {
    components: {
        InputText,
        Dropdown,
        PatientInfoComponent,
        ScannerComponent
    },
    data() {
        const selectedArticleType = ref();
        const articlesType = ref([
            { id: '1', text: 'Tous' },
            { id: '2', text: 'Médicaments' },
            { id: '3', text: 'Matériel de soins' },
            { id: '4', text: 'Protections' },
        ]);
        return {
            articles: [],
            searchValue: "",
            showScanner: false,
            selectedArticleType,
            articlesType
        };
    },
    async created() {
        this.focusSearchBar();
        this.articles = await ArticlesMasterDataService.getAllAsync();
    },
    methods: {
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
        goToArticlePage() {
            this.$router.push({ name: 'OrderArticle' });
        },
    },
    computed: {
        filteredArticles() {
            const searchValue = this.searchValue.toLowerCase().trim();
            if (!searchValue) {
                return this.articles;
            } else if (searchValue.length < 3) {
                return [];
            } else {
                return this.articles.filter(articles =>
                    articles.title.toLowerCase().includes(searchValue)
                );
            }
        },
        showPage() {
            return !this.showScanner
        }
    }
}
</script>
<style type="text/css" scoped src="./Content//articles-search.css"></style>
