<template>
    <div class="accordion-content">
        <span style="font-weight: bold;">Agregar dosis</span>
        <div class="label-container">
            <span>Fecha de inicio :</span>
            <Calendar v-model="changeForm.startDate" style="width: 50% !important" showIcon />
        </div>
        <div class="label-container">
            <span>Fecha de fin :</span>
            <Calendar v-model="changeForm.endDate" style="width: 50% !important" showIcon />
        </div>
        <div class="label-container">
            <span>Dosificación :</span>
            <i @click="addPosology" style="font-size: 26px;" class="fa fa-solid fa-add"></i>
        </div>
        <div class="posology-container">
            <div v-for="(posologyItem, index) in changeForm.posology" :key="index">
                <div class="label-container">
                    <InputNumber v-model="posologyItem.quantity" ref="inputNumber" showButtons buttonLayout="horizontal" style="width: 55%; height: 40px !important;"
                                 decrementButtonClassName="p-button-secondary" incrementButtonClassName="p-button-secondary"
                                 incrementButtonIcon="fa fa-solid fa-plus" decrementButtonIcon="fa fa-solid fa-minus" />
                    <Calendar v-model="posologyItem.hour" style="width: 30% !important" timeOnly @update:modelValue="resetMinutes"/>
                    <i v-show="changeForm.posology.length > 1" @click="deletePosology(index)" style="font-size: 24px;" class="fa fa-solid fa-close"></i>
                </div>
            </div>
        </div>
        <Button @click="submitPosology" style="height: 40px !important;" label="Confirmar" />
    </div>
</template>

<script>
    import ProtectionService from '@/ElderAid/Patients/Services/protection-service';
    import Button from 'primevue/button';
    import InputNumber from 'primevue/inputnumber';
    import Calendar from 'primevue/calendar';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import ProtectionsMasterDataService from '@/ElderAid/Patients/Services/protections-master-data-service';
    import ProtectionDosagesMasterDataService from '@/ElderAid/Referencials/Services/protection-dosages-master-data-service'
    import ArticleMasterDataService from '@/ElderAid/Articles/Services/recent-articles-master-data-service';

    export default {
        components: {
            Button,
            InputNumber,
            Calendar
        },
        data() {
            return {
                changeForm: {
                    startDate: new Date(),
                    endDate: null,
                    posology: []
                },
            };
        },
        props: {
            article: {
                type: Object,
                required: true
            },
            patient: {
                type: Object,
                required: true
            }
        },
        async created() {
            this.addPosology()
        },
        methods: {
            async submitPosology() {
                const model = {
                    patientId: this.patient.id,
                    articleId: this.article.id,
                    startDate: new Date(this.changeForm.startDate),
                    stopDate: this.changeForm.endDate ? new Date(this.changeForm.endDate) : null,
                    protectionDosages: this.posologyArrayToDictionnary(),
                    articleUnit: this.article.unit ? this.article.unit : this.article.invoicingUnit
                };

                try {
                    await ProtectionService.createAsync(model).then(async () => {
						NotificationService.showConfirmation("La dosificación ha sido creada");
                        if (this.$route.name !== 'Protection') {
                            this.$router.push({ name: 'Protection' });
                        }
                        await ProtectionsMasterDataService.clearCacheAsync();
                        await ProtectionDosagesMasterDataService.clearCacheAsync();
                        await ArticleMasterDataService.clearCacheAsync();
                    });
                } catch {
                    NotificationService.showError("Sucedió un error en la creación de la dosificación");
                }
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
            posologyArrayToDictionnary() {
                this.changeForm.posology.forEach(element => {
                    element.hour = new Date(element.hour).getHours() + ":" + new Date(element.hour).getMinutes();
                });

                return this.changeForm.posology.reduce((acc, posology) => {
                    acc[posology.hour] = posology.quantity;
                    return acc;
                }, {});
            },
            resetMinutes(date) {
                date.setMinutes(0);
            }
        },
        computed: {

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
</style>