import { showLoadingPanel } from "@/Fwamework/LoadingPanel/Services/loading-panel-service";
import UserService from '@/Fwamework/Users/Services/user-service';
import UserSettingsPartsRegistry from '@/Fwamework/UserSettings/Services/user-settings-parts-registry';
import CurrentUserService from "@/Fwamework/Users/Services/current-user-service";
import { AsyncLazy } from "@/Fwamework/Core/Services/lazy-load";

export default {
	data() {
		return {
			currentUser: null,
			userLazy: new AsyncLazy(async () => {
				const currentUser = await CurrentUserService.getAsync();
				return await UserService.getUserByIdAsync(currentUser.id);
			}),
		};
	},
	created: showLoadingPanel(async function () {
		await this.loadAsync();
	}),
	methods: {
		getPartHandlers() {
			return UserSettingsPartsRegistry.getAll();
		},
		async getUserAsync() {
			return await this.userLazy.getValueAsync();
		},
		async getContextAsync() {
			let currentUser = await CurrentUserService.getAsync();
			this.currentUser = currentUser;
			return {
				currentPage: this,
				isNew: false,
				currentUser: currentUser
			};
		},
		bindModelByLocationAsync(model) {
			throw 'Unknown location: ' + model.location;
		},
		getModelUserToSave() {
			return {
				id: this.user.id,
				parts: {}
			};
		},
		getGeneralInformationComponent() {
			return this.$refs.generalInformation;
		},
		async saveUserAsync(userId, userToSave) {
			await UserService.saveAsync(userId, userToSave).then(() => {
				this.$router.go(-1);
			});
		}
	}
}