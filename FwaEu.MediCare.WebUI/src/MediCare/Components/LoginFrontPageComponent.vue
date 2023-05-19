<template>
    <div class="login-page">
        <page-container type="form">
            <box :title="$t('title')">
                <component v-if="loginComponent" :is="loginComponent"></component>
            </box>
        </page-container>
    </div>
</template>
<script lang="js">
    import PageContainer from "@/Fwamework/PageContainer/Components/PageContainerComponent.vue";
    import Box from "@/Fwamework/Box/Components/BoxComponent.vue";
    import LocalizationMixin from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
    import LoginFormComponent from '@/Modules/DefaultAuthentication/UserParts/Credentials/Components/LoginFormComponent.vue'; 
    import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
    import { shallowRef } from "vue";

    export default {
        mixins: [LocalizationMixin],
        i18n: {
            messages: {
                getMessagesAsync(locale) {
                    return import(`./../../Fwamework/Authentication/Components/Content/login-messages.${locale}.json`);
                }
            }
        },
        data() {
            return {
                loginComponent: null,
                LoginFormComponent
            };
        },
        created: async function () {
            console.log("HNA")
            const loginComponent = shallowRef(await AuthenticationService.createLoginComponentAsync())?._value;
            if(loginComponent?.length > 0){
                this.loginComponent = loginComponent[0].loginComponent;
            }
        },
        components: {
            Box,
            PageContainer
        }
    };
</script>
<style type="text/css" src="@/MediCare/Components/Content/login-front-page.css"></style>
