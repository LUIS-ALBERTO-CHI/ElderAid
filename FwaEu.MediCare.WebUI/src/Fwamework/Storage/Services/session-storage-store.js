import Store from "./abstract-store";

class SessionStorageStore extends Store {

	/**
	 * @param {String} keyPrefix
	 */
	constructor(keyPrefix = "") {
		super();
		this.keyPrefix = keyPrefix;
	}

	setValue(cacheKey, objValue) {
		sessionStorage.setItem(this.getItemKey(cacheKey), JSON.stringify(objValue));
	}

	getValue(cacheKey) {
		return JSON.parse(sessionStorage.getItem(this.getItemKey(cacheKey)));
	}

	removeValue(cacheKey) {
		sessionStorage.removeItem(this.getItemKey(cacheKey));
	}

	//Implement base store functions
	setValueAsync = this.setValue;
	getValueAsync = this.getValue;
	removeValueAsync = this.removeValue;

	getItemKey(cacheKey) {
		return this.keyPrefix + "_" + cacheKey;
	}
}
export default SessionStorageStore;

export const SessionStorage = new SessionStorageStore();
