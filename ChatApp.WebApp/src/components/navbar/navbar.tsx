"use client";
import React, { useEffect } from "react";
import ThemeSwitcher from "../common/theme-switcher";
// import { useAppSelector } from "@/store";
import NavbarProfile from "./navbar-profile";
import LanguageSwitcher from "../common/language-switcher";
import { NavbarProps } from "@/application/models/props/NavbarProps";
import { userServiceClient as _userService } from "@/application/services/client/userService";

function Navbar({ props }: { props: NavbarProps }) {
	// var user = useAppSelector((state) => state.user);
	useEffect(() => {
		if (typeof window !== "undefined") {
			require("bootstrap/dist/js/bootstrap.bundle.min.js");
		}
	}, []);
	return (
		<nav className="navbar navbar-expand-lg tw-bg-opacity-0 dark:tw-bg-opacity-0 tw-px-10 tw-border-b-3 tw-border-t-0 tw-border-l-0 tw-border-r-0 tw-border-solid tw-border-b-red-650 tw-text-neutral-800 dark:tw-text-neutral-200">
			<div className="container-fluid">
				<a className="navbar-brand tw-text-neutral-800 dark:tw-text-neutral-200" href="/">
					ChatApp
				</a>

				<button
					className="navbar-toggler custom-navbar-toggler"
					type="button"
					data-bs-toggle="collapse"
					data-bs-target="#navbarNav"
					aria-controls="navbarNav"
					aria-expanded="false"
					aria-label="Toggle navigation"
				>
					<span className="navbar-toggler-icon" />
				</button>
				<div className="collapse navbar-collapse tw-flex-grow-[7]" id="navbarNav">
					<ul className="navbar-nav tw-justify-evenly tw-flex-grow">
						{props.items?.map((navbarItem, index) => (
							<li key={index} className="nav-item">
								<a
									className="nav-link tw-text-neutral-800 dark:tw-text-neutral-200"
									// href={`${props.locale}${navbarItem.path}`}
									href={`${navbarItem?.path ?? ""}`.replace("/", "")}
								>
									{navbarItem?.text}
								</a>
							</li>
						))}
					</ul>
				</div>
				<div className="tw-flex tw-flex-row tw-justify-evenly tw-gap-6 tw-items-center">
					<ThemeSwitcher />
					<LanguageSwitcher props={props.languageSwitcherProps} />
					<NavbarProfile props={props.navbarProfileProps} />
				</div>
			</div>
		</nav>
	);
}

export default Navbar;
