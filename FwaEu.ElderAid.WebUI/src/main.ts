import "@/Modules/PureCSS/purecss";
import "@/Modules/FontAwesome/font-awesome-module";
import '@UILibrary/Extensions/Themes/generated/theme.base.css';


import IndexApp from './IndexApp.vue';
import { SentryModule } from "@/Modules/Sentry/sentry-module";
import { RoutingModule } from '@/Fwamework/Routing/routing-module';
import { CoreModule } from '@/Fwamework/Core/core-module';
import { ServerMonitoringModule } from '@/Modules/ServerMonitoring/server-monitoring-module';
import ServerApplicationInfoProvider from "@/Modules/ServerMonitoring/Services/server-application-info-provider";
import { CultureModule } from '@/Fwamework/Culture/culture-module';
import { AuthenticationModule } from '@/Fwamework/Authentication/authentication-module';
import { ErrorModule } from '@/Fwamework/Errors/error-module';
import { LoadingPanelModule } from '@/Fwamework/LoadingPanel/loading-panel-module';
import { UILibraryModule as DevExtremeModule } from '@/DevExtreme/module';//NOTE: We still need DevExtreme for Fallback
import { UILibraryModule } from '@UILibrary/module';

import { DialogModule } from '@/Modules/Dialog/dialog-module';
import { UsersModule } from "@/Fwamework/Users/users-module";
import { PermissionModule } from "@/Fwamework/Permissions/permissions-module";
import { GenericAdminModule } from '@/Modules/GenericAdmin/generic-admin-module';
import { DataImportModule } from "@/Modules/DataImport/data-import-module";
import { DefaultAuthenticationModule } from '@/Modules/DefaultAuthentication/default-authentication-module';
import { ApplicationModule } from "@/ElderAid/application-module";
import { UserCultureModule } from "@/Modules/UserCulture/user-culture-module";
import { UsersMasterDataModule } from "@/Modules/UserMasterData/users-master-data-module";
import { UserHistoryPartModule } from "@/Modules/UserHistory/user-history-part-module";
import { UserGroupsModule } from "@/Modules/UserGroups/user-groups-module";
import { UserOrganizationsModule } from "@/Modules/UserOrganizations/user-organizations-module";
import { UserPerimeterPartsModule } from "@/Modules/UserPerimeter/user-perimeter-part-module";
import { PermissionsByRoleModule } from "@/Modules/Roles/permissions-by-role-module";
import { UserAmdinStatePartModule } from "@/Modules/UserAdminState/user-admin-state-part-module";
import { AdministrationMenuModule } from "@/Fwamework/AdministrationMenu/administration-menu-module";
import { MasterDataModule } from "@/Fwamework/MasterData/master-data-module";
import { DotNetTypeConversionModule } from "@/Fwamework/DotNetTypeConversion/dot-net-type-conversion-module";
import { UserNotificationsModule } from "@/Modules/UserNotifications/user-notifications-module";
import SignalRUserNotificationsAdapter  from "@/Modules/UserNotifications/Services/signalr-user-notifications-adapter";
import { DataModule } from "@/Fwamework/Data/data-module";
import { PermissionsByIsAdminModule } from "@/ElderAid/PermissionsByIsAdmin/permissions-by-is-admin-module";

import { ApplicationUsersModule } from "@/ElderAid/Users/users-module";
import { ApplicationCultureModule } from "@/ElderAid/Culture/culture-module";
import { UserSettingsModule } from "@/Fwamework/UserSettings/user-settings-module";

import Application from "@/Fwamework/Core/Services/application";
import InMemoryStore from "@/Fwamework/Storage/Services/in-memory-store";

import DefaultAuthenticationHandler from "@/Modules/DefaultAuthentication/Services/default-authentication-handler";
import { UserTasksNavigationMenuModule } from "@/Modules/UserTasksNavigationMenu/user-tasks-navigation-menu-module";
import { UserTasksNotificationModule } from "@/Modules/UserTasksNotification/user-tasks-notification-module";
import { UserTasksUserNotificationsModule } from "@/Modules/UserTasksUserNotifications/user-tasks-user-notifications-module";
import { UserTasksListModule } from "@/Modules/UserTasksList/user-tasks-list-module";
import { MasterDataNotificationsModule } from "@/Modules/MasterDataNotification/master-data-notifications-module";
import { ContextViewModule } from "@/ElderAid/ViewContext/view-context-module";

