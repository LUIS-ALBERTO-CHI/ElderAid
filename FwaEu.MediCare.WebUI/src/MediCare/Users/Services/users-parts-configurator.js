import CredentialsPart from '@/Modules/DefaultAuthentication/UserParts/Credentials/credentials-user-part';
import CulturePart from '@/Modules/UserCulture/Components/culture-user-part';
import HistoryPart from '@/Modules/UserHistory/Components/history-user-part';
import AdminStatePart from '@/Modules/UserAdminState/Components/admin-state-user-part';
import UserPartsRegistry from '@/Fwamework/Users/Services/users-parts-registry';
import UserApplicationUserPart from './user-application-user-part';

export default {

	configure() {
		UserPartsRegistry.addUserPart(CredentialsPart);
		UserPartsRegistry.addUserPart(CulturePart);
		UserPartsRegistry.addUserPart(HistoryPart);
		UserPartsRegistry.addUserPart(AdminStatePart);
		UserPartsRegistry.addUserPart(UserApplicationUserPart);
	}
}