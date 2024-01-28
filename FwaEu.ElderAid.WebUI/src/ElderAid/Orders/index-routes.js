const OrdersPageComponent = () => import('@/ElderAid/Orders/Components/OrdersPageComponent.vue');
const PeriodicOrdersPageComponent = () => import('@/ElderAid/Orders/Components/PeriodicOrdersPageComponent.vue');
const PeriodicOrderPageComponent = () => import('@/ElderAid/Orders/Components/PeriodicOrderPageComponent.vue');

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
	{
		path: '/Patient/:id/PeriodicOrder',
		name: 'PeriodicOrder',
		component: PeriodicOrderPageComponent,
		meta: {
			title: 'Commandes périodiques à valider',
			breadcrumb: {
				titleKey: 'Commandes périodiques à valider',
				parentName: 'Patient'
			},
		}
	}
];