"use server";

import {
	SendApiRequest,
	SendApiRequest as _sendApiRequest,
} from "@/application/server/common/apiRequestHandler";
import { LoginFormInput } from "@/application/models/LoginFormTypes";
import { HttpRequest } from "@/application/models/common/HttpRequest";
import {
	JwtPayload,
	Session,
	SessionTokens,
	SessionUser,
} from "@/application/models/common/Session";
import { UserSessionResponse } from "@/application/models/response/UserSessionResponse";
import { compactDecrypt, SignJWT, jwtVerify, importJWK, jwtDecrypt, KeyLike } from "jose";
import { cookies } from "next/headers";
import { NextRequest, NextResponse } from "next/server";
import { getTranslations } from "next-intl/server";
import jwt from "jsonwebtoken";
// import crypto from "crypto";
import { ResponseCookie } from "next/dist/compiled/@edge-runtime/cookies";
import { Constants } from "@/application/helpers/Constants";

const jwtEncryptionKey = process.env.JWT_ENCRYPTION_KEY ?? "";
const jwtIssuerKey = process.env.JWT_ISSUER_KEY ?? "";

export async function login(loginForm: LoginFormInput): Promise<string[] | null> {
	let request = new HttpRequest({
		RequestMethod: "POST",
		RequestUrl: "api/user/logInWithSession",
		RequestBody: loginForm,
		RequestQueryStrings: undefined,
		RequestHeaders: undefined,
	});
	let response = await _sendApiRequest<UserSessionResponse>(request);

	if (!response.IsSuccess) {
		if (response.Errors) {
			return response.Errors;
		}
	}

	const sessionUser = {
		UserId: response.Data.UserId,
		UserStatus: response.Data.UserStatus,
		FirstName: response.Data.FirstName,
		LastName: response.Data.LastName,
		Username: response.Data.Username,
		EmailAddress: response.Data.EmailAddress,
		UserDateCreated: response.Data.UserDateCreated,
		LanguageCode: response.Data.LanguageCode,
		IsAdmin: response.Data.IsAdmin,
	} as SessionUser;

	const sessionTokens = {
		AccessToken: response.Data.AccessToken,
		RefreshToken: response.Data.RefreshToken,
	} as SessionTokens;

	const session = {
		User: sessionUser,
		Tokens: sessionTokens,
	} as Session;

	cookies().set("access-token", response.Data.AccessToken, Constants.CookieOptions);
	cookies().set("refresh-token", response.Data.RefreshToken, Constants.CookieOptions);

	return [];
}

export async function clearTokens(response: Response) {
	response.headers.append(
		"Set-Cookie",
		"access-token=; Path=/; Expires=Thu, 01 Jan 1970 00:00:00 GMT;"
	);
	response.headers.append(
		"Set-Cookie",
		"refresh-token=; Path=/; Expires=Thu, 01 Jan 1970 00:00:00 GMT;"
	);
}

// export async function getSession(): Promise<SessionUser | null> {
// 	const encryptedJwtToken = cookies().get("access-token")?.value;
// 	if (!encryptedJwtToken) return null;
// 	try {
// 		const encryptionKey = Buffer.from(jwtEncryptionKey, "utf-8");
// 		const sha512 = crypto.createHash("sha512");
// 		sha512.update(encryptionKey);
// 		const result = sha512.digest();
// 		var decryptedToken = await compactDecrypt(encryptedJwtToken, result, {
// 			contentEncryptionAlgorithms: ["A256CBC-HS512"],
// 		});

// 		const issuerKey = Buffer.from(jwtIssuerKey, "utf-8");
// 		const sha256 = crypto.createHash("sha256");
// 		sha256.update(issuerKey);
// 		const signingKey = sha256.digest();

// 		const jwtString = Buffer.from(decryptedToken.plaintext).toString("utf8");

// 		const verifiedToken = jwt.verify(jwtString, signingKey, {
// 			algorithms: ["HS256"],
// 		}) as JwtPayload;

// 		return {
// 			userId: verifiedToken.userId,
// 			userStatus: verifiedToken.userStatus,
// 			firstName: verifiedToken.firstName,
// 			lastName: verifiedToken.lastName,
// 			username: verifiedToken.username,
// 			emailAddress: verifiedToken.emailAddress,
// 			userDateCreated: verifiedToken.userDateCreated,
// 			languageCode: verifiedToken.languageCode,
// 			isAdmin: verifiedToken.isAdmin,
// 		} as SessionUser;
// 	} catch (error) {
// 		return null;
// 	}
// }

