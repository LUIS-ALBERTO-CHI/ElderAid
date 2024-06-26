<template>
    <div class="protection-page-container"  v-if="patient !== null">
        <patient-info-component :patient="patient" />
        <div @click="goToIncontinenceLevelPage" class="protection-info-item">
            <div class="alert-content">
                <span>
                    Nivel de incontinencia : {{ $t(''+patient.incontinenceLevel) }}
                </span>
                <div v-if="patient.isIncontinenceLevelOverPassed" :style="{ color: '#f44538' }" class="alert-container">
                    <i class="fa-sharp fa-solid fa-circle-exclamation alert-icon"></i>
                    <span>Se supera el paquete de incontinencia</span>
                </div>
                <div v-else :style="{ color: '#00b300' }" class="alert-container">
                    <i class="fa-solid fa-circle-check alert-icon"></i>
                    <span>Se respeta el paquete de incontinencia</span>
                </div>
            </div>
            <i style="font-size: 30px;" class="fa-regular fa-angle-right chevron-icon"></i>
        </div>
        <div v-if="!isLoading" style="display: flex; flex-direction: column; margin-top: 20px;">
            <div v-if="filteredProtections.some(protection => 'article' && 'posology' in protection) && protectionDosages"
                 v-for="(protection, index) in filteredProtections" :key="index">
                <ProtectionAccordionTabComponent v-if="protection.posology.length > 0" :protection="protection" :protectionDosages="protection.posology"
                                                 @refreshData="refreshData" />
            </div>
        </div>
        <ProgressSpinner v-else />
        <Button @click="goToSearchArticle" label="Añadir protección"></Button>
    </div>
</template>
<script>
    import Accordion from 'primevue/accordion';
    import PatientInfoComponent from './PatientInfoComponent.vue';
    import Button from 'primevue/button';
    import ProtectionAccordionTabComponent from './ProtectionAccordionTabComponent.vue';
    import PatientService, { usePatient } from "@/ElderAid/Patients/Services/patients-service";
    import ProtectionsMasterDataService from '@/ElderAid/Patients/Services/protections-master-data-service';
    import ProtectionDosagesMasterDataService from '@/ElderAid/Referencials/Services/protection-dosages-master-data-service'
    import RecentArticlesMasterDataService from '@/ElderAid/Articles/Services/recent-articles-master-data-service';
    import { protectionType } from '@/ElderAid/Articles/article-filter-types';
    import ProgressSpinner from 'primevue/progressspinner';
    export default {
        components: {
            Accordion,
            PatientInfoComponent,
            Button,
            ProtectionAccordionTabComponent,
            ProgressSpinner
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
                patient: null,
                filteredProtections: [],
                protectionDosages: [],
                isLoading: false,
				patientData: {},
            };
        },
        async created() {
            this.patient = await this.patientLazy.getValueAsync();
            const incontinenceLevelData = await PatientService.getIncontinenceLevelAsync(this.patient.id);
            this.patient.isIncontinenceLevelOverPassed = incontinenceLevelData.overPassed > 0;
			this.patientData = await PatientService.getIncontinenceLevelAsync(this.patient.id);
            this.refreshData();
        },
        methods: {
            async fillProtections() {
                this.protectionDosages = await ProtectionDosagesMasterDataService.getAllAsync();
                const protections = await ProtectionsMasterDataService.getAllAsync();
                const protectionsArticleIds = protections.map(x => x.articleId);
                const articles = await RecentArticlesMasterDataService.getByIdsAsync(protectionsArticleIds);
                const protectionIds = protections.map(protection => protection.id)
                const filteredProtectionDosages = this.protectionDosages.filter(x => protectionIds.includes(x.protectionId))
                protections.forEach(protection => {
                    protection.article = articles.find(article => article.id === protection.articleId);
                    protection.posology = filteredProtectionDosages.filter(x => x.protectionId === protection.id);
                });
                this.filteredProtections = protections.filter(protection => protection.patientId === this.patient.id && protection.article != null);
                this.isLoading = false;
            },
            goToIncontinenceLevelPage() {
                const patientId = this.patient.id;
                this.$router.push({ name: 'IncontinenceLevel', params: { id: patientId } });
            },
            goToSearchArticle() {
                this.$router.push({ name: 'SearchArticleFromProtection', params: { id: this.patient.id }, query: { articleFilterType: protectionType } });
            },
            async refreshData() {
                this.isLoading = true;
                await ProtectionsMasterDataService.clearCacheAsync();
                await ProtectionDosagesMasterDataService.clearCacheAsync();
                this.fillProtections();
            }
        }
    }
</script>
<style type="text/css" scoped src="./Content/protection-page.css"></style>
