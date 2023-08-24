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
import DialogService from '@/MediCare/Cache/Services/dialog-service';
import LoaderComponent from '@/MediCare/Cache/Components/LoaderComponent.vue';

let initialized = false;

const CachePreloaderService = {
    async loadAllMasterDataAsync(onlyEms, forceReload = false) {
        if (forceReload ){
            initialized = false
        }
        let dialog = null;
        if (!initialized) {
            this.startLoadTime = new Date().getTime();
            dialog = DialogService.open(LoaderComponent,
                {
                    props: {
                        header: 'Chargement des données, veuillez patienter...',
                        style: {
                            width: '380px',
                            height: '430px',
                            background: '#f8f8f8',
                            position: 'center'
                        },
                        modal: true,
                        closable: false,
                        closeOnEscape: false,
                    },
                });
        }
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

                if (!initialized) {
                    const loadingTime = new Date().getTime() - this.startLoadTime;
                    if (loadingTime < 5000) {
                        await new Promise(resolve => setTimeout(resolve, 5000 - loadingTime));
                    }
                }
            } finally {
                if (!initialized) {
                    dialog.close();
                    initialized = true;
                }
            }
    }
}

export default CachePreloaderService;