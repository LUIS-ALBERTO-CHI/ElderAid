import SearchService from '@/Modules/Search/Services/search-service';
import { UserIdSearchProvider, UserSearchProvider } from '@/Modules/UsersSearch/Services/users-search-provider';


export default {
	async configureAsync() {
		SearchService.register(new UserSearchProvider());
		SearchService.register(new UserIdSearchProvider());
	}
}