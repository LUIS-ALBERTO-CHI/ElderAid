import AbstractConfiguration from '@/Modules/GenericAdmin/Services/abstract-configuration';

class DosageFormConfiguration extends AbstractConfiguration {
    constructor() {
        super();

        this.columnsCustomizer.addCustomization('name', { index: 2000, width: 100 });
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
    }
};