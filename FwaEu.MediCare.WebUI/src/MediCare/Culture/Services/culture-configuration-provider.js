import imageFr from "@/MediCare/Culture/Content/Flags/fr-FR.png";

export default {
	getAllCultures() {
		return [
			{ code: "fr", imageSrc: imageFr}
		];
	},
	getDefaultCulture() {
		return this.getAllCultures()[0];
	}
}