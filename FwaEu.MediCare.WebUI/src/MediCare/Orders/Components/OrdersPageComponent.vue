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
                <div v-for="(order, index) in filteredOrders" :key="index">
                    <AccordionOrderComponent :order="order">
                        <div v-if="!order.isDelivered" class="accordion-content">
                            <Button label="Annuler la commande" style="height: 45px !important;"></Button>
                            <Button label="Commander à nouveau pour Dimitri" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                            <Button label="Commander pour un autre patient" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                            <Button label="Commander pour EMS" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                            <Button label="Consulter la fiche article" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                        </div>
                        <div v-else class="accordion-content">
                            <Button label="Commander pour un autre patient" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                            <Button label="Commander pour EMS" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                            <Button label="Consulter la fiche article" style="height: 45px !important;" icon="fa fa-solid fa-angle-right" iconPos="right"></Button>
                        </div>
                    </AccordionOrderComponent>
                </div>
                <span class="more-command-text">Plus de commande</span>
            </div>
        </div>
        <div v-else class="new-order-container">
            <span style="font-weight: bold; font-size: 18px;">Nouvelle commande :</span>
            <Button @click="goToSearchPatient" label="Pour un patient" icon="fa fa-solid fa-angle-right" iconPos="right"/>
            <Button @click="goToSearchPatient" label="Pour EMS" icon="fa fa-solid fa-angle-right" iconPos="right"/>
            <Button @click="displayNewOrder" label="Retour" />
        </div>
    </div>

</template>
<script>
    import InputText from 'primevue/inputtext';
    import Button from 'primevue/button';
    import Dropdown from 'primevue/dropdown';
    import AccordionOrderComponent from './AccordionOrderComponent.vue';



    export default {
        components: {
            InputText,
            Dropdown,
            Button,
            AccordionOrderComponent
        },
        data() {
            return {
                searchOrders: "",
                ordersTypeOptions: ["Toutes", "Patients", "EMS"],
                selectedOrdersType: "Toutes",
                isNewOrder: false,
                orders: [
                    {
                        patientName: "Jean Dupont",
                        nurseName: "Claire Dupont",
                        medicationName: "ADAPATRIC 10 mg, comprimé pelliculé sécable",
                        date: "12/12/2020",
                        box: "4 boîtes",
                        isDelivered: true,
                        room: "A506",
                    },
                    {
                        patientName: "",
                        nurseName: "Carolie Data",
                        medicationName: "ANTIDRY lotion huilde amande 500ml",
                        date: "12/12/2028",
                        box: "8 boites",
                        isDelivered: false,
                        room: "A809",
                    }
                ]
            };
        },
        async created() {
            this.focusSearchBar();

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
                this.$router.push({ name: "SearchPatient" });
            }
        },
        computed: {
            filteredOrders() {
                return this.orders.filter(order => {
                    return (
                        (order.medicationName.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                            order.room.toLowerCase().includes(this.searchOrders.toLowerCase())) ||
                        (order.patientName.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                            order.nurseName.toLowerCase().includes(this.searchOrders.toLowerCase())) ||
                        (order.date.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
                            order.box.toLowerCase().includes(this.searchOrders.toLowerCase()))
                             &&
                        (this.selectedOrdersType == "Toutes" ||
                            (this.selectedOrdersType == "Patients" && order.patientName != "") ||
                            (this.selectedOrdersType == "EMS" && order.patientName == ""))
                    );
                });
            },
        },

    }
</script>
<style type="text/css" scoped src="./Content/orders-page.css">
</style>