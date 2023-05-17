<template>
    <div id="app" :class="zoneName" :style="customStyle">
        <div :class="cssClasses">
            <component v-if="pageLayout" :is="pageLayout" :title="currentPageTitle" :is-x-small="screen.isXSmall" :is-large="screen.isLarge">
                <template #content-header>
                    <breadcrumbs></breadcrumbs>
                </template>
                <header class="app-header">
                    <a href="">
                        <div class="container-logo">
                            <img class="cites-educatives" alt="" src="@/MediCare/Content/logo-cites-educatives.svg" />
                            <img class="logo" alt="" :src="getUrlFile('/logo.svg')" />
                        </div>
                    </a>
                </header>
                <div class="home-content">
                    <router-view :key="$route.fullPath" />
                </div>
                <footer class="app-footer">
                    <img v-if="secondLogoFooterEnabled" class="logo" alt="" :src="getUrlFile('/logo-ministry.png')" />
                    <img class="logo-fix" alt="" src="@/MediCare/Content/logo-ministry.png" />
                    {{ trackGoogleAnalytics() }}
                </footer>
            </component>
        </div>
    </div>
</template>

<script>
import { defineAsyncComponent, shallowRef, computed } from 'vue'
    import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
    const path = Configuration.application.publicUrl + Configuration.application.customResourcesPath;
    import DefaultPageLayout from '@/MediCare/Components/Layouts/PublicApplicationLayoutComponent.vue';
    import Breadcrumbs from "@/Fwamework/Breadcrumbs/Components/BreadcrumbsComponent.vue";
    import { sizes, subscribe, unsubscribe } from "@/Fwamework/DevExtreme/Content/utils/media-query";
    import ApplicationZoneService from "@/Fwamework/ApplicationZones/application-zone-service";
    import { page } from 'vue-analytics';

    function getScreenSizeInfo() {
        const screenSizes = sizes();

        return {
            isXSmall: screenSizes["screen-x-small"],
            isLarge: screenSizes["screen-large"],
            cssClasses: Object.keys(screenSizes).filter(cl => screenSizes[cl])
        };
    }

    function getScreenInfo() {
        let screenInfo = { isTouchEnabled: false };
        screenInfo.isTouchEnabled = ('ontouchstart' in window || (navigator.maxTouchPoints > 0) || (navigator.msMaxTouchPoints > 0))
        return screenInfo;
    }
    export default {
        name: 'app',
        data() {
            return {
                screen: getScreenSizeInfo(),
                currentPageTitle: Configuration.application.name,
                customBgColor: Configuration.application.backgroundColor,
                secondLogoFooterEnabled: Configuration.application.secondLogoFooterEnabled,
                zoneName: null,
                pageLayout: null,
                screenInfo: {}
            };
        },
        provide: {
            screenSizeInfo: getScreenSizeInfo(),
            deviceInfo: getScreenInfo()
        },
        watch: {
            currentPageTitle(newPageTitle) {
                document.title = newPageTitle;
            },
            '$route': {
                inmediate: true,
				handler(to, from) {
					this.loadCurrentRouteSetup(to, from);
				}
            }
        },
        computed: {
            cssClasses() {
                return ["app"].concat(this.screen.cssClasses);
            },
            customStyle() {
                return {
                    '--bg-color': this.customBgColor
                }
            }
        },
        methods: {
            loadCurrentRouteSetup(to, from) {
				if (!to.matched.length)
					return;
				if (!this.pageLayout || to.meta?.layout || (from.meta?.layout && !to.meta?.layout)) {
					this.pageLayout = shallowRef(to.meta?.layout ?? DefaultPageLayout);
				}
				this.zoneName = ApplicationZoneService.getCurrentZoneName(this);
			},
            screenSizeChanged() {
                this.screen = getScreenSizeInfo();
            },
            getDefaultPageTitle() {
                return Configuration.application.name;
            },
            getRoutePageTitle(route) {
                return route.meta?.pageTitle ?? (route.meta?.pageTitleKey ? this.$t(route.meta?.pageTitleKey) : null);
            },
            trackGoogleAnalytics() {
                if (Configuration.googleAnalytics.enableFullPtah)
                    page(this.$route.fullPath)
                else
                    page(this.$route.path)
            },
            getUrlFile(nameOfFile) {
                return path + nameOfFile;
            }
        },
        mounted() {
            subscribe(this.screenSizeChanged);
        },
        beforeDestroy() {
            unsubscribe(this.screenSizeChanged);
        },
        components: {
            Breadcrumbs
        }
    }
</script>

<style lang="scss" src="@/MediCare/Content/application-styles.scss" />
<style type="text/css" src="@/MediCare/Components/Content/home-page.css"></style>
<style type="text/css" src="@/MediCare/Content/slider.css"></style>
<style type="text/css" src="@/MediCare/Components/Content/search.css"></style>

<style scoped>
    .app-header {
        background-color: var(--bg-color);
    }
    .app-footer {
        border-top: 1px solid var(--bg-color);
    }
    .input-field {
        border: 1px solid var(--bg-color);
    }
</style>