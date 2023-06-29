import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateDosageFormMasterData } from '@/MediCare/Referencials/referencials-permissions';

class DosageFormConfiguration extends AbstractConfiguration {
    constructor() {
        super();
    }

    getResources(locale) {
        return [import(`@/MediCare/Referencials/Content/dosage-form-messages.${locale}.json`)];
    }

    getPageTitle(resourcesManager) {
        return resourcesManager.getResource(['dosageFormTitle']);
    }

    getDescription(resourcesManager) {
        return resourcesManager.getResource(['dosageFormDescription']);
    }
}

export default {
    configurationKey: 'DosageFormEntity',
    icon: "fa-solid fa-pills",
    getConfiguration: function () {
        return new DosageFormConfiguration();
    },
    async isAccessibleAsync() {
        return await hasPermissionAsync(CanAdministrateDosageFormMasterData);
    }
};