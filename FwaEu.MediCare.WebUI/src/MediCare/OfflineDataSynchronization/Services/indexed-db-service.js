import { Configuration } from "@/Fwamework/Core/Services/configuration-service";

// Define a class for your IndexedDB service
class IndexedDBService {
    constructor(objectStoreName) {
        this.databaseName = Configuration.application.technicalName;
        this.objectStoreName = objectStoreName;
        this.db = null; // Hold a reference to the database connection
    }

    openDatabase() {
        return new Promise((resolve, reject) => {
            if (this.db) {
                // Database connection already exists, resolve with the existing connection
                resolve(this.db);
            } else {
                const request = window.indexedDB.open(this.databaseName, 1);

                request.onupgradeneeded = (event) => {
                    const db = event.target.result;
                    db.createObjectStore(this.objectStoreName, { keyPath: 'id', autoIncrement: true });
                };

                request.onsuccess = (event) => {
                    this.db = event.target.result;
                    resolve(this.db);
                };

                request.onerror = (event) => {
                    reject(event.target.error);
                };
            }
        });
    }

    withTransaction(mode, callback) {
        return new Promise((resolve, reject) => {
            this.openDatabase()
                .then((db) => {
                    const transaction = db.transaction([this.objectStoreName], mode);
                    const objectStore = transaction.objectStore(this.objectStoreName);

                    const request = callback(objectStore);

                    request.onsuccess = () => {
                        resolve(request.result);
                    };

                    request.onerror = (event) => {
                        reject(event.target.error);
                    };

                    transaction.oncomplete = () => {
                        db.close();
                    };
                })
                .catch((error) => {
                    reject(error);
                });
        });
    }

    addToObjectStore(data) {
        return this.withTransaction('readwrite', (objectStore) => {
            return objectStore.add(data);
        });
    }

    readObjectStore() {
        return this.withTransaction('readonly', (objectStore) => {
            return objectStore.getAll();
        });
    }

    clearObjectStore() {
        return this.withTransaction('readwrite', (objectStore) => {
            return objectStore.clear();
        });
    }
}

export default IndexedDBService;