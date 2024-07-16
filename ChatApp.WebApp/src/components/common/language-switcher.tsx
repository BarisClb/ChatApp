import React from "react";
import "./styles/styles.css";
import { LanguageSwitcherLanguage } from "@/application/models/props/LanguageSwitcherLanguage";
// import { Link } from "../../navigation";
import { LanguageSwitcherProps } from "@/application/models/props/LanguageSwitcherProps";

function LanguageSwitcher({ props }: { props: LanguageSwitcherProps }) {
	return (
		<div className="dropdown-center tw-w-28">
			<button
				className="btn dropdown-toggle tw-w-28 dark:tw-bg-gray-700 tw-bg-gray-300 tw-text-gray-800 dark:tw-text-gray-200"
				type="button"
				data-bs-toggle="dropdown"
				aria-expanded="false"
			>
				{props.LanguageList &&
					props.LanguageList.length > 0 &&
					(props.LanguageList?.find((lang) => lang.Code == props.CurrentLanguage)?.Name ||
						props.LanguageList[0]?.Name)}
			</button>
			<ul className="dropdown-menu custom-dropdown-menu tw-w-28 dark:tw-bg-gray-700 tw-bg-gray-300">
				{props.LanguageList?.map((lang: LanguageSwitcherLanguage, index) => (
					<li key={index}>
						<a
							key={lang.Code}
							className="dropdown-item tw-text-gray-800 dark:tw-text-gray-200 hover:tw-cursor-pointer hover:tw-text-gray-800 hover:dark:tw-text-gray-800"
							href={`/${lang.Code}${props.CurrentPathname}`}
							// href={`/`}
							// locale={`${lang.code}`}
						>
							{lang.Name}
						</a>
					</li>
				))}
			</ul>
		</div>
	);
}

export default LanguageSwitcher;
