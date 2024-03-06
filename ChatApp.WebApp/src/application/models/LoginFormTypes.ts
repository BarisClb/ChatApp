export type LoginFormInput = {
	userField: string;
	password: string;
}

export type LoginFormSubmitFunction = (formData: LoginFormInput) => string[];
