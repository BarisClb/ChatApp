import React from "react";
import Navbar from "./navbar";
import { useLocale } from "next-intl";
import { NavbarItem } from "@/application/models/NavbarItems";
import { NavbarProfileProps } from "@/application/models/props/NavbarProfileProps";
import { Constants } from "@/application/helpers/Constants";
import { LanguageSwitcherLanguage } from "@/application/models/LanguageSwitcherLanguage";
import { NavbarProps } from "@/application/models/props/NavbarProps";
import { LanguageSwitcherProps } from "@/application/models/props/LanguageSwitcherProps";
import { getTranslations } from "next-intl/server";
import { headers } from "next/headers";
import { authService as _authService } from "@/application/services/server/authService";

async function NavbarWrapper() {
	const headersList = headers();
	const pathnameWithoutLocale = headersList.get("x-pathname-without-locale") || "";
	const currentLanguage = useLocale();
	console.log(currentLanguage);
	const session = await _authService.getSession();
	const translations = await getTranslations("Navbar");
	const navbarItems = translations.raw("items") as NavbarItem[];
	const navbarProfileLoginItems = translations.raw("profile.loginItems") as NavbarItem[];
	const navbarProfileLogoutItems = translations.raw("profile.logoutItems") as NavbarItem[];
	const languageList = (
		(await getTranslations("LanguageSwitcher")).raw("languages") as LanguageSwitcherLanguage[]
	).filter((lang) => {
		return Constants.languages.includes(lang.code);
	});
	const navbarProfileProps = {
		title: translations("profile.title", {
			username: session?.firstName ?? "GuestGuestGuestGuestGuestGuest",
		}),
		loginItems: navbarProfileLoginItems,
		logoutItems: navbarProfileLogoutItems,
		locale: currentLanguage,
		session: session,
	} as NavbarProfileProps;

	const languageSwitcherProps = {
		languageList: languageList,
		currentLanguage: currentLanguage,
		currentPathname: pathnameWithoutLocale,
	} as LanguageSwitcherProps;

	const navbarProps = {
		items: navbarItems,
		locale: currentLanguage,
		navbarProfileProps: navbarProfileProps,
		languageSwitcherProps: languageSwitcherProps,
	} as NavbarProps;

	return <Navbar props={navbarProps} />;
}

export default NavbarWrapper;
