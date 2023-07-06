<template>
    <div class="page-articles">
        <span>Articles en stock</span>
        <div class="search-container">
            <InputText ref="searchInput" v-model="searchValue" class="search-input" placeholder="Rechercher un article">
            </InputText>
            <div>
                <i @click="removeSearch" class="fa fa-solid fa-close remove-icon"
                    :style="searchValue.length === 0 ? 'opacity: 0.5;' : ''" />
            </div>
            <i @click="goToScanCode" class="fa-sharp fa-regular fa-qrcode qr-code-icon"></i>
        </div>
        <div class="vignette-list">
            <div v-for="article in filteredArticles" :key="article.name">
                <div @click="goToArticleDetails(article)" class="vignette-item">
                    <span>{{ article.name }}, {{ article.unit }}</span>
                    <span>{{ article.countInbox }}</span>
                </div>
            </div>
        </div>
        <div v-show="filteredArticles.length === 0" class="article-not-found">
            <i class="fa-solid fa-box-open icon-not-found"></i>
            <span>Aucun article trouvé</span>
        </div>
        <div v-if="showScanner">
            <ScannerComponent @codeScanned="handleCodeScanned"></ScannerComponent>
        </div>
    </div>
</template>

<script>
import InputText from 'primevue/inputtext';
import articles from './articles.json';
import CabinetsMasterDataService from "@/MediCare/Referencials/Services/cabinets-master-data-service";
import ScannerComponent from './ScanCodeComponent.vue';
export default {
    components: {
        InputText,
        ScannerComponent
    },
    data() {
        return {
            articles: articles,
            searchValue: "",
            cabinetName: '',
            showScanner: false
        };
    },
    async created() {
        this.focusSearchBar();
        await this.getCurrentCabinetAsync();
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
        goToArticleDetails(article) {
            localStorage.setItem("selectedArticle", JSON.stringify(article));
            this.$router.push({ name: "Articles" });
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
        },
    },
    computed: {
        filteredArticles() {
            const searchValue = this.searchValue.toLowerCase().trim();
            if (!searchValue) {
                return this.articles;
            } else {
                return this.articles.filter(article =>
                    article.name.toLowerCase().includes(searchValue)
                );
            }
        },
    }
};
</script>
<style type="text/css" scoped src="./Content/articles.css"></style>