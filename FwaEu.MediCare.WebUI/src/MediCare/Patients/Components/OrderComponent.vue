<template>
    <div class="order-container" v-if="article">
        <div v-show="isOrderAlreadyInProgress" class="alert-container">
            <i class="fa-solid fa-triangle-exclamation"></i>
            <span> Commande déjà en cours de cet article pour ce patient</span>
        </div>
        <div class="type-quantity-container" v-if="article.countInBox == 1">
            <span>Commander des boites complètes :</span>
            <div class="icon-right-container">
                <InputSwitch :disabled="true" v-model="isBox" class="custom-switch" />
            </div>
        </div>
        <div v-else>
            <div class="type-quantity-container" v-if="isBox">
                <span>Commander des boites complètes :</span>
                <div class="icon-right-container">
                    <InputSwitch :disabled="isSwitchDisabled" v-model="isBox" class="custom-switch" />
                </div>
            </div>
            <div class="type-quantity-container" v-else>
                <span>Commander des comprimés :</span>
                <div class="icon-right-container">
                    <InputSwitch :disabled="isSwitchDisabled" v-model="isBox" class="custom-switch" />
                </div>
            </div>
        </div>
        <div v-show="!moreQuantityDisplayed" class="quantity-container">
            <div style="width: 75%;">
                <SelectButton class="quantity-select-button" v-model="selectedQuantity" :options="quantityOptions" />
            </div>
            <i @click="displayMoreQuantity()" class="fa fa-solid fa-plus add-icon" style="font-size: 20px;"></i>
        </div>
        <div v-show="moreQuantityDisplayed">
            <InputNumber ref="inputNumber" v-model="selectedQuantity" showButtons buttonLayout="horizontal"
                         style="width: 100%;" decrementButtonClassName="p-button-secondary"
                         incrementButtonClassName="p-button-secondary" incrementButtonIcon="fa fa-solid fa-plus"
                         decrementButtonIcon="fa fa-solid fa-minus" />

        </div>
        <div v-if="article.isDeleted" style="text-align: center; color : darkorange;">
            Cet article est supprimé
        </div>
        <div class="confirmation-container" v-else-if="showConfirmationDisplayed">
            <span>Etes vous sûr de commander ?</span>
            <div class="confirmaton-button-container">
                <Button @click="submitOrder()" label="OUI" outlined class="button-confirmation" />
                <Button @click="showConfirmation()" label="NON" outlined class="button-confirmation" />
            </div>
        </div>
        <Button v-if="isBox" @click="showConfirmation()" style="height: 35px !important;" :label="getQuantitySentance()" />
        <Button v-else @click="showConfirmation()" style="height: 35px !important;" :label="getQuantitySentanceIsBox()" />
        <div v-show="!showConfirmationDisplayed" class="footer-button-container">
            <Button style="height: 40px !important; width: 50%; font-size: 14px;"
                    label="Autres formats"
                    icon="fa fa-solid fa-angle-right" iconPos="right"
                    @click="goToSearchFormats(article.groupName)" />
            <Button style="height: 40px !important; width: 50%; font-size: 14px;"
                    label="Substitutions"
                    icon="fa fa-solid fa-angle-right" iconPos="right"
                    @click="goToSearchSubstituts(article.groupName)"
                    :disabled="article.substitutionsCount < 1" />
        </div>
    </div>
</template>

<script>

    import Button from 'primevue/button';
    import SelectButton from 'primevue/selectbutton';
    import InputNumber from 'primevue/inputnumber';
    import OrdersService from '@/MediCare/Orders/Services/orders-service';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import MasterDataManagerService from "@/Fwamework/MasterData/Services/master-data-manager-service";
    import OrderMasterDataService from "@/MediCare/Orders/Services/orders-master-data-service"
    import ArticleMasterDataService from '@/MediCare/Articles/Services/recent-articles-master-data-service';
    import InputSwitch from 'primevue/inputswitch';

    export default {
        components: {
            Button,
            SelectButton,
            InputNumber,
            InputSwitch
        },
        props: {
            article: {
                type: Object,
                required: true
            },
            patientOrders: {
                type: Array,
                required: true
            },
            patientId: {
                type: Number,
                required: true
            }
        },
        data() {
            return {
                moreQuantityDisplayed: false,
                quantityOptions: [1, 2, 3],
                selectedQuantity: 1,
                showConfirmationDisplayed: false,
                isBox: true,
                orderAlreadyInProgress: true,
                isSwitchDisabled: false,
                isOtherBox: this.article.countInBox == 1
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
            async submitOrder() {
                const modelOrder = [{
                    patientId: this.patientId,
                    articleId: this.article.id,
                    quantity: this.selectedQuantity,
                    isGalenicForm: this.isGalenicDosageForm,
                    isBox: this.isBox
                }];
                try {
                    await OrdersService.saveAsync(modelOrder).then(() => {
                        NotificationService.showConfirmation('Votre commande a été passée avec succès')
                    })
                    await OrderMasterDataService.clearCacheAsync();
                    await ArticleMasterDataService.clearCacheAsync();

                    if (!(this.$route.name === "Orders")) {
                        this.$router.push("/Orders")
                    }
                } catch (error) {
                    NotificationService.showError('Une erreur est survenue lors de la commande')
                }
                this.showConfirmationDisplayed = false;
                this.$emit('order-done');
            },
            getQuantitySentance() {
                let textToDisplay = "Commander"

                const quantityType = this.article.countInBox > 1 ? `boite de ${this.article.countInBox}` : this.article.invoicingUnit;
                if (this.article.isBox) {
                    textToDisplay += ` ${this.selectedQuantity} ${this.selectedQuantity > 1 ? 'comprimés' : 'comprimé'}`
                } else {
                    textToDisplay += ` ${this.selectedQuantity} ${quantityType}`
                }

                return textToDisplay;
            },
            getQuantitySentanceIsBox() {
                let textToDisplay = "Commander"

                if (!this.isBox) {
                    textToDisplay += ` ${this.selectedQuantity} ${this.selectedQuantity > 1 ? 'comprimés' : 'comprimé'}`
                }

                return textToDisplay;
            },
            goToSearchSubstituts(articleTitle) {
                this.currentArticle = articleTitle;
                this.$router.push({ name: 'SearchArticle', query: { searchMode: "substituts:" + articleTitle } })
            },
            goToSearchFormats(groupName) {
                this.currentArticle = groupName;
                this.$router.push({ name: 'SearchArticle', query: { searchMode: `formats:${groupName}` } })
            }
        },
        watch: {
            selectedQuantity: function (newValue, oldValue) {
                if (newValue == null || newValue == undefined || newValue <= 0)
                    this.selectedQuantity = oldValue;
            }
        },
        computed: {
            isOrderAlreadyInProgress() {
                return this.patientOrders.some(order => order.articleId === this.article.id && order.state === 'Pending');
            },
        },

    }
</script>
<style scoped type="text/css">
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

    .button-confirmation {
        border: none !important;
        height: 30px !important;
        color: #7092be !important
    }

    .type-quantity-container {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-top: 20px;
    }

    .custom-switch .p-inputswitch-slider {
        background-color: #FFFFFF;
    }

        .custom-switch .p-inputswitch-slider:before {
            background-color: #7092be;
        }
</style>