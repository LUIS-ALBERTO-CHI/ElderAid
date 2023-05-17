const persistentNotificationFilesContext = import.meta.globEager('/**/*-persistent-notification.js');

export default {
    async getAllAsync() {
		return Object.keys(persistentNotificationFilesContext).map(path => persistentNotificationFilesContext[path].default)
    }
}