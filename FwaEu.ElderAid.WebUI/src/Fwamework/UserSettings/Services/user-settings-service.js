import HttpService from '@/Fwamework/Core/Services/http-service';

export default {
	async saveAsync(id, user) {
		let response = await HttpService.put(`UserSettings/save/${id}`, user);//TODO:a cambiar cuando se proporcione la API web
		return response.data;
	}
}