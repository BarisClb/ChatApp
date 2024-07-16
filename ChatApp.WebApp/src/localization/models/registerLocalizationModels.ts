export interface RegisterLocalizationModel {
	Fields: RegisterLocalizationFieldModel;
	Errors: RegisterLocalizationErrorModel;
	Misc: RegisterLocalizationMiscModel;
	LanguageOptions: RegisterLocalizationLanguageModel[];
}

export interface RegisterLocalizationFieldModel {
	FirstName: string;
	LastName: string;
	Username: string;
	EmailAddress: string;
	Password: string;
	Language: string;
}

export interface RegisterLocalizationErrorModel {
	FirstNameNotEmpty: string;
	LastNameNotEmpty: string;
	UsernameNotEmpty: string;
	EmailAddressNotEmpty: string;
	PasswordNotEmpty: string;
	LanguageNotSelected: string;
	UsernameLength: string;
	PasswordLength: string;
	EmailAddressFormat: string;
	GenericError: string;
}

export interface RegisterLocalizationMiscModel {
	ShowPassword: string;
	HidePassword: string;
	RegisterButton: string;
	RegisterHeader: string;
}

export interface RegisterLocalizationLanguageModel {
	LanguageName: string;
	LanguageCode: string;
}
