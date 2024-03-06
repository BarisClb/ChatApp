import React from "react";
import { NavbarProfileProps } from "@/application/models/props/NavbarProfileProps";

function NavbarProfile({ props }: { props: NavbarProfileProps }) {
	const width = "tw-w-48";
	return (
		<div className={`dropdown-center ${width}`}>
			<button
				className={`btn dropdown-toggle tw-w-28 dark:tw-bg-neutral-700 tw-bg-neutral-300 tw-text-neutral-800 dark:tw-text-neutral-200 tw-truncate ${width}`}
				type="button"
				data-bs-toggle="dropdown"
				aria-expanded="false"
			>
				{props.title}
			</button>
			<ul
				className={`dropdown-menu custom-dropdown-menu dark:tw-bg-neutral-700 tw-bg-neutral-300 ${width}`}
			>
				{props.session == null
					? props.logoutItems?.map((navbarProfileItem, index) => (
							<li key={index} className="nav-item tw-text-center">
								<a
									className="nav-link tw-text-neutral-800 dark:tw-text-neutral-200 tw-pt-2 tw-pb-2"
									href={`/${props.locale}${navbarProfileItem.path}`}
								>
									{navbarProfileItem.text}
								</a>
							</li>
					  ))
					: props.loginItems?.map((navbarProfileItem, index) => (
							<li key={index} className="nav-item tw-text-center">
								<a
									className="nav-link tw-text-neutral-800 dark:tw-text-neutral-200 tw-pt-2 tw-pb-2"
									href={`/${props.locale}${navbarProfileItem.path}`}
								>
									{navbarProfileItem.text}
								</a>
							</li>
					  ))}
			</ul>
		</div>
	);
}

export default NavbarProfile;
