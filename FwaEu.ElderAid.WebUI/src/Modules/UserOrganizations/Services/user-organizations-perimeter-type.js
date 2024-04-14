import UserOrganizationMasterDataService from "@/ElderAid/Organizations/Services/organizations-master-data-service"

export default {
	titleKey: 'userOrganizationsTitle',
	key: 'UsersOrganizations',
	async fetchDataAsync() {
		return await UserOrganizationMasterDataService.getAllAsync();
	}
}