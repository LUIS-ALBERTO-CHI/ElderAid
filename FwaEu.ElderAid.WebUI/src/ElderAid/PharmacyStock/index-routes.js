const CabinetsListPageComponent = () =>
	import("@/ElderAid/PharmacyStock/Components/CabinetsListPageComponent.vue");
const ArticlesInStockPageComponent = () =>
	import(
		"@/ElderAid/PharmacyStock/Components/ArticlesInStockPageComponent.vue"
	);
const ArticleOutboundPageComponent = () =>
	import(
		"@/ElderAid/PharmacyStock/Components/ArticleOutboundPageComponent.vue"
	);

export default [
	{
		path: "/stockPharmacy",
		name: "stockPharmacy",
		component: CabinetsListPageComponent,
		meta: {
			title: "Stock de farmacia",
			breadcrumb: {
				titleKey: "Stock de farmacia",
				parentName: "default",
			},
		},
	},
	{
		path: "/Cabinet/:id",
		name: "Cabinet",
		component: ArticlesInStockPageComponent,
		meta: {
			title: "Gabinete",
			breadcrumb: {
				parentName: "stockPharmacy",
				async onNodeResolve(node, context) {
					if (
						typeof context.currentComponent.getCurrentCabinetAsync !==
						"function"
					) {
						throw new Error(
							"Las páginas secundarias de detalles del gabinete deben implementar el método getCurrentCabinetAsync"
						);
					}
					const cabinet =
						await context.currentComponent.getCurrentCabinetAsync();
					node.text = cabinet.name;
					node.to = {
						name: "Cabinet",
						params: { id: cabinet.id },
					};
					return node;
				},
			},
		},
	},
	{
		path: "/Cabinet/:id/Articles/:articleId/Stock/:stockId",
		name: "Articles",
		component: ArticleOutboundPageComponent,
		meta: {
			title: "Articles",
			breadcrumb: {
				onNodeResolve(node, context) {
					if (context.currentComponent.isPatientSelected) {
						node.text = "Agotado";
					} else {
						node.text = "Pacientes";
					}
					node.parentNode = "Gabinete";
					node.to = "";
					return node;
				},
			},
		},
	},
];
