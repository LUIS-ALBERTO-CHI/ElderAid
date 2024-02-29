<template>
    <div>
        <div v-if="!isNewOrder" class="orders-container ">
            <span class="p-input-icon-right">
                <i @click="removeSearch" class="fa fa-solid fa-close" :style="searchOrders.length == 0 ? 'opacity: 0.5;' : ''" />
                <InputText ref="searchInput" v-model="searchOrders" class="search-input" placeholder="Buscar un pedido" />
            </span>
            <Dropdown v-model="selectedOrdersType" :options="ordersTypeOptions" class="select-sector" />
            <Button style="width: 100%;" @click="displayNewOrder" label="Nuevo pedido" />
            <div style="display: flex; flex-direction: column;">
                <div v-if="orders.some(orders => 'article' in orders)" v-for="(order, index) in filteredOrders" :key="index">
                    <AccordionOrderComponent :order="order">
                        <div v-if="orderComponentDisplayedIndex !== index" class="accordion-content">

                            <div v-show="order.state == 'Pending'">
                                <Button v-if="cancelOrderDisplayedIndex !== index" style="width: 100% !important;"
                                        @click="showCancelOrderDisplay(index)" label="Annuler la commande" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                                <div v-else class="cancel-confirmation-container">
                                    <span>Etes vous sûr d'annuler la commande ?</span>
                                    <div class="confirmaton-button-container">
                                        <Button @click="cancelOrder(order.id)" label="OUI" outlined class="button-confirmation " />
                                        <Button @click="hideCancelOrderDisplay()" label="NON" outlined class="button-confirmation" />
                                    </div>
                                </div>
                            </div>
                                <Button v-for="(button, buttonIndex) in buttonConfig(order, index)" :key="buttonIndex" @click="button.action()" :label="button.label" 
                                        style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right" />
                        </div>
                        <OrderComponent v-else :article="order.article" :patientOrders="getPatientOrders(order.patientId)"
                                        @order-done="orderSubmitted" :patientId="getPatientId(order.patientId)" />
                    </AccordionOrderComponent>
                </div>
                <span v-show="!isEndOfPagination" @click="getMoreOrders()" class="load-more-text">Más pedidos</span>
            </div>
        </div>
        <div v-else class="new-order-container">
            <span style="font-weight: bold; font-size: 18px;">Nouvelle commande :</span>
            <Button @click="goToSearchPatient" label="Por un paciente" icon="fa fa-solid fa-angle-right" iconPos="right" />
            <Button @click="goToSearchArticleForEms" label="Por EMS" icon="fa fa-solid fa-angle-right" iconPos="right" />
            <Button @click="displayNewOrder" label="Regresar" />
        </div>
    </div>

