const LoginPageComponent = () => import('@/Fwamework/Authentication/Components/LoginPageComponent.vue');
const Home = () => import('@/MediCare/Components/HomePageComponent.vue');


export default [
	{
		path: '/Login',
		name: 'Login',
		component: LoginPageComponent,
		meta: {
			zoneName: 'admin',
			allowAnonymous: true
		}
	},
	{
		path: '/SearchPatient',
		name: 'SearchPatient',
		component: Home,
	},
]