export async function getSession(): Promise<SessionUser | null> {
	const encryptedJwtToken = cookies().get("access-token")?.value;
	if (!encryptedJwtToken) return null;
	try {
		// Hash the keys
		const encryptionKeyHash: string = await hashKey(jwtEncryptionKey, "SHA-512", "base64");
		const issuerKeyHash: string = await hashKey(jwtIssuerKey, "SHA-256", "base64");

		// Import the hashed keys as JWK
		const decryptionKey = await importJWK({
			k: encryptionKeyHash,
			alg: "A256CBC-HS512",
			kty: "oct",
		});
		const verificationKey = await importJWK({
			k: issuerKeyHash,
			alg: "HS256",
			kty: "oct",
		});

		const { plaintext } = await compactDecrypt(encryptedJwtToken, decryptionKey);
		const jwtString: string = new TextDecoder().decode(plaintext);
		const { payload } = await jwtVerify(jwtString, verificationKey);

		const currentUnixTime = Math.floor(Date.now() / 1000);
		if (payload.exp && payload.exp < currentUnixTime) {
			throw new Error("Token expired");
		}

		return {
			UserId: payload.UserId,
			UserStatus: payload.UserStatus,
			FirstName: payload.FirstName,
			LastName: payload.LastName,
			Username: payload.Username,
			EmailAddress: payload.EmailAddress,
			UserDateCreated: payload.UserDateCreated,
			LanguageCode: payload.LanguageCode,
			IsAdmin: payload.IsAdmin,
		} as SessionUser;
	} catch (error) {
		return null;
	}
}

export async function verifySession(
	request: NextRequest,
	response: NextResponse,
	retry: boolean = false
): Promise<SessionUser | null> {
	const encryptedJwtToken = request.cookies.get("access-token");
	if (!encryptedJwtToken) return null;
	try {
		// Hash the keys
		const encryptionKeyHash: string = await hashKey(jwtEncryptionKey, "SHA-512", "base64");
		const issuerKeyHash: string = await hashKey(jwtIssuerKey, "SHA-256", "base64");

		// Import the hashed keys as JWK
		const decryptionKey = await importJWK({
			k: encryptionKeyHash,
			alg: "A256CBC-HS512",
			kty: "oct",
		});
		const verificationKey = await importJWK({
			k: issuerKeyHash,
			alg: "HS256",
			kty: "oct",
		});

		const { plaintext } = await compactDecrypt(encryptedJwtToken.value, decryptionKey);
		const jwtString: string = new TextDecoder().decode(plaintext);
		const { payload } = await jwtVerify(jwtString, verificationKey);

		const currentUnixTime = Math.floor(Date.now() / 1000);
		if (payload.exp && payload.exp < currentUnixTime) {
			throw new Error("Token expired");
		}

		return {
			UserId: payload.UserId,
			UserStatus: payload.UserStatus,
			FirstName: payload.FirstName,
			LastName: payload.LastName,
			Username: payload.Username,
			EmailAddress: payload.EmailAddress,
			UserDateCreated: payload.UserDateCreated,
			LanguageCode: payload.LanguageCode,
			IsAdmin: payload.IsAdmin,
		} as SessionUser;
	} catch (error) {
		if (retry) {
			const refreshTokenResult = await refreshAccessToken(response);
			if (refreshTokenResult) {
				return await verifySession(request, response, false);
			}
		}

		return null;
	}
}

async function hashKey(
	key: string,
	algorithm: AlgorithmIdentifier,
	outputFormat: BufferEncoding
): Promise<string> {
	const encoder = new TextEncoder();
	const Data = encoder.encode(key);
	const hashBuffer = await crypto.subtle.digest(algorithm, Data);
	return Buffer.from(hashBuffer).toString(outputFormat);
}

async function refreshAccessToken(response: NextResponse): Promise<boolean> {
	const refreshToken = cookies().get("refresh-token")?.value;
	if (refreshToken) {
		let request = new HttpRequest({
			RequestMethod: "POST",
			RequestUrl: "/api/UserToken/refreshAccessToken",
			RequestBody: undefined,
			RequestQueryStrings: undefined,
			RequestHeaders: [["refresh-token", `${refreshToken}`]],
		});
		const apiResponse = await SendApiRequest<SessionTokens>(request);
		if (
			apiResponse?.IsSuccess &&
			apiResponse.Data &&
			apiResponse.Data.AccessToken &&
			apiResponse.Data.AccessToken != "" &&
			apiResponse.Data.RefreshToken &&
			apiResponse.Data.RefreshToken != ""
		) {
			response.headers.append(
				"Set-Cookie",
				`access-token=${apiResponse.Data.AccessToken}; SameSite=${
					Constants.CookieOptions.sameSite
				}${Constants.CookieOptions.domain ? `; Domain=${Constants.CookieOptions.domain}` : ""}`
			);
			response.headers.append(
				"Set-Cookie",
				`refresh-token=${apiResponse.Data.RefreshToken}; SameSite=${
					Constants.CookieOptions.sameSite
				}${Constants.CookieOptions.domain ? `; Domain=${Constants.CookieOptions.domain}` : ""}`
			);
			return true;
		}
	}

	await clearTokens(response);
	return false;
}

async function getLocalizedError(key: string): Promise<string | null> {
	return (await getTranslations("Errors"))(key);
}

export async function testFunc() {
	const translations = await getTranslations("Errors");
	const globalError = translations("GlobalError");
	const error2 = (await getTranslations("Errors"))("GlobalError");
}
