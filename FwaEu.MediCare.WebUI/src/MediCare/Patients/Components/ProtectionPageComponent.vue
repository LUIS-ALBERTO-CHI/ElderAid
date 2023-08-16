<template>
    <div class="protection-page-container">
        <patient-info-component v-if="patient" :patient="patient" />
        <div @click="goToIncontinenceLevelPage" class="protection-info-item">
            <div class="alert-content">
                <span>Niveau d'incontience: légère</span>
                <div v-if="isAlert" :style="{ color: '#f44538' }" class="alert-container">
                    <i class="fa-sharp fa-solid fa-circle-exclamation alert-icon"></i>
                    <span>Le forfait d'incontinence est dépassé</span>
                </div>
                <div v-else :style="{ color: '#00b300' }" class="alert-container">
                    <i class="fa-solid fa-circle-check alert-icon"></i>
                    <span>Le forfait d'incontinence est respecté</span>
                </div>
            </div>
            <i style="font-size: 30px;" class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div style="display: flex; flex-direction: column; margin-top: 20px;">
            <div v-if="filteredProtections && filteredProtections.some(protection => 'article' in protection) "
                 v-for="(protection, index) in filteredProtections.filter(protection => protection.article != null)" :key="index">
                <ProtectionAccordionTabComponent :protection="protection" :protectionDosages="getProtectionDosages(protection)"
                                                 @refreshData="refreshData" />
            </div>
        </div>
        <Button label="Imprimer le protocole"></Button>
        <Button @click="goToSearchArticle" label="Ajouter une protection"></Button>

    </div>
</template>

<script>

    import Accordion from 'primevue/accordion';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import Button from 'primevue/button';
    import ProtectionAccordionTabComponent from './ProtectionAccordionTabComponent.vue';
    import { usePatient } from "@/MediCare/Patients/Services/patients-service";
    import ProtectionsMasterDataService from '@/MediCare/Patients/Services/protections-master-data-service';
    import ProtectionDosagesMasterDataService from '@/MediCare/Referencials/Services/protection-dosages-master-data-service'
    import ArticlesMasterDataService from '@/MediCare/Articles/Services/articles-master-data-service';

    export default {
        components: {
            Accordion,
            PatientInfoComponent,
            Button,
            ProtectionAccordionTabComponent,
        },
        setup() {
            const { patientLazy, getCurrentPatientAsync } = usePatient();
            return {
                patientLazy,
                getCurrentPatientAsync
            }
        },
        data() {
            return {
                isAlert: true,
                patient: null,
                protections: [],
                filteredProtections: [],
                protectionDosages: [],
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();
            this.protections = await ProtectionsMasterDataService.getAllAsync();
            this.fillProtections();

        },
        methods: {
            async fillProtections() {
                const model = {
                    patientId: this.patient.id,
                };
                const protections = await ProtectionsMasterDataService.getAllAsync(model);

                const protectionsArticleIds = protections.map(x => x.articleId);
                const articles = await ArticlesMasterDataService.getByIdsAsync(protectionsArticleIds);
                protections.forEach(protection => {
                    const article = articles.find(article => article.id === protection.articleId);
                    protection.article = article;
                });

                this.protections = protections;
                this.filteredProtections = protections.filter(protection => protection.patientId === this.patient.id);
                this.protectionDosages = await ProtectionDosagesMasterDataService.getAllAsync();

            },
            goToIncontinenceLevelPage() {
                const patientId = this.patient.id;
                this.$router.push({ name: 'IncontinenceLevel', params: { id: patientId } });
            },
            getProtectionDosages(protection) {
                return this.protectionDosages.filter(x => x.protectionId === protection.id)
            },
            goToSearchArticle() {
                this.$router.push({ name: 'SearchArticleFromProtection', params: { id: this.patient.id } });
            },
            async refreshData() {
                await ProtectionsMasterDataService.clearCacheAsync();
                await ProtectionDosagesMasterDataService.clearCacheAsync();
                this.fillProtections();
            }
        },
        computed: {

        },

    }
</script>
<style type="text/css" scoped src="./Content/protection-page.css"></style>