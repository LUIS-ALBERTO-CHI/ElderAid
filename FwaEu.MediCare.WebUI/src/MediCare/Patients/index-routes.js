const SearchPatientPageComponent = () => import('@/MediCare/Patients/Components/SearchPatientPageComponent.vue');
const PatientPageComponent = () => import('@/MediCare/Patients/Components/PatientPageComponent.vue');
const TreatmentPageComponent = () => import('@/MediCare/Patients/Components/TreatmentPageComponent.vue');
const PatientOrdersPageComponent = () => import('@/MediCare/Patients/Components/PatientOrdersPageComponent.vue');
const StockConsumptionPageComponent = () => import('@/MediCare/Patients/Components/StockConsumptionPageComponent.vue');
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
			title: 'Patients',
			breadcrumb: {
				titleKey: 'Patients',
				parentName: 'default'
			},
		},
	},
	{
		path: '/Patient/:id',
		name: 'Patient',
		component: PatientPageComponent,
		meta: {
			title: 'Patient',
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
			title: 'Médicaments',
			breadcrumb: {
				titleKey: 'Médicaments',
				parentName: 'Patient'
			},
		}
	},

	//NOTE: Move all treatment route to a seperate file when module treatment is created
	{
		path: '/Patient/:id/Medications/Treatment/',
		name: 'Treatments',
		component: TreatmentPageComponent,
		meta: {
			title: 'Traitements',
			breadcrumb: {
				titleKey: 'Traitements',
				parentName: 'Patient'
			},
		}
	},
	{
		path: '/Patient/:id/Medications/Treatment/:treatmentType?',
		name: 'TreatmentsReserve',
		component: TreatmentPageComponent,
		meta: {
			title: 'Traitements de réserve',
			breadcrumb: {
				titleKey: 'Traitements de réserve',
				parentName: 'PatientMedications'
			},
		}
	},
	{
		path: '/Patient/:id/Medications/Treatment/:treatmentType?',
		name: 'TreatmentsFixe',
		component: TreatmentPageComponent,
		meta: {
			title: 'Traitements fixes',
			breadcrumb: {
				titleKey: 'Traitements fixes',
				parentName: 'PatientMedications'
			},
		}
	},
	{
		path: '/Patient/:id/Medications/Treatment/:treatmentType?',
		name: 'TreatmentsErased',
		component: TreatmentPageComponent,
		meta: {
			title: 'Traitements effacés',
			breadcrumb: {
				titleKey: 'Traitements effacés',
				parentName: 'PatientMedications'
			},
		}
	},
	{
		path: '/Patient/:id/PatientOrders/',
		name: 'PatientOrders',
		component: PatientOrdersPageComponent,
		meta: {
			title: 'Commandes du patient',
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
			title: 'Stock de consommation',
			breadcrumb: {
				titleKey: 'Stock de consommation',
				parentName: 'Patient'
			},
		}
	},
	{
		path: '/Patient/:id/OrderArticle/:articleId',
		name: 'OrderArticle',
		component: OrderArticlePageComponent,
		meta: {
			title: 'Commander un article',
			breadcrumb: {
				titleKey: 'Commander un article',
				parentName: 'SearchArticle'
			},
		}
	},
	{
		path: '/Patient/:id/Protection',
		name: 'Protection',
		component: ProtectionPageComponent,
		meta: {
			title: 'Protections',
			breadcrumb: {
				titleKey: 'Protections',
				parentName: 'Patient'
			},
		}
	},
	{
		path: '/Patient/:id/IncontinenceLevel',
		name: 'IncontinenceLevel',
		component: IncontinenceLevelPageComponent,
		meta: {
			title: 'Niveau d\'incontinence',
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
			title: 'Commandes périodiques à valider',
			breadcrumb: {
				titleKey: 'Commandes périodiques à valider',
				parentName: 'Patient'
			},
		}
	}
];