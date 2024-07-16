"use server";

import React from "react";
import { userServiceClient as _userService } from "@/application/client/services/userService";
import RegisterForm from "@/components/forms/register-form";
import { getLocale, getTranslations } from "next-intl/server";
import { RegisterLocalizationModel } from "@/localization/models/registerLocalizationModels";
import { getSession as _getSession } from "@/application/server/services/authService";
import { RegisterFormProps } from "@/application/models/props/RegisterFormProps";

async function RegisterPage({
	searchParams,
}: {
	searchParams: { [key: string]: string | string[] | undefined };
}) {
	const session = await _getSession();
	const translations = await getTranslations();
	const localization = translations.raw("Register") as RegisterLocalizationModel;
	const refererUrl = (searchParams?.referer as string) || null;
	const registerFormProps = {
		Languages: localization.LanguageOptions,
		Localization: localization,
		RefererUrl: refererUrl,
	} as RegisterFormProps;

	return session == null ? (
		<div className="tw-flex tw-items-center tw-justify-center tw-pt-14">
			<div className="tw-w-full tw-max-w-md tw-bg-white tw-p-8 tw-rounded-lg tw-shadow-md dark:tw-bg-gray-800 dark:tw-text-white">
				<h1 className="tw-text-2xl tw-font-semibold tw-mb-6 tw-text-center dark:tw-text-white">
					{localization.Misc.RegisterHeader}
				</h1>
				<RegisterForm props={registerFormProps} />
			</div>
		</div>
	) : (
		<div className="tw-flex tw-items-center tw-justify-center tw-pt-14">
			<div className="tw-w-full tw-max-w-md tw-bg-white tw-p-8 tw-rounded-lg tw-shadow-md dark:tw-bg-gray-800 dark:tw-text-white">
				<div className="tw-text-center">You are already logged in, {session.Username}</div>
			</div>
		</div>
	);
}

export default RegisterPage;
