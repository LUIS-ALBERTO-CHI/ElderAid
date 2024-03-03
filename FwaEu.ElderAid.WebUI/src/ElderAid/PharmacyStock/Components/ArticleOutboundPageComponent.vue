<template>
    <div class="page-article">
        <SearchPatientComponent v-show="!isPatientSelected" @selectedPatient="handleSelectedPatient" />
        <div v-show="isPatientSelected">
            <div v-if="article">
                <div class="text-left">
                    <span class="article-name">{{ article.title }}</span>
                </div>
                <div class="text-left">
                    <span>Cantidad en stock;: {{this.$route?.query?.stockQuantity}}</span>
                </div>
            </div>
            <div class="info-container">
                <div class="text-left">
                    <span>Para {{ patientName }}</span>
                </div>
                <div class="icon-right-container">
                    <span @click="openSearchPatientComponent">Otro Paciente?</span>
                </div>
            </div>
            <div class="info-container">
                <div class="text-left">
                    <span>Caja completa</span>
                </div>
                <div class="icon-right-container">
                    <InputSwitch :disabled="isSwitchDisabled" v-model="fullBox" class="custom-switch" />
                </div>
            </div>
            <div class="info-container" v-if="fullBox">
                <div class="text-left">
                    <span>Caja de</span>
                </div>
                <div class="icon-right-container">
                    <Dropdown v-model="selectedBoite" :options="boiteOptionsWithUnit" optionLabel="label" optionValue="countInBox" />
                </div>
            </div>
            <div class="info-container" v-if="!fullBox">
                <div class="text-left">
                    <span>Cantidad para surtir(Tabletas)</span>
                </div>
                <div class="icon-right-container">
                    <InputNumber id="quantity" v-model="quantity" :min="1" :max="100" showButtons buttonLayout="horizontal"
                         style="width: 100%;" decrementButtonClassName="p-button-secondary"
                         incrementButtonClassName="p-button-secondary" incrementButtonIcon="fa fa-solid fa-plus"
                         decrementButtonIcon="fa fa-solid fa-minus"/>
                </div>
            </div>
            <div class="confirmation-container" v-if="showConfirmationDisplayed">
                <span>¿Seguro que desea retirar esta cantidad? <small>La cantidad restante en stock sera negativa</small></span>
                <div class="confirmaton-button-container" style="margin-top: 10px;">
                    <Button @click="confirmOrderAsync()" label="SI" outlined class="button-confirmation" />
                    <Button @click="() => showConfirmationDisplayed = false" label="NO" outlined class="button-confirmation" />
                </div>
            </div>
            <div v-else class="button-confirmer">
                <Button @click="confirmOrderAsync" class="confirmer" style="width: 100%; margin-top: 50px; align-self: center;"
                        label="Retirar de stock" />
            </div>
        </div>
    </div>
</template>

<script>
    import InputSwitch from 'primevue/inputswitch';
    import Button from 'primevue/button';
    import Dropdown from 'primevue/dropdown';
    import InputNumber from 'primevue/inputnumber';
    import CabinetsMasterDataService from "@/ElderAid/Referencials/Services/cabinets-master-data-service";
    import SearchPatientComponent from '@/ElderAid/Patients/Components/SearchPatientPageComponent.vue';
    import NotificationService from "@/Fwamework/Notifications/Services/notification-service";
    import ArticlesService from '@/ElderAid/Articles/Services/articles-service';
    import RecentArticlesMasterDataService from '@/ElderAid/Articles/Services/recent-articles-master-data-service';
    import PharmacyStockService from '@/ElderAid/PharmacyStock/Services/pharmacy-stock-service';
	import BreadcrumbService from '@/Fwamework/Breadcrumbs/Services/breadcrumbs-service';
	import ResolveContext from '@/Fwamework/Breadcrumbs/Services/resolve-context'

    export default {
        components: {
            InputSwitch,
            Button,
            Dropdown,
            InputNumber,
            SearchPatientComponent,
        },
        data() {
            return {
                article: null,
                fullBox: false,
                availableUnitCounts: [],
                boiteOptions: [],
                boiteOptionsWithUnit: [],
                selectedBoite: null,
                quantity: 1,
                selectedPatient: null,
                cabinetName: "",
                isSwitchDisabled: false,
                showConfirmationDisplayed: false,
                isPatientSelected: false,
            };
        },
        async created() {
            await this.getCurrentCabinetAsync();
            const articleId = this.$route.params.articleId;
            if (articleId) {
                const article = await RecentArticlesMasterDataService.getAsync(articleId);
                this.article = article;

                if (!article) {
                    const [article] = await ArticlesService.getByIdsAsync([articleId]);
                    this.article = article;
                }
                const groupName = this.article.groupName;
                const articleType = this.article.articleType;

                this.availableUnitCounts = await ArticlesService.getAllBySearchAsync(`formats:${groupName}`, articleType, 0, 30);
                this.boiteOptions = this.availableUnitCounts
                    .filter(article => article.countInBox > 0)
                    .map(article => article.countInBox)
                    .filter((value, index, self) => self.indexOf(value) === index)
                    .sort((a, b) => a - b);

                this.boiteOptionsWithUnit = this.availableUnitCounts
                    .filter(article => article.countInBox > 0)
                    .map(article => ({
                        label: `${article.countInBox} ${article.unit}`,
                        countInBox: article.countInBox,
                    }));

                if (this.boiteOptions.length > 0) {
                    this.selectedBoite = this.boiteOptions[0];
                } else {
                    this.isSwitchDisabled = true;
                }
            }
        },
        methods: {
            async getCurrentCabinetAsync() {
                const cabinetId = this.$route.params.id;
                const cabinet = await CabinetsMasterDataService.getAsync(cabinetId);
                this.cabinetName = cabinet.id;
                return cabinet;
            },
            async handleSelectedPatient(args) {
                args.cancelNavigation = true
                this.selectedPatient = args.selectedPatient;
                this.isPatientSelected = true;
                const context = new ResolveContext(this.$router, this.$i18n);
                context.currentComponent = this;
                await BreadcrumbService.processRouteAsync(this.$route, context);
            },
            async openSearchPatientComponent() {
                this.selectedPatient = null;
                this.isPatientSelected = false;
                const context = new ResolveContext(this.$router, this.$i18n);
                context.currentComponent = this;
                await BreadcrumbService.processRouteAsync(this.$route, context);
            },
            async confirmOrderAsync() {
                const patientId = this.selectedPatient ? this.selectedPatient.id : null;

                if (this.showConfirmationDisplayed || (this.$route?.query?.stockQuantity !== null && this.$route?.query?.stockQuantity - this.quantity > 0)) {
                    if (this.fullBox) {
                        this.quantity = this.quantity * this.selectedBoite;
                    }
                    await PharmacyStockService.updateAsync({ stockId: this.$route.params.stockId, quantity: this.quantity, patientId}).then(async () => {
                        NotificationService.showConfirmation("Se retiro el producto " + this.article.title + " del stock de manera correcta.");
                        this.$router.push({ name: "Cabinet" });
                    }).catch(() => {
                        NotificationService.showError("Error al procesar el retiro del articulo " + this.article.title + " del stock.");
                    });
                }
                else {
                    this.showConfirmationDisplayed = true;
                }
            },
        },
        computed: {
            patientName() {
                return this.selectedPatient ? this.selectedPatient.fullName : "";
            },
        }
    };
</script>

<style type="text/css" scoped src="./Content/articles.css"></style>
