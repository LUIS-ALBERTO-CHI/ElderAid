<template>
    <Accordion v-if="order != null">
        <AccordionTab>
            <template #header>
                <div class="accordion-header">
                    <div class="accordion-top-area">
                        <div>
                            <div v-if="isPatientOrder" class="accordion-header-title-area">
                                <i class="fa-solid fa-user" />
                                <span>{{ patient.fullName }}</span>
                            </div>
                            <div v-else class="accordion-header-title-area">
                                <i class="fa-regular fa-hospital"></i>
                                <span>{{ organization.name }}</span>
                            </div>
                        </div>
                        <div v-if="isPatientOrder" class="accordion-header-title-area">
                            <i class="fa-solid fa-bed" />
                            <span>{{ patient.roomName }}</span>
                        </div>
                    </div>
                    <span style="width: 90%;">{{ article.title }}</span>
                    <div v-if="order.isBox">
                        <span class="header-subtitle">{{ order.quantity }} {{ getBox() }}</span>
                    </div>
                    <div v-else>
                        <span class="header-subtitle">{{ getSubtitle() }}</span>
                    </div>
                    <span class="header-subtitle">{{ $d(new Date(order.updatedOn)) }} a las {{ new Intl.DateTimeFormat('default', { hour: '2-digit', minute: '2-digit' }).format(new Date(order.updatedOn)) }}</span>
                    <div class="accordion-footer-area">
                        <span>{{ order.updatedBy }}</span>
                        <div v-if="order.state == 'Delivred'" style="color: #2ba859" class="accordion-header-title-area">
                            <i class="fa-solid fa-truck" />
                            <span>Entregado</span>
                        </div>
                        <div v-else-if="order.state == 'Pending'" style="opacity: 0.8;" class="accordion-header-title-area">
                            <i class="fa-solid fa-hourglass"></i>
                            <span>Pendiente</span>
                        </div>
                        <div v-else style="color: red;" class="accordion-header-title-area">
                            <i class="fa-solid fa-xmark"></i>
                            <span>Cancelado</span>
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
    import RecentArticlesMasterDataService from "@/ElderAid/Articles/Services/recent-articles-master-data-service";
    import ViewContextService from "@/ElderAid/ViewContext/Services/view-context-service";
    import PatientService from "@/ElderAid/Patients/Services/patients-service";


    export default {
        components: {
            Accordion,
            AccordionTab
        },
        data() {
            return {
                article: {},
                organization: {},
                patient: {}
            };
        },
        props: {
            order: {
                type: Object,
                required: true
            },
        },
        async created() {
            if (this.order.patientId != null && this.order.patientId > 0)
                this.patient = await PatientService.getPatientById(this.order.patientId);
            const articles = await RecentArticlesMasterDataService.getAllAsync();
            this.article = articles.find(x => x.id == this.order.articleId);
            this.organization = ViewContextService.get();
        },
        methods: {
            getSubtitle() {
                let subtitle = `${this.order.quantity} ${this.order.quantity > 1 ? 'tabletas' : 'tableta'}`


                return subtitle
            },
            getBox() {
                let box = ` ${this.order.quantity > 1 ? 'cajas' : 'caja'}`


                return box
            }
        },
        computed: {
            isPatientOrder() {
                return this.order.patientId != null && this.order.patientId > 0;
            },
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