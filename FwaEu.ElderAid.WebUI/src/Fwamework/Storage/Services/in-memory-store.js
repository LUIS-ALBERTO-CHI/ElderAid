import Store from "./abstract-store";

class InMemoryStore extends Store {
	storedItems = new Map();

	setValue(cacheKey, objValue) {
		this.storedItems.set(cacheKey, objValue);
	}

	getValue(cacheKey) {
		let cacheItem = this.storedItems.get(cacheKey);
		return (cacheItem != undefined) ? cacheItem : null;
	}

	removeValue(cacheKey) {
		this.storedItems.delete(cacheKey);
	}

	//Implement base store functions
	setValueAsync = this.setValue;
	getValueAsync = this.getValue;
	removeValueAsync = this.removeValue;
}

export default InMemoryStore;