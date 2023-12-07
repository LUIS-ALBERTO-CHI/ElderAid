import Router from '@/Fwamework/Routing/Services/vue-router-service';

export default {
	async configureAsync() {
        Router.afterEach((route) => {
            if (route.meta.zoneName == "admin")
                document.title = `Administration - ${route.name}`
            else
                document.title = route.meta.title;
        });
	}
}