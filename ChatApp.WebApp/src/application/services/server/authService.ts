import { apiServerRequestHandler as _apiRequestHandler } from "@/application/api/apiServerRequestHandler";
import { LoginFormInput } from "@/application/models/LoginFormTypes";
import { ApiResponse } from "@/application/models/common/ApiResponse";
import { HttpRequest } from "@/application/models/common/HttpRequest";
import {
	JwtPayload,
	Session,
	SessionTokens,
	SessionUser,
} from "@/application/models/common/Session";
import { UserSessionResponse } from "@/application/models/response/UserSessionResponse";
import { compactDecrypt, SignJWT, jwtVerify } from "jose";
import { cookies } from "next/headers";
import { NextRequest, NextResponse } from "next/server";
import { getTranslations } from "next-intl/server";
import jwt from "jsonwebtoken";
import crypto from "crypto";

const jwtEncryptionKey = process.env.JWT_ENCRYPTION_KEY ?? "";
const jwtIssuerKey = process.env.JWT_ISSUER_KEY ?? "";

export async function login(loginForm: LoginFormInput): Promise<string[] | null> {
	let request = new HttpRequest({
		requestMethod: "POST",
		requestUrl: "api/user/logInWithSession",
		requestBody: loginForm,
		requestQueryStrings: undefined,
		requestHeaders: undefined,
	});
	let response = await _apiRequestHandler.SendApiRequest<UserSessionResponse>(request);

	if (!response.isSuccess) {
		if (response.errors) {
			return response.errors;
		}
	}

	const sessionUser = {
		userId: response.data.userId,
		userStatus: response.data.userStatus,
		firstName: response.data.firstName,
		lastName: response.data.lastName,
		username: response.data.username,
		emailAddress: response.data.emailAddress,
		userDateCreated: response.data.userDateCreated,
		languageCode: response.data.languageCode,
		isAdmin: response.data.isAdmin,
	} as SessionUser;

	const sessionTokens = {
		accessToken: response.data.accessToken,
		refreshToken: response.data.refreshToken,
	} as SessionTokens;

	const session = {
		user: sessionUser,
		tokens: sessionTokens,
	} as Session;

	cookies().set("access-token", response.data.accessToken, {
		httpOnly: true,
		secure: true,
		sameSite: true,
	});
	cookies().set("refresh-token", response.data.refreshToken, {
		httpOnly: true,
		secure: true,
		sameSite: true,
	});

	return [];
}

export async function logout() {
	// Destroy the session
	cookies().set("access-token", "", { expires: new Date(0) });
	cookies().set("refresh-token", "", { expires: new Date(0) });
}

async function getSession(): Promise<SessionUser | null> {
	const encryptedJwtToken = cookies().get("access-token")?.value;
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
			userId: verifiedToken.userId,
			userStatus: verifiedToken.userStatus,
			firstName: verifiedToken.firstName,
			lastName: verifiedToken.lastName,
			username: verifiedToken.username,
			emailAddress: verifiedToken.emailAddress,
			userDateCreated: verifiedToken.userDateCreated,
			languageCode: verifiedToken.languageCode,
			isAdmin: verifiedToken.isAdmin,
		} as SessionUser;
	} catch (error) {
		await logout();
		return null;
	}
}

async function getLocalizedError(key: string): Promise<string | null> {
	return (await getTranslations("Service.Errors"))(key);
}

export const authService = {
	login,
	logout,
	getSession,
};
