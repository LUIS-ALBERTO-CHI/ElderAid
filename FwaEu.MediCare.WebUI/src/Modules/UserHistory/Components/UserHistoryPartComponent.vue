<template>
	<div>
		<timeline :items="getHistoryEntries()" />
	</div>
</template>
<script>
	import Timeline from '@/Fwamework/Users/Components/TimelineComponent.vue';
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	export default {
		components: {
			Timeline
		},

		mixins: [LocalizationMixin],
		i18n: {

			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/user-history-part-messages.${locale}.json`);
				}
			}
		},
		props: {
			modelValue: {
				type: Object,
				default: () => { }
			}
		},

		methods: {
			getHistoryEntries() {
				let historyEntries = [];
				if (this.modelValue.data.createdOn || this.modelValue.data.createdById) {
					historyEntries.push({
						text: this.$t('creation'),
						data: {
							date: this.modelValue.data.createdOn ? new Date(this.modelValue.data.createdOn) : null,
							userId: this.modelValue.data.createdById
						}
					});
				}
				if (this.modelValue.data.updatedOn || this.modelValue.data.updatedById) {

					historyEntries.push(
					{
						text: this.$t('modification'),
						data: {
							date: this.modelValue.data.updatedOn ? new Date(this.modelValue.data.updatedOn) : null,
							userId: this.modelValue.data.updatedById
						}
					});
				}
				return historyEntries;
			}
		}
	}
</script>