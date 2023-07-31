import Router from '@/Fwamework/Routing/Services/vue-router-service';

export default {
	async configureAsync() {
        Router.afterEach((route) => {
            if (route.name == "default")
                document.title = route.meta.title;
            else
                document.title = `Administration - ${route.name}`
        });
	}
}