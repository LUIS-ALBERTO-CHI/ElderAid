import { createApp, defineComponent } from 'vue';
import LanguageSelectBox from '@/Fwamework/Culture/Components/LanguageSelectBoxComponent.vue';
import LocalizationService from '@/Fwamework/Culture/Services/localization-service';

export const createLanguageBox = (props) => {
	const componentDefinition = defineComponent({
		extends: defineComponent({ ...LanguageSelectBox })
	});
	const vueApp = createApp(componentDefinition, {
		languages: LocalizationService.getSupportedLanguages(),
		...props
	});
	LocalizationService.configureAsync(vueApp);
	const vueAppContainer = document.createElement('div');
	const componentInstance = vueApp.mount(vueAppContainer);
	componentInstance.unmount = function () {
		vueApp.unmount.bind(vueApp)();
		vueAppContainer.remove();
	}
	return componentInstance;
};