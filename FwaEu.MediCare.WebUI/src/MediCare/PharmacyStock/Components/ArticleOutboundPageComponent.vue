<template>
    <div class="page-articles">
        <SearchPatientComponent v-show="!isPatientSelected" @selectedPatient="handleSelectedPatient" />
        <div v-show="isPatientSelected">
            <div v-if="selectedArticle">
                <div class="text-left">
                    <span class="article-name">{{ selectedArticle.title }}, {{ selectedArticle.unit }}, {{selectedArticle.countInBox }}</span>
                </div>
            </div>
            <div class="info-container">
                <div class="text-left">
                    <span>Pour {{ patientName }}</span>
                </div>
                <div class="icon-right-container">
                    <Button @click="openSearchPatientComponent" class="custom-button" style="width: 100px;"
                        label="Modifier" />
                </div>
            </div>
            <div class="info-container">
                <div class="text-left">
                    <span>Boîte entière</span>
                </div>
                <div class="icon-right-container">
                    <InputSwitch v-model="fullBox" class="custom-switch" />
                </div>
            </div>
            <div class="info-container" v-if="fullBox">
                <div class="text-left">
                    <span>Boîte de</span>
                </div>
                <div class="icon-right-container">
                    <Dropdown v-model="selectedBoite" :options="boiteOptionsWithUnit" optionLabel="label" optionValue="countInBox" />
                </div>
            </div>
            <div class="info-container" v-if="!fullBox">
                <div class="text-left">
                    <span>Quantité sortie (comprimés)</span>
                </div>
                <div class="icon-right-container">
                    <InputNumber id="quantity" v-model="quantity" :min="1" :max="100" />
                </div>
            </div>
            <div class="button-confirmer">
                <Button @click="confirmOrder" class="confirmer" style="width: 100%; margin-top: 50px; align-self: center;"
                    label="Confirmer" />
            </div>
        </div>
    </div>
</template>

<script>
import InputSwitch from 'primevue/inputswitch';
import Button from 'primevue/button';
import Dropdown from 'primevue/dropdown';
import InputNumber from 'primevue/inputnumber';
import CabinetsMasterDataService from "@/MediCare/Referencials/Services/cabinets-master-data-service";
import ViewContextService from '@/MediCare/ViewContext/Services/view-context-service';
import SearchPatientComponent from '@/MediCare/Patients/Components/SearchPatientPageComponent.vue';
import ArticlesService from '@/MediCare/Referencials/Services/articles-service';


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
            selectedArticle: null,
            fullBox: ViewContextService.get().isStockPharmacyPerBox,
            availableUnitCounts: [],
            boiteOptions: [],
            boiteOptionsWithUnit: [],
            selectedBoite: null,
            quantity: 1,
            selectedPatient: null,
        };
    },
    async created() {
        await this.getCurrentCabinetAsync();
        const selectedArticleData = this.$route.query.selectedArticle;
        if (selectedArticleData) {
            this.selectedArticle = JSON.parse(selectedArticleData);

            const groupName = this.selectedArticle.groupName;
            const articleType = this.selectedArticle.articleType;

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
            }
        }
    },

    methods: {
        async getCurrentCabinetAsync() {
            const cabinetId = this.$route.params.id;
            const cabinet = await CabinetsMasterDataService.getAsync(cabinetId);
            this.cabinetName = cabinet.name;
            return cabinet;
        },
        async handleSelectedPatient(args) {
            args.cancelNavigation = true
            this.selectedPatient = args.selectedPatient;
        },
        openSearchPatientComponent() {
            this.selectedPatient = null;
        },
        confirmOrder() {
            this.$router.push({ name: "Cabinet" });
        },
    },
    computed: {
        patientName() {
            return this.selectedPatient ? this.selectedPatient.fullName : "";
        },
        isPatientSelected() {
            return !!this.selectedPatient;
        },
    },
}; 
</script>

<style type="text/css" scoped src="./Content/articles.css"></style>
