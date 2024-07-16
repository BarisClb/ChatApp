export type LoginFormInput = {
	UserField: string;
	Password: string;
}

export type LoginFormSubmitFunction = (formData: LoginFormInput) => string[];
