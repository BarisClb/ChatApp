import { LanguageSwitcherProps } from "./LanguageSwitcherProps";
import { NavbarProfileProps } from "./NavbarProfileProps";

export type NavbarProps = {
	Items: NavbarItem[];
	Locale: string;
	NavbarProfileProps: NavbarProfileProps;
	LanguageSwitcherProps: LanguageSwitcherProps;
};

export interface NavbarItem {
	Text: string;
	Path: string;
}
