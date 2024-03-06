"use client";
import "./styles/styles.css";
import React, { useEffect, useState } from "react";
import { useTheme } from "next-themes";

function ThemeSwitcher() {
	const { systemTheme, theme, setTheme } = useTheme();
	const [mounted, setMounted] = useState(false);
	const currentTheme = theme === "system" ? systemTheme : theme;
	const setDarkMode = () => {
		if (currentTheme == "dark") {
			setTheme("light");
		} else {
			setTheme("dark");
		}
	};

	useEffect(() => {
		setMounted(true);
	}, []);

	if (!mounted) return null;
	return (
		<div className="tw-flex tw-justify-center">
			<div
				className={currentTheme == "dark" ? "night" : "night day"}
				onClick={() => setDarkMode()}
			>
				<div className={currentTheme == "dark" ? "moon" : "moon sun"} />
			</div>
		</div>
	);
}

export default ThemeSwitcher;
