const LoginFrontPageComponent = () => import('@/MediCare/Components/LoginFrontPageComponent.vue');
const LoginPageComponent = () => import('@/Fwamework/Authentication/Components/LoginPageComponent.vue');


export default [
	{
		path: '/Login',
		name: 'Login',
		component: LoginPageComponent,
		meta: {
			zoneName: 'admin',
			allowAnonymous: true
		}
	}
]