import React from "react";
import "./styles/styles.css";
import { LanguageSwitcherLanguage } from "@/application/models/LanguageSwitcherLanguage";
// import { Link } from "../../navigation";
import { LanguageSwitcherProps } from "@/application/models/props/LanguageSwitcherProps";

function LanguageSwitcher({ props }: { props: LanguageSwitcherProps }) {
	return (
		<div className="dropdown-center tw-w-28">
			<button
				className="btn dropdown-toggle tw-w-28 dark:tw-bg-neutral-700 tw-bg-neutral-300 tw-text-neutral-800 dark:tw-text-neutral-200"
				type="button"
				data-bs-toggle="dropdown"
				aria-expanded="false"
			>
				{props.languageList &&
					props.languageList.length > 0 &&
					(props.languageList?.find((lang) => lang.code == props.currentLanguage)?.name ||
						props.languageList[0]?.name)}
			</button>
			<ul className="dropdown-menu custom-dropdown-menu tw-w-28 dark:tw-bg-neutral-700 tw-bg-neutral-300">
				{props.languageList?.map((lang: LanguageSwitcherLanguage, index) => (
					<li key={index}>
						<a
							key={lang.code}
							className="dropdown-item tw-text-neutral-800 dark:tw-text-neutral-200 hover:tw-cursor-pointer hover:tw-text-neutral-800 hover:dark:tw-text-neutral-800"
							href={`/${lang.code}${props.currentPathname}`}
							// href={`/`}
							// locale={`${lang.code}`}
						>
							{lang.name}
						</a>
					</li>
				))}
			</ul>
		</div>
	);
}

export default LanguageSwitcher;
