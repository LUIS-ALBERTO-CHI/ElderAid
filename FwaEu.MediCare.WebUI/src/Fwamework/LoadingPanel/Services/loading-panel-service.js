import { waitFor } from 'vue-wait';
import { done, start } from '@/Modules/NavigationIndicator/Services/navigation-indicator-service';
export const ModalLoadingName = "modal_loading";
export const NoModalLoadingName = "no_modal_loading";
let waitInstance;

let lastWaitId = 1;

const getWaitIdentifier = function (loaderName) {
    return loaderName + (lastWaitId++);
};

export function showLoadingPanel(asyncFunction, loaderName = ModalLoadingName) {
    let waitForFunction = waitInstance ? waitFor.bind(waitInstance.$root) : waitFor;

    return async function (...args) {
        try {
            start();
            return await waitForFunction(getWaitIdentifier(loaderName), asyncFunction).apply(this, args);
        } finally {
            done();
        }
    };
}

let lastManualLoaderId = 0;
export default {
    configure(vueWait) {
		waitInstance = vueWait;
    },
    showLoadingPanel,
    showModal() {
		const waitName = ModalLoadingName + '_manual_' + (++lastManualLoaderId);
        start();
        waitInstance.start(waitName);

    },
    hideModal() {
		const waitName = ModalLoadingName + '_manual_' + (lastManualLoaderId--);
        waitInstance.end(waitName);
        done();
    }
};
