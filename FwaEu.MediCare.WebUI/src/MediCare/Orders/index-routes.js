const OrdersPageComponent = () => import('@/MediCare/Orders/Components/OrdersPageComponent.vue');
const PeriodicOrdersPageComponent = () => import('@/MediCare/Orders/Components/PeriodicOrdersPageComponent.vue');


export default [
	{
		path: '/Orders',
		name: 'Orders',
		component: OrdersPageComponent,
		meta: {
			title: 'Commandes',
			breadcrumb: {
				titleKey: 'Commandes periodiqués',
				parentName: 'default'
			},
		}
	},
	{
		path: '/periodicCommandes',
		name: 'periodicCommandes',
		component: PeriodicOrdersPageComponent,
		meta: {
			title: 'Commandes Periodiques',
			breadcrumb: {
				titleKey: 'Commandes périodiques',
				parentName: 'default'
			},
		}
	},

];