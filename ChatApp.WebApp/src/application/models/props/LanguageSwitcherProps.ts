import { LanguageSwitcherLanguage } from "../LanguageSwitcherLanguage";

export type LanguageSwitcherProps = {
	languageList: LanguageSwitcherLanguage[];
	currentLanguage: string;
   currentPathname: string;
};
