const LoginPageComponent = () => import('@/MediCare/Components/LoginFrontPageComponent.vue');

export default [
	{
		path: '/Login',
		name: 'Login',
		component: LoginPageComponent,
		meta: {
			zoneName: 'admin',
			allowAnonymous: true,
		}
	},
]