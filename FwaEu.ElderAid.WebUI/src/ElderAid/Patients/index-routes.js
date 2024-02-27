const SearchPatientPageComponent = () =>
	import("@/ElderAid/Patients/Components/SearchPatientPageComponent.vue");
const PatientPageComponent = () =>
	import("@/ElderAid/Patients/Components/PatientPageComponent.vue");
const TreatmentPageComponent = () =>
	import("@/ElderAid/Patients/Components/TreatmentPageComponent.vue");
const PatientOrdersPageComponent = () =>
	import("@/ElderAid/Patients/Components/PatientOrdersPageComponent.vue");
const StockConsumptionPageComponent = () =>
	import("@/ElderAid/Patients/Components/StockConsumptionPageComponent.vue");
const PatientMedicationsPageComponent = () =>
	import("@/ElderAid/Patients/Components/PatientMedicationsPageComponent.vue");
const ProtectionPageComponent = () =>
	import("@/ElderAid/Patients/Components/ProtectionPageComponent.vue");
const IncontinenceLevelPageComponent = () =>
	import("@/ElderAid/Patients/Components/IncontinenceLevelPageComponent.vue");

export default [
	{
		path: "/SearchPatient",
		name: "SearchPatient",
		component: SearchPatientPageComponent,
		meta: {
			title: "Pacientes",
			breadcrumb: {
				titleKey: "Pacientes",
				parentName: "default",
			},
		},
	},
	{
		path: "/Orders/SearchPatient/",
		name: "SearchPatientFromOrder",
		component: SearchPatientPageComponent,
		meta: {
			title: "Pacientes",
			breadcrumb: {
				titleKey: "Pacientes",
				parentName: "Órdenes",
			},
		},
	},
	{
		path: "/Orders/SearchPatient/:articleId",
		name: "SearchPatientFromOrderWithArticleId",
		component: SearchPatientPageComponent,
		meta: {
			title: "Pacientes",
			breadcrumb: {
				titleKey: "Pacientes",
				parentName: "Orders",
			},
		},
	},
	{
		path: "/Patient/:id",
		name: "Paciente",
		component: PatientPageComponent,
		meta: {
			title: "Pacientes",
			breadcrumb: {
				parentName: "SearchPatient",
				async onNodeResolve(node, context) {
					if (
						typeof context.currentComponent.getCurrentPatientAsync !==
						"function"
					) {
						throw new Error(
							"Children pages of search patient page must implement a getCurrentPatientAsync method"
						);
					}
					const currentPatient =
						await context.currentComponent.getCurrentPatientAsync();
					node.text = currentPatient.fullName;
					node.to = {
						name: "Patient",
						params: { id: currentPatient.id },
					};
					return node;
				},
			},
		},
	},
	{
		path: "/Patient/:id/Medications",
		name: "PatientMedications",
		component: PatientMedicationsPageComponent,
		meta: {
			title: "Medicamentos",
			breadcrumb: {
				titleKey: "Medicamentos",
				parentName: "Patient",
			},
		},
	},

	//NOTE: Move all treatment route to a seperate file when module treatment is created
	{
		path: "/Patient/:id/Medications/Treatment/",
		name: "Treatments",
		component: TreatmentPageComponent,
		meta: {
			title: "Tratamientos",
			breadcrumb: {
				titleKey: "Tratamientos",
				parentName: "Patient",
			},
		},
	},
	{
		path: "/Patient/:id/Medications/Treatment/:treatmentType?",
		name: "TreatmentsReserve",
		component: TreatmentPageComponent,
		meta: {
			title: "Tratamientos de reserva",
			breadcrumb: {
				titleKey: "Tratamientos de reserva",
				parentName: "PatientMedications",
			},
		},
	},
	{
		path: "/Patient/:id/Medications/Treatment/:treatmentType?",
		name: "TreatmentsFixe",
		component: TreatmentPageComponent,
		meta: {
			title: "Tratamientos fijos",
			breadcrumb: {
				titleKey: "Tratamientos fijos",
				parentName: "PatientMedications",
			},
		},
	},
	{
		path: "/Patient/:id/Medications/Treatment/:treatmentType?",
		name: "TreatmentsErased",
		component: TreatmentPageComponent,
		meta: {
			title: "Tratamientos suprimidos",
			breadcrumb: {
				titleKey: "Tratamientos suprimidos",
				parentName: "PatientMedications",
			},
		},
	},
	{
		path: "/Patient/:id/PatientOrders/",
		name: "PatientOrders",
		component: PatientOrdersPageComponent,
		meta: {
			title: "Controles de pacientes",
			breadcrumb: {
				titleKey: "Controles",
				parentName: "Paciente",
			},
		},
	},
	{
		path: "/Patient/:id/StockConsumption",
		name: "StockConsumption",
		component: StockConsumptionPageComponent,
		meta: {
			title: "Stock de consommation",
			breadcrumb: {
				titleKey: "Stock de consommation",
				parentName: "Patient",
			},
		},
	},
	{
		path: "/Patient/:id/Protection",
		name: "Protection",
		component: ProtectionPageComponent,
		meta: {
			title: "Protections",
			breadcrumb: {
				titleKey: "Protections",
				parentName: "Patient",
			},
		},
	},
	{
		path: "/Patient/:id/IncontinenceLevel",
		name: "IncontinenceLevel",
		component: IncontinenceLevelPageComponent,
		meta: {
			title: "Niveau d'incontinence",
			breadcrumb: {
				titleKey: "Niveau d'incontinence",
				parentName: "Protection",
			},
		},
	},
];
