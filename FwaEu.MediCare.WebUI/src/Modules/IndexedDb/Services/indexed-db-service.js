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
				await upgradeDatabase(request.result, event.target.result);
				debugger;
				const db = await $this._internalOpenAsync(databaseName, databaseVersion, upgradeDatabase);
				console.warn(db)
				resolve(db);
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