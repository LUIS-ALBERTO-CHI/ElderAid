<template>
    <div class="order-article-page-container">
        <patient-info-component />
        <span class="command-title">Commander un article:</span>

        <span>Utilisé par le petient en mars 2022</span>
        <div class="article-container">
            <div class="article-info-container">
                <span>{{`CHF ${article.price}`}}</span>
                <span>{{`${article.pricePaid} reste à charge`}}</span>
                <span>{{`boîte de 10 pièces`}}</span>
            </div>
            <img href="https://i.webareacontrol.com/fullimage/1000-X-1000/2/g/24520175419systagenix-adaptic-touch-non-adhering-silicone-dressing-L" />
        </div>

        <span>Commander une quantité</span>

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
                article: {
                    name: "Lingettes nettoyantes 10x15cm sach 100 pce",
                    isFavorite: true,
                    isInHistory: true,
                    date: "12/12/2020",
                    price: "12.00",
                    pricePaid: "2.12",
                    stock: "12",
                }
            };
        },
        async created() {
        },
        methods: {
        },
        computed: {
        },

    }
</script>
<style type="text/css" scoped src="./Content/order-article-page.css">
</style>