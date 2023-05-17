<template>
    <div :class="[{'user-notification-item-seen' : isNotificationSeen, 'button-delete-visible' : displayDeleteButton}, 'user-notification-item']"
		 @mouseover="showDeleteButton"
		 @mouseleave="hideDeleteButton">
        <div class="user-notification-item-title">
            <div class="user-notification-item-text">
                {{notification.model}}
            </div>
            <dx-button @click="deleteNotification" icon="clear" class="button-delete">
            </dx-button>
        </div>
        <div class="user-notification-item-date">
            {{ $t('sentOn') }}  {{ notification.sentOn.toLocaleString() }}
        </div>
    </div>
</template>
<script>
    import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
	import DxButton from 'devextreme-vue/button';
	
    export default {
        components: {
            DxButton
		},
        mixins: [LocalizationMixin],
        i18n: {
			messages: {
				getMessagesAsync(locale) {
					return import(`./Content/persistent-notifications-messages.${locale}.json`);
				}
			}
		},
		props: {
			smallModeEnabled: Boolean,
            notification: {
                required: true
            }
		},
		data() {
			return {
				displayDeleteButton: false
			}
		},
		created() {
			this.displayDeleteButton = this.smallModeEnabled && !this.notification.isSticky;
		},
        methods: {
            deleteNotification() {
                this.$emit('delete-notification', this.notification.id);
			},
			showDeleteButton() {
				if (!this.smallModeEnabled && !this.notification.isSticky)
					this.displayDeleteButton = true;
			},
			hideDeleteButton() {
				if (!this.smallModeEnabled)
					this.displayDeleteButton = false;
			}
		},
        computed: {
			isNotificationSeen() {
                return this.notification.seenOn != null;
			},
			deleteButtonVisibility() {
				return this.notification.isSticky ? 'hidden' : 'visible';
			}
        }
	}
</script>
<style>
	.user-notification-item {
        padding-left: 12px;
		color: var(--primary-text-color);
	}

	.user-notification-item-title {
		display: flex;
		align-items: center;
		justify-content: space-between;
	}

		.user-notification-item-title .user-notification-item-text {
			font-size: 12px;
		}

	.user-notification-item-date {
		font-size: 11px;
        padding-top: 6px;
		color: #AAAAAA;
	}

	.user-notification-item-text,
	.user-notification-item-date {
		font-weight: bold;
	}

	.user-notification-item-seen .user-notification-item-text,
	.user-notification-item-seen .user-notification-item-date {
		opacity: 0.55;
        font-weight: normal;
		transition: opacity ease-in-out 200ms;
	}

	.button-delete {
		opacity: 0;
		transition: opacity ease-in-out 200ms;
		min-width: 36px;
		height: 36px;
		visibility: v-bind(deleteButtonVisibility);
	}

	.button-delete-visible .button-delete {
		opacity: 1;
	}

	body .user-notification-item-title .button-delete.dx-button-has-icon .dx-icon {
		color: var(--secondary-text-color);
		line-height: 18px;
		font-size: 18px;
		height: 18px;
		width: 18px;
	}

	.user-notification-item-title .dx-button-mode-contained.dx-state-hover {
		background-color: transparent;
	}
</style>