<template>
    <div>
        <p v-if="isOnline" class="status-point-online" title="En ligne"></p>
        <p v-else class="status-point-offline" title="Hors ligne"></p>
    </div>
</template>

<script>

import OfflineDataSynchronizationService from "@/ElderAid/OfflineDataSynchronization/Services/indexed-db-service";
import CheckInternetService from "@/ElderAid/OfflineDataSynchronization/Services/check-internet-service";

import OrdersService from '@/ElderAid/Orders/Services/orders-service';
	


export default {
        data() {
            return {
                isOnline: CheckInternetService.isOnline(),
            }
        },
        async mounted() {
            CheckInternetService.addOnlineListener(await this.handleOnlineEventAsync);
            CheckInternetService.addOfflineListener(this.handleOfflineEvent);
        },
        methods: {
            async handleOnlineEventAsync() {
                this.isOnline = true;
                
                const indixedDbService = new OfflineDataSynchronizationService('orders');
                const orders = await indixedDbService.readObjectStore();
                await OrdersService.saveAsync(orders).then(async (result) => {

                    // Perform actions to notify your application about the online status
                    const indixedDbService = new OfflineDataSynchronizationService('orders');
                    await indixedDbService.clearObjectStore();
                    console.log('Internet is online. Notifying application...');
                    
                })
                
            },
            handleOfflineEvent() {
                this.isOnline = false;
                
                //NOTE: Create an instance of the IndexedDB service

               
   
            // Add each item from the list to the object store
            //buildings.forEach((data) => {
            //  indixedDbService.addToObjectStore({ id : data.id, name : data.name })
            //    .then(() => {
            //      console.log('Data added successfully');
            //    })
            //    .catch((error) => {
            //      console.error('Error adding data', error);
            //    });
            //});






                console.log('Internet is offline');
                // Perform actions to handle the offline status
            }
        }
}
</script>

<style>
    .status-point-online {
        width: 20px;
        height: 20px;
        border-radius: 50%;
        background-color: green;
        cursor: pointer;
    }

    .status-point-offline {
        width: 20px;
        height: 20px;
        border-radius: 50%;
        background-color: red;
        cursor: pointer;
    }
</style>