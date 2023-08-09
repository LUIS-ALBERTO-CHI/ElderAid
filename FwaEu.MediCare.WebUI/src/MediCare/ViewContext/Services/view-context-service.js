import HttpService from '@/Fwamework/Core/Services/http-service';
import { LocalStorage } from '@/Fwamework/Storage/Services/local-storage-store';
import AsyncEventEmitter from '@/Fwamework/Core/Services/event-emitter-class';

const viewContextKey = 'View-Context';

const viewContextChangedEmitter = new AsyncEventEmitter();

export class ViewContextModel {

	constructor(organization) {
		this.id = organization.id;
		this.name = organization.name;
		this.orderPeriodicityDays = organization.orderPeriodicityDays;
		this.orderPeriodicityDayOfWeek = organization.orderPeriodicityDayOfWeek;
		this.lastPeriodicityOrder = new Date(organization.lastPeriodicityOrder);
		this.nextPeriodicityOrder = new Date(organization.nextPeriodicityOrder);
		this.periodicityOrderActivationDaysNumber = organization.periodicityOrderActivationDaysNumber;
		this.isStockPharmacyPerBox = organization.isStockPharmacyPerBox;
		this.updatedOn = new Date(organization.updatedOn);
	}

	databaseName = null
}

export default {
	configure() {
		const $this = this;
		HttpService.interceptors.request.use(async config => {

			config.headers.common[viewContextKey] = JSON.stringify($this.get());
			return config;
		});
	},

	onChanged(listener) {
		return viewContextChangedEmitter.addListener(listener);
	},

	/** @param {ViewContextModel} viewContext */
	set(viewContext) {
		LocalStorage.setValue(viewContextKey, viewContext);
		viewContextChangedEmitter.emitAsync(viewContext);
	},

	/** @returns {ViewContextModel} */
	get() {
		const viewContext = LocalStorage.getValue(viewContextKey);
		return viewContext;
	}
}