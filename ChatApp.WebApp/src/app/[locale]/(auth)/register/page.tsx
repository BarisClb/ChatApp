import React from "react";
import { userClientRequestHandler } from "@/application/api/userClientRequestHandler";
import RegisterForm from "@/components/forms/register-form";
import {
	RegisterFormInput,
	RegisterFormLanguage,
} from "@/application/models/RegisterFormTypes";
import { getSession } from "next-auth/react";
import { RegisterFormProps } from "@/application/models/props/RegisterFormProps";

async function RegisterPage() {
	const session = await getSession();

	const languages = [] as RegisterFormLanguage[];

	const submitRegisterForm = async (registerForm: RegisterFormInput) => {
		return await userClientRequestHandler.RegisterUser(registerForm);
	};

	const registerFormProps = {
		submitForm: submitRegisterForm,
		languages: languages,
	} as RegisterFormProps;

	return (
		<div>
			<RegisterForm props={registerFormProps} />
		</div>
	);
}

export default RegisterPage;
