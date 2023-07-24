import Router from '@/Fwamework/Routing/Services/vue-router-service';

export default {
	async configureAsync() {
        Router.afterEach((route) => {
            document.title = route.meta.title;
        });
	}
}