<template>
    <div id="app" :class="zoneName" :style="customStyle">
        <div :class="cssClasses">
            <component v-if="pageLayout" :is="pageLayout" :title="currentPageTitle" :is-x-small="screen.isXSmall" :is-large="screen.isLarge">
                <template #content-header>
                </template>
                <header class="app-header">
                    <div class="main-header">
                        <PublicHeaderToolbarComponent>
                        </PublicHeaderToolbarComponent>
                    </div>
                </header>
                <div class="home-content">
                    <router-view :key="$route.fullPath" />
                </div>
            </component>
        </div>
    </div>
    <DynamicDialog/>
</template>

<script>
	import { defineAsyncComponent, shallowRef, computed } from 'vue'
    import { Configuration } from "@/Fwamework/Core/Services/configuration-service";
    const path = Configuration.application.publicUrl + Configuration.application.customResourcesPath;
	const DefaultPageLayout = defineAsyncComponent(() => import('@/ElderAid/Components/Layouts/PublicApplicationLayoutComponent.vue'));
    import { sizes, subscribe, unsubscribe } from "@UILibrary/Extensions/Content/utils/media-query";
    import ApplicationZoneService from "@/Fwamework/ApplicationZones/application-zone-service";
	import "primevue/resources/themes/lara-light-indigo/theme.css";
	import "primevue/resources/primevue.min.css";

    import DynamicDialog from 'primevue/dynamicdialog';
    
    import PublicHeaderToolbarComponent from "@/ElderAid/Components/PublicHeaderToolbarComponent.vue";

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
            goHome() {
                this.$router.push("/Default");
            },
            goProfil() {
                this.$router.push("/UserSettings");
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
            PublicHeaderToolbarComponent,
            DynamicDialog
        }
    }
</script>

<style lang="scss" src="@/ElderAid/Content/application-styles.scss" />
<style type="text/css" src="@/ElderAid/Components/Content/home-page.css"></style>
<style type="text/css" src="@/ElderAid/Content/slider.css"></style>

<style scoped>
    .app-header {
        background-color: #f7f9fa;
        border-bottom: 1px solid var(--bg-color);
    }
    .app-footer {
        border-top: 1px solid var(--bg-color);
    }
    .input-field {
        border: 1px solid var(--bg-color);
    }
</style>