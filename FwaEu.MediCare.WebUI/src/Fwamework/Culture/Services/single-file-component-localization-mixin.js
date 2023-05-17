import { loadMessagesAsync } from "./single-file-component-localization";

export default {
	created: async function () {
		await this.loadComponentMessagesAsync();

		if (typeof this.onMessagesLoadedAsync === 'function') {
			await this.onMessagesLoadedAsync();
		}

		this.$forceUpdate();//Force the current component to re-render (this only affects the current component, not all the page)
	},
	methods: {
		async loadComponentMessagesAsync() {
			if (!this.$options.i18n || !this.$options.i18n.messages || !this.$options.i18n.messages.getMessagesAsync) {
				throw new Error('You must declare a function getMessagesAsync(locale) within i18n.messages option');
			}

			await loadMessagesAsync(this, this.$options.i18n.messages.getMessagesAsync);
		}
	}
}