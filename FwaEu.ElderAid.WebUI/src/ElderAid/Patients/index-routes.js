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
			title: "Patients",
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
			title: "Patients",
			breadcrumb: {
				titleKey: "Pacientes",
				parentName: "Orders",
			},
		},
	},
	{
		path: "/Orders/SearchPatient/:articleId",
		name: "SearchPatientFromOrderWithArticleId",
		component: SearchPatientPageComponent,
		meta: {
			title: "Patients",
			breadcrumb: {
				titleKey: "Pacientes",
				parentName: "Orders",
			},
		},
	},
	{
		path: "/Patient/:id",
		name: "Patient",
		component: PatientPageComponent,
		meta: {
			title: "Patient",
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
			title: "Médicaments",
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
			title: "Traitements",
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
			title: "Traitements fixes",
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
			title: "Traitements effacés",
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
			title: "Commandes du patient",
			breadcrumb: {
				titleKey: "Pedidos",
				parentName: "Patient",
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
				titleKey: "Existencias de consumo",
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
				titleKey: "Protecciones",
				parentName: "Patient",
			},
		},
	},
	{
		path: "/Patient/:id/IncontinenceLevel",
		name: "IncontinenceLevel",
		component: IncontinenceLevelPageComponent,
		meta: {
			title: "Nivel de incontinencia",
			breadcrumb: {
				titleKey: "Nivel de incontinencia",
				parentName: "Protection",
			},
		},
	},
];
