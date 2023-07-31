const OrdersPageComponent = () => import('@/MediCare/Orders/Components/OrdersPageComponent.vue');
const PeriodicOrdersPageComponent = () => import('@/MediCare/Orders/Components/PeriodicOrdersPageComponent.vue');


export default [
	{
		path: '/Orders',
		name: 'Orders',
		component: OrdersPageComponent,
		meta: {
			title: 'Commandes unitaires',
			breadcrumb: {
				titleKey: 'Commandes unitaires',
				parentName: 'default'
			},
		}
	},
	{
		path: '/PeriodicOrders',
		name: 'PeriodicOrders',
		component: PeriodicOrdersPageComponent,
		meta: {
			title: 'Commandes périodiques',
			breadcrumb: {
				titleKey: 'Commandes périodiques',
				parentName: 'default'
			},
		}
	},

];