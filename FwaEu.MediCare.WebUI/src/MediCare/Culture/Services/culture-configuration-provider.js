import imageFr from "@/MediCare/Culture/Content/Flags/fr-FR.png";
import imageEn from "@/MediCare/Culture/Content/Flags/en-GB.png";

export default {
	getAllCultures() {
		return [
			{ code: "fr", imageSrc: imageFr},
		];
	},
	getDefaultCulture() {
		return this.getAllCultures()[0];
	}
}