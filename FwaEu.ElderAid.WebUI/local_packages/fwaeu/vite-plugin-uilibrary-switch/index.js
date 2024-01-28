const glob = require('glob');
const { resolve, extname } = require('path');

module.exports = function (uiLibraryPath, fallbackUiLibrary) {

	let fallBackUiLibComponents = [];
	let uiLibComponents = [];
	let fallbackComponents = [];
	let targetLibrary = {};

	if (uiLibraryPath != fallbackUiLibrary) {

		fallBackUiLibComponents = glob.sync('**/*.*', { cwd: fallbackUiLibrary });

		uiLibComponents = glob.sync('**/*.*', { cwd: uiLibraryPath });

		fallbackComponents = fallBackUiLibComponents.filter(u => !(uiLibComponents.includes(u)));

		fallbackComponents.forEach(function (comPath) {
			targetLibrary["@UILibrary/" + comPath.replace(extname(comPath), "")] = resolve(fallbackUiLibrary, comPath);
			targetLibrary["@UILibrary/" + comPath] = resolve(fallbackUiLibrary, comPath);
		});
	}

	return targetLibrary;

}
