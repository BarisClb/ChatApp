"use server";

import React from "react";
import LoginForm from "@/components/forms/login-form";
import { getSession } from "@/application/server/services/authService";
import { getLocale, getTranslations } from "next-intl/server";
import { LoginLocalizationModel } from "@/localization/models/loginLocalizationModels";
import { LoginFormProps } from "@/application/models/props/LoginFormProps";

async function Login({
	searchParams,
}: {
	searchParams: { [key: string]: string | string[] | undefined };
}) {
	const session = await getSession();
	const locale = await getLocale();
	const localization = (await getTranslations()).raw("Login") as LoginLocalizationModel;
	const refererUrl = (searchParams?.referer as string) || null;
	const loginFormProps = {
		Localization: localization,
		RefererUrl: refererUrl,
		Locale: locale,
	} as LoginFormProps;

	return session == null ? (
		<div className="tw-flex tw-items-center tw-justify-center tw-pt-14">
			<div className="tw-w-full tw-max-w-md tw-bg-white tw-p-8 tw-rounded-lg tw-shadow-md dark:tw-bg-gray-800 dark:tw-text-white">
				<h1 className="tw-text-2xl tw-font-semibold tw-mb-6 tw-text-center dark:tw-text-white">
					{localization.Misc.LoginHeader}
				</h1>
				<LoginForm props={loginFormProps} />
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

export default Login;
