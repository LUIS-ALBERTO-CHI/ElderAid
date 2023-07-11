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
		path: '/Patient/:id',
		name: 'Patient',
		component: PatientPageComponent,
		meta: {
			breadcrumb: {
				parentName: 'SearchPatient',
				async onNodeResolve(node, context) {
                    if (typeof context.currentComponent.getCurrentPatientAsync !== "function") {
						throw new Error("Children pages of search patient page must implement a getCurrentPatientAsync method");
					}
                    const currentPatient = await context.currentComponent.getCurrentPatientAsync();
					node.text = currentPatient.fullName;
					node.to = {
						name: 'Patient',
						params: { id: currentPatient.id }
					};
					return node;
                }
			},
		}
	},
	{
		path: '/Patient/:id/Medications',
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
		path: '/Patient/:id/Medications/Treatment',
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
		path: '/Patient/:id/PatientOrders',
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
		path: '/Patient/:id/StockConsumption',
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
		path: '/Patient/:id/PeriodicOrders',
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