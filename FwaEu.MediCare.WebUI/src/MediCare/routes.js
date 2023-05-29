const LoginPageComponent = () => import('@/Fwamework/Authentication/Components/LoginPageComponent.vue');
const Home = () => import('@/MediCare/Components/HomePageComponent.vue');
const SearchPatientPageComponent = () => import('@/MediCare/Components/SearchPatientPageComponent.vue');
const PatientPageComponent = () => import('@/MediCare/Components/PatientPageComponent.vue');
const TreatmentPageComponent = () => import('@/MediCare/Components/TreatmentPageComponent.vue');




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
	{
		path: '/SearchPatient',
		name: 'SearchPatient',
		component: SearchPatientPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Recherche de patient',
				parentName: 'default'
			},
		}
	},
	{
		path: '/Patient',
		name: 'Patient',
		component: PatientPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Patient',
				parentName: 'SearchPatient'
			},
		}
	},
	{
		path: '/Treatment',
		name: 'Treatment',
		component: TreatmentPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Traitement',
				parentName: 'Patient'
			},
		}
	},
]