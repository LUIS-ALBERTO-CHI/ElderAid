const context = import.meta.glob('/**/routes.js', { eager: true });

let globalRoutes = [
    {
        name: 'default',
        path: "/",
        redirect: "/Administration",
        meta: {
            zoneName: 'admin',
            breadcrumb: {
                titleKey: 'homeTitle'
            },
        },
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