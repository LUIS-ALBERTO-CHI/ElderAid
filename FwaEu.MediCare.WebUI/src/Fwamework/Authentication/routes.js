import { defineAsyncComponent } from 'vue';
const LoginPageComponent = () => import('@/Fwamework/Authentication/Components/LoginPageComponent.vue');
const AnonymousPageLayoutComponent = defineAsyncComponent(() => import('@/Fwamework/Authentication/Components/DefaultAnonymousPageLayoutComponent.vue'));

export default [
	{
		path: '/Login',
		name: 'Login',
		component: LoginPageComponent,
		meta: {
			allowAnonymous: true,
			zoneName: 'admin',
			breadcrumb: {
				titleKey: 'Login',
				parentName: 'default'
			},
			pageTitleKey: 'loginPageTitle',
			layout: AnonymousPageLayoutComponent
		}
	}
];