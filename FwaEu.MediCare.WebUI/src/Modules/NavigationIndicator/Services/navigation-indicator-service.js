//NOTE: Copied from https://github.com/chrisvfritz/vue-enterprise-boilerplate/blob/master/src/router/index.js

import Router from '@/Fwamework/Routing/Services/vue-router-service';
import NProgress from 'nprogress/nprogress';
window.NProgress = NProgress;
export default {
	configure() {

		NProgress.configure({ trickleSpeed: 70 });
		Router.beforeEach((routeTo, routeFrom, next) => {
			// If this isn't an initial page load...
			start();
			next();
		});

		// When each route is finished evaluating...
		Router.afterEach((routeTo, routeFrom) => {
			// NOTE: We will not lunch NProgress if the fullPath of routeFrom and routeTo are equals
			if (routeTo.fullPath != routeFrom.fullPath) {
				NProgress.inc(0.25);//Use directly NProgress in order because we don't want to hide yet the load panel and we want to increase the progress bar value
				done();
				// Complete the animation of the route progress bar.
			}
		});
	}
}
let startedLoaders = 0;
export function start() {
	NProgress.start();
	startedLoaders++;
}

export function done() {
	startedLoaders--;
	if (startedLoaders === 0) {
		setTimeout(() => {
			if (startedLoaders === 0) {
				NProgress.done();
			}
		}, 200);
	}
}