import { Constants } from "@/application/helpers/Constants";
import errorTranslations from "@/localization/static/errors.json";

export async function getTranslatedError(
	errorKey: string,
	locale: string | null = null
): Promise<string | null> {
	if (locale == null) {
		locale = await getLocaleFromUrl();
	}
	return (errorTranslations as any)[locale ?? Constants.DefaultLanguage ?? ""][errorKey];
}

export async function getLocaleFromUrl(): Promise<string> {
	let locale = Constants.DefaultLanguage;
	const pathArray = window.location.pathname.split("/");
	if (pathArray && pathArray.length > 0) {
		const pathLocale = pathArray[1];
		if (Constants.Languages.includes(pathLocale)) {
			locale = pathLocale;
		}
	}
	return locale;
}
