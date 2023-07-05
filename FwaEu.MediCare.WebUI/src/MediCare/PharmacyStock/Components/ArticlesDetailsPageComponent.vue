<template>
    <div class="page-articles">
        <div v-if="selectedArticle">
            <div class="text-left">
                <span class="article-name">{{ selectedArticle.name }}, {{ selectedArticle.unit }}, {{
                    selectedArticle.countInbox }}</span>
            </div>
        </div>
        <div class="info-container">
            <div class="text-left">
                <span>Pour Dimitri ASHIKHMIN</span>
            </div>
            <div class="icon-right-container">
                <Button class="custom-button" style="width: 100px;" label="Modifier"></Button>
            </div>
        </div>
        <div class="info-container">
            <div class="text-left">
                <span>Boîte entière</span>
            </div>
            <div class="icon-right-container">
                <InputSwitch v-model="checked" class="custom-switch" />
            </div>
        </div>
        <div class="info-container" v-if="checked">
            <div class="text-left">
                <span>Boîte de</span>
            </div>
            <div class="icon-right-containerr">
                <Dropdown v-model="selectedBoite" :options="boiteOptions" />
            </div>
        </div>
        <div class="info-container" v-if="!checked">
            <div class="text-left">
                <span>Quantité sortie (comprimés)</span>
            </div>
            <div class="icon-right-container">
                <InputNumber id="quantity" v-model="quantity" :min="1" :max="100" />
            </div>
        </div>
        <Button class="confirmer" style="width: 350px; margin-top: 50px; align-self: center;" label="Confirmer"></Button>
    </div>
</template>

<script>
import InputSwitch from 'primevue/inputswitch';
import Button from 'primevue/button';
import { ref } from 'vue';
import Dropdown from 'primevue/dropdown';
import InputNumber from 'primevue/inputnumber';
import CabinetsMasterDataService from "@/MediCare/Referencials/Services/cabinets-master-data-service";

export default {
    components: {
        InputSwitch,
        Button,
        Dropdown,
        InputNumber,
    },
    data() {
        const checked = ref(true);
        return {
            selectedArticle: null,
            checked: checked.value,
            boiteOptions: ["10 comprimes", "20 comprime", "30 comprime"],
            selectedBoite: "30 comprime",
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
    }
};
</script>
<style type="text/css" scoped src="./Content/articles.css">
</style>