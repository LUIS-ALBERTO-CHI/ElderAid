/**
 * @param {import("vue/types/vue").Vue} componentInstance
 * @param {(locale: String)=> Promise} loadMessagesFunction
 */
export async function loadMessagesAsync(componentInstance, loadMessagesFunction) {
	const messagesResult = await loadMessagesFunction(componentInstance.$i18n.locale.value);
	componentInstance.$i18n.mergeLocaleMessage(componentInstance.$i18n.locale.value, messagesResult.default);
}