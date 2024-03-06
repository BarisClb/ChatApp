import { getRequestConfig } from "next-intl/server";
import { Constants } from "@/application/helpers/Constants";
import deepmerge from "deepmerge";

const locales = Constants.languages;

export default getRequestConfig(async ({ locale }) => {
	if (!locales.includes(locale as any)) {
		locale = Constants.defaultLanguage;
	}

	var localeMessages = {
		messages: {
			...(await import(`@/localization/${locale}/home.json`)).default,
			...(await import(`@/localization/${locale}/register.json`)).default,
			...(await import(`@/localization/${locale}/login.json`)).default,
			...(await import(`@/localization/${locale}/language-switcher.json`)).default,
			...(await import(`@/localization/${locale}/navbar.json`)).default,
			...(await import(`@/localization/${locale}/errors.json`)).default,
		},
	};

	var defaultLanguageMessages = {
		messages: {
			...(await import(`@/localization/${Constants.defaultLanguage}/home.json`)).default,
			...(await import(`@/localization/${Constants.defaultLanguage}/register.json`)).default,
			...(await import(`@/localization/${Constants.defaultLanguage}/login.json`)).default,
			...(await import(`@/localization/${Constants.defaultLanguage}/language-switcher.json`))
				.default,
			...(await import(`@/localization/${Constants.defaultLanguage}/navbar.json`)).default,
			...(await import(`@/localization/${Constants.defaultLanguage}/errors.json`)).default,
		},
	};

	return deepmerge(defaultLanguageMessages, localeMessages, { arrayMerge: ignoreArrays });
});

function ignoreArrays(target: any, source: any, options?: deepmerge.Options): any {
	if (Array.isArray(target) && Array.isArray(source)) {
		return source;
	} else {
		return deepmerge(target, source, options);
	}
}
