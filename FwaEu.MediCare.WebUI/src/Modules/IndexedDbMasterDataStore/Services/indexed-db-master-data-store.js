import IndexedDbService from '@/Modules/IndexedDb/Services/indexed-db-service';
import Store from '@/Fwamework/Storage/Services/abstract-store';


const databaseVersion = 1;
const databaseName = "masterData";


const openMasterDataDataBaseAsync = async () => {
	let db = await IndexedDbService.openAsync(databaseName, databaseVersion);

	if (db.version !== databaseVersion) {
		// Close the existing connection
		db.close();

		// Perform the upgrade by opening a new connection with a higher version
		db = await IndexedDbService.openAsync(databaseName, databaseVersion, upgradeDatabase);
	}

	return db;
};



const upgradeDatabase = async (db) => {

	const masterDataManager = (await import("@/Fwamework/MasterData/Services/master-data-manager-service"));
	const masterDataManagerService = masterDataManager.default;
	const getMasterDataChangeInfoKey = masterDataManager.getMasterDataChangeInfoKey;
	//const masterDataManager = (await import("@/Fwamework/MasterData/Services/master-data-manager-service")).default;
	//const getMasterDataChangeInfoKey = (await import("@/Fwamework/MasterData/Services/master-data-manager-service")).getMasterDataChangeInfoKey;

	// Loop for every object name
	masterDataManagerService._masterDataRegistry.forEach(md => {
		const masterDataKey = md.configuration.masterDataKey;
		if (!db.objectStoreNames.contains(masterDataKey)) {
			db.createObjectStore(masterDataKey, { keyPath: "__id", autoIncrement: true });
			db.createObjectStore(getMasterDataChangeInfoKey(masterDataKey), { keyPath: "__id", autoIncrement: true });
		}
	});
};

//const openMasterDataDataBaseAsync = async () => await IndexedDbService.openAsync(databaseName, databaseVersion, async dbUpgrade => {
//	const masterDataManager = (await import("@/Fwamework/MasterData/Services/master-data-manager-service"));
//	const masterDataManagerService = masterDataManager.default;
//	const getMasterDataChangeInfoKey = masterDataManager.getMasterDataChangeInfoKey;

//	masterDataManagerService._masterDataRegistry.forEach(md => {
//		const masterDataKey = md.configuration.masterDataKey;
//		if (!dbUpgrade.objectStoreNames.contains(masterDataKey)) {
//			console.warn(dbUpgrade)
//			console.warn(masterDataKey)
//			dbUpgrade.createObjectStore(masterDataKey, { keyPath: "__id", autoIncrement: true });
//			dbUpgrade.createObjectStore(getMasterDataChangeInfoKey(masterDataKey), { keyPath: "__id", autoIncrement: true });
//		}
//	});
//});

//Failed to execute 'createObjectStore' on 'IDBDatabase': The transaction is not active.

//Failed to execute 'transaction' on 'IDBDatabase': One of the specified object stores was not found.

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
