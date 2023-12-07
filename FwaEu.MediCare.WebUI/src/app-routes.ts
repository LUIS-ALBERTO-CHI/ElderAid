const context = import.meta.glob('/**/index-routes.js', { eager: true });
const contextTs = import.meta.glob('/**/index-routes.ts', { eager: true });
import { defineAsyncComponent } from 'vue';
const Home = () => import('@/MediCare/Components/HomePageComponent.vue');
const PublicLayoutComponent = defineAsyncComponent(() => import('@/MediCare/Components/Layouts/PublicApplicationLayoutComponent.vue'));
const OrganizationSelection = () => import('@/MediCare/Components/OrganizationSelectionPage.vue');

let globalRoutes = [
	{
		name: 'default',
		path: "/",
		component: Home,
		meta: {
			title: 'Accueil',
			allowAnonymous: false,
			breadcrumb: {
				titleKey: 'Accueil'
			},
			layout: PublicLayoutComponent
		},
	},
	{
		path: '/:pathMatch(.*)',
		redirect: "/"
	},
	{
		name: 'OrganizationSelection',
		path: "/OrganizationSelection",
		component: OrganizationSelection,
		meta: {
			title: 'Selection de l\'organisation',
			allowAnonymous: false,
			breadcrumb: {
				titleKey: 'Selection de l\'organisation',
				parentName: 'default'
			},
			layout: PublicLayoutComponent
		}
	},
];
AddRoutes(context);
AddRoutes(contextTs);

function AddRoutes(context: any){
    Object.keys(context).forEach(function (path) {
        let exportedModule = context[path];
        let exportedRoutes = exportedModule.default;
        if (exportedRoutes) {
            globalRoutes = globalRoutes.concat(exportedRoutes);
        }
    });
}


export default globalRoutes;