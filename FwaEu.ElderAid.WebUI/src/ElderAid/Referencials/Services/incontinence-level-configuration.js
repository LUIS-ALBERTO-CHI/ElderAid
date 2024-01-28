import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';
import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
import { CanAdministrateDosageFormMasterData } from '@/ElderAid/Referencials/referencials-permissions';

class IncontinenceLevelConfiguration extends AbstractConfiguration {
    constructor() {
        super();
    }

    getResources(locale) {
        return [import(`@/ElderAid/Referencials/Content/incontinence-level-messages.${locale}.json`)];
    }

    getPageTitle(resourcesManager) {
        return resourcesManager.getResource(['incontinenceLevelTitle']);
    }

    getDescription(resourcesManager) {
        return resourcesManager.getResource(['incontinenceLevelDescription']);
    }
}

export default {
    configurationKey: 'IncontinenceLevelEntity',
    icon: "fa-solid fa-dial",
    getConfiguration: function () {
        return new IncontinenceLevelConfiguration();
    },
    async isAccessibleAsync() {
        return await hasPermissionAsync(CanAdministrateDosageFormMasterData);
    }
};