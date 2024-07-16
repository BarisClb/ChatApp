"use server";

import { Constants } from "@/application/helpers/Constants";
import { cookies, headers } from "next/headers";

export async function initializeHeaders(
	requestHeaders: [string, string][]
): Promise<[string, string][]> {
	const headersMap = new Map<string, string>(requestHeaders);

	if (!headersMap.has("Accept")) {
		headersMap.set("Accept", "application/json");
	}
	if (!headersMap.has("Content-type")) {
		headersMap.set("Content-type", "application/json");
	}
	if (!headersMap.has("Authorization")) {
		const cookieStore = cookies();
		const accessToken = cookieStore.get("access-token");
		if (accessToken) {
			headersMap.set("Authorization", `Bearer ${accessToken}`);
		}
	}
	if (!headersMap.has("Accept-Language")) {
		const headerStore = headers();
		let headerLocale = headerStore.get("x-locale");
		if (headerLocale == null || !Constants.Languages.includes(headerLocale ?? "")) {
			headerLocale = Constants.DefaultLanguage;
		}
		headersMap.set("Accept-Language", headerLocale);
	}

	return Array.from(headersMap.entries());
}

// export async function initializeHeaders(requestHeaders: [string, string][]): Promise<[string, string][]> {
// 	let initRequestHeaders: HeadersInit = [];
// 	const cookieStore = cookies();
// 	const headerStore = headers();
// 	const accessToken = cookieStore.get("access-token");
// 	let headerLocale = headerStore.get("x-locale");
// 	if (!Constants.languages.includes(headerLocale ?? "")) {
// 		headerLocale = Constants.defaultLanguage;
// 	}
// 	initRequestHeaders.push(["Accept", "application/json"]);
// 	initRequestHeaders.push(["Content-type", "application/json"]);
// 	if (accessToken) {
// 		initRequestHeaders.push(["Authorization", `Bearer ${accessToken}`]);
// 	}
// 	initRequestHeaders.push(["Accept-Language", `${headerLocale}`]);
// 	if (requestHeaders && requestHeaders.length > 0) {
// 		for (let i = 0; i < requestHeaders.length; i++) {
// 			initRequestHeaders.push(requestHeaders[i]);
// 		}
// 	}
// 	return initRequestHeaders;
// }

// export const apiRequestHelperServer = {
// 	initializeHeaders,
// };
