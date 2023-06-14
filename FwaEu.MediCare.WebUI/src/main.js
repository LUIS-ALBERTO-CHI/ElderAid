import "@/Modules/PureCSS/purecss";
import "@/Modules/FontAwesome/font-awesome-module";
import '@/Fwamework/DevExtreme/Themes/generated/theme.base.css';

// import Vue from 'vue';
import IndexApp from './IndexApp.vue';
import { createApp } from 'vue';

import { SentryModule } from "@/Modules/Sentry/sentry-module";

import { ServerMonitoringModule } from '@/Modules/ServerMonitoring/server-monitoring-module';
import { RoutingModule } from '@/Fwamework/Routing/routing-module';
import { CoreModule } from '@/Fwamework/Core/core-module';
import ServerApplicationInfoProvider from "@/Modules/ServerMonitoring/Services/server-application-info-provider";
import { CultureModule } from '@/Fwamework/Culture/culture-module';
import { AuthenticationModule } from '@/Fwamework/Authentication/authentication-module';

import { LoadingPanelModule } from '@/Fwamework/LoadingPanel/loading-panel-module';
import { DevextremeModule } from '@/Fwamework/DevExtreme/devextreme-module';

import { UsersModule } from "@/Fwamework/Users/users-module";
import { ErrorModule } from '@/Fwamework/Errors/error-module';
import { GenericAdminModule } from '@/Modules/GenericAdmin/generic-admin-module';
import { DataImportModule } from "@/Modules/DataImport/data-import-module";
import { PermissionModule } from "@/Fwamework/Permissions/permissions-module";
import { DefaultAuthenticationModule } from '@/Modules/DefaultAuthentication/default-authentication-module';
import { MasterDataModule } from "./Fwamework/MasterData/master-data-module";

import { DotNetTypeConversionModule } from "@/Fwamework/DotNetTypeConversion/dot-net-type-conversion-module";
import { UserCultureModule } from "@/Modules/UserCulture/user-culture-module";
import { ApplicationUsersModule } from "@/MediCare/Users/users-module";

import Application from "@/Fwamework/Core/Services/application";
import InMemoryStore from "@/Fwamework/Storage/Services/in-memory-store";
import IndexedDbMasterDataStore from "@/Modules/IndexedDbMasterDataStore/Services/indexed-db-master-data-store"
import { PermissionsByIsAdminModule } from "@/MediCare/PermissionsByIsAdmin/permissions-by-is-admin-module";

import { UserSettingsModule } from "@/Fwamework/UserSettings/user-settings-module";
import { UsersMasterDataModule } from "@/Modules/UserMasterData/users-master-data-module";
import { UserHistoryPartModule } from "@/Modules/UserHistory/user-history-part-module";
import { UserAmdinStatePartModule } from "@/Modules/UserAdminState/user-admin-state-part-module";
import DefaultAuthenticationHandler from "@/Modules/DefaultAuthentication/Services/default-authentication-handler";


import AppRoutes from './app-routes';
import { UtilsModule } from '@/Fwamework/Utils/utils-module';
import { ImpersonateAuthenticationModule } from "./Modules/ImpersonateAuthentication/impersonate-authentication-module";
import SetupImpersonateAuthenticationHandler from "./Modules/ImpersonateAuthentication/Setup/setup-impersonate-authentication-handler";

import { ApplicationModule } from "@/MediCare/application-module";

import { ReferencialsModule } from "@/MediCare/Referencials/referencials-module";
import { PatientsModule } from "@/MediCare/Patients/patients-module";
import { OrdersModule } from "@/MediCare/Orders/orders-module";



import PrimeVue from 'primevue/config';

const application = new Application(IndexApp)
.useModule(new CoreModule({
	//NOTE: We currently use the same version as server because managing the version for both server and client will require unnecessary efforts			
		applicationInfoProvider: ServerApplicationInfoProvider
	}))
	.useModule(new SentryModule())
	.useModule(new ServerMonitoringModule())
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
	.useModule(new ErrorModule())
	.useModule(new LoadingPanelModule())
	.useModule(new MasterDataModule({
		//NOTE: You can use another store like IndexedDbMasterDataStore or create your own store implementation
		defaultStore: new IndexedDbMasterDataStore()
	}))

	
	.useModule(new DevextremeModule())

	.useModule(new DotNetTypeConversionModule())
	.useModule(new PermissionModule())
	.useModule(new GenericAdminModule())
	.useModule(new DataImportModule())
	.useModule(new ApplicationModule())
	.useModule(new UserCultureModule())
	.useModule(new UsersMasterDataModule())
	.useModule(new UserHistoryPartModule())
	.useModule(new UserAmdinStatePartModule())
	.useModule(new UserSettingsModule())
	.useModule(new ApplicationUsersModule())

	.useModule(new ReferencialsModule())
	.useModule(new PatientsModule())
	.useModule(new OrdersModule())

	
	.useModule(new PermissionsByIsAdminModule())
	.useModule(new RoutingModule({
			routerOptions: {
				routes: AppRoutes
			}
		}
	))
	.useModule(new UtilsModule());

application.vueApp.use(PrimeVue);
application.mountAsync("#app");