<template>
    <Accordion v-if="protection != null">
        <AccordionTab>
            <template #header>
                <div class="accordion-header">
                    <span class="header-title">{{protection.article.title}}</span>
                    <span class="header-subtitle">{{protection.dosageDescription }}</span>
                    <span class="header-subtitle">De {{ $d(new Date(protection.dateStart)) }} à {{ $d(new Date(protection.dateEnd)) }}</span>
                </div>
            </template>
            <div v-if="protectionState == ProtectionState.Normal" class="accordion-content">
                <Button @click="changePosology" label="Changer la posologie" style="height: 40px !important;"></Button>
                <Button v-show="isStopDatePassed(protection.dateEnd)" @click="stopPosology" label="Arrêter" style="height: 40px !important;"></Button>
            </div>
            <div v-else-if="protectionState == ProtectionState.Change" class="accordion-content">
                <span style="font-weight: bold;">Changer la posologie</span>
                <div class="label-container">
                    <span>Date de changement :</span>
                    <Calendar v-model="changeForm.startDate" style="width: 50% !important" showIcon />
                </div>
                <div class="label-container">
                    <span>Date de fin :</span>
                    <Calendar v-model="changeForm.endDate" style="width: 50% !important" showIcon />
                </div>
                <div class="label-container">
                    <span>Posologie :</span>
                    <i @click="addPosology" style="font-size: 26px;" class="fa fa-solid fa-add"></i>
                </div>
                <div class="posology-container">
                    <div v-for="(posologyItem, index) in changeForm.posology" :key="index">
                        <div class="label-container">
                            <InputNumber v-model="posologyItem.quantity" ref="inputNumber" showButtons buttonLayout="horizontal" style="width: 55%; height: 40px !important;"
                                         decrementButtonClassName="p-button-secondary" incrementButtonClassName="p-button-secondary"
                                         incrementButtonIcon="fa fa-solid fa-plus" decrementButtonIcon="fa fa-solid fa-minus" />
                            <Calendar v-model="posologyItem.hour" style="width: 30% !important" timeOnly />
                            <i v-show="changeForm.posology.length > 1" @click="deletePosology(index)" style="font-size: 24px;" class="fa fa-solid fa-close"></i>
                        </div>
                    </div>
                </div>
                <Button @click="submitChangedPosology" style="height: 40px !important;" label="Confirmer" />
            </div>
            <div v-else class="accordion-content">
                <span style="font-weight: bold;">Arrêter</span>
                <div class="label-container">
                    <span>Date de fin :</span>
                    <Calendar v-model="stopEndDate" style="width: 50% !important" showIcon />
                </div>
                <Button @click="submitStoppedPosology" style="height: 40px !important;" label="Confirmer" />

            </div>
        </AccordionTab>
    </Accordion>
</template>

<script>

    import Accordion from 'primevue/accordion';
    import AccordionTab from 'primevue/accordiontab';
    import Button from 'primevue/button';
    import InputNumber from 'primevue/inputnumber';
    import Calendar from 'primevue/calendar';
    import ProtectionService from '@/MediCare/Patients/Services/protection-service';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';



    export default {
        components: {
            Accordion,
            AccordionTab,
            Button,
            InputNumber,
            Calendar
        },
        data() {
            const ProtectionState = Object.freeze({ Normal: 1, Change: 2, Stop: 3 });
            return {
                ProtectionState,
                protectionState: ProtectionState.Normal,
                changeForm: {
                    startDate: new Date(),
                    endDate: new Date(),
                    posology: []
                },
                stopEndDate: new Date(),
            };
        },
        props: {
            protection: {
                type: Object,
                required: true
            },
            protectionDosages: {
                type: Array,
                required: true
            }
        },
        async created() {
            this.changeForm.startDate = new Date(this.protection.dateStart);
            this.changeForm.endDate = new Date(this.protection.dateEnd);
            this.stopEndDate = new Date(this.protection.dateEnd);
            this.fillPosology();
        },
        methods: {
            changePosology() {
                this.protectionState = this.ProtectionState.Change;
            },
            stopPosology() {
                this.protectionState = this.ProtectionState.Stop;
            },
            addPosology() {
                this.changeForm.posology.push({
                    quantity: 0,
                    hour: new Date()
                });
            },
            deletePosology(index) {
                this.changeForm.posology.splice(index, 1)
            },
            isStopDatePassed(protectionDateEnd) {
                var dateEnd = new Date(protectionDateEnd);
                return dateEnd < new Date();
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
            async submitChangedPosology() {
                const model = {
                    protectionId: this.protection.id,
                    startDate: new Date(this.changeForm.startDate),
                    stopDate: new Date(this.changeForm.endDate),
                    protectionDosages: this.posologyArrayToDictionnary(),
                    articleUnit: "test"
                }
                try {
                    await ProtectionService.updateAsync(model);
                    NotificationService.showConfirmation("La posologie a bien été modifiée");
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
            async submitStoppedPosology() {
                const model = {
                    protectionId: this.protection.id,
                    stopDate: new Date(this.stopEndDate),
                }
                try {
                    await ProtectionService.stopAsync(model);
                    NotificationService.showConfirmation("La posologie a bien été arrêtée");
                } catch {
                    NotificationService.showError("Une erreur est survenue lors de l'arrêt de la posologie");
                }
            }
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

    .header-title {
        font-size: 16px;
        font-weight: bold;
        width: 90%;
    }

    .header-subtitle {
        font-size: 14px;
        opacity: 0.8;
        font-weight: 500;
        width: 90%;
    }

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
</style>