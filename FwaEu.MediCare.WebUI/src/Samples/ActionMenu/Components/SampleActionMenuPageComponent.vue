<template>
	<page-container type="form">
		<box>
			Test multiple menu
			<action-menu :items="menus" menu-display-direction="right" />
			Test single menu
			<action-menu :items="singleItem" />
			Test single menu (force menu mode off)
			<action-menu :items="singleItem" :force-menu-mode="false" menu-display-direction="right" />
			Test multiple button actions (force menu mode off)
			<action-menu :items="buttonActions" :force-menu-mode="false" menu-display-direction="right" />
		</box>
	</page-container>
</template>

<script>
	import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
	import ActionMenu from "@/Fwamework/ActionMenu/Components/ActionMenuComponent.vue";
	import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
	import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";

	export default {
		components: {
			Box,
			ActionMenu,
			PageContainer
		},
		data() {
			const
				menus = [{
					icon: 'coffee',
					text: 'Cafe',
					action: 'https://google.com'
				},
				{
					icon: 'globe',
					text: 'Map',
					action: () => { alert("Test action comme fonction") },
					color: '#fc0303'
				},
				{
					icon: 'airplane',
					text: 'Plane',
					action: '/Notification'
				}
				];
			return {
				menus,
				singleItem: [menus[0]],
				buttonActions: [menus[0], menus[1]],
			};
		},
		created: showLoadingPanel(async function () {

		}),
	}
</script>
