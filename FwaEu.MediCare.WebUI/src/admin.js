import "@/Modules/PureCSS/purecss";
import "@/Modules/FontAwesome/font-awesome-module";
import '@/Fwamework/DevExtreme/Themes/generated/theme.base.css';

import App from './App.vue';
import { RoutingModule } from '@/Fwamework/Routing/routing-module';
import { CoreModule } from '@/Fwamework/Core/core-module';
import { ServerMonitoringModule } from '@/Modules/ServerMonitoring/server-monitoring-module';
import ServerApplicationInfoProvider from "@/Modules/ServerMonitoring/Services/server-application-info-provider";
import { CultureModule } from '@/Fwamework/Culture/culture-module';
import { AuthenticationModule } from '@/Fwamework/Authentication/authentication-module';
import { ErrorModule } from '@/Fwamework/Errors/error-module';
import { LoadingPanelModule } from '@/Fwamework/LoadingPanel/loading-panel-module';
import { DevextremeModule } from '@/Fwamework/DevExtreme/devextreme-module';
import { UsersModule } from "@/Fwamework/Users/users-module";
import { PermissionModule } from "@/Fwamework/Permissions/permissions-module";
import { GenericAdminModule } from '@/Modules/GenericAdmin/generic-admin-module';
import { DataImportModule } from "@/Modules/DataImport/data-import-module";
import { DefaultAuthenticationModule } from '@/Modules/DefaultAuthentication/default-authentication-module';
import { ApplicationModule } from "@/MediCare/application-module";
import { UserCultureModule } from "@/Modules/UserCulture/user-culture-module";
import { UsersMasterDataModule } from "@/Modules/UserMasterData/users-master-data-module";
import { UserHistoryPartModule } from "@/Modules/UserHistory/user-history-part-module";
import { UserAmdinStatePartModule } from "@/Modules/UserAdminState/user-admin-state-part-module";
import { NavigationIndicatorModule } from '@/Modules/NavigationIndicator/navigation-indicator-module';
import { AdministrationMenuModule } from "@/Fwamework/AdministrationMenu/administration-menu-module";
import { MasterDataModule } from "./Fwamework/MasterData/master-data-module";
import { DotNetTypeConversionModule } from "@/Fwamework/DotNetTypeConversion/dot-net-type-conversion-module";
import { ApplicationUsersModule } from "@/MediCare/Users/users-module";
/*import { ApplicationCultureModule } from "@/MediCare/Culture/culture-module";*/
import { OrganizationsModule } from "@/MediCare/Organizations/organizations-module";

import { UtilsModule } from '@/Fwamework/Utils/utils-module';
import { ImpersonateAuthenticationModule } from "./Modules/ImpersonateAuthentication/impersonate-authentication-module";
import SetupImpersonateAuthenticationHandler from "./Modules/ImpersonateAuthentication/Setup/setup-impersonate-authentication-handler";


import Application from "@/Fwamework/Core/Services/application";
import InMemoryStore from "@/Fwamework/Storage/Services/in-memory-store";
import { PermissionsByIsAdminModule } from "@/MediCare/PermissionsByIsAdmin/permissions-by-is-admin-module";

import { UserSettingsModule } from "@/Fwamework/UserSettings/user-settings-module";
import DefaultAuthenticationHandler from "@/Modules/DefaultAuthentication/Services/default-authentication-handler";

import BreadcrumbsService from "@/Fwamework/Breadcrumbs/Services/breadcrumbs-service";
import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
import AdminRoutes from "./admin-routes";

BreadcrumbsService.onNodeResolved(async (e) => {
	if (e.resolvingRoute?.name === 'default') {
		e.node.to = Configuration.application.publicUrl;
	}
})

const application = new Application(App)
	.useModule(new CoreModule({
		//NOTE: We currently use the same version as server because managing the version for both server and client will require unnecessary efforts
		applicationInfoProvider: ServerApplicationInfoProvider
	}))
	.useModule(new ServerMonitoringModule())
	
	.useModule(new DefaultAuthenticationModule())
	.useModule(new ImpersonateAuthenticationModule())

	.useModule(new AuthenticationModule({
		//NOTE: You can provide a diferent authentication handler, like azure-ad-authentication-handler
		authenticationHandlers:[
			SetupImpersonateAuthenticationHandler,
			DefaultAuthenticationHandler
		] 
	}))
	.useModule(new UsersModule())
	.useModule(new CultureModule())
	.useModule(new ErrorModule())
	.useModule(new LoadingPanelModule())
	.useModule(new DevextremeModule())
	.useModule(new DotNetTypeConversionModule())
	.useModule(new PermissionModule())
	.useModule(new GenericAdminModule())
	.useModule(new DataImportModule())
	.useModule(new OrganizationsModule())
	.useModule(new ApplicationModule())
	.useModule(new UserCultureModule())
	.useModule(new UsersMasterDataModule())
	.useModule(new UserHistoryPartModule())
	.useModule(new UserAmdinStatePartModule())
	.useModule(new UserSettingsModule())
	.useModule(new NavigationIndicatorModule())
	.useModule(new AdministrationMenuModule())

	.useModule(new MasterDataModule({
		//NOTE: You can use another store like IndexedDbMasterDataStore or create your own store implementation
		defaultStore: new InMemoryStore()
	}))
	.useModule(new ApplicationUsersModule())
	/*.useModule(new PermissionsByIsAdminModule())*/
	.useModule(new RoutingModule({
		routerOptions: {
			routes: AdminRoutes
		}
	}
))
	.useModule(new UtilsModule());

application.mountAsync("#app");