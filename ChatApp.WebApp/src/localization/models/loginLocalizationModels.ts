export interface LoginLocalizationModel {
	Fields: LoginLocalizationFieldModel;
	Misc: LoginLocalizationMiscModel;
	Errors: LoginLocalizationErrorModel;
}

export interface LoginLocalizationFieldModel {
	UserField: string;
	Password: string;
}

export interface LoginLocalizationMiscModel {
	ShowPassword: string;
	HidePassword: string;
	LoginButton: string;
	LoginHeader: string;
}

export interface LoginLocalizationErrorModel {
	LoginFieldNotEmpty: string;
	PasswordNotEmpty: string;
	GenericError: string;
}
