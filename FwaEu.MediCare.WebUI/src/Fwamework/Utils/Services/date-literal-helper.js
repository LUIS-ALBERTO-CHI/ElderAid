import { createApp, defineComponent } from 'vue';
import DateLiteral from '@/Fwamework/Utils/Components/DateLiteralComponent.vue';
import LocalizationService from '@/Fwamework/Culture/Services/localization-service';

export const createDateLiteral = (props) => {
	const componentDefinition = defineComponent({
		extends: defineComponent({ ...DateLiteral })
	});
	const vueApp = createApp(componentDefinition, props);
	LocalizationService.configureAsync(vueApp);
	const vueAppContainer = document.createElement('div');
	const componentInstance = vueApp.mount(vueAppContainer);
	componentInstance.unmount = function () {
		vueApp.unmount.bind(vueApp)();
		vueAppContainer.remove();
	}
	return componentInstance;
};