const context = import.meta.glob('/**/index-routes.js', { eager: true });
import { defineAsyncComponent } from 'vue';
const Home = () => import('@/MediCare/Components/HomePageComponent.vue');
const PublicLayoutComponent = defineAsyncComponent(() => import('@/MediCare/Components/Layouts/PublicApplicationLayoutComponent.vue'));

let globalRoutes = [
    {
        name: 'default',
        path: "/",
        component: Home,
        meta: {
            allowAnonymous: false,
            breadcrumb: {
                titleKey: 'Accueil'
            },
            layout: PublicLayoutComponent
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