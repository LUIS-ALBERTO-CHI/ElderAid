import TestPage from "@/MediCare/Test/Components/TestComponent.vue";

export default [
	{
		path: '/Test',
		name: 'Test',
		component: TestPage,
		meta: {
			breadcrumb: {
				titleKey: 'testpage',
				parentName: 'default'
			},
		}
	},
]