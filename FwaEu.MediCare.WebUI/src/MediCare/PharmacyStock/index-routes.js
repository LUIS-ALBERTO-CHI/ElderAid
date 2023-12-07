const CabinetsListPageComponent = () => import('@/MediCare/PharmacyStock/Components/CabinetsListPageComponent.vue');
const ArticlesInStockPageComponent = () => import('@/MediCare/PharmacyStock/Components/ArticlesInStockPageComponent.vue');
const ArticleOutboundPageComponent = () => import('@/MediCare/PharmacyStock/Components/ArticleOutboundPageComponent.vue');

export default [
	{
		path: '/stockPharmacy',
		name: 'stockPharmacy',
		component: CabinetsListPageComponent,
		meta: {
			title: 'Stock pharmarcie',
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
			title: 'Cabinet',
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
		path: '/Cabinet/:id/Articles/:articleId/Stock/:stockId',
		name: 'Articles',
		component: ArticleOutboundPageComponent,
		meta: {
			title: 'Articles',
			breadcrumb: {
				onNodeResolve(node, context) {
					if (context.currentComponent.isPatientSelected) {
						node.text = "Sortie du stock";
					} else {
						node.text = "Patients";
					}
					node.parentNode = 'Cabinet';
					node.to = '';
                    return node;
				}
			},
		}
	},
];