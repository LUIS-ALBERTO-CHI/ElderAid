<template>
    <div class="page-articles">
        <SearchPatientComponent v-show="!patientSelected" @selectedPatient="handleSelectedPatient" />
        <div v-show="patientSelected">
            <div v-if="selectedArticle">
                <div class="text-left">
                    <span class="article-name">{{ selectedArticle.name }}, {{ selectedArticle.unit }}, {{
                        selectedArticle.countInbox }}</span>
                </div>
            </div>
            <div class="info-container">
                <div class="text-left">
                    <span>Pour {{ selectedPatientName }}</span>
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
                    <Dropdown v-model="selectedBoite" :options="boiteOptions" />
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
            boiteOptions: ["10 comprimes", "20 comprime", "30 comprime"],
            selectedBoite: "30 comprime",
            quantity: 1,
            patientSelected: false,
            selectedPatientName: "",
        };
    },
    async created() {
        const storedArticle = localStorage.getItem("selectedArticle");
        if (storedArticle) {
            this.selectedArticle = JSON.parse(storedArticle);
        }
        await this.getCurrentCabinetAsync();
    },
    methods: {
        async getCurrentCabinetAsync() {
            const cabinetId = this.$route.params.id;
            const cabinet = await CabinetsMasterDataService.getAsync(cabinetId);
            this.cabinetName = cabinet.name;
            return cabinet;
        },
        handleSelectedPatient(args) {
            if (!args.cancelNavigation) {
                this.patientSelected = true;
                this.selectedPatient = args.selectedPatient;
                this.selectedPatientName = `${args.selectedPatient.fullName}`;

                if (this.selectedPatientName) {
                    args.cancelNavigation = true;
                }
            }
        },
        openSearchPatientComponent() {
            this.patientSelected = false;
        },
        confirmOrder() {
            this.$router.push({ name: "Cabinet" });
        },
    },
}; 
</script>

<style type="text/css" scoped src="./Content/articles.css"></style>