</template>
<script>
    import InputText from 'primevue/inputtext';
    import Button from 'primevue/button';
    import Dropdown from 'primevue/dropdown';
    import AccordionOrderComponent from './AccordionOrderComponent.vue';
    import OrderMasterDataService from "@/ElderAid/Orders/Services/orders-master-data-service";
    import RecentArticlesMasterDataService from "@/ElderAid/Articles/Services/recent-articles-master-data-service";
    import PatientsMasterDataService from "@/ElderAid/Patients/Services/patients-master-data-service";
    import OrderService from '@/ElderAid/Orders/Services/orders-service'
    import { Configuration } from '@/Fwamework/Core/Services/configuration-service';
    import OnlineService from '@/Fwamework/OnlineStatus/Services/online-service';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import OrderComponent from '@/ElderAid/Patients/Components/OrderComponent.vue';



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
                ordersTypeOptions: ["Todos", "Pacientes", "EMS"],
                selectedOrdersType: "Todos",
                isNewOrder: false,
                orders: [],
                patients: [],
                actualPage: 0,
                isEndOfPagination: false,
                orderComponentDisplayedIndex: -1,
                cancelOrderDisplayedIndex: -1,
                isOrderForEms: false,
            };
        },
        async created() {
            this.focusSearchBar();
            this.patients = await PatientsMasterDataService.getAllAsync();
            this.orders = await OrderMasterDataService.getAllAsync();
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
                this.$router.push({ name: "SearchPatientFromOrder"});
            },
            goToSearchPatientWithArticleId(articleId) {
                this.$router.push({ name: "SearchPatientFromOrderWithArticleId", params: { articleId: articleId } });
            },
            goToSearchArticleForEms() {
                this.$router.push({ name: "SearchArticleForEMSFromOrder", params: { id: 0 } });
            },
            goToArticle(articleId) {
                this.$router.push({ name: "OrderArticleFromOrder", params: { id: 0, articleId: articleId } });
            },
            async fillOrders() {
                const ordersArticleIds = this.orders.map(x => x.articleId);
                const articles = await RecentArticlesMasterDataService.getByIdsAsync(ordersArticleIds);
                this.orders.forEach(order => {
                    const article = articles.find(x => x.id == order.articleId);
                    order.article = article;
                    if (order.patientId != null || order.patientId > 0)
                        order.patient = this.patients.find(x => x.id == order.patientId);
                });
                this.orders.sort((a, b) => {
                    return new Date(b.updatedOn) - new Date(a.updatedOn);
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
                    await OrderMasterDataService.clearCacheAsync();
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
            async orderSubmitted() {
                this.orderComponentDisplayedIndex = -1;
                this.isOrderForEms = false;
                await OrderMasterDataService.clearCacheAsync();
                await RecentArticlesMasterDataService.clearCacheAsync();
                this.actualPage = 0;
                this.$router.go(0)
            },
            getPatientOrders(patientId) {
                if (this.isOrderForEms)
                    return this.orders.filter(order => order.patientId == null)
                else
                    return this.orders.filter(order => order.patientId == patientId)
            },
            getPatientId(patientId) {
                return patientId ?? 0;
            },
            showCancelOrderDisplay(index) {
                this.cancelOrderDisplayedIndex = index;
            },
            hideCancelOrderDisplay() {
                this.cancelOrderDisplayedIndex = -1;
            },
            async cancelOrder(id) {
                try {
                    await OrderService.cancelOrderAsync(id).then(() => {
						NotificationService.showConfirmation("El pedido ha sido cancelado")
                        this.fillOrders();
                        this.hideCancelOrderDisplay();
                    })
                } catch (error) {
                    NotificationService.showError("Une erreur est survenue lors de l'annulation de la commande")
                }
            },
        },
        computed: {
            filteredOrders() {
                return this.orders.filter(order => {
                    return (
                        (order?.article?.title.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                        order?.patient?.roomName.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                        order?.patient?.fullName.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                        order?.updatedOn.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                        order?.state.toLowerCase().includes(this.searchOrders.toLowerCase())) &&
                        (this.selectedOrdersType == "Toutes" ||
                            (this.selectedOrdersType == "Patients" && order.patientId != null) ||
                            (this.selectedOrdersType == "EMS" && order.patientId == null))
                    );
                });
            },
            buttonConfig() {
                return (order, index) => {
                    const buttons = [];

                    if (order.patientId === null || !order.patient) {
                        buttons.push({
                            label: 'Commander pour EMS',
                            action: () => this.displayOrderComponent(true, index),
                        });
                        buttons.push({
                            label: 'Commander pour un patient',
                            action: () => this.goToSearchPatientWithArticleId(order.articleId),
                        });
                    } else {
                        buttons.push({
                            label: `Commander à nouveau pour ${order.patient.fullName}`,
                            action: () => this.displayOrderComponent(false, index),
                        });
                        buttons.push({
                            label: 'Commander pour un autre patient',
                            action: () => this.goToSearchPatientWithArticleId(order.articleId),
                        });
                        buttons.push({
                            label: 'Commander pour EMS',
                            action: () => this.displayOrderComponent(true, index),
                        });
                    }

                    buttons.push({
                        label: 'Consulter la fiche article',
                        action: () => this.goToArticle(order.articleId),
                    });

                    return buttons;
                };
            },
        },

    }
</script>
<style type="text/css" scoped src="./Content/orders-page.css">
</style>