import "@/app/globals.css";
import type { Metadata } from "next";
import { Inter } from "next/font/google";
// import ReduxContextProvider from "@/providers/redux-context-provider";
// import LoadingContextProvider from "@/providers/loading-context-provider";
import ThemeContextProvider from "@/providers/theme-context-provider";
import NavbarWrapper from "@/components/navbar/navbar-wrapper";
import { NextIntlClientProvider, useMessages } from "next-intl";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
	title: "ChatApp",
	description: "This is a ChatApp Project by BarisClb.com",
};

function RootLayout({
	children,
	params: { locale },
}: {
	children: React.ReactNode;
	params: { locale: string };
}) {
	const messages = useMessages();
	return (
		<html lang={locale}>
			<body
				className={`${inter.className} tw-bg-neutral-200 dark:tw-bg-gray-900 tw-color tw-text-black dark:tw-text-white`}
			>
				{/* <ReduxContextProvider> */}
				<NextIntlClientProvider messages={messages}>
					{/* <LoadingContextProvider> */}
					<ThemeContextProvider>
						<NavbarWrapper />
						{children}
					</ThemeContextProvider>
					{/* </LoadingContextProvider> */}
				</NextIntlClientProvider>
				{/* </ReduxContextProvider> */}
			</body>
		</html>
	);
}

export default RootLayout;
