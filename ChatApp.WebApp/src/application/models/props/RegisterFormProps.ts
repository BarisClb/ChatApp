import {
	RegisterFormLanguage,
	RegisterFormSubmitFunction,
} from "@/application/models/RegisterFormTypes";

export type RegisterFormProps = {
	languages: RegisterFormLanguage[];
	submitForm: RegisterFormSubmitFunction;
};
