import HttpService from "@/Fwamework/Core/Services/http-service";
import { I18n } from "../../../Fwamework/Culture/Services/localization-service";
import MasterDataService from "@/Fwamework/MasterData/Services/master-data-service";

export default {

	async getAllAsync() {
	},

	async getMasterDataByPatientId(patientId, masterDataKey) {
		const masterDataService = new MasterDataService(masterDataKey, ['id'], false);
		const models = await masterDataService.getAllAsync();
		return models.filter(t => t?.patientId === patientId);
	}
}
