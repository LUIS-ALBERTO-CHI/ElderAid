const SearchPatientPageComponent = () => import('@/MediCare/Patients/Components/SearchPatientPageComponent.vue');
const PatientPageComponent = () => import('@/MediCare/Patients/Components/PatientPageComponent.vue');
const TreatmentPageComponent = () => import('@/MediCare/Patients/Components/TreatmentPageComponent.vue');
const PatientOrdersPageComponent = () => import('@/MediCare/Patients/Components/PatientOrdersPageComponent.vue');
const StockConsumptionPageComponent = () => import('@/MediCare/Patients/Components/StockConsumptionPageComponent.vue');
const OrderOtherProductPageComponent = () => import('@/MediCare/Patients/Components/OrderOtherProductPageComponent.vue');
const OrderArticlePageComponent = () => import('@/MediCare/Patients/Components/OrderArticlePageComponent.vue');
const PatientMedicationsPageComponent = () => import('@/MediCare/Patients/Components/PatientMedicationsPageComponent.vue');
const ProtectionPageComponent = () => import('@/MediCare/Patients/Components/ProtectionPageComponent.vue');
const IncontinenceLevelPageComponent = () => import('@/MediCare/Patients/Components/IncontinenceLevelPageComponent.vue');
const PeriodicOrdersPageComponent = () => import('@/MediCare/Patients/Components/PeriodicOrdersPageComponent.vue');


export default [
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
				parentName: 'PatientMedications'
			},
		}
	},
	{
		path: '/PatientOrders',
		name: 'PatientOrders',
		component: PatientOrdersPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Commandes',
				parentName: 'Patient'
			},
		}
	},
	{
		path: '/StockConsumption',
		name: 'StockConsumption',
		component: StockConsumptionPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Stock de consommation',
				parentName: 'Patient'
			},
		}
	},
	{
		path: '/OrderOtherProduct',
		name: 'OrderOtherProduct',
		component: OrderOtherProductPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Commander un autre produit',
				parentName: 'Patient'
			},
		}
	},
	{
		path: '/OrderArticle',
		name: 'OrderArticle',
		component: OrderArticlePageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Commander un article',
				parentName: 'OrderOtherProduct'
			},
		}
	},
	{
		path: '/Medications',
		name: 'PatientMedications',
		component: PatientMedicationsPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Médicaments',
				parentName: 'Patient'
			},
		}
	},
	{
		path: '/Protection',
		name: 'Protection',
		component: ProtectionPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Protections',
				parentName: 'Patient'
			},
		}
	},
	{
		path: '/IncontinenceLevel',
		name: 'IncontinenceLevel',
		component: IncontinenceLevelPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Niveau d\'incontinence',
				parentName: 'Protection'
			},
		}
	},
	{
		path: '/PeriodicOrders',
		name: 'PeriodicOrders',
		component: PeriodicOrdersPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Commandes périodiques à valider',
				parentName: 'Patient'
			},
		}
	}
];