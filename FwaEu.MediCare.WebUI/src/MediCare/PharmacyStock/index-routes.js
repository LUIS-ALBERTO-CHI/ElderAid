const CabinetsListPageComponent = () => import('@/MediCare/PharmacyStock/Components/CabinetsListPageComponent.vue');
const ArticlesInStockPageComponent = () => import('@/MediCare/PharmacyStock/Components/ArticlesInStockPageComponent.vue');

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
];