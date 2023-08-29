<template>
    <div class="accordion-content">
        <span style="font-weight: bold;">Changer la posologie</span>
        <div class="label-container">
            <span>Date de changement :</span>
            <Calendar v-model="changeForm.startDate" style="width: 50% !important" showIcon :class="{'p-invalid': validationErrors.startDate }" />
        </div>
        <div class="label-container">
            <span>Date de fin :</span>
            <Calendar v-model="changeForm.endDate" style="width: 50% !important" showIcon :class="{'p-invalid': validationErrors.endDate }" />
        </div>
        <div class="label-container">
            <span>Posologie :</span>
            <i @click="addPosology" style="font-size: 26px;" class="fa fa-solid fa-add"></i>
        </div>
        <div class="posology-container">
            <div v-for="(posologyItem, index) in changeForm.posology" :key="index">
                <div class="label-container">
                    <InputNumber v-model="posologyItem.quantity"
                                 ref="inputNumber"
                                 showButtons
                                 buttonLayout="horizontal"
                                 style="width: 55%; height: 40px !important;"
                                 decrementButtonClassName="p-button-secondary"
                                 incrementButtonClassName="p-button-secondary"
                                 incrementButtonIcon="fa fa-solid fa-plus"
                                 decrementButtonIcon="fa fa-solid fa-minus" :class="{'p-invalid': checkErrorQuantity(index) }" />
                    <Calendar v-model="posologyItem.hour"
                              style="width: 30% !important"
                              timeOnly
                              @update:modelValue="resetMinutes" :class="{'p-invalid': checkErrorDate(index) }" />
                    <i v-show="changeForm.posology.length > 1"
                       @click="deletePosology(index)"
                       style="font-size: 24px;"
                       class="fa fa-solid fa-close"></i>
                </div>
                <div class="error-message" v-if="validationErrors[index]">{{ validationErrors[index] }}</div>
            </div>
        </div>
        <div class="error-message" v-if="formError">{{ formError }}</div>
        <Button @click="checkValidation" style="height: 40px !important;" label="Confirmer" />
    </div>
</template>

<script>

    import Button from 'primevue/button';
    import InputNumber from 'primevue/inputnumber';
    import Calendar from 'primevue/calendar';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import ProtectionService from '@/MediCare/Patients/Services/protection-service';
    import * as Yup from 'yup';

    export default {
        components: {
            Button,
            InputNumber,
            Calendar
        },
        props: {
            protection: {
                type: Object,
                required: true
            },
            protectionDosages: {
                type: Object,
                required: true
            }
        },
        data() {
            return {
                changeForm: {
                    startDate: new Date(),
                    endDate: null,
                    posology: []
                },
                validationErrors: {
                    startDate: null,
                    endDate: null,
                    posology: []
                },
                formError: "",
            };
        },
        async created() {
            this.changeForm.startDate = new Date(this.protection.dateStart);
            this.changeForm.endDate = new Date(this.protection.dateEnd);
            this.fillPosology();

        },
        methods: {
            checkErrorQuantity(index) {
                return this.validationErrors?.posology?.some(x => x.index == index && x.hour !== null);
            },
            checkErrorDate(index) {
                return this.validationErrors?.posology?.some(x => x.index == index && x.hour === null);
            },
            addPosology() {
                var date = new Date();
                date = date.setMinutes(0);
                this.changeForm.posology.push({
                    quantity: 1,
                    hour: new Date(date)
                });
            },
            deletePosology(index) {
                this.changeForm.posology.splice(index, 1)
            },
            async submitChangedPosology() {
                const model = {
                    protectionId: this.protection.id,
                    startDate: new Date(this.changeForm.startDate),
                    stopDate: this.changeForm.endDate ? new Date(this.changeForm.endDate) : null,
                    protectionDosages: this.posologyArrayToDictionnary(),
                    articleUnit: this.protection.article.unit ? this.protection.article.unit : this.protection.article.invoicingUnit
                }
                try {
                    await ProtectionService.updateAsync(model).then(() => {
                        NotificationService.showConfirmation("La posologie a bien été modifiée");
                        this.$emit('changePosologySubmitted');
                    })
                } catch {
                    NotificationService.showError("Une erreur est survenue lors de la modification de la posologie");
                }
            },
            posologyArrayToDictionnary() {
                this.changeForm.posology.forEach(element => {
                    element.hour = new Date(element.hour).getHours() + ":" + new Date(element.hour).getMinutes();
                });

                return this.changeForm.posology.reduce((acc, posology) => {
                    acc[posology.hour] = posology.quantity;
                    return acc;
                }, {});
            },
            fillPosology() {
                this.protectionDosages.forEach(posology => {
                    this.changeForm.posology.push({
                        quantity: posology.quantity,
                        hour: new Date(posology.hour)
                    });
                });
                if (this.changeForm.posology.length == 0) {
                    this.addPosology();
                }
            },
            resetMinutes(date) {
                if (date != null)
                    date.setMinutes(0);
            },
            async checkValidation() {
                this.formError = "";
                const validationSchema = Yup.object().shape({
                    startDate: Yup.date().required("La date de changement est requise."),
                    endDate: Yup.date()
                        .required("La date de fin est requise.")
                        .min(Yup.ref('startDate'), "La date de fin doit être supérieure ou égale à la date de changement."),
                    posology: Yup.array().of(
                        Yup.object().shape({
                            quantity: Yup.number()
                                .required("La quantité de la posologie est requise.")
                                .min(1, "La quantité doit être supérieure à 0"),
                            hour: Yup.date().required("L'heure est requise."),
                        })
                    ),
                });
                try {
                    await validationSchema.validate(this.changeForm, { abortEarly: false });
                    await this.submitChangedPosology();
                } catch (validationError) {
                    if (validationError.inner) {
                        validationError.inner.forEach(error => {
                            if (error.path.startsWith("posology")) {
                                const index = Number(error.path.split(/[\[\]]/)[1]);
                                this.validationErrors.posology.push({
                                    index: index,
                                    quantity: this.changeForm.posology[index]?.quantity,
                                    hour: this.changeForm.posology[index]?.hour ?? null
                                });
                            } else {
                                this.validationErrors[error.path] = error.message;
                            }
                            this.formError = error.message;
                        });
                    }
                }
            },

        },
    }
</script>
<style type="text/css">
    .accordion-content {
        display: flex;
        flex-direction: column;
        row-gap: 20px;
        color: var(--alt-secondary-text-color);
    }

    .label-container {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .posology-container {
        display: flex;
        flex-direction: column;
        row-gap: 10px;
    }

    .error-message {
        color: red;
        font-size: 14px;
        margin-top: 4px;
        align-self: center;
    }
</style>