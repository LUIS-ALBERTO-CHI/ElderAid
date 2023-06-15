import PubSubService from './pub-sub-service.js';

class InternetService {
    static isOnline() {
        return navigator.onLine;
    }

    static addOnlineListener(callback) {
        const pubSubService = new PubSubService();
        pubSubService.subscribe('internetOnline', callback);

        window.addEventListener('online', () => {
            pubSubService.publish('internetOnline');
        });
    }

    static addOfflineListener(callback) {
        window.addEventListener('offline', callback);
    }
}

export default InternetService;
