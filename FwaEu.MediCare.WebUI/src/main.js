import "@/Modules/PureCSS/purecss";
import "@/Modules/FontAwesome/font-awesome-module";
import '@/Fwamework/DevExtreme/Themes/generated/theme.base.css';

// import Vue from 'vue';
import IndexApp from './IndexApp.vue';
import { createApp } from 'vue';

import { SentryModule } from "@/Modules/Sentry/sentry-module";

import { RoutingModule } from '@/Fwamework/Routing/routing-module';
import { CoreModule } from '@/Fwamework/Core/core-module';
import ServerApplicationInfoProvider from "@/Modules/ServerMonitoring/Services/server-application-info-provider";
import { CultureModule } from '@/Fwamework/Culture/culture-module';
import { AuthenticationModule } from '@/Fwamework/Authentication/authentication-module';

import { LoadingPanelModule } from '@/Fwamework/LoadingPanel/loading-panel-module';
import { DevextremeModule } from '@/Fwamework/DevExtreme/devextreme-module';

import { UsersModule } from "@/Fwamework/Users/users-module";
import { DefaultAuthenticationModule } from '@/Modules/DefaultAuthentication/default-authentication-module';
import { MasterDataModule } from "./Fwamework/MasterData/master-data-module";

import Application from "@/Fwamework/Core/Services/application";
import InMemoryStore from "@/Fwamework/Storage/Services/in-memory-store";

import DefaultAuthenticationHandler from "@/Modules/DefaultAuthentication/Services/default-authentication-handler";

import AppRoutes from './app-routes';
import { UtilsModule } from '@/Fwamework/Utils/utils-module';
import { ImpersonateAuthenticationModule } from "./Modules/ImpersonateAuthentication/impersonate-authentication-module";
import SetupImpersonateAuthenticationHandler from "./Modules/ImpersonateAuthentication/Setup/setup-impersonate-authentication-handler";

import { ApplicationModule } from "@/MediCare/application-module";

const application = new Application(IndexApp)
.useModule(new VueSelectModule())
.useModule(new CoreModule({
	//NOTE: We currently use the same version as server because managing the version for both server and client will require unnecessary efforts			
		applicationInfoProvider: ServerApplicationInfoProvider
	}))
	.useModule(new SentryModule())
	.useModule(new DefaultAuthenticationModule())
	.useModule(new ImpersonateAuthenticationModule())
	.useModule(new AuthenticationModule({
		//NOTE: You can provide diferent authentication handler, like azure-ad-authentication-handler
		authenticationHandlers: [
			SetupImpersonateAuthenticationHandler,
			DefaultAuthenticationHandler,
			//AzureAdAuthenticationHandler
		]
	}))
	.useModule(new UsersModule())
	.useModule(new CultureModule())
	.useModule(new MasterDataModule({
		//NOTE: You can use another store like IndexedDbMasterDataStore or create your own store implementation
		defaultStore: new InMemoryStore()
	}))

	.useModule(new LoadingPanelModule())
	.useModule(new DevextremeModule())

	.useModule(new ApplicationModule())
	.useModule(new RoutingModule({
			routerOptions: {
				routes: AppRoutes
			}
		}
	))
	.useModule(new UtilsModule());

application.mountAsync("#app");