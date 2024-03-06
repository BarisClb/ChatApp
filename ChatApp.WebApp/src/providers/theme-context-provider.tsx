"use client";
import { ThemeProvider } from "next-themes";
import React, { useEffect, useState } from "react";

function ThemeContextProvider({ children }: { children: React.ReactNode }) {
	const [mounted, setMounted] = useState(false);

	useEffect(() => {
		setMounted(true);
	}, []);
	if (!mounted) {
		return null;
	}

	return (
		<ThemeProvider enableSystem={true} attribute="class">
			{children}
		</ThemeProvider>
	);
}

export default ThemeContextProvider;
