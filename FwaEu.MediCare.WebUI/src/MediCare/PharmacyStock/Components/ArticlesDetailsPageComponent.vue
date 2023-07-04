<template>
    <div class="article-details">
        <div v-if="selectedArticle">
            <div class="name-container">
                <span class="article-name">{{ selectedArticle.name }}, {{ selectedArticle.unit }}, {{
                    selectedArticle.countInbox }}</span>
            </div>
        </div>
        <div class="info-container">
            <div class="name-container">
                <span>Pour Dimitri ASHIKHMIN</span>
            </div>
            <div class="button-container">
                <Button class="custom-button" style="width: 200px;" label="Modifier"></Button>
            </div>
        </div>
        <div class="switch-container">
            <div class="boite-default">
                <span>Boîte entière</span>
            </div>
            <div class="switch">
                <InputSwitch v-model="checked" class="custom-switch" />
            </div>
        </div>
        <div class="switch-option" v-if="checked">
            <div class="select-option">
                <span>Boîte de</span>
            </div>
            <div class="select-sector">
                <Dropdown v-model="selectedBoite" :options="boiteOptions" />
            </div>
        </div>
        <div class="number-option" v-if="!checked">
            <div class="select-option">
                <span>Quantité sortie (comprimés)</span>
            </div>
            <div class="select-sector">
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
    created() {
        const storedArticle = localStorage.getItem("selectedArticle");
        if (storedArticle) {
            this.selectedArticle = JSON.parse(storedArticle);
        }
    },
};
</script>