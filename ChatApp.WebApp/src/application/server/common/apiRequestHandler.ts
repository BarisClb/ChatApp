"use server";

import { ApiResponse } from "@/application/models/common/ApiResponse";
import { HttpRequest } from "@/application/models/common/HttpRequest";
import { getTranslations } from "next-intl/server";
import { getSession as _getSession } from "@/application/server/services/authService";
import { initializeHeaders as _initializeHeaders } from "../../server/helpers/apiRequestHelper";

export async function SendApiRequest<T>(httpRequest: HttpRequest): Promise<ApiResponse<T>> {
	try {
		if (!httpRequest) {
			throw new Error("Bad request.");
		}
		let httpResponse = await SendHttpRequest(httpRequest);
		if (!httpResponse) {
			console.log(httpResponse);
			throw new Error("Something went wrong.");
		}

		return (await httpResponse.json()) as ApiResponse<T>;
	} catch (error) {
		return {
			IsSuccess: false,
			StatusCode: 500,
			Data: null,
			Errors: ["Something went wrong."],//[(await getTranslations()).raw("Errors.GlobalError")],
			Tokens: { AccessToken: "", RefreshToken: "" },
		} as ApiResponse<T>;
	}
}

export async function SendHttpRequest(httpRequest: HttpRequest): Promise<Response> {
	try {
		if (!httpRequest) {
			throw new Error("Bad request.");
		}
		let url = process.env.API_URL + httpRequest.RequestUrl;
		let body =
			httpRequest.RequestMethod !== "GET" && httpRequest.RequestBody
				? JSON.stringify(httpRequest.RequestBody)
				: null;

		httpRequest.RequestHeaders = (await _initializeHeaders(httpRequest.RequestHeaders)) as [
			string,
			string
		][];

		let response = await fetch(url, {
			cache: "no-store",
			body: body,
			headers: httpRequest.RequestHeaders,
			method: httpRequest.RequestMethod,
		});
		return response;
	} catch (error) {
		console.log(error);
		throw new Error("Something went wrong.");
		// TODO: redirect to homepage
	}
}

// export const apiRequestHandlerServer = {
// 	SendApiRequest,
// };
