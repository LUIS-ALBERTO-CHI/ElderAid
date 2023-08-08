import { PrimeIcons } from "primevue/api";

export default {
    configure(vueApp) {
        this.vueApp = vueApp;
    },
    async alertAsync(message, title) {
        const I18n = (await import("@/Fwamework/Culture/Services/localization-service")).I18n;
        this.vueApp.$confirm.require({
            header: title,
            message: message,
            icon: PrimeIcons.TIMES_CIRCLE,
            rejectLabel: ' ',
            acceptLabel: I18n.t('ok'),
            accept: () => {
                this.vueApp.$confirm.close();
            }
        });
    },
    open(component, options){
        return this.vueApp.$dialog.open(component, options);
    },
}