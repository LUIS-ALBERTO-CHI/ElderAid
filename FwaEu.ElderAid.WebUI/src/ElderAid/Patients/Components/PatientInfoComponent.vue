<template>
	<div v-if="patient" class="patient-container">
		<div class="patient-main-info-container">
			<div class="patient-area">
				<span style="font-weight: bold">{{ cuttedName(patient) }}</span>
				<i
					class="fa-solid fa-circle"
					:class="
						patient.isActive ? 'active-patient-text' : 'inactive-patient-text'
					"
				/>
			</div>
			<span
				><i class="fa fa-solid fa-bed" style="margin-right: 10px"></i
				>{{ patient.roomName }}</span
			>
		</div>
		<span>Actualización : 29/10/22 a las 9:12</span>
	</div>
</template>

<script>
export default {
	data() {
		return {};
	},
	props: {
		patient: {
			type: Object,
			required: true,
		},
	},
	async created() {},
	methods: {
		cuttedName(patient) {
			if (patient.fullName == null) return "";
			return patient?.fullName.length > 20
				? patient?.fullName.substring(0, 20) + "..."
				: patient?.fullName;
		},
	},
	computed: {},
};
</script>
<style type="text/css">
.patient-container {
	display: flex;
	flex-direction: column;
	row-gap: 10px;
}

.patient-main-info-container {
	display: flex;
	justify-content: space-between;
	color: var(--alt-secondary-text-color);
}

.patient-area {
	display: flex;
	flex-direction: row;
	align-items: center;
	column-gap: 10px;
}

.active-patient-text {
	color: #22c55e;
}

.inactive-patient-text {
	color: #484349;
	opacity: 0.5;
}
</style>
