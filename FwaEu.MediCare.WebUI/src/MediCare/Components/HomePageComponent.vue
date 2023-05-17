<template>
    <div class="page-home" :aria-hidden="isShowSearchComponent">
        <div class="panel-title text-align-center">
            <h3> {{ getTitleText() }} </h3>
        </div>
        <div class="panel-search" role="search">
            <div class="input-icons" @click="onOpenSearchModal">
                <i class="fa fa-search action-button"></i>
                <input type="text"
                       readonly
                       class="input-field"
                       aria-label="Tapez la touche entrer pour la recherche"
                       placeholder="Acteur..."
                       @keyup.enter="onOpenSearchModal" />
            </div>
            <Search v-if="isShowSearchComponent"
                    :search-data="seachData" />
        </div>
        <div v-if="expandableTextEnabled">
            <expandable-text-block class="text-align-center" :visible-lines="7">
                <div class="panel-description">
                    {{ description }}
                    <Markdown :html="true" v-if="description" :source="description" />
                </div>
            </expandable-text-block>
        </div>
        <div class="panel-description" v-else>
            <Markdown :html="true" v-if="description" :source="description" />
        </div>
        <a v-if="!isCurrentUserAuthenticated" class="connexion-container" @click="goToLoginFront">{{$t('toLogin')}}</a>
        <a v-else class="connexion-container" @click="logoutAsync">{{$t('toLogout')}}</a>
    </div>
</template>
<script>
    import LocalizationMixing from '@/Fwamework/Culture/Services/single-file-component-localization-mixin';
    import Search from "@/MediCare/Components/SearchComponent.vue";
    import ExpandableTextBlock from "@/MediCare/Components/Common/ExpandableTextBlockComponent.vue";
    import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
    const path = Configuration.application.customResourcesPath;
    import Markdown from 'vue3-markdown-it';
    import AuthenticationService from '@/Fwamework/Authentication/Services/authentication-service';
    import GennevilliersDesc from '@/MediCare/Content/Gennevilliers/home-description-typography.txt?type=text'

    export default {
        components: {
            Search,
            ExpandableTextBlock,
            Markdown
        },
        inject: ["deviceInfo"],
        mixins: [LocalizationMixing],
        i18n: {
            messages: {
                getMessagesAsync(locale) {
                    return import(`@/MediCare/Components/Content/home-page-messages.${locale}.json`);
                }
            }
        },
        props: {
            searchCriteria: {
                type: Object,
                required: false
            }
        },
        data() {
            return {
                isShowSearchComponent: false,
                expandableTextEnabled: Configuration.application.expandableTextEnabled,
                description: GennevilliersDesc,
                seachData: this.searchCriteria,
                isCurrentUserAuthenticated: false,
            };
        },
        async created() {
            const module = path === "/Gennevilliers" ? (await import('../Content/Gennevilliers/home-description-typography.txt')) :
                (await import('../Content/Villierslebel/home-description-typography.txt'));
            const moduleUrl = module.default;
            fetch(moduleUrl)
                .then(response => response.text())
                .then(text => this.description = text);
            this.isCurrentUserAuthenticated = await AuthenticationService.isAuthenticatedAsync();
            if (this.searchCriteria) {
                this.isShowSearchComponent = true;
            }
        },
        methods: {
            getTitleText() {
                return this.$t('homePage');
            },
            onOpenSearchModal() {
                this.$router.push({
                    name: 'index',
                    query: {
                        searchCriteria: JSON.stringify(this.searchCriteria || {})
                    }
                });
            },
            goToLoginFront() {
                this.$router.push("/Login")
            },
            async logoutAsync() {
                AuthenticationService.logoutAsync().then(() => {
                    this.$router.push("/Login")
                });
            },
        },
    }
</script>