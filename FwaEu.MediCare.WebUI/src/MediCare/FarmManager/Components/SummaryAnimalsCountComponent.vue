<template>
	<box :title="$t('animals')" :menu-items="menuButtons">
		<!-- TODO: Display the quantity of different types of animals https://dev.azure.com/fwaeu/MediCare/_workitems/edit/4931-->

		<dx-data-grid :data-source="dataSource">
			<dx-column data-field="animalSpeciesId" :caption="$t('animalSpecies')" :lookup="animalSpeciesOptions" />
			<dx-column data-field="quantity" :caption="$t('quantity')" />
		</dx-data-grid>
	</box>
</template>
<script>
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { DxDataGrid, DxColumn } from 'devextreme-vue/data-grid';
	import { hasPermissionAsync } from '@/Fwamework/Permissions/Services/current-user-permissions-service';
	import { CanSaveFarms } from '@/MediCare/FarmManager/farms-permissions';
	import { FarmAnimalSpeciesDataSourceOptions } from "@/MediCare/FarmManager/Services/farm-animal-species-master-data-service";

	export default {
		components: {
			Box,
			DxDataGrid,
			DxColumn
		},
		mixins: [LocalizationMixin],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/summary-animals-count-messages.${locale}.json`);
				}
			}
		},
		props: {
			dataSource: {
				type: Array,
				required: true
			}
		},
		data() {
			return {
				menuButtons: [],
				animalSpeciesOptions: {
					valueExpr: "id",
					displayExpr: "name",
					dataSource: FarmAnimalSpeciesDataSourceOptions
				}
			}
		},
		methods: {
			async onMessagesLoadedAsync() {
				let $this = this;
				if (await hasPermissionAsync(CanSaveFarms)) {
					this.menuButtons = [{
						text: $this.$t('modifyQuantities'),
						icon: "edit",
						action() {
							$this.$router.push({ name: 'AnimalsQuantities', params: { id: $this.$route.params.id } });
						}
					}]
				}
			}
		}
	}
</script>