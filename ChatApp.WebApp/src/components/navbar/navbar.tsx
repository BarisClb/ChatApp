"use client";

import React, { useEffect } from "react";
import ThemeSwitcher from "../common/theme-switcher";
import NavbarProfile from "./navbar-profile";
import LanguageSwitcher from "../common/language-switcher";
import { NavbarProps } from "@/application/models/props/NavbarProps";

function Navbar({ props }: { props: NavbarProps }) {
	useEffect(() => {
		if (typeof window !== "undefined") {
			require("bootstrap/dist/js/bootstrap.bundle.min.js");
		}
	}, []);

	return (
		<nav className="navbar navbar-expand-xl tw-bg-opacity-0 dark:tw-bg-opacity-0 tw-px-10 tw-border-b-3 tw-border-t-0 tw-border-l-0 tw-border-r-0 tw-border-solid tw-border-b-red-650 tw-text-neutral-800 dark:tw-text-neutral-200">
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
				<div className="collapse navbar-collapse" id="navbarNav">
					<ul className="navbar-nav tw-justify-evenly tw-flex-grow tw-flex tw-flex-row">
						{props.Items?.map((navbarItem, index) => (
							<li key={index} className="nav-item">
								<a
									className="nav-link tw-text-neutral-800 dark:tw-text-neutral-200"
									href={`${navbarItem?.Path ?? ""}`.replace("/", "")}
								>
									{navbarItem?.Text}
								</a>
							</li>
						))}
					</ul>
				</div>
				<div className="tw-flex tw-flex-col md:tw-flex-row tw-justify-evenly tw-gap-6 tw-items-center mt-2 xl:mt-0">
					<ThemeSwitcher />
					<LanguageSwitcher props={props.LanguageSwitcherProps} />
					<NavbarProfile props={props.NavbarProfileProps} />
				</div>
			</div>
		</nav>
	);
}

export default Navbar;
