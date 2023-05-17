<template>
	<div>
		<dx-tooltip ref="toolTipAction"
					v-model:target="target"
					width="350px"
					class="user-tooltip-container"
					@hiding="onTooltipHiding"
					@showing="onTooltipShowing">
			<div>
				<div v-if="userData" class="user-tooltip"
					 @mouseenter="keepTooltipOpen"
					 @mouseleave="hideTooltip">
					<div class="user-tooltip-avatar">
						<user-avatar :user="userData" size="large"></user-avatar>
					</div>
					<div class="user-tooltip-date" v-if="date">
						<i class="dx-icon dx-icon-clock" />
						{{$d(date, 'numericLong')}}
					</div>
					<div class="user-tooltip-user-details">
						<strong class="full-name">
							{{userData.fullName}}
						</strong>
						<!--TODO: Implementer l'affichage des données confidentiels https://dev.azure.com/fwaeu/TemplateCore/_workitems/edit/4010-->
						<!--<a href="mailto:todo@fwa.eu">todo@fwa.eu</a>-->
					</div>
				</div>
				<user-loader-tooltip v-else />
			</div>
		</dx-tooltip>
	</div>
</template>

<script>
	import { DxTooltip } from 'devextreme-vue/tooltip';
	import UserAvatar from '@/Fwamework/Users/Components/UserAvatarComponent.vue';
	import UsersMasterDataService from "@/Modules/UserMasterData/Services/users-master-data-service";
	import UserLoaderTooltip from '@/Fwamework/Users/Components/UserLoaderTooltip.vue';
	import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';

	export default {
		components: {
			DxTooltip,
			UserAvatar,
			UserLoaderTooltip
		},
		props: {
			user: Object,
			userId: Number,
			date: Date
		},
		data() {
			return {
				userData: this.user,
				userLazy: new AsyncLazy(() => UsersMasterDataService.getAsync(this.userId)),
				keepOpen: false,
				target: null
			}
		},

		methods: {
			keepTooltipOpen() {
				this.keepOpen = true;
			},
			hideTooltip() {
				this.keepOpen = false;
				this.$refs.toolTipAction.instance.hide();
			},
			onTooltipHiding(e) {
				if (this.keepOpen) {
					e.cancel = true;
				}
			},
			async onTooltipShowing() {
				if (!this.userData) {
					this.userData = await this.userLazy.getValueAsync();
				}
			}
		}
	}

</script>
<style type="text/css" src="./Content/user-tooltip.css"></style>