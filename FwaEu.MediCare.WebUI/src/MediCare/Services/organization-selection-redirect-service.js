import Router from '@/Fwamework/Routing/Services/vue-router-service';
import ViewContextService from '@/MediCare/ViewContext/Services/view-context-service';



export default {
	async configureAsync() {
        Router.afterEach((route) => {
            const organization = ViewContextService.get();
            if (!(route.meta.zoneName == "admin") && organization == null && route.name != "OrganizationSelection") {
                Router.push({ name: 'OrganizationSelection' });
            }
        });
	}
}