<template>
	<div class="page-home">
		<div class="flex-section justify-content-center" v-if="isSingleOrganization">
			<span class="organization-text" v-if="organizations.length > 0">{{ organizations[0].name }}</span>
			<span class="organization-text" v-else>Vous n'êtes affecté à aucun EMS (base de données)</span>
		</div>
		<div v-else class="change-organization-container">
			<span @click="goToOrganizationSelectionPage">{{ organization?.name }}</span>
		</div>
		<div v-if="this.patientsActive.length > 0" class="vignette-list">
			<div class="vignette-item">
				<div @click="goToPatientPage" class="vignette-main-info">
					<i class="fa-regular fa-user fa-fw vignette-icon" style="color: #94a595;" />
					<span v-show="patientsActive.length > 0" class="vignette-text">
						{{ patientsActive.length }}
						Pacientes
					</span>
				</div>
				<i class="fa-regular fa-angle-right chevron-icon" />
			</div>
			<!-- <div @click="goToOrdersPage" class="vignette-item">
				<div class="vignette-main-info">
					<i class="fa-regular fa-cart-plus fa-fw vignette-icon" style="color: #bda6a0;" />
					<span class="vignette-text">Pedidos unitarios</span>
				</div>
				<i class="fa-regular fa-angle-right chevron-icon" />
			</div> -->
			<div @click="goToPeriodicPage" class="vignette-item">
				<div class="vignette-main-info">
					<i class="fa-regular fa-repeat vignette-icon fa-fw" style="color: #d8b291;" />
					<div style="display: flex; flex-direction: column;">
						<span class="vignette-text">Pedidos periodicos</span>
						<!-- <span class="vignette-text-subtitle">{{ getNumberOfPatientToValidate() }}</span> -->
					</div>
				</div>
				<i class="fa-regular fa-angle-right chevron-icon" />
			</div>
			<!-- <div @click="goToCabinetsPage" class="vignette-item">
				<div class="vignette-main-info">
					<i class="fa-regular fa-inbox vignette-icon fa-fw" style="color: #d9c4b3;" />
					<span class="vignette-text">Stock de farmacia</span>
				</div>
				<i class="fa-regular fa-angle-right chevron-icon" />
			</div> -->
			<!-- <div class="vignette-item" style="opacity: 0.4;">
				<div class="vignette-main-info">
					<i class="fa-regular fa-truck vignette-icon fa-fw" style="color: #a5ae9d;" />
					<span class="vignette-text">Livraison (en cours de développement)</span>
				</div>
				<i class="fa-regular fa-angle-right chevron-icon" />
			</div> -->
			<!-- <div @click="goToProfilPage" class="vignette-item">
				<div class="vignette-main-info">
					<i class="fa-regular fa-gear vignette-icon fa-fw" style="color: #bb8a7c;" />
					<span class="vignette-text">Ajustes</span>
				</div>
				<i class="fa-regular fa-angle-right chevron-icon" />
			</div> -->
		</div>
		<footer>
			<application-footer-component />
		</footer>
	</div>
</template>
<script>
	import { loadMessagesAsync } from "@/Fwamework/Culture/Services/single-file-component-localization";
	import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
	const path = Configuration.application.customResourcesPath;
	import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
	import Dropdown from 'primevue/dropdown';

	import { showLoadingPanel } from '@/Fwamework/LoadingPanel/Services/loading-panel-service';

	import OrganizationsMasterDataService from "@/ElderAid/Organizations/Services/organizations-master-data-service";
	import CabinetsMasterDataService from "@/ElderAid/Referencials/Services/cabinets-master-data-service";

	import PatientsMasterDataService from '@/ElderAid/Patients/Services/patients-master-data-service';
	import ViewContextService from "@/ElderAid/ViewContext/Services/view-context-service";
	import ProtectionMasterDataService from "@/ElderAid/Patients/Services/protections-master-data-service"
	import PeriodicOrdersMasterDataService from '@/ElderAid/Orders/Services/periodic-orders-master-data-service';

	export default {
		inject: ["deviceInfo"],
		components: {
			Dropdown
		},
		data() {
			const $this = this;
			return {
				selectedOrganization: null,
				isSingleOrganization: false,
				patientsActive: [],
				organizations: [],
				startLoadTime: 0,
				cabinets: [],
				organization: null,
				protections: [],
				periodicOrders: null,
			};
		},
		created: showLoadingPanel(async function () {
			await loadMessagesAsync(this, import.meta.glob('./Content/home-page-messages.*.json'));
			localStorage.removeItem("searchPatient")
			const patients = await PatientsMasterDataService.getAllAsync();
			this.organizations = await OrganizationsMasterDataService.getAllAsync();

			this.cabinets = await CabinetsMasterDataService.getAllAsync();
			if (this.organizations.length <= 1) {
				this.isSingleOrganization = true;
			}
			this.organization = ViewContextService.get();
			this.patientsActive = patients.filter(x => x.isActive);
			this.protections = (await ProtectionMasterDataService.getAllAsync()).filter(x => new Date(x.dateEnd) > new Date());
			this.periodicOrders = await PeriodicOrdersMasterDataService.getAllAsync();
		}),
		methods: {
			goToLoginFront() {
				this.$router.push("/Login")
			},
			async logoutAsync() {
				AuthenticationService.logoutAsync().then(() => {
					this.$router.push("/Login")
				});
			},
			goToPatientPage() {
				this.$router.push("/SearchPatient")
				localStorage.removeItem("searchPatient")
			},
			goToProfilPage() {
				debugger;
				this.$router.push("/UserSettings")
			},
			goToOrdersPage() {
				this.$router.push("/Orders")
			},
			goToCabinetsPage() {
				if (this.cabinets.length == 1) {
					this.$router.push("/Cabinet/" + this.cabinets[0].id);
				} else {
					this.$router.push("/stockPharmacy")
				}
			},
			goToPeriodicPage() {
				this.$router.push("/PeriodicOrders")
			},
			goToOrganizationSelectionPage() {
				this.$router.push("/OrganizationSelection")
			},
			getNumberOfPatientToValidate() {
				if (!this.protections || !this.periodicOrders) {
					return '0 patients à valider';
				}

				const patientsToValidate = this.protections
					.map(patient => patient.patientId)
					.filter((patientId, index, array) =>
						patientId && !this.isPatientInPeriodicOrders(patientId) && array.indexOf(patientId) === index
					)
					.length;

				return `${patientsToValidate} ${patientsToValidate !== 1 ? 'patients' : 'patient'} à valider`;
			},
			isPatientInPeriodicOrders(patientId) {
				return this.periodicOrders.some(order => order.patientId === patientId);
			}
		}
	}
</script>

