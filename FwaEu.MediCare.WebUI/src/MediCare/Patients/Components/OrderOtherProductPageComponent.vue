<template>
    <div class="order-other-product-page-container">
        <patient-info-component />
        <span class="command-title">Commander un article:</span>
        <span class="p-input-icon-right">
            <i @click="removeSearch" class="fa fa-solid fa-close"  />
            <InputText  ref="searchInput" v-model="searchProduct" class="search-input" placeholder="Rechercher un article" />
        </span>
        <div class="patient-list">
            <div v-for="(product, index) in filteredProducts" :key="index">
                <div class="patient-item" @click="goToArticlePage">
                    <span>{{product.name}}</span>
                    <div class="icons-container">
                        <i v-show="isProductFavorite(product)" class="fa-solid fa-heart favorite-icon"></i>
                        <i v-show="isProductInHistory(product)" class="fa-solid fa-clock-rotate-left history-icon"></i>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>

    import PatientInfoComponent from './PatientInfoComponent.vue';
    import InputText from 'primevue/inputtext';



    export default {
        components: {
            PatientInfoComponent,
            InputText
        },
        data() {
            return {
                products: [{
                    name: "ADAPTRIC pensements 7.6x7.6 stériles sach 10 pce",
                    isFavorite: true,
                    isInHistory: true,
                },
                {
                    name: "Lingettes nettoyantes 10x15cm sach 100 pce Lingettes nettoyantes 10x15cm sach 100 pceLingettes nettoyantes 10x15cm sach 100 pce",
                    isFavorite: false,
                    isInHistory: true,
                },
                {
                    name: "Xylocaine 2% gel 30g",
                    isFavorite: true,
                    isInHistory: false,
                },
                {
                    name: "Bétadine 10% sol cutanée 125ml",
                    isFavorite: false,
                    isInHistory: false,
                },
                {
                    name: "Sparadrap microporeux 2.5cmx5m",
                    isFavorite: false,
                    isInHistory: false,
                }],
                searchProduct: "",
            };
        },
        async created() {
        },
        methods: {
            isProductFavorite(product) {
                return product.isFavorite;
            },
            isProductInHistory(product) {
                return product.isInHistory;
            },
            removeSearch() {
                this.searchProduct = "";
                this.focusSearchBar();

            },
            focusSearchBar() {
                this.$nextTick(() => {
                    this.$refs.searchInput.$el.focus();
                });
            },
            goToArticlePage() {
                this.$router.push({ name: 'OrderArticle' });
            }
        },
        computed: {
            filteredProducts() {
                return this.products.filter(product => product.name.toLowerCase().includes(this.searchProduct.toLowerCase()));
            }
        },

    }
</script>
<style type="text/css" scoped src="./Content/order-other-product-page.css">
</style>