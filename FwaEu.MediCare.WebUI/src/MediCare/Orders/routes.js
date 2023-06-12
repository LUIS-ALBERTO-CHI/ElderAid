const OrdersPageComponent = () => import('@/MediCare/Orders/Components/OrdersPageComponent.vue');



export default [
	{
		path: '/Orders',
		name: 'Orders',
		component: OrdersPageComponent,
		meta: {
			breadcrumb: {
				titleKey: 'Commandes',
				parentName: 'default'
			},
		}
	},
];