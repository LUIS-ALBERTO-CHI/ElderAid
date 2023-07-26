import HttpService from "@/Fwamework/Core/Services/http-service";
import { I18n } from "../../../Fwamework/Culture/Services/localization-service";
import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";
import { AsyncLazy } from '@/Fwamework/Core/Services/lazy-load';
import { useRoute } from "vue-router";
import { ref } from "vue";


const PatientService = {
	async getPatientById(id) {
		const masterDataService = new MasterDataService('Patients', ['id'], false);
		const models = await masterDataService.getAllAsync();
		return(models.find(t => t.id == id))
	},

	async getMasterDataByPatientId(patientId, masterDataKey) {
		const masterDataService = new MasterDataService(masterDataKey, ['id'], false);
		const models = await masterDataService.getAllAsync();
		return models.filter(t => t?.patientId === patientId);
	},

	async getIncontinenceLevelAsync(patientId) {
		const dates = ['dateStart', 'dateEnd', 'virtualDateWithoutOverPassed'];
		const response = await HttpService.get(`/Patients/${patientId}/GetIncontinenceLevel`);

		for (const date of dates) {
			if (response.data[date]) {
				response.data[date] = this.formatDate(response.data[date]);
			}
		}
		return response.data;
	},
	formatDate(date) {
		const options = { year: 'numeric', month: '2-digit', day: '2-digit' };
		const formattedDate = new Intl.DateTimeFormat('default', options).format(new Date(date));
		return formattedDate;
	}
}

export const usePatient = () => {
	const route = useRoute();
	const patientLazy = new AsyncLazy(async () => {
		return await PatientService.getPatientById(route.params.id);
	});
	const getCurrentPatientAsync = async () => {
		return await patientLazy.getValueAsync();
	};
	return {
		patientLazy,
		getCurrentPatientAsync
	};
}


export default PatientService;

