<template>
    <div class="order-article-page-container">
        <patient-info-component />
        <div class="article-title-container">
            <span style="width: 90%;" class="command-title">ADAPTRIC pensements 7.6x7.6 stériles sach 10 pce</span>
            <i class="fa-solid fa-heart"></i>
        </div>
        <div class="article-label-container">
            <i class="fa-solid fa-clock-rotate-left history-icon"></i>
            <span>Utilisé par le patient en mars 2022</span>
        </div>
        <div class="article-container">
            <div class="article-info-container">
                <div class="article-label-container">
                    <span style="font-weight: bold;">CHF</span>
                    <span>{{`${article.price}`}}</span>
                </div>
                <div class="article-label-container">
                    <i class="fa-solid fa-money-bill-1"></i>
                    <span>{{`${article.pricePaid} reste à charge`}}</span>
                </div>
                <div class="article-label-container">
                    <i class="fa-regular fa-box"></i>
                    <span>{{`boîte de 10 pièces`}}</span>
                </div>
            </div>
            <img class="article-image" src="https://i.webareacontrol.com/fullimage/1000-X-1000/2/g/24520175419systagenix-adaptic-touch-non-adhering-silicone-dressing-L.png" />
        </div>
        
        <OrderComponent v-if="!isOrderSubmitted" @submitOrder="orderSubmit"/>
        <div v-else class="order-submitted-container">
            <span>Commande réalisée avec succès !</span>
            <span>Votre prochaine action :</span>
            <Button label="Voir les commandes en cours pour Dimitri ASHIKHMIN" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
            <Button label="Commander un autre article pour Dimitri ASHIKHMIN" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
            <Button label="Consulter la fiche du patient Dimitri ASHIKHMIN" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
            <Button label="Revenir à l'accueil" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
        </div>
    </div>
</template>
<script>

    import PatientInfoComponent from './PatientInfoComponent.vue';
    import OrderComponent from './OrderComponent.vue';
    import Button from 'primevue/button';




    export default {
        components: {
            PatientInfoComponent,
            OrderComponent,
            Button
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
                },
                isOrderSubmitted: false,
            };
        },
        async created() {
        },
        methods: {
            orderSubmit(quantity) {
                console.log(quantity)
                this.isOrderSubmitted = true;
            }
        },
        computed: {
        },

    }
</script>
<style type="text/css" scoped src="./Content/order-article-page.css">


</style>