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

    //openDatabase() {
    //    return new Promise((resolve, reject) => {
    //        const request = window.indexedDB.open(this.databaseName, 1);

    //        request.onupgradeneeded = (event) => {
    //            const db = event.target.result;
    //            db.createObjectStore(this.objectStoreName, { keyPath: 'id' });
    //        };

    //        request.onsuccess = (event) => {
    //            const db = event.target.result;
    //            resolve(db);
    //        };

    //        request.onerror = (event) => {
    //            reject(event.target.error);
    //        };
    //    });
    //}

    //addToObjectStore(data) {
    //    return new Promise((resolve, reject) => {
    //        this.openDatabase()
    //            .then((db) => {
    //                let transaction = db.transaction([this.objectStoreName], 'readwrite');
    //                let objectStore = transaction.objectStore(this.objectStoreName);

    //                console.log(data)
    //                const request = objectStore.add(data);

    //                request.onsuccess = () => {
    //                    resolve();
    //                };

    //                request.onerror = (event) => {
    //                    reject(event.target.error);
    //                };

    //                transaction.oncomplete = () => {
    //                    db.close();
    //                };
    //            })
    //            .catch((error) => {
    //                reject(error);
    //            });
    //    });
    //}

    //readObjectStore() {
    //    return new Promise((resolve, reject) => {
    //        this.openDatabase()
    //            .then((db) => {
    //                const transaction = db.transaction([this.objectStoreName], 'readonly');
    //                const objectStore = transaction.objectStore(this.objectStoreName);
    //                const request = objectStore.getAll();

    //                request.onsuccess = () => {
    //                    resolve(request.result);
    //                };

    //                request.onerror = (event) => {
    //                    reject(event.target.error);
    //                };

    //                transaction.oncomplete = () => {
    //                    db.close();
    //                };
    //            })
    //            .catch((error) => {
    //                reject(error);
    //            });
    //    });
    //}

    //clearObjectStore() {
    //    return new Promise((resolve, reject) => {
    //        this.openDatabase()
    //            .then((db) => {
    //                const transaction = db.transaction([this.objectStoreName], 'readwrite');
    //                const objectStore = transaction.objectStore(this.objectStoreName);
    //                const request = objectStore.clear();

    //                request.onsuccess = () => {
    //                    resolve();
    //                };

    //                request.onerror = (event) => {
    //                    reject(event.target.error);
    //                };

    //                transaction.oncomplete = () => {
    //                    db.close();
    //                };
    //            })
    //            .catch((error) => {
    //                reject(error);
    //            });
    //    });
    //}
}

export default IndexedDBService;