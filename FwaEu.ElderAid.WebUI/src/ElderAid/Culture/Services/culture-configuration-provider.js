import imageMx from "@/ElderAid/Culture/Content/Flags/mx-MX.png";
import imageEn from "@/ElderAid/Culture/Content/Flags/en-GB.png";

export default {
	getAllCultures() {
		return [
			{ code: "fr", imageSrc: imageMx },
			{ code: "en", imageSrc: imageEn }
		];
	},
	getDefaultCulture() {
		return this.getAllCultures()[0];
	}
}