import AbstractModule from "@/Fwamework/Core/Services/abstract-module-class";
import VSelect from "vue-select";
import 'vue-select/dist/vue-select.css';


export class VueSelectModule extends AbstractModule {
    onInitAsync(vueApp) {
        vueApp.component("v-select", VSelect)
    }
}