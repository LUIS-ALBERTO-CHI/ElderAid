import UserHeaderService from '@/Modules/UserHeader/Services/user-header-service';

export default {
	async configureAsync() {
		await UserHeaderService.configureAsync();
	}
}