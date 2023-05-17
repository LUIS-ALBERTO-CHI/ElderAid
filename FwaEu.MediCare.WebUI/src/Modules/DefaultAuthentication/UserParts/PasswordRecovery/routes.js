import { defineAsyncComponent } from 'vue';
const PasswordRecoveryPageComponent = () => import('./Components/PasswordRecoveryPageComponent.vue');
const AnonymousPageLayoutComponent = defineAsyncComponent(() => import('@/Fwamework/Authentication/Components/DefaultAnonymousPageLayoutComponent.vue'));

export default [
	{
		path: '/PasswordRecovery',
		name: 'PasswordRecovery',
		component: PasswordRecoveryPageComponent,
		meta: {
			allowAnonymous: true,
			breadcrumb: {
				titleKey: 'passwordRecoveryMenuItemText',
				parentName: 'default'
			},
			layout: AnonymousPageLayoutComponent
		},

		props: (route) => ({ userId: Number(route.query.userId), guid: route.query.guid })
	}
	
]