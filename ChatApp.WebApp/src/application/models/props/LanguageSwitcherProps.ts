import { LanguageSwitcherLanguage } from "@/application/models/props/LanguageSwitcherLanguage";

export type LanguageSwitcherProps = {
	LanguageList: LanguageSwitcherLanguage[];
	CurrentLanguage: string;
   CurrentPathname: string;
};
