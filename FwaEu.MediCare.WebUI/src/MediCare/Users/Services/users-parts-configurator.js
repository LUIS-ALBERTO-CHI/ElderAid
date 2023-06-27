//import CredentialsPart from '@/Modules/DefaultAuthentication/UserParts/Credentials/credentials-user-part';
import CulturePart from '@/Modules/UserCulture/Components/culture-user-part';
import HistoryPart from '@/Modules/UserHistory/Components/history-user-part';
import RolePart from '@/Modules/Roles/Components/role-user-part';
import AdminStatePart from '@/Modules/UserAdminState/Components/admin-state-user-part';
import UserPartsRegistry from '@/Fwamework/Users/Services/users-parts-registry';
import UserApplicationUserPart from '@/MediCare/Users/Services/user-application-user-part';

import OrganizationUserPart from '@/MediCare/Organizations/Components/organization-user-part';

export default {

	configure() {
		//UserPartsRegistry.addUserPart(CredentialsPart);
		UserPartsRegistry.addUserPart(CulturePart);
		UserPartsRegistry.addUserPart(HistoryPart);
		UserPartsRegistry.addUserPart(AdminStatePart);
		UserPartsRegistry.addUserPart(UserApplicationUserPart);
		UserPartsRegistry.addUserPart(RolePart);
		UserPartsRegistry.addUserPart(OrganizationUserPart);
	}
}