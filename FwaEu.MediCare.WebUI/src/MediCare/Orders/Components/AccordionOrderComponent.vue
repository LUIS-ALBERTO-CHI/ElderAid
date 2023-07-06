<template>
    <Accordion v-if="order != null">
        <AccordionTab>
            <template #header>
                <div class="accordion-header">
                    <!-- <div v-show="!isPatientUnique" class="accordion-top-area">
                        <div>
                            <div class="accordion-header-title-area" v-if="order.patientName.length > 0">
                                <i class="fa-solid fa-user" />
                                <span>{{order.patientName}}</span>
                            </div>
                            <div class="accordion-header-title-area" v-else>
                                <i class="fa-regular fa-hospital"></i>
                                <span>EMS Potterie</span>
                            </div>
                        </div>
                        <div class="accordion-header-title-area">
                            <i class="fa-solid fa-bed" />
                            <span>{{order.room}}</span>
                        </div>
                    </div> -->
                    <span style="width: 90%;" >{{article.title}}</span>
                    <span class="header-subtitle">{{order.quantity }} {{ article.invoicingUnit }}</span>
                    <span class="header-subtitle">{{ $d(new Date(order.updatedOn)) }} à {{new Intl.DateTimeFormat('default', { hour: '2-digit', minute: '2-digit' }).format(new Date(order.updatedOn))}}</span>
                    <div class="accordion-footer-area">
                        <span>{{ order.updatedBy }}</span>
                        <div v-if="order.state == 'Delivred'" style="color: #2ba859" class="accordion-header-title-area">
                            <i class="fa-solid fa-truck" />
                            <span>Livrée</span>
                        </div>
                        <div v-else-if="order.state == 'Pending'" style="opacity: 0.8;" class="accordion-header-title-area">
                            <i class="fa-solid fa-hourglass"></i>
                            <span>En attente</span>
                        </div>
                        <div v-else style="color: red;" class="accordion-header-title-area">
                            <i class="fa-solid fa-xmark"></i>
                            <span>Annulé</span>
                        </div>
                    </div>
                </div>
            </template>
            <slot></slot>
        </AccordionTab>
    </Accordion>

</template>
<script>

    import Accordion from 'primevue/accordion';
    import AccordionTab from 'primevue/accordiontab';
    import PatientService from "@/MediCare/Patients/Services/patients-service";
    import ArticlesMasterDataService from "@/MediCare/Referencials/Services/articles-master-data-service";


    export default {
        components: {
            Accordion,
            AccordionTab
        },
        data() {
            return {
                patient: {},
                article: {},
            };
        },
        props: {
            order: {
                type: Object,
                required: true
            },
            isPatientUnique: {
                type: Boolean,
                required: false,
                default: false
            },
        },
        async created() {
            this.patient = JSON.parse(localStorage.getItem('patient'));
            const articles = await ArticlesMasterDataService.getAllAsync();
            
            
            this.article = articles.find(x => x.id == this.order.articleId);
            console.log(this.article)
            console.warn(this.order)
        },
        methods: {
        },
        computed: {

        },

    }
</script>
<style type="text/css">
.accordion-header {
    display: flex;
    flex-direction: column;
    color: var(--alt-secondary-text-color);
    text-decoration: none;
    width: 100%;
    row-gap: 5px;
    height: auto;
}

.accordion-header:focus {
    text-decoration: none !important;
  }

.accordion-top-area { 
    width: 100%;
    display: flex;
    justify-content: space-between;
}

.accordion-header-title-area {
    display: flex;
    column-gap: 5px;
    font-size: 16px;
    align-items: center;
    width: auto;
}

.header-subtitle {
    font-size: 14px;
    font-weight: 500;
    width: 90%;
}

.accordion-footer-area {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}
</style>