import PersistentNotificationTypesService from "@/Modules/PersistentNotifications/Services/persistent-notification-type-service";
import NotificationDataService from "@/Modules/PersistentNotifications/Services/persistent-notifications-service";
import PersistentNotificationModel from "@/Modules/PersistentNotifications/Services/persistent-notifications-model";

export default {
    async getAllAsync() {
		const notifications = await NotificationDataService.getAllAsync();
		let notificationsList = [];
		let notifGetMessages = [];
		//NOTE:Busca entre todos los archivos *-persistent-notification.js los notificationTypes que queremos mostrar.
        const persistentNotificationTypes = await PersistentNotificationTypesService.getAllAsync();
        for (let persistentNotifType of persistentNotificationTypes) {
			for (let notif of notifications) { 
				if (notif.notificationType == persistentNotifType.notificationType) { 	
					notificationsList.push( //NOTE: Adicionar el objeto modelo de notificación persistente a la lista de notificationsList.
						new PersistentNotificationModel(notif.id, notif.notificationType, notif.sentOn, notif.seenOn, undefined, notif.isSticky) /* NOTE: model in undefined at this stage */
					);
					notifGetMessages.push( persistentNotifType.getMessageAsync(notif.model) );
				}
			}
		}
		notificationsList = await this.processGetMessageAsync(notificationsList, notifGetMessages);
		return notificationsList.sort(function (a, b) { return b.sentOn - a.sentOn; });
    },
	async processGetMessageAsync(notificationsList, notifGetMessages)
	{
		let responsesNotifGetMessages = await Promise.all(notifGetMessages);
		for (let i = 0; i < responsesNotifGetMessages.length; i++) 
		{
			notificationsList[i].model = responsesNotifGetMessages[i];
		}
		return notificationsList;
	}
}