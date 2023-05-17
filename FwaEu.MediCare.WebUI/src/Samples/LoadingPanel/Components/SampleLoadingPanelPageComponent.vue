<template>
	<page-container type="summary">
		<box>
			<div>
				<p>Action 1 result: {{ action1Result}}</p>
				<p>Action 2 result: {{ action2Result}}</p>
				<p>Action 3 result: <input v-model="action3Result" /></p>
			</div>
			<div class="block">
				<loading-panel loader-name="custom-loader">
					<box title="Container with visible content when loading" :menu-items="boxActionMenuItems">
						<div class="sample-click-loading-panel-container">

							<p>This is a box with some specific content to save</p>
						</div>
					</box>
				</loading-panel>
			</div>
		</box>
	</page-container>
</template>
<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';
	import NotificationService from "@/Fwamework/Notifications/Services/notification-service";
	import { defineAsyncComponent } from "vue";
	const LoadingPanel = defineAsyncComponent(() => import('@/Fwamework/LoadingPanel/Components/LoadingPanelComponent.vue'));

	export default {
		components: {
			Box,
			LoadingPanel,
			PageContainer
		},
		data() {
			return {
				action1Result: null,
				action2Result: null,
				action3Result: null,
				boxActionMenuItems: [{ icon: "save", text: "Save", action: this.onSaveClick }]
			};
		},
		created: showLoadingPanel(async function () {//Use function() instead of arrow function in order to keep access to 'this'

			let [result1, result2, result3] = await Promise.all([this.slowActionAsync(), this.slowAction2Async(), this.slowAction3Async()]);

			//Affect all values at the same time in order to prevent multiple DOM refreshes
			this.action1Result = result1;
			this.action2Result = result2;
			this.action3Result = result3;
		}),
		methods: {
			sleepAsync(ms) {
				return new Promise((resolve) => {
					setTimeout(resolve, ms);
				});
			},
			async slowActionAsync() {
				await this.sleepAsync(1000);
				return "Finished action 1";
			},
			async slowAction2Async() {
				await this.sleepAsync(1000);
				return "Finished action 2";
			},
			async slowAction3Async() {
				await this.sleepAsync(1000);
				return "Finished action 3";
			},
			onSaveClick: showLoadingPanel(async function () {
				await this.sleepAsync(5 * 1000);
				NotificationService.showConfirmation("Save success");
			}, 'custom-loader')
		}
	}
</script>
<style>

	.sample-click-loading-panel-container {
		height: 300px;
		width: 600px;
		background-color: skyblue;
		position: relative;
	}

		.sample-click-loading-panel-container p {
			margin: 0;
			position: absolute;
			top: 50%;
			left: 50%;
			transform: translate(-50%, -50%);
		}
</style>