import { ImpersonateAuthenticationModule } from "@/Modules/ImpersonateAuthentication/impersonate-authentication-module";
import SetupImpersonateAuthenticationHandler from "@/Modules/ImpersonateAuthentication/Setup/setup-impersonate-authentication-handler";
import { DatePickerModule } from '@/Modules/DatePicker/date-picker-module';
import AppRoutes from './app-routes';
import { OnlineStatusModule } from '@/Fwamework/OnlineStatus/online-module';
import { UtilsModule } from '@/Fwamework/Utils/utils-module';
import { DevExtremeCompatibilityModule } from "@/PrimeVue/Modules/DevExremeCompatibility/devextreme-compatibility-module";

import { VeeValidateModule } from "@/Modules/VeeValidation/vee-validate-module";

import { ReferencialsModule } from "@/ElderAid/Referencials/referencials-module";
import { PatientsModule } from "@/ElderAid/Patients/patients-module";
import { OrdersModule } from "@/ElderAid/Orders/orders-module";
import { CabinetsModule } from "@/ElderAid/PharmacyStock/cabinets-module";
import { StockConsumptionModule } from "./ElderAid/StockConsumption/stock-consumption-module";
import { ArticlesModule } from "./ElderAid/Articles/articles-module";
import { OrganizationsModule } from "@/ElderAid/Organizations/organizations-module";
import { CachePreloaderModule } from '@/ElderAid/Cache/cache-module';
import PrimeVue from 'primevue/config';

const application = new Application(IndexApp)
	.useModule(new CoreModule({
		//NOTE: We currently use the same version as server because managing the version for both server and client will require unnecessary efforts			
		applicationInfoProvider: ServerApplicationInfoProvider
	}))
	.useModule(new ServerMonitoringModule())
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
	.useModule(new ErrorModule())
	.useModule(new LoadingPanelModule())
	.useModule(new DotNetTypeConversionModule())
	.useModule(new DevExtremeModule())

	.useModule(new UILibraryModule())
	.useModule(new DevExtremeCompatibilityModule())
	.useModule(new DatePickerModule())
	.useModule(new DialogModule())
	.useModule(new PermissionModule())
	.useModule(new GenericAdminModule())
	.useModule(new DataImportModule())
	.useModule(new ApplicationModule())
	.useModule(new UserCultureModule())
	.useModule(new UsersMasterDataModule())
	.useModule(new UserHistoryPartModule())
	.useModule(new UserGroupsModule())
	.useModule(new UserOrganizationsModule())
	.useModule(new UserPerimeterPartsModule())
	.useModule(new UserAmdinStatePartModule())
	.useModule(new UserSettingsModule())
	.useModule(new PermissionsByRoleModule())
	//.useModule(new PermissionsByUserModule())
	.useModule(new AdministrationMenuModule())
	.useModule(new MasterDataModule({
		//NOTE: You can use another store like IndexedDbMasterDataStore or create your own store implementation
		defaultStore: new InMemoryStore()
	}))
	.useModule(new ApplicationUsersModule())
	.useModule(new ApplicationCultureModule())
	.useModule(new DataModule())
	.useModule(new UserNotificationsModule({
		signalRAdapter: new SignalRUserNotificationsAdapter()
	}))
	.useModule(new ContextViewModule())

	.useModule(new UserTasksUserNotificationsModule())
	.useModule(new UserTasksNavigationMenuModule())
	.useModule(new UserTasksNotificationModule())
	.useModule(new PermissionsByIsAdminModule())
	.useModule(new UserTasksListModule())
	.useModule(new MasterDataNotificationsModule())
	.useModule(new RoutingModule({
		routerOptions: {
			routes: AppRoutes
		}
	}
	))
	.useModule(new OnlineStatusModule())
	.useModule(new UtilsModule())
	.useModule(new VeeValidateModule())
	.useModule(new OrganizationsModule())
	.useModule(new ReferencialsModule())
	.useModule(new PatientsModule())
	.useModule(new OrdersModule())
	.useModule(new CabinetsModule())
	.useModule(new StockConsumptionModule())
	.useModule(new CachePreloaderModule())
	.useModule(new ArticlesModule());
application.vueApp.use(PrimeVue);

//NOTE: To make works chartjs-plugin-datalabels you need to register it globally i didn't find a better way to do it
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { Chart } from 'chart.js';
Chart.register(ChartDataLabels);

application.mountAsync("#app");