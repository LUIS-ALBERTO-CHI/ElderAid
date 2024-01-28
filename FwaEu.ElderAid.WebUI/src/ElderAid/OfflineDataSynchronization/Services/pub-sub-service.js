class PubSubService {
    constructor() {
        this.subscribers = {};
    }

    subscribe(event, callback) {
        if (!this.subscribers[event]) {
            this.subscribers[event] = [];
        }

        this.subscribers[event].push(callback);
    }

    unsubscribe(event, callback) {
        if (this.subscribers[event]) {
            const index = this.subscribers[event].indexOf(callback);
            if (index !== -1) {
                this.subscribers[event].splice(index, 1);
            }
        }
    }

    publish(event, data) {
        if (this.subscribers[event]) {
            this.subscribers[event].forEach((callback) => {
                callback(data);
            });
        }
    }
}

export default PubSubService;
