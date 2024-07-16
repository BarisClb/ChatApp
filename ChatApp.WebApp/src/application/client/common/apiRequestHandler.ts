"use client";

import { ApiResponse } from "@/application/models/common/ApiResponse";
import { HttpRequest } from "@/application/models/common/HttpRequest";
import { apiRequestHelperClient as _apiRequestHelper } from "@/application/client/helpers/apiRequestHelper";

async function SendApiRequest<T>(
	httpRequest: HttpRequest,
	tryRefreshToken: boolean = false
): Promise<ApiResponse<T>> {
	try {
		if (!httpRequest) {
			throw new Error("Bad request.");
		}

		let httpResponse = await SendHttpRequest(httpRequest);
		if (!httpResponse) {
			throw new Error("Something went wrong.");
		}

		let response = (await httpResponse.json()) as ApiResponse<T>;

		if (response?.IsSuccess) {
			return response;
		} else if (response?.StatusCode === 401) {
			if (tryRefreshToken) {
				let refreshTokenResponse = await RefreshToken();
				if (refreshTokenResponse !== null) {
					await HandleSuccessRefreshTokenResponse();
				} else {
					await HandleUnauthorizedResponse();
				}
			}
			await HandleUnauthorizedResponse();
		} else if (response?.Errors) {
			return response;
		}
		throw new Error("Something went wrong.");
	} catch (error) {
		console.log(error);
		throw new Error("Something went wrong.");
		// TODO: redirect to homepage
	}
}

async function SendHttpRequest(httpRequest: HttpRequest): Promise<Response> {
	try {
		if (!httpRequest) {
			throw new Error("Bad request.");
		}

		let url = httpRequest.RequestUrl;
		const body =
			httpRequest.RequestMethod !== "GET" && httpRequest.RequestBody
				? JSON.stringify(httpRequest.RequestBody)
				: null;

		httpRequest.RequestHeaders = (await _apiRequestHelper.initializeHeaders(
			httpRequest.RequestHeaders
		)) as [string, string][];

		let response = await fetch(url, {
			cache: "no-store",
			body: body,
			headers: httpRequest.RequestHeaders || [],
			method: httpRequest.RequestMethod,
		});
		console.log(response);
		return response;
	} catch (error) {
		console.log(error);
		throw new Error("Something went wrong.");
	}
}

async function RefreshToken(): Promise<string | null> {
	return null;
}

async function HandleUnauthorizedResponse() {
	// clear access-token cookie
	// clear refresh-token cookie
}

async function HandleSuccessRefreshTokenResponse() {
	// set access-token cookie
}

export const apiRequestHandlerClient = { SendApiRequest, SendHttpRequest };
