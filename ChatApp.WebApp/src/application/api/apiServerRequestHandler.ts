import { cookies, headers } from "next/headers";
import { ApiResponse, NoContent } from "../models/common/ApiResponse";
import { HttpRequest } from "../models/common/HttpRequest";
import { Constants } from "../helpers/Constants";
import { useLocale } from "next-intl";
import { getTranslations } from "next-intl/server";
import { authService as _authService } from "../services/server/authService";
import { apiRequestHelper } from "../helpers/apiRequestHelper";

async function SendApiRequest<T>(httpRequest: HttpRequest): Promise<ApiResponse<T>> {
	try {
		const language = useLocale() || Constants.defaultLanguage;
		const translations = await getTranslations("Errors");
		const errorMessage = translations("globalError");
		if (!httpRequest) {
			throw new Error((await getTranslations("Errors"))("globalError"));
		}
		let response = await SendHttpRequest<T>(httpRequest);
		if (!response) {
			console.log(response);
			throw new Error((await getTranslations("Errors"))("globalError"));
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
				return response;
			}
			return {
				isSuccess: false,
				statusCode: response.statusCode ?? 500,
				data: null,
				errors: [(await getTranslations("Errors"))("globalError")],
				tokens: { accessToken: "", refreshToken: "" },
			} as ApiResponse<T>;
		}
		return {
			isSuccess: false,
			statusCode: 500,
			data: null,
			errors: [(await getTranslations("Errors"))("globalError")],
			tokens: { accessToken: "", refreshToken: "" },
		} as ApiResponse<T>;
	} catch (error) {
		return {
			isSuccess: false,
			statusCode: 500,
			data: null,
			errors: [(await getTranslations("Errors"))("globalError")],
			tokens: { accessToken: "", refreshToken: "" },
		} as ApiResponse<T>;
	}
}

async function SendHttpRequest<T>(httpRequest: HttpRequest): Promise<ApiResponse<T>> {
	try {
		if (!httpRequest) {
			throw new Error("Bad request.");
		}
		const session = await _authService.getSession();
		let url = process.env.API_URL + httpRequest.requestUrl;
		let body = null;
		httpRequest.requestHeaders = (await apiRequestHelper.initializeHeaders(
			httpRequest.requestHeaders
		)) as [string, string][];
		if (httpRequest.requestMethod != "GET" && httpRequest.requestBody) {
			body = JSON.stringify(httpRequest.requestBody);
		}
		let cookieStore = cookies();
		let accessToken = cookieStore.get("access-token")?.value;
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
			headers: httpRequest.requestHeaders,
			method: httpRequest.requestMethod,
		}).catch((error) => {
			console.log(error);
			throw Error("Something went wrong.");
		});
		let result = response.json();
		return result;
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

export const apiServerRequestHandler = {
	SendApiRequest,
};
