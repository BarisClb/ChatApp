import { NavbarItem } from "../NavbarItems";
import { SessionUser } from "../common/Session";

export type NavbarProfileProps = {
	title: string;
	loginItems: NavbarItem[];
	logoutItems: NavbarItem[];
   locale: string;
	session: SessionUser
};
