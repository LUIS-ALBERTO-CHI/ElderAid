<template>
    <div class="order-container">
        <div v-show="orderAlreadyInProgress" class="alert-container">
            <i class="fa-solid fa-triangle-exclamation"></i>
            <span> Commande déjà en cours de cet article pour ce patient</span>
        </div>
        <span>Commander une quantité :</span>
        <div v-show="!moreQuantityDisplayed" class="quantity-container">
            <div style="width: 75%;">
                <SelectButton  class="quantity-select-button" v-model="selectedQuantity" :options="quantityOptions"/>
            </div>
            <i @click="displayMoreQuantity()" class="fa fa-solid fa-plus add-icon" style="font-size: 20px;"></i>
        </div>
        <div v-show="moreQuantityDisplayed">
            <InputNumber ref="inputNumber" v-model="selectedQuantity" showButtons buttonLayout="horizontal" style="width: 75%;"
                         decrementButtonClassName="p-button-secondary" incrementButtonClassName="p-button-secondary"
                         incrementButtonIcon="fa fa-solid fa-plus" decrementButtonIcon="fa fa-solid fa-minus" />

        </div>
        <div class="confirmation-container" v-if="showConfirmationDisplayed">
            <span>Etes vous sûr de commander ?</span>
            <div class="confirmaton-button-container">
                <Button @click="submitOrder()" label="OUI" outlined style="border: none !important; height: 30px !important;" />
                <Button @click="showConfirmation()" label="NON" outlined style="border: none !important; height: 30px !important;" />
            </div>
        </div>
        <Button v-else @click="showConfirmation()" style="height: 35px !important;" label="Commander" />
        <div v-show="!showConfirmationDisplayed" class="footer-button-container">
            <Button  style="height: 35px !important; width: 50%;" label="4 autres formats" icon="fa fa-solid fa-angle-right" iconPos="right"/>
            <Button  style="height: 35px !important; width: 50%;" label="8 substitutions"  icon="fa fa-solid fa-angle-right" iconPos="right"/>
        </div>
    </div>

</template>
<!-- eslint-disable @fwaeu/custom-rules/no-local-storage -->
<script>

import Button from 'primevue/button';
import SelectButton from 'primevue/selectbutton';
import InputNumber from 'primevue/inputnumber';

    export default {
        components: {
            Button,
            SelectButton,
            InputNumber
        },
        data() {
            return {
                patient: {},
                moreQuantityDisplayed: false,
                quantityOptions: [1, 2, 3],
                selectedQuantity: 1,
                showConfirmationDisplayed: false,
                orderAlreadyInProgress: true,
            };
        },
        async created() {
        },
        methods: {
            displayMoreQuantity() {
                this.moreQuantityDisplayed = !this.moreQuantityDisplayed;
            },
            showConfirmation() {
                this.showConfirmationDisplayed = !this.showConfirmationDisplayed;
            },
            submitOrder() {
                this.$emit('submitOrder', this.selectedQuantity);
                this.showConfirmationDisplayed = false;
            }
        },
        watch: {
            selectedQuantity: function (newValue, oldValue) {
                if (newValue == null || newValue == undefined || newValue <= 0)
                    this.selectedQuantity = oldValue;
            }
        },
        computed: {

        },

    }
</script>
<style type="text/css">
.order-container {
    display: flex;
    flex-direction: column;
    row-gap: 20px;
}
.quantity-container { 
    width: 100%;
    display: flex;
    align-items: center;
    column-gap: 15px
}

.confirmation-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    flex-wrap: wrap;
}

.confirmation-button-container {
    display: flex;
    column-gap: 10px;
}

.footer-button-container {
    display: flex;
    flex-direction: row;
    column-gap: 5px;
}
.alert-container {
    color: #f44538;

}
</style>