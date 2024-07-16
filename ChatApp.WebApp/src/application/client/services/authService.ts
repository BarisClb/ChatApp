"use client";

import { JwtPayload, SessionUser } from "@/application/models/common/Session";
import { compactDecrypt } from "jose";
import { getCookie, setCookie, deleteCookie } from "cookies-next";
import jwt from "jsonwebtoken";
import crypto from "crypto";
import { LoginFormInput } from "@/application/models/LoginFormTypes";
import { UserTokenResponse } from "@/application/models/response/UserSessionResponse";
import { HttpRequest } from "@/application/models/common/HttpRequest";
import { apiRequestHandlerClient as _apiRequestHandler } from "../common/apiRequestHandler";
import { Constants } from "@/application/helpers/Constants";
import { NoContent } from "@/application/models/common/ApiResponse";
import { RegisterFormInput } from "@/application/models/RegisterFormTypes";

const jwtEncryptionKey = process.env.JWT_ENCRYPTION_KEY ?? "";
const jwtIssuerKey = process.env.JWT_ISSUER_KEY ?? "";

export async function getSession(): Promise<SessionUser | null> {
	const encryptedJwtToken = getCookie("access-token");
	if (!encryptedJwtToken) return null;

	try {
		const encryptionKey = Buffer.from(jwtEncryptionKey, "utf-8");
		const sha512 = crypto.createHash("sha512");
		sha512.update(encryptionKey);
		const result = sha512.digest();
		var decryptedToken = await compactDecrypt(encryptedJwtToken, result, {
			contentEncryptionAlgorithms: ["A256CBC-HS512"],
		});

		const issuerKey = Buffer.from(jwtIssuerKey, "utf-8");
		const sha256 = crypto.createHash("sha256");
		sha256.update(issuerKey);
		const signingKey = sha256.digest();

		const jwtString = Buffer.from(decryptedToken.plaintext).toString("utf8");

		const verifiedToken = jwt.verify(jwtString, signingKey, {
			algorithms: ["HS256"],
		}) as JwtPayload;

		return {
			UserId: verifiedToken.UserId,
			UserStatus: verifiedToken.UserStatus,
			FirstName: verifiedToken.FirstName,
			LastName: verifiedToken.LastName,
			Username: verifiedToken.Username,
			EmailAddress: verifiedToken.EmailAddress,
			UserDateCreated: verifiedToken.UserDateCreated,
			LanguageCode: verifiedToken.LanguageCode,
			IsAdmin: verifiedToken.IsAdmin,
		} as SessionUser;
	} catch (error) {
		return null;
	}
}

export async function login(loginRequest: LoginFormInput): Promise<string[]> {
	let apiRequest = new HttpRequest({
		RequestMethod: "POST",
		RequestUrl: "/api/auth/login",
		RequestBody: loginRequest,
		RequestQueryStrings: undefined,
		RequestHeaders: undefined,
	});
	let apiResponse = await _apiRequestHandler.SendApiRequest<UserTokenResponse>(apiRequest);

	if (!apiResponse.IsSuccess) {
		if (apiResponse.Errors) {
			return apiResponse.Errors;
		}
	}

	setCookie("access-token", apiResponse.Data.AccessToken, Constants.CookieOptions);
	setCookie("refresh-token", apiResponse.Data.RefreshToken, Constants.CookieOptions);

	return [];
}

export async function logout(): Promise<string[] | null> {
	const refreshToken = getCookie("refresh-token");
	let apiRequest = new HttpRequest({
		RequestMethod: "POST",
		RequestUrl: "/api/auth/logout",
		RequestBody: undefined,
		RequestQueryStrings: undefined,
		RequestHeaders: [["refresh-token", `${refreshToken}`]],
	});
	let apiResponse = await _apiRequestHandler.SendApiRequest<NoContent>(apiRequest);
	clearTokens();
	if (!apiResponse || !apiResponse.IsSuccess) {
		if (apiResponse?.Errors) {
			return apiResponse.Errors;
		}
	}

	return [];
}

export async function register(registerRequest: RegisterFormInput): Promise<string[]> {
	let apiRequest = new HttpRequest({
		RequestMethod: "POST",
		RequestUrl: "/api/auth/register",
		RequestBody: registerRequest,
		RequestQueryStrings: undefined,
		RequestHeaders: undefined,
	});
	let apiResponse = await _apiRequestHandler.SendApiRequest<UserTokenResponse>(apiRequest);

	if (!apiResponse.IsSuccess) {
		if (apiResponse.Errors) {
			return apiResponse.Errors;
		}
	}

	setCookie("access-token", apiResponse.Data.AccessToken, Constants.CookieOptions);
	setCookie("refresh-token", apiResponse.Data.RefreshToken, Constants.CookieOptions);

	return [];
}

export async function clearTokens() {
	deleteCookie("access-token");
	deleteCookie("refresh-token");
}

export const authServiceClient = {
	getSession,
	login,
	logout,
	register,
};
