"use client";

import React from "react";
import { NavbarProfileProps } from "@/application/models/props/NavbarProfileProps";
import { authServiceClient } from "@/application/client/services/authService";

function NavbarProfile({ props }: { props: NavbarProfileProps }) {
	const width = "tw-w-full lg:tw-w-48";
	const handleLogout = async () => {
		var response = await authServiceClient.logout();
		if (response && response.length === 0) {
			window.location.reload();
		}
	};
	return (
		<div className={`dropdown-center ${width}`}>
			<button
				className={`btn dropdown-toggle tw-w-28 dark:tw-bg-gray-700 tw-bg-gray-300 tw-text-gray-800 dark:tw-text-gray-200 tw-truncate ${width}`}
				type="button"
				data-bs-toggle="dropdown"
				aria-expanded="false"
			>
				{props.Title}
			</button>
			<ul
				className={`dropdown-menu custom-dropdown-menu dark:tw-bg-gray-700 tw-bg-gray-300 ${width}`}
			>
				{props.Session == null ? (
					props.LogoutItems?.map((navbarProfileItem, index) => (
						<li key={index} className="nav-item tw-text-center">
							<a
								className="nav-link dropdown-item tw-text-gray-800 dark:tw-text-gray-200 tw-pt-2 tw-pb-2 hover:tw-cursor-pointer hover:tw-text-gray-800 hover:dark:tw-text-gray-800"
								href={`/${props.Locale}${navbarProfileItem.Path}`}
							>
								{navbarProfileItem.Text}
							</a>
						</li>
					))
				) : (
					<>
						{props.LoginItems?.map((navbarProfileItem, index) => (
							<li key={index} className="nav-item tw-text-center">
								<a
									className="nav-link dropdown-item tw-text-gray-800 dark:tw-text-gray-200 tw-pt-2 tw-pb-2 hover:tw-cursor-pointer hover:tw-text-gray-800 hover:dark:tw-text-gray-800"
									href={`/${props.Locale}${navbarProfileItem.Path}`}
								>
									{navbarProfileItem.Text}
								</a>
							</li>
						))}
						<li className="nav-item tw-text-center">
							<a
								className="nav-link dropdown-item tw-text-gray-800 dark:tw-text-gray-200 tw-pt-2 tw-pb-2 hover:tw-cursor-pointer hover:tw-text-gray-800 hover:dark:tw-text-gray-800"
								onClick={handleLogout}
							>
								{props.LogoutButton}
							</a>
						</li>
					</>
				)}
			</ul>
		</div>
	);
}

export default NavbarProfile;
