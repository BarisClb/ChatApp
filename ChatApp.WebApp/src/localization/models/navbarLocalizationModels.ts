export interface NavbarLocalizationModel {
	Items: NavbarLocalizationItemModel[];
	Profile: NavbarLocalizationProfileModel;
}
export interface NavbarLocalizationProfileModel {
	Title: string;
	LoginItems: NavbarLocalizationItemModel[];
	LogoutItems: NavbarLocalizationItemModel[];
}

export interface NavbarLocalizationItemModel {
	Text: string;
	Path: string;
}
