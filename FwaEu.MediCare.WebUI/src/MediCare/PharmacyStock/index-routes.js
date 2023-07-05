const CabinetsListPageComponent = () => import('@/MediCare/PharmacyStock/Components/CabinetsListPageComponent.vue');
const ArticlesInStockPageComponent = () => import('@/MediCare/PharmacyStock/Components/ArticlesInStockPageComponent.vue');
const ArticlesDetailsPageComponent = () => import('@/MediCare/PharmacyStock/Components/ArticlesDetailsPageComponent.vue');

export default [
	{
		path: '/stockPharmacy',
		name: 'stockPharmacy',
		component: CabinetsListPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'stockPharmacy',
				parentName: 'default'
			},
		}
	},
	{
		path: '/Cabinet/:id',
		name: 'Cabinet',
		component: ArticlesInStockPageComponent,
		meta: {
			breadcrumb: {
				parentName: "stockPharmacy",
				async onNodeResolve(node, context) {
					if (
						typeof context.currentComponent.getCurrentCabinetAsync !==
						"function"
					) {
						throw new Error(
							"Children pages of cabinet details must implement a getCurrentCabinetAsync method"
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
		}
	},
	{
		path: '/Cabinet/:id/Articles',
		name: 'Articles',
		component: ArticlesDetailsPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Articles',
				parentName: 'Cabinet'
			},
		}
	},
];