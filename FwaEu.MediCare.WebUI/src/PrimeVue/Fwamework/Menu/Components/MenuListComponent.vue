<template>
	<div class="mb-3 menu-list-search-input-container">
		<span class="p-input-icon-left">
			<i class="pi pi-search" />
			<InputText :placeholder="$t('search')" type="text" v-model="value"/>
		</span>
	</div>
	<Menu :model="primeMenuItems" class="custom-menu-list">
		<template #item="slotProps">
			<div v-if="slotProps.item.isGroup" class="col-fixed menu-list-item-group">
				{{ slotProps.item.label }}
			</div>
			<a v-if="!slotProps.item.isGroup" class="p-menuitem-link menu-list-item-link" tabindex="-1" aria-hidden="true">
				<div class="menu-list-item-title">
					<span class="p-menuitem-icon p-mr-1" :class="slotProps.item.icon"></span>
					<span class="menu-list-item-label">{{ slotProps.item.label }}</span>
				</div>
				<span>{{ slotProps.item.description }}</span>
			</a>
		</template>
	</Menu>
</template>
<script>
	import Menu from 'primevue/menu';
	import InputText from 'primevue/inputtext';
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";

	export default {
		components: {
			Menu,
			InputText
		},
		data() {
			return {
				value: null
			}
		},
		props: {
			menuItems: {
				type: Array,
				required: true
			}
		},
		async created() {
			await loadMessagesAsync(this, import.meta.glob('./Content/menu-list-messages.*.json'));
		},
		computed: {
			primeMenuItems() {
				let menu = this.menuItems.map((menuItem) => {
					if (menuItem.items) {
						const subItems = menuItem.items.map(
							(item) =>{ 
								return {
									label: item.text,
									icon: item.icon,
									description: item.descriptionText,
									command: () => this.$router.push(item.path)
								}
							}
						);

						return {
							label: menuItem.text,
							items: subItems,
							isGroup: true
						};

					} else {
						return {
							label: menuItem.text,
							icon: menuItem.icon
						};
					}
				});

				function filterDataByLabel(data, labelToFilter) {
					if (!data) return [];

					const filteredData = [];
					for (const item of data) {
						if (item.separator) 
							filteredData.push(item);
						else {
							const { label, items } = item;
							const filteredItems = filterDataByLabel(items, labelToFilter);
							if (label.getNormalizedText().toLowerCase().includes(labelToFilter.getNormalizedText().toLowerCase()) || filteredItems.length > 0) {
								const filteredItem = { ...item };
								filteredItem.items = filteredItems;
								filteredData.push(filteredItem);
							}							
						}

					}
					return filteredData;
				};

				if (this.value) {
					menu = filterDataByLabel(menu, this.value);
				}

				return menu;
			},
		},

	}
</script>

<style lang="scss">
	.custom-menu-list {
		width: 100%;
	}
	.menu-list-search-input-container {
		text-align:right
	}
	.menu-list-item-group {
		padding-bottom: 0px; color: var(--secondary-text-color)
	}
	.menu-list-item-link {
		height: 10px;margin-bottom: 10px; margin-top: 10px;
	}
	.menu-list-item-title {
		width: 250px; margin-left: 50px; 
	}
	.menu-list-item-label {
		color: var(--secondary-text-color); 
	}
</style>