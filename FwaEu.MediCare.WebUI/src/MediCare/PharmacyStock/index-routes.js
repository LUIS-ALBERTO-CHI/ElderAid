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
		path: '/Cabinet',
		name: 'Cabinet',
		component: ArticlesInStockPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Cabinet',
				parentName: 'stockPharmacy'
			},
		}
	},
	{
		path: '/Articles',
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