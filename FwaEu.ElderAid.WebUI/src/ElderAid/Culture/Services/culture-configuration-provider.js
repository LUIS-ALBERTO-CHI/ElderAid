import imageFr from "@/ElderAid/Culture/Content/Flags/fr-FR.png";
import imageEn from "@/ElderAid/Culture/Content/Flags/en-GB.png";

export default {
	getAllCultures() {
		return [
			{ code: "fr", imageSrc: imageFr},
			{ code: "en", imageSrc: imageEn }
		];
	},
	getDefaultCulture() {
		return this.getAllCultures()[0];
	}
}