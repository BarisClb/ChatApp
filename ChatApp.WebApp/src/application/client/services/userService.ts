"use client";

import { getCookies, getCookie, setCookie, deleteCookie } from "cookies-next";
import { apiRequestHandlerClient as _apiRequestHandler } from "@/application/client/common/apiRequestHandler";
import { LoginFormInput } from "@/application/models/LoginFormTypes";
import { HttpRequest } from "@/application/models/common/HttpRequest";
import { JwtPayload, SessionUser } from "@/application/models/common/Session";
import { UserTokenResponse } from "@/application/models/response/UserSessionResponse";
import { compactDecrypt } from "jose";
import jwt from "jsonwebtoken";
import crypto from "crypto";
import { Constants } from "@/application/helpers/Constants";
import { RegisterFormInput } from "@/application/models/RegisterFormTypes";

const jwtEncryptionKey = process.env.JWT_ENCRYPTION_KEY ?? "";
const jwtIssuerKey = process.env.JWT_ISSUER_KEY ?? "";

async function userRegister(registerForm: RegisterFormInput): Promise<string[] | null> {
	let request = new HttpRequest({
		RequestMethod: "POST",
		RequestUrl: "/api/user/register",
		RequestBody: registerForm,
		RequestQueryStrings: undefined,
		RequestHeaders: undefined,
	});
	let response = await _apiRequestHandler.SendApiRequest<UserTokenResponse>(request);

	if (!response.IsSuccess) {
		if (response.Errors) {
			return response.Errors;
		}
	}

	setCookie("access-token", response.Data.AccessToken, {
		sameSite: "lax",
		// domain: "barisclb.com"
	});
	setCookie("refresh-token", response.Data.RefreshToken, {
		sameSite: "lax",
		// domain: "barisclb.com"
	});

	return [];
}

async function userLogin(loginForm: LoginFormInput): Promise<string[] | null> {
	let request = new HttpRequest({
		RequestMethod: "POST",
		RequestUrl: "/api/user/logIn",
		RequestBody: loginForm,
		RequestQueryStrings: undefined,
		RequestHeaders: undefined,
	});
	let response = await _apiRequestHandler.SendApiRequest<UserTokenResponse>(request);

	if (!response.IsSuccess) {
		if (response.Errors) {
			return response.Errors;
		}
	}

	setCookie("access-token", response.Data.AccessToken, {
		sameSite: "lax",
		// domain: "barisclb.com"
	});
	setCookie("refresh-token", response.Data.RefreshToken, {
		sameSite: "lax",
		// domain: "barisclb.com"
	});

	return [];
}

async function getSession(): Promise<SessionUser | null> {
	const encryptedJwtToken = getCookie("access-token");
	if (!encryptedJwtToken) return null;

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
}

async function refreshAccessToken() {}

interface NoContent {}

function sleep(ms: any) {
	return new Promise((resolve) => setTimeout(resolve, ms));
}

export const userServiceClient = {
	userRegister,
	userLogin,
	refreshAccessToken,
};
