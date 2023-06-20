import UserColumnsCustomizerService from "@/Fwamework/Users/Services/user-columns-customizer-service";

export default {

	configure() {

		UserColumnsCustomizerService.addUserListCustomizer(this);
		//add the column which you want to customizer here
	},

	onColumnsCreated(pageComponent, columns) {
		columns.push(
			{
				dataField: 'parts.application.email',
				allowFiltering: false,
				visibleIndex: 3,
				caption: pageComponent.$i18n.t('email')
			},
			{
				dataField: 'parts.application.firstName',
				allowFiltering: false,
				visibleIndex: 3,
				caption: pageComponent.$i18n.t('firstName')
			},
			{
				dataField: 'parts.application.lastName',
				allowFiltering: false,
				visibleIndex: 3,
				caption: pageComponent.$i18n.t('lastName')
			},
			{
				dataField: 'parts.application.login',
				caption: pageComponent.$i18n.t('login')
			}
		);
	}
}