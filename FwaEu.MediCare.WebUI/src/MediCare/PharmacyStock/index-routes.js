const CabinetsListPageComponent = () => import('@/MediCare/PharmacyStock/Components/CabinetsListPageComponent.vue');
const ArticlesInStockPageComponent = () => import('@/MediCare/PharmacyStock/Components/ArticlesInStockPageComponent.vue');
const ArticleOutboundPageComponent = () => import('@/MediCare/PharmacyStock/Components/ArticleOutboundPageComponent.vue');

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
		path: '/Cabinet/:id/Articles/:articleId',
		name: 'Articles',
		component: ArticleOutboundPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Articles',
				parentName: 'Cabinet'
			},
		}
	},
];