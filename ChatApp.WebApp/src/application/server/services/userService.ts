"use server";

import { SendApiRequest as _sendApiRequest } from "@/application/server/common/apiRequestHandler";
import { LoginFormInput } from "@/application/models/LoginFormTypes";
import { HttpRequest } from "@/application/models/common/HttpRequest";
import { UserTokenResponse } from "@/application/models/response/UserSessionResponse";
import { cookies, headers } from "next/headers";
import { ResponseCookie } from "next/dist/compiled/@edge-runtime/cookies";
import { RegisterFormInput } from "@/application/models/RegisterFormTypes";
import { Constants } from "@/application/helpers/Constants";

const jwtEncryptionKey = process.env.JWT_ENCRYPTION_KEY ?? "";
const jwtIssuerKey = process.env.JWT_ISSUER_KEY ?? "";

async function userRegister(registerForm: RegisterFormInput): Promise<null> {
	console.log("register start");
	let request = new HttpRequest({
		RequestMethod: "POST",
		RequestUrl: "api/test/register",
		RequestBody: registerForm,
		RequestQueryStrings: undefined,
		RequestHeaders: undefined,
	});
	let response = await _sendApiRequest<NoContent>(request);
	console.log("register end");
	return null;
}

async function userLogin(loginForm: LoginFormInput): Promise<string[] | null> {
	let request = new HttpRequest({
		RequestMethod: "POST",
		RequestUrl: "/api/user/logIn",
		RequestBody: loginForm,
		RequestQueryStrings: undefined,
		RequestHeaders: undefined,
	});
	let response = await _sendApiRequest<UserTokenResponse>(request);

	if (!response.IsSuccess) {
		if (response.Errors) {
			return response.Errors;
		}
	}

	const cookieStore = cookies();

	cookieStore.set("access-token", response.Data.AccessToken, Constants.CookieOptions);
	cookieStore.set("refresh-token", response.Data.RefreshToken, Constants.CookieOptions);

	return [];
}

// async function test() {
// 	const headersList = headers();
// 	const pathnameWithoutLocale = headersList.get("x-pathname-without-locale") || "";
// 	console.log(pathnameWithoutLocale);
// 	const cookieStore = cookies();
// 	const currentLocale = cookieStore.get("NEXT_LOCALE")?.value;

// 	console.log("end");
// }

// async function refreshAccessToken() {}

interface NoContent {}

function sleep(ms: any) {
	return new Promise((resolve) => setTimeout(resolve, ms));
}
