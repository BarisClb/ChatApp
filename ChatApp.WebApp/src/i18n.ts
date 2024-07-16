import { getRequestConfig } from "next-intl/server";
import { Constants } from "@/application/helpers/Constants";
import deepmerge from "deepmerge";

const locales = Constants.Languages;

export default getRequestConfig(async ({ locale }) => {
	if (!locales.includes(locale as any)) {
		locale = Constants.DefaultLanguage;
	}

	var localeMessages = {
		messages: {
			...(await import(`@/localization/${locale}/home.json`)).default,
			...(await import(`@/localization/${locale}/register.json`)).default,
			...(await import(`@/localization/${locale}/login.json`)).default,
			...(await import(`@/localization/${locale}/languageSwitcher.json`)).default,
			...(await import(`@/localization/${locale}/navbar.json`)),
			...(await import(`@/localization/${locale}/errors.json`)).default,
		},
	};
	var defaultLanguageMessages = {
		messages: {
			...(await import(`@/localization/${Constants.DefaultLanguage}/home.json`)).default,
			...(await import(`@/localization/${Constants.DefaultLanguage}/register.json`)).default,
			...(await import(`@/localization/${Constants.DefaultLanguage}/login.json`)).default,
			...(await import(`@/localization/${Constants.DefaultLanguage}/languageSwitcher.json`))
				.default,
			...(await import(`@/localization/${Constants.DefaultLanguage}/navbar.json`)),
			...(await import(`@/localization/${Constants.DefaultLanguage}/errors.json`)).default,
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
