import { NavbarItem } from "@/application/models/props/NavbarProps";
import { SessionUser } from "../common/Session";

export type NavbarProfileProps = {
	Title: string;
	LoginItems: NavbarItem[];
	LogoutItems: NavbarItem[];
   Locale: string;
	Session: SessionUser;
	LogoutButton: string;
};
