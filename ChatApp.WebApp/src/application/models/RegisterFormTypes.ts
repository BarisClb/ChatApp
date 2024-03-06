export type RegisterFormInput = {
	FirstName: string;
	LastName: string;
	EmailAddress: string;
	Username: string;
	Password: string;
	LanguageCode: string;
}

export type RegisterFormLanguage = {
	code: string;
	name: string;
}

export type RegisterFormSubmitFunction = (formData: RegisterFormInput) => string[];
