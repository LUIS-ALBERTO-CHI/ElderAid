<template>
    <div class="accordion-content">
        <span style="font-weight: bold;">Ajouter la posologie</span>
        <div class="label-container">
            <span>Date de début :</span>
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
        <Button @click="submitPosology" style="height: 40px !important;" label="Confirmer" />
    </div>
</template>

<script>
    import ProtectionService from '@/MediCare/Patients/Services/protection-service';
    import Button from 'primevue/button';
    import InputNumber from 'primevue/inputnumber';
    import Calendar from 'primevue/calendar';
    import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
    import ProtectionsMasterDataService from '@/MediCare/Patients/Services/protections-master-data-service';
    import ProtectionDosagesMasterDataService from '@/MediCare/Referencials/Services/protection-dosages-master-data-service'

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
                    stopDate: new Date(this.changeForm.endDate),
                    protectionDosages: this.posologyArrayToDictionnary(),
                    articleUnit: "test"
                }
                try {
                    await ProtectionService.createAsync(model).then(async() => {
                        NotificationService.showConfirmation("La posologie a bien été créée");
                        if (this.$route.name !== 'Protection') {
                            this.$router.push({ name: 'Protection' })
                        }
                        await ProtectionsMasterDataService.clearCacheAsync();
                        await ProtectionDosagesMasterDataService.clearCacheAsync();
                    })
                } catch {
                    NotificationService.showError("Une erreur est survenue lors de la création de la posologie");
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