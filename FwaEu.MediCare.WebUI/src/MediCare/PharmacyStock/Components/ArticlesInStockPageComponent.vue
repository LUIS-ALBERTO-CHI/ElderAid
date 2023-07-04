<template>
    <div class="page-articles">
        <span>Articles en stock</span>
        <div class="search-container">
            <div class="input-container">
                <InputText ref="searchInput" v-model="searchArticle" class="search-input"
                    placeholder="Rechercher un article"></InputText>
                <i @click="removeSearch" class="fa fa-solid fa-close remove-icon"
                    :style="searchArticle.length === 0 ? 'opacity: 0.5;' : ''" />
            </div>
            <i @click="codeqr" class="fa-sharp fa-regular fa-qrcode qr-code-icon" />
        </div>
        <div class="articles-list">
            <div v-for="article in filteredArticles" :key="article.name">
                <div @click="goToArticleDetails(article)" class="article-item">
                    <span>{{ article.name }}, {{ article.unit }}</span>
                    <span>{{ article.countInbox }}</span>
                </div>
            </div>
        </div>
        <div v-show="filteredArticles.length === 0" class="article-not-found">
            <i class="fa-solid fa-box-open icon-not-found"></i>
            <span>Aucun article trouvé</span>
        </div>
    </div>
</template>

<script>
import InputText from 'primevue/inputtext';
import articles from './articles.json';

export default {
    components: {
        InputText
    },
    data() {
        return {
            articles: articles,
            searchArticle: "",
        };
    },
    async created() {
        this.focusSearchBar();
    },
    methods: {
        removeSearch() {
            this.searchArticle = "";
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
    },
    computed: {
        filteredArticles() {
            const searchArticle = this.searchArticle.toLowerCase().trim();
            if (!searchArticle) {
                return this.articles;
            } else {
                return this.articles.filter(article =>
                    article.name.toLowerCase().includes(searchArticle)
                );
            }
        }
    }
};
</script>