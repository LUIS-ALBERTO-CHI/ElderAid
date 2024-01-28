<template>
    <div class="organization-selection-page-container">
        <span class="title">Veuillez sélectionner un EMS dans la liste déroulante</span>
        <Dropdown v-model="selectedOrganization" :options="organizationsOptions"
            optionLabel="name" style="width: 100%;" placeholder="Choisir une organisation"/>
            <Button :disabled="selectedOrganization == null" @click="refreshMasterDataByDatabaseInvariantId" label="Confirmer le changement" />
    </div>
</template>


<script>

import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';
import Dropdown from 'primevue/dropdown';
import ViewContextService, { ViewContextModel } from '@/ElderAid/ViewContext/Services/view-context-service';

import OrganizationsMasterDataService from "@/ElderAid/Organizations/Services/organizations-master-data-service";

import MasterDataManagerService from "@/Fwamework/MasterData/Services/master-data-manager-service";
import CachePreloaderService from '@/ElderAid/Cache/Services/cache-preloader-service';
import Button from 'primevue/button';
import NotificationService from '@/Fwamework/Notifications/Services/notification-service';
import viewContextService from '@/ElderAid/ViewContext/Services/view-context-service';


export default {
    components: {
        Dropdown,
        Button
    },
    data() {
        return {
            selectedOrganization: null,
            organizationsOptions: [],
            currentDatabase: ViewContextService.get()?.id,
            viewContextChangeOff: ViewContextService.onChanged((viewContext) => {
                this.currentDatabase = viewContext.id;
            }),

        };
    },
    created: showLoadingPanel(async function () {
        this.organizations = await OrganizationsMasterDataService.getAllAsync();
        this.organizationsOptions = this.organizations
        this.selectedOrganization = this.organizations.find(x => x.id == this.currentDatabase);


        const currentOrganization = viewContextService.get();
        if (currentOrganization != null && currentOrganization.updatedOn != new Date(this.selectedOrganization.updatedOn)) {
            ViewContextService.set(new ViewContextModel(this.selectedOrganization));
        }

    }),
    methods: {
        refreshMasterDataByDatabaseInvariantId: showLoadingPanel(async function (e) {

            // NOTE : Update the ViewContext to save the selected database
            ViewContextService.set(new ViewContextModel(this.selectedOrganization));
            // NOTE : refraichir toutes les masterdata
            await MasterDataManagerService.clearCacheAsync();

            await CachePreloaderService.loadAllMasterDataAsync(true, true);

            NotificationService.showConfirmation("Organisation mis à jour avec succès");
            this.$router.push({ name: 'default' });

        }),
    },
}
</script>
<style type="text/css" scoped src="./Content/organization-selection-page.css">
</style>