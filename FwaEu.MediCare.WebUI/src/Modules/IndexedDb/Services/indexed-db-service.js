import { Configuration } from "@/Fwamework/Core/Services/configuration-service";

/**
 * @type {IDBFactory}
 * */
const indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;
let firstOpenLock = {};

export default {
	_internalOpenAsync(databaseName, databaseVersion, upgradeDatabase) {
		const $this = this;
		const request = indexedDB.open(databaseName, databaseVersion);

		return new Promise((resolve, reject) => {
			request.onerror = (error) => {

				reject(error);
			};
			request.onsuccess = () => resolve(request.result);
			request.onupgradeneeded = async (event) => {
				console.log(event)
				const db = event.target.result;

				if (typeof upgradeDatabase === 'function') {
					await upgradeDatabase(db);
				}

				const upgradedDb = await $this._internalOpenAsync(databaseName, databaseVersion, upgradeDatabase);
				resolve(upgradedDb);
				//await upgradeDatabase(request.result);

				//const db = await $this._internalOpenAsync(databaseName, databaseVersion, upgradeDatabase);
				//resolve(db);
			};
		});
	},

	/**
	 * @param {String} databaseName
	 * @param {Number} databaseVersion
	 * @param {(dbUpdate: IDBDatabase) => any | (dbUpdate: IDBDatabase) => Promise<any>} upgradeDatabase
	 * @returns {IDBDatabase}
	 */
	async openAsync(databaseName, databaseVersion, upgradeDatabase) {
		if (!firstOpenLock[databaseName]) {
			firstOpenLock[databaseName] = this._internalOpenAsync(this.getDatabaseName(databaseName), databaseVersion, upgradeDatabase);
			return await firstOpenLock[databaseName];
		}
		else {
			await firstOpenLock[databaseName];
		}
		return await this._internalOpenAsync(this.getDatabaseName(databaseName), databaseVersion, upgradeDatabase);
	},

	getDatabaseName(databaseName) {
		return Configuration.application.technicalName + "_" + databaseName;
	}

}