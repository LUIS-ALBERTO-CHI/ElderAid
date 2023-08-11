<template>
    <div>
        <div v-if="!isNewOrder" class="orders-container ">
            <span class="p-input-icon-right">
                <i @click="removeSearch" class="fa fa-solid fa-close" :style="searchOrders.length == 0 ? 'opacity: 0.5;' : ''" />
                <InputText ref="searchInput" v-model="searchOrders" class="search-input" placeholder="Rechercher une commande" />
            </span>
            <Dropdown v-model="selectedOrdersType" :options="ordersTypeOptions" class="select-sector" />
            <Button style="width: 100%;" @click="displayNewOrder" label="Nouvelle commande" />
            <div style="display: flex; flex-direction: column;">
                <div v-if="orders.some(orders => 'article' in orders)" v-for="(order, index) in filteredOrders" :key="index">
                    <AccordionOrderComponent :order="order">
                        <div v-if="orderComponentDisplayedIndex !== index" class="accordion-content">
                            <Button v-show="order.state != 'Delivred'" label="Annuler la commande" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                            <Button v-if="order.patientId != null" @click="displayOrderComponent(false, index)" :label="`Commander à nouveau pour ${order.patient?.fullName}`" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                            <Button @click="goToSearchPatient()" label="Commander pour un autre patient" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                            <Button @click="displayOrderComponent(true, index)" label="Commander pour EMS" style="height: 45px !important;" icon="fa fa-solid fa-angle-right"
                                    iconPos="right" />
                            <Button @click="goToArticle(order.articleId)" label="Consulter la fiche article" style="height: 45px !important;" icon="fa fa-solid fa-angle-right"
                                    iconPos="right" />
                        </div>
                        <OrderComponent v-else :article="order.article" :patientOrders="getPatientOrders(order.patientId)"
                        @order-done="orderSubmitted" :patientId="getPatientId(order.patientId)"/>
                    </AccordionOrderComponent>
                </div>
                <span v-show="!isEndOfPagination" @click="getMoreOrders()" class="load-more-text">Plus de commande</span>
            </div>
        </div>
        <div v-else class="new-order-container">
            <span style="font-weight: bold; font-size: 18px;">Nouvelle commande :</span>
            <Button @click="goToSearchPatient" label="Pour un patient" icon="fa fa-solid fa-angle-right" iconPos="right" />
            <Button @click="goToSearchArticleForEms" label="Pour EMS" icon="fa fa-solid fa-angle-right" iconPos="right" />
            <Button @click="displayNewOrder" label="Retour" />
        </div>
    </div>

</template>
<script>
    import InputText from 'primevue/inputtext';
    import Button from 'primevue/button';
    import Dropdown from 'primevue/dropdown';
    import AccordionOrderComponent from './AccordionOrderComponent.vue';
    import OrderMasterDataService from "@/MediCare/Orders/Services/orders-master-data-service";
    import ArticlesMasterDataService from "@/MediCare/Articles/Services/articles-master-data-service";
    import PatientsMasterDataService from "@/MediCare/Patients/Services/patients-master-data-service";
    import OrderService from '@/MediCare/Orders/Services/orders-service'
    import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
    import OnlineService from '@/fwamework/OnlineStatus/Services/online-service';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import OrderComponent from '@/MediCare/Patients/Components/OrderComponent.vue';



    export default {
        components: {
            InputText,
            Dropdown,
            Button,
            AccordionOrderComponent,
            OrderComponent
        },
        data() {
            return {
                searchOrders: "",
                ordersTypeOptions: ["Toutes", "Patients", "EMS"],
                selectedOrdersType: "Toutes",
                isNewOrder: false,
                orders: [],
                patients: [],
                actualPage: 0,
                isEndOfPagination: false,
                orderComponentDisplayedIndex: -1,
                isOrderForEms: false,
            };
        },
        async created() {
            this.focusSearchBar();
            this.orders = await OrderMasterDataService.getAllAsync();
            this.patients = await PatientsMasterDataService.getAllAsync();
            this.orders[0].patientId = null;
            this.fillOrders();
        },
        methods: {
            removeSearch() {
                this.searchOrders = "";
                this.focusSearchBar();
            },
            focusSearchBar() {
                this.$nextTick(() => {
                    this.$refs.searchInput.$el.focus();
                });
            },
            displayNewOrder() {
                this.isNewOrder = !this.isNewOrder;
            },
            goToSearchPatient() {
                this.$router.push({ name: "SearchPatientFromOrder" });
            },
            goToSearchArticleForEms() {
                this.$router.push({ name: "SearchArticleForEMSFromOrder", params: { id: 0 } });
            },
            goToArticle(articleId) {
                this.$router.push({ name: "OrderArticleFromOrder", params: { id: 0, articleId: articleId } });
            },
            async fillOrders() {
                const ordersArticleIds = this.orders.map(x => x.articleId);
                const articles = await ArticlesMasterDataService.getByIdsAsync(ordersArticleIds);

                this.orders.forEach(order => {
                    const article = articles.find(x => x.id == order.articleId);
                    order.article = article;
                    if (order.patientId != null || order.patientId > 0)
                        order.patient = this.patients.find(x => x.id == order.patientId);
                });
            },
            async getMoreOrders() {
                if (OnlineService.isOnline()) {
                    var model = {
                        patientId: null,
                        page: this.actualPage++,
                        pageSize: Configuration.paginationSize.orders,
                    }

                    var orders = await OrderService.getAllAsync(model)

                    if (orders.length < Configuration.paginationSize.orders)
                        this.isEndOfPagination = true;
                    this.orders = this.orders.concat(orders)
                    this.fillOrders();
                } else {
                    NotificationService.showError("La connexion avec le serveur a été perdue. Retentez plus tard")
                }
            },
            displayOrderComponent(isOrderForEms, index) {
                this.orderComponentDisplayedIndex = index;
                if (isOrderForEms)
                    this.isOrderForEms = true;
            },
            orderSubmitted() {
                this.orderComponentDisplayedIndex = -1;
                this.isOrderForEms = false;
            },
            getPatientOrders(patientId) {
                if (this.isOrderForEms)
                    return this.orders.filter(order => order.patientId == null)
                else
                    return this.orders.filter(order => order.patientId == patientId)
            },
            getPatientId(patientId) {
                return patientId ?? 0;
            }
        },
        computed: {
            filteredOrders() {
                return this.orders.filter(order => {
                    return (
                        (order?.article?.title.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                            order?.patient?.fullName.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                            order.updatedBy.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                            order?.patient?.roomName.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                            order.updatedOn.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                            order.state.toLowerCase().includes(this.searchOrders.toLowerCase())) &&
                        (this.selectedOrdersType == "Toutes" ||
                            (this.selectedOrdersType == "Patients" && order.patientId != null) ||
                            (this.selectedOrdersType == "EMS" && order.patientId == null))
                    );
                });
            },
        },

    }
</script>
<style type="text/css" scoped src="./Content/orders-page.css">
</style>