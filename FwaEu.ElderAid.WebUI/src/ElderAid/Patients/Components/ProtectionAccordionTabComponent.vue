<template>
    <Accordion v-if="protection != null">
        <AccordionTab>
            <template #header>
                <div class="accordion-header">
                    <span class="header-title">{{protection.article.title}}</span>
                    <span class="header-subtitle">{{protection.dosageDescription }}</span>
                    <span class="header-subtitle">
                        {{
isGoodEndDate(protection.dateEnd, protection.dateStart) ?
                    `Del ${$d(new Date(protection.dateStart))} a las ${$d(new Date(protection.dateEnd))}`
                    : $d(new Date(protection.dateStart))
                        }}
                    </span>
                </div>
            </template>
            <div v-if="protectionState == ProtectionState.Normal" class="accordion-content">
                <Button @click="changePosology" label="Cambiar la dosis" style="height: 40px !important;"></Button>
                <Button v-show="isStopDatePassed(protection.dateEnd)" @click="stopPosology" label="Detener cuidado" style="height: 40px !important;"></Button>
            </div>
            <div v-else-if="protectionState == ProtectionState.Change">
                <ChangePosologyComponent :protection="protection" :protectionDosages="protectionDosages" @changePosologySubmitted="changedPosology" />
            </div>
            <div v-else class="accordion-content">
                <span style="font-weight: bold;">Detener cuidado</span>
                <div class="label-container">
                    <span>Date de fin :</span>
                    <Calendar v-model="stopEndDate" style="width: 50% !important" showIcon />
                </div>
                <Button @click="submitStoppedPosology" style="height: 40px !important;" label="Confirmar" />
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
    import ProtectionService from '@/ElderAid/Patients/Services/protection-service';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import ChangePosologyComponent from '@/ElderAid/Patients/Components/ChangePosologyComponent.vue'


    export default {
        components: {
            Accordion,
            AccordionTab,
            Button,
            InputNumber,
            Calendar,
            ChangePosologyComponent


        },
        data() {
            const ProtectionState = Object.freeze({ Normal: 1, Change: 2, Stop: 3 });
            return {
                ProtectionState,
                protectionState: ProtectionState.Normal,
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
            this.stopEndDate = new Date(this.protection.dateEnd);
        },
        methods: {
            changePosology() {
                this.protectionState = this.ProtectionState.Change;
            },
            stopPosology() {
                this.protectionState = this.ProtectionState.Stop;
            },
            isStopDatePassed(protectionDateEnd) {
                var dateEnd = new Date(protectionDateEnd);
                return dateEnd < new Date();
            },
            changedPosology() {
                this.protectionState = this.ProtectionState.Normal;
                this.$emit('refreshData');
            },
            async submitStoppedPosology() {
                const model = {
                    protectionId: this.protection.id,
                    stopDate: new Date(this.stopEndDate),
                    patientId: this.protection.patientId
                }
                try {
                    await ProtectionService.stopAsync(model).then(() => {
						NotificationService.showConfirmation("Se ha suspendido la dosis");
                        this.protectionState = this.ProtectionState.Normal;
                        this.$emit('refreshData');
                    });
                } catch {
                    NotificationService.showError("Une erreur est survenue lors de l'arrêt de la posologie");
                }
            },
            isGoodEndDate(dateEnd, dateStart) {
                if (dateEnd === null || dateEnd < dateStart) {
                    return false
                } else {
                    return true
                }
            }
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