import notificationService from '@/Fwamework/Notifications/Services/notification-service';
import DosageFormMasterDataService from "@/MediCare/Referencials/Services/dosage-form-master-data-service";
import ProtectionsMasterDataService from "@/MediCare/Patients/Services/protections-master-data-service";
import TreatmentsMasterDataService from "@/MediCare/Patients/Services/treatments-master-data-service";
import StockConsumptionMasterDataService from "@/MediCare/StockConsumption/Services/stock-consumption-master-data-service";
import ArticlesTypeMasterDataService from "@/MediCare/Articles/Services/articles-type-master-data-service";
import PeriodicOrdersMasterDataService from "@/MediCare/Orders/Services/periodic-orders-master-data-service";
import OrdersMasterDataService from "@/MediCare/Orders/Services/orders-master-data-service";
import BuildingsMasterDataService from "@/MediCare/Referencials/Services/buildings-master-data-service";
import UserOrganizationsMasterDataService from "@/MediCare/Organizations/Services/organizations-user-master-data-service";
import ArticlesMasterDataService from "@/MediCare/Articles/Services/articles-master-data-service";
import PatientsMasterDataService from "@/MediCare/Patients/Services/patients-master-data-service";
import OrganizationsMasterDataService from "@/MediCare/Organizations/Services/organizations-master-data-service";
import ViewContextService, { ViewContextModel } from '@/MediCare/ViewContext/Services/view-context-service';

let notFirstLoad = false;

const CachePreloaderService = {
    async loadAllMasterDataAsync(onlyEms) {

        this.startLoadTime = new Date().getTime();
        const notification = notificationService.showInformation("Chargement des données, veuillez patienter...",
            {
                progressBar: true,
                layout: 'center',
                killer: true,
                timeout: false,
                closeWith: [],
                modal: true
            });
        try {

            const Organizations = await OrganizationsMasterDataService.getAllAsync();
            if (ViewContextService.get() == null) {

                ViewContextService.set(new ViewContextModel(Organizations[0]));
            }

                await Promise.all([
                    ArticlesMasterDataService.getAllAsync(),
                    OrdersMasterDataService.getAllAsync(),
                    BuildingsMasterDataService.getAllAsync(),
                    UserOrganizationsMasterDataService.getAllAsync(),
                    ProtectionsMasterDataService.getAllAsync(),
                    TreatmentsMasterDataService.getAllAsync(),
                    StockConsumptionMasterDataService.getAllAsync(),
                    PatientsMasterDataService.getAllAsync(),
                    PeriodicOrdersMasterDataService.getAllAsync()
                ]);

            if (!onlyEms) {
                await Promise.all([
                    DosageFormMasterDataService.getAllAsync(),
                    ArticlesTypeMasterDataService.getAllAsync()
                ]);
            }

            if (!notFirstLoad) {
            const loadingTime = new Date().getTime() - this.startLoadTime;
            if (loadingTime < 5000) {
                await new Promise(resolve => setTimeout(resolve, 5000 - loadingTime));
            }
                notFirstLoad = true;
        }
        } finally {
            notification.close();
        }
    }
}

export default CachePreloaderService;