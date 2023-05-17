const context = import.meta.glob('/**/routes.js', { eager: true });
const Home = () => import('@/MediCare/Components/HomePageComponent.vue');

let globalRoutes = [
    {
        name: 'default',
        path: "/",
        component: Home,
        meta: {
            breadcrumb: {
                titleKey: 'homeTitle'
            }
        }
    },
    {
		path: '/:pathMatch(.*)',
        redirect: "/"
    }
];

Object.keys(context).forEach(function (path) {
	let exportedModule = context[path];
	let exportedRoutes = exportedModule.default;
    if (exportedRoutes) {
        globalRoutes = globalRoutes.concat(exportedRoutes);
    }
});

export default globalRoutes;