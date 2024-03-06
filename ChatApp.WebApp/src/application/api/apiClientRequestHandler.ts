"use client"

import { getCookies, getCookie, setCookie, deleteCookie } from "cookies-next";
import { ApiResponse } from "../models/common/ApiResponse";
import { HttpRequest } from "../models/common/HttpRequest";
import { apiRequestHelper } from "../helpers/apiRequestHelper";

async function SendApiRequest<T>(httpRequest: HttpRequest): Promise<ApiResponse<T>> {
	try {
		if (!httpRequest) {
			throw new Error("Bad request.");
		}
		let response = await SendHttpRequest<T>(httpRequest);
		if (!response) {
			console.log(response);
			throw new Error("Something went wrong.");
		}
		if (response.isSuccess) {
			return response;
		} else if (response.statusCode === 401) {
			let refreshTokenResponse = await RefreshToken();
			if (refreshTokenResponse !== null) {
				await HandleSuccessRefreshTokenResponse();
			} else {
				await HandleUnauthorizedResponse();
			}
		} else if (!response.isSuccess) {
			if (response.errors) {
				throw new Error(response.errors[0]);
			}
			throw new Error("Something went wrong.");
		}
		throw new Error("Something went wrong.");
	} catch (error) {
		console.log(error);
		throw new Error("Something went wrong.");
		// TODO: redirect to homepage
	}
}

async function SendHttpRequest<T>(httpRequest: HttpRequest): Promise<ApiResponse<T>> {
	try {
		if (!httpRequest) {
			throw new Error("Bad request.");
		}
		console.log(process.env);
		let url = process.env.API_URL + httpRequest.requestUrl;
		let body = null;
		httpRequest.requestHeaders = await apiRequestHelper.initializeHeaders(httpRequest.requestHeaders) as [string, string][];
		if (httpRequest.requestMethod != "GET" && httpRequest.requestBody) {
			body = JSON.stringify(httpRequest.requestBody);
		}
		let accessToken = getCookie("access-token");
		console.log(accessToken);
		if (accessToken && accessToken !== "") {
			if (httpRequest.requestHeaders) {
				httpRequest.requestHeaders.push(["Authorization", `Bearer ${accessToken}`]);
			} else {
				httpRequest.requestHeaders = [["Authorization", `Bearer ${accessToken}`]];
			}
		}
		let response = await fetch(url, {
			cache: "no-store",
			body: body,
			headers: httpRequest.requestHeaders == undefined ? [] : httpRequest.requestHeaders,
			method: httpRequest.requestMethod,
		}).catch((error) => {
			console.log(error);
			throw Error("Something went wrong.");
		});
		return await response.json();
	} catch (error) {
		console.log(error);
		throw new Error("Something went wrong.");
		// TODO: redirect to homepage
	}
}

async function RefreshToken(): Promise<string | null> {
	return null;
}

async function HandleUnauthorizedResponse() {
	// clear access-token cookie
	// clear refresh-token cookie
	// clear next-auth currentuser
}

async function HandleSuccessRefreshTokenResponse() {
	// set access-token cookie
	// set next-auth currentuser
}

export const apiClientRequestHandler = { SendApiRequest };
