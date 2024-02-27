const SearchPatientPageComponent = () => import('@/ElderAid/Patients/Components/SearchPatientPageComponent.vue');
const PatientPageComponent = () => import('@/ElderAid/Patients/Components/PatientPageComponent.vue');
const TreatmentPageComponent = () => import('@/ElderAid/Patients/Components/TreatmentPageComponent.vue');
const PatientOrdersPageComponent = () => import('@/ElderAid/Patients/Components/PatientOrdersPageComponent.vue');
const StockConsumptionPageComponent = () => import('@/ElderAid/Patients/Components/StockConsumptionPageComponent.vue');
const PatientMedicationsPageComponent = () => import('@/ElderAid/Patients/Components/PatientMedicationsPageComponent.vue');
const ProtectionPageComponent = () => import('@/ElderAid/Patients/Components/ProtectionPageComponent.vue');
const IncontinenceLevelPageComponent = () => import('@/ElderAid/Patients/Components/IncontinenceLevelPageComponent.vue');

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
		path: '/Orders/SearchPatient/',
		name: 'SearchPatientFromOrder',
		component: SearchPatientPageComponent,
		meta: {
			title: 'Patients',
			breadcrumb: {
				titleKey: 'Patients',
				parentName: 'Orders'
			},
		},
	},
	{
		path: '/Orders/SearchPatient/:articleId',
		name: 'SearchPatientFromOrderWithArticleId',
		component: SearchPatientPageComponent,
		meta: {
			title: 'Patients',
			breadcrumb: {
				titleKey: 'Patients',
				parentName: 'Orders'
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
	}
];