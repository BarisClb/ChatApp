"use client";

import { Constants } from "@/application/helpers/Constants";
import { getCookie } from "cookies-next";
import { getLocaleFromUrl } from "@/application/client/helpers/localizationHelper";

async function initializeHeaders(requestHeaders: [string, string][]): Promise<[string, string][]> {
	let initRequestHeaders: HeadersInit = [];
	const accessToken = getCookie("access-token");
	const urlLocale = await getLocaleFromUrl();
	initRequestHeaders.push(["Accept", "application/json"]);
	initRequestHeaders.push(["Content-type", "application/json"]);
	if (accessToken) {
		initRequestHeaders.push(["Authorization", `Bearer ${accessToken}`]);
	}
	initRequestHeaders.push(["Accept-Language", `${urlLocale}`]);
	if (requestHeaders && requestHeaders.length > 0) {
		for (let i = 0; i < requestHeaders.length; i++) {
			initRequestHeaders.push(requestHeaders[i]);
		}
	}
	return initRequestHeaders;
}

export const apiRequestHelperClient = {
	initializeHeaders,
};
