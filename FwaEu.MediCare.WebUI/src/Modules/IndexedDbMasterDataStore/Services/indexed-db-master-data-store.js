import IndexedDbService from '@/Modules/IndexedDb/Services/indexed-db-service';
import Store from '@/Fwamework/Storage/Services/abstract-store';



const databaseVersion = 1;
const databaseName = "masterData";
const openMasterDataDataBaseAsync = async () => await IndexedDbService.openAsync(databaseName, databaseVersion, async (dbUpgrade, dbUpgradeEvent) => {

	const masterDataManager = (await import("@/Fwamework/MasterData/Services/master-data-manager-service"));
	const masterDataManagerService = masterDataManager.default;
	const getMasterDataChangeInfoKey = masterDataManager.getMasterDataChangeInfoKey;

	masterDataManagerService._masterDataRegistry.forEach(md => {
		const masterDataKey = md.configuration.masterDataKey;
		if (!dbUpgrade.objectStoreNames.contains(masterDataKey)) {
			console.warn(dbUpgrade);
			console.warn(dbUpgradeEvent);
			const objectStoreEvent = dbUpgradeEvent.createObjectStore('myObjectStore', { keyPath: 'id', autoIncrement: true });
			console.log(objectStoreEvent);
			const objectStore = dbUpgrade.createObjectStore('myObjectStore', { keyPath: 'id', autoIncrement: true });
			console.log(objectStore);
			//dbUpgrade.createObjectStore(masterDataKey, { keyPath: "__id", autoIncrement: true });
			//dbUpgrade.createObjectStore(getMasterDataChangeInfoKey(masterDataKey), { keyPath: "__id", autoIncrement: true });
		}
	});
});

/**
 * @param {String} cacheKey
 */
const isChangeInfoAsync = async (cacheKey) => {
	const ChangeInfoStoreKeySuffix = (await import("@/Fwamework/MasterData/Services/master-data-manager-service")).ChangeInfoStoreKeySuffix;
	return cacheKey.endsWith(ChangeInfoStoreKeySuffix);
};

class IndexedDbMasterDataStore extends Store {

	/**
	 * @param {String} cacheKey
	 * @param {Array<any>} objValue
	 * @returns {Promise<void>}
	 */
	async setValueAsync(cacheKey, objValue) {
		if (!objValue)
			return;
		const isChangeInfo = await isChangeInfoAsync(cacheKey);
		const dataToSave = isChangeInfo ? [objValue] : objValue;
		const db = await openMasterDataDataBaseAsync();

		const transaction = db.transaction(cacheKey, 'readwrite');
		const store = transaction.objectStore(cacheKey);

		return new Promise(function (resolve, reject) {
			let i = 0;
			function recursivePut() {
				if (i < dataToSave.length) {
					var item = dataToSave[i];
					var insertQuery = store.put(item);
					insertQuery.onsuccess = recursivePut;
					insertQuery.onerror = reject;
					i++;
				}
				else {
					resolve();
				}
			}
			recursivePut();
		});
	}

	/**
	 * @param {String} cacheKey
	 * @returns {Promise<any>}
	 */
	async getValueAsync(cacheKey) {
		const isChangeInfo = await isChangeInfoAsync(cacheKey);
		const db = await openMasterDataDataBaseAsync();

		const transaction = db.transaction(cacheKey, 'readonly');
		const store = transaction.objectStore(cacheKey);
		const itemsRequest = store.getAll();

		return new Promise(function (resolve, reject) {
			itemsRequest.onsuccess = function (es) {
				db.close();
				const result = isChangeInfo ? es.target.result[0] : es.target.result;
				resolve(result);
			};

			itemsRequest.onerror = function (er) {
				db.close();
				reject(er);
			};
		});
	}


	/**
	 * @param {String} cacheKey
	 * @returns {Promise<void>}
	 */
	async removeValueAsync(cacheKey) {
		const db = await openMasterDataDataBaseAsync();

		const transaction = db.transaction(cacheKey, 'readwrite');
		const store = transaction.objectStore(cacheKey);

		await new Promise((resolve, reject) => {
			var clearRequest = store.clear();

			clearRequest.onsuccess = resolve;
			clearRequest.onerror = reject;
		});
		db.close();
	}

}

export default IndexedDbMasterDataStore;

export const IndexedDb = new IndexedDbMasterDataStore();
