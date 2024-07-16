"use server";

import React from "react";
import Navbar from "./navbar";
import { headers } from "next/headers";
import { useLocale } from "next-intl";
import { NavbarProfileProps } from "@/application/models/props/NavbarProfileProps";
import { Constants } from "@/application/helpers/Constants";
import { NavbarProps } from "@/application/models/props/NavbarProps";
import { LanguageSwitcherProps } from "@/application/models/props/LanguageSwitcherProps";
import { getTranslations } from "next-intl/server";
import { getSession as _getSession } from "@/application/server/services/authService";
import { NavbarLocalizationModel } from "@/localization/models/navbarLocalizationModels";
import { LanguageSwitcherLocalizationModel } from "@/localization/models/languageSwitcherLocalizationModels";

async function NavbarWrapper() {
	const pathnameWithoutLocale = headers()?.get("x-pathname-without-locale") || "";
	const currentLanguage = useLocale();
	const session = await _getSession();
	const translations = await getTranslations();
	const navbarTranslations = translations.raw("Navbar") as NavbarLocalizationModel;
	const navbarProfileLoginItems = navbarTranslations.Profile.LoginItems;
	const navbarProfileLogoutItems = navbarTranslations.Profile.LogoutItems;
	const languageSwitcherTranslations = translations.raw(
		"LanguageSwitcher"
	) as LanguageSwitcherLocalizationModel;
	const languageList = languageSwitcherTranslations.Languages.filter((lang) => {
		return Constants.Languages.includes(lang.Code);
	});
	const navbarProfileTitle = translations("Navbar.Profile.Title", {
		username: session?.FirstName ?? "Guest",
	});

	const navbarProfileProps = {
		Title: navbarProfileTitle,
		LoginItems: navbarProfileLoginItems,
		LogoutItems: navbarProfileLogoutItems,
		Locale: currentLanguage,
		Session: session,
		LogoutButton: translations("Navbar.Profile.LogoutButton"),
	} as NavbarProfileProps;

	const languageSwitcherProps = {
		LanguageList: languageList,
		CurrentLanguage: currentLanguage,
		CurrentPathname: pathnameWithoutLocale,
	} as LanguageSwitcherProps;

	const navbarProps = {
		Items: navbarTranslations.Items,
		Locale: currentLanguage,
		NavbarProfileProps: navbarProfileProps,
		LanguageSwitcherProps: languageSwitcherProps,
	} as NavbarProps;

	return <Navbar props={navbarProps} />;
}

export default NavbarWrapper;
