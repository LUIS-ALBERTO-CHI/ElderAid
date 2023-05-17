import { confirm, alert } from 'devextreme/ui/dialog';

export default {
	async confirmAsync(message, title = null) {
		return await confirm(message, title);
	},
	async alertAsync(message, title = null) {
		return await alert(message, title);
	}
};