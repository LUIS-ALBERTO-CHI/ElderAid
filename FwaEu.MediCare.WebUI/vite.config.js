import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import vueI18n from '@intlify/vite-plugin-vue-i18n';
import appSettingsJson from '@fwaeu/vite-plugin-app-settings-json';
import { resolve } from "path";
import { VitePWA } from 'vite-plugin-pwa';

const rootPath = __dirname;
const srcPath = resolve(rootPath, "./src");
export default defineConfig({
	plugins: [
		appSettingsJson({
			rootPath,
			externalize: false//NOTE: Set to true if you want to externalize the settings into an appSettings.js file
		}),
		vue(),
		vueI18n({
			fullInstall: false,
			runtimeOnly: false,
			include: resolve(srcPath, '/**/HACK-UNEXISTING_PATH.json')
		}),
		VitePWA({
			registerType: 'autoUpdate',
			injectRegister: 'inline',
			devOptions: {
				enabled: true
			},
			includeAssets: ['favicon.ico', 'apple-touch-icon.png', 'masked-icon.svg'],
			manifest: {
				"lang": "fr-fr",
				"android_package_name": "fwaeu.MediCare",
				"name": "MediCare",
				"short_name": "MediCare",
				"description": "MediCare",
				"start_url": "https://wild.fwa.eu/MediCare/",
				"background_color": "#0078D4",
				"theme_color": "#0078D4",
				"orientation": "any",
				"display": "standalone",
				"icons": [
					{
						"src": "/icon512.png",
						"sizes": "512x512"
					}
				]
			}

		})],
	resolve: {
		alias: {
			"@": srcPath,
			'vue-i18n': 'vue-i18n/dist/vue-i18n.esm-bundler.js',
			"devextreme/ui": 'devextreme/esm/ui' // Hack: Bug devextreme fails in production due to the different behavior of vite (than that of webpack) to import the compiled modules
			//https://supportcenter.devexpress.com/ticket/details/t1054272/vue3-react-vite-rollup-devextreme-fails-in-production-because-some-modules-do-not-pass

		}
	},
	build: {
		chunkSizeWarningLimit: 2000,
		rollupOptions: {
			input: {
				main: resolve(rootPath, 'index.html'),
				nested: resolve(rootPath, 'setup.html')
			}
		}
	}
});