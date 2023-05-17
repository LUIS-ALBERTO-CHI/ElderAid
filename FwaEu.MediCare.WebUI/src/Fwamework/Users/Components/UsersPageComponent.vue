<template>
	<page-container type="list" custom-class="user-list">
		<box>
			<dx-data-grid :data-source="users"
						  width="100%"
						  key-expr="id"
						  :columns="columns"
						  @init-new-row="onInitNewRow($event)">
				<dx-search-panel :visible="true" :width="250" />
				<dx-editing :allow-adding="true" />
				<dx-export :enabled="true"
						   file-name="Users" />
				<dx-paging :page-size="35" />
			</dx-data-grid>
		</box>
	</page-container>
</template>
<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import { DxDataGrid, DxPaging, DxExport, DxEditing, DxSearchPanel } from 'devextreme-vue/data-grid';
	import PageContainer from '@/Fwamework/PageContainer/Components/PageContainerComponent.vue';
	import UserService from '@/Fwamework/Users/Services/user-service';
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
	import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import UsersMasterDataService from "@/Modules/UserMasterData/Services/users-master-data-service";
	import UserFormatterService from '@/Fwamework/Users/Services/user-formatter-service';
	import UserDataGridHelperService from "@/Fwamework/Users/Services/user-data-grid-helper-service";
	import UserColumnsCustomizerService from '@/Fwamework/Users/Services/user-columns-customizer-service';

	export default {
		components: {
			Box,
			PageContainer,
			DxDataGrid,
			DxPaging,
			DxEditing,
			DxExport,
			DxSearchPanel
		},
		mixins: [LocalizationMixin],
		i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/user-messages.${locale}.json`);
				}
			}
		},
		data() {
			return {
				users: [],
				columns: null
			};
		},
		created: showLoadingPanel(async function () {
			const users = await UserService.getAllAsync();
			const userIds = users.map(u => u.id);
			const usersMasterData = await UsersMasterDataService.getByIdsAsync(userIds);
			users.forEach(function (user) {
				const masterData = usersMasterData.find(um => um.id == user.id);
				user.fullName = masterData.fullName;
				user.avatarUrl = masterData.avatarUrl;
				user.initials = UserFormatterService.generateInitials(masterData);
			});

			this.users = users;

		}),

		methods: {
			onInitNewRow(e) {
				e.promise = this.$router.push({
					name: 'NewUserDetails'
				});
			},
			onMessagesLoadedAsync() {
				let columns = UserDataGridHelperService.createColumns(this);
				UserColumnsCustomizerService.onColumnsCreated(this, columns);
				this.columns = columns;
			}
		}
	};
</script>
<style type="text/css" src="./Content/users.css"></style>