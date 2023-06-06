const SearchPatientPageComponent = () => import('@/MediCare/Patients/Components/SearchPatientPageComponent.vue');
const PatientPageComponent = () => import('@/MediCare/Patients/Components/PatientPageComponent.vue');
const TreatmentPageComponent = () => import('@/MediCare/Patients/Components/TreatmentPageComponent.vue');
const PatientOrdersPageComponent = () => import('@/MediCare/Patients/Components/PatientOrdersPageComponent.vue');
const StockConsumptionPageComponent = () => import('@/MediCare/Patients/Components/StockConsumptionPageComponent.vue');
const OrderOtherProductPageComponent = () => import('@/MediCare/Patients/Components/OrderOtherProductPageComponent.vue');


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
				parentName: 'Patient'
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
];