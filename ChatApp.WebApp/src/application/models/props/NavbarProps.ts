import { NavbarItem } from "../NavbarItems";
import { LanguageSwitcherProps } from "./LanguageSwitcherProps";
import { NavbarProfileProps } from "./NavbarProfileProps";

export type NavbarProps = {
	items: NavbarItem[];
	locale: string;
	navbarProfileProps: NavbarProfileProps;
	languageSwitcherProps: LanguageSwitcherProps;
};
