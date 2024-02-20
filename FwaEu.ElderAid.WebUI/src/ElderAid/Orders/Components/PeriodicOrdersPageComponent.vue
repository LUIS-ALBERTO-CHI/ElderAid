<template>
	<div>
		<div class="orders-container">
			<span class="p-input-icon-right">
				<i @click="removeSearch" class="fa fa-solid fa-close"
				   :style="searchOrders.length == 0 ? 'opacity: 0.5;' : ''" />
				<InputText ref="searchInput" v-model="searchOrders" class="search-input"
						   placeholder="Buscar un pedido" />
			</span>
			<Dropdown v-show="buildings.length > 1" v-model="selectedBuilding" :options="buildingOptions" class="select-sector" />

			<Dropdown v-model="selectedOrderType" :options="ordersTypeOptions" class="select-sector" />
			<div v-if="periodicOrders"
				 class="periodic-orders-container">
				<div v-for="patient in filteredPatients" @click="goToPeriodicOrdersPage(patient)" :key="patient.id"
					 class="periodic-orders-item">
					<div class="header">
						<div class="patient-info">
							<i class="fa-sharp fa-solid "
							   :class="isPeriodicOrderValidated(patient) ? 'fa-check check-icon' : 'fa-circle-exclamation alert-periodic-icon'" />
							<span>{{ patient.fullName }}</span>
						</div>
						<div class="room-info">
							<i class="fa-solid fa-bed" />
							<span>{{ patient.roomName }}</span>
						</div>
					</div>
					<div class="quantity-info">
						<span v-if="isPeriodicOrderValidated(patient)">{{ getQuantityPeriodicOrderValidated(patient) }}</span>
						<span v-else>{{ getQuantityPeriodicOrderNotValidated(patient) }}</span>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
	import InputText from 'primevue/inputtext';
	import Dropdown from 'primevue/dropdown';

	import PeriodicOrdersMasterDataService from '../Services/periodic-orders-master-data-service';
	import PatientsMasterDataService from "@/ElderAid/Patients/Services/patients-master-data-service";

	import BuildingsMasterDataService from "@/ElderAid/Referencials/Services/buildings-master-data-service";
	import ProtectionMasterDataService from "@/ElderAid/Patients/Services/protections-master-data-service"

	export default {
		components: {
			InputText,
			Dropdown,
		},
		data() {
			return {
				ordersTypeOptions: ["Todos los pedidos", "Pacientes validados", "Pacientes a validar"],
				selectedOrderType: "Pacientes a validar",
				periodicOrders: null,
				patients: [],
				searchOrders: "",
				filteredPatients: [],
				buildingOptions: ["Todos los sectores"],
				selectedBuilding: "Todos los sectores",
				buildings: [],
				protections: [],
			};
		},
		async created() {
			this.buildings = await BuildingsMasterDataService.getAllAsync();
			this.periodicOrders = await PeriodicOrdersMasterDataService.getAllAsync();
			this.protections = (await ProtectionMasterDataService.getAllAsync()).filter(x => new Date(x.dateEnd) > new Date());
			this.patients = await PatientsMasterDataService.getAllAsync().then((patients) => {
				return patients.filter(patient => patient.isActive == true)
			});
			this.buildings = await BuildingsMasterDataService.getAllAsync();
			this.buildingOptions = this.buildingOptions.concat(this.buildings);
			this.selectedBuilding = this.buildingOptions[0]
		},
		methods: {
			isPeriodicOrderValidated(patient) {
				return this.periodicOrders.some(x => x.patientId == patient.id)
			},
			removeSearch() {
				this.searchOrders = "";
				this.focusSearchBar();
			},
			focusSearchBar() {
				this.$nextTick(() => {
					this.$refs.searchInput.$el.focus();
				});
			},
			goToPeriodicOrdersPage(patient) {
				if (!this.isPeriodicOrderValidated(patient))
					this.$router.push({
						name: "PeriodicOrder",
						params: { id: patient.id },
					});
			},
			getQuantityPeriodicOrderNotValidated(patient) {
				const patientProtections = this.protections.filter(protection => protection.patientId === patient.id);
				if (patientProtections.length > 0)
					return `${patientProtections.length} protection${patientProtections.length > 1 ? 's' : ''}`
				else
					return "Aucune protection"
			},
			getQuantityPeriodicOder(patient) {
				const patientProtections = this.protections.filter(protection => protection.patientId === patient.id);
				if (patientProtections.length >= 1) {
					return patientProtections.length;
				}
			},
			getQuantityPeriodicOrderValidated(patient) {
				const periodicOrdersOrdered = this.periodicOrders.filter(periodicOrder => periodicOrder.patientId === patient.id && periodicOrder.validatedOn != null);
				return `${periodicOrdersOrdered.length} protection${periodicOrdersOrdered.length > 1 ? 's' : ''}`
			}
		},
		computed: {
			filteredPatients() {
				return this.patients.filter(patient => {

					const isValidated = this.isPeriodicOrderValidated(patient);

					const hasProtections = this.getQuantityPeriodicOder(patient) > 0;

					return (
						(patient.fullName.toLowerCase().includes(this.searchOrders.toLowerCase()) ||
							patient.roomName.toLowerCase().includes(this.searchOrders.toLowerCase())) &&
						(
							this.selectedOrderType === "Tous les états des commandes" ||
							(this.selectedOrderType === "Patients validés" && patient.id != null && isValidated) ||
							(this.selectedOrderType === "Patients à valider" && patient.id != null && !isValidated && hasProtections)
						)
						&& (this.selectedBuilding === "Tous les secteurs" || patient.buildingId === this.selectedBuilding.id));
				});
			}
		}
	};
</script>
<style type="text/css" scoped src="./Content/orders-page.css"></style>