import { getCookies, getCookie, setCookie, deleteCookie } from "cookies-next";
import { apiClientRequestHandler as _apiRequestHandler } from "@/application/api/apiClientRequestHandler";
import { LoginFormInput } from "@/application/models/LoginFormTypes";
import { HttpRequest } from "@/application/models/common/HttpRequest";
import { JwtPayload, SessionUser } from "@/application/models/common/Session";
import { UserTokenResponse } from "@/application/models/response/UserSessionResponse";
import { compactDecrypt } from "jose";
import jwt from "jsonwebtoken";
import crypto from "crypto";
import { Constants } from "@/application/helpers/Constants";

const jwtEncryptionKey = process.env.JWT_ENCRYPTION_KEY ?? "";
const jwtIssuerKey = process.env.JWT_ISSUER_KEY ?? "";

async function userRegister() {
	console.log("register start");
	let request = new HttpRequest({
		requestMethod: "POST",
		requestUrl: "api/test/getCurrentUserWithPost",
		requestBody: undefined,
		requestQueryStrings: undefined,
		requestHeaders: undefined,
	});
	let response = await _apiRequestHandler.SendApiRequest<NoContent>(request);
	console.log("register end");
}

async function userLogin(loginForm: LoginFormInput): Promise<string[] | null> {
	let request = new HttpRequest({
		requestMethod: "POST",
		requestUrl: "/api/user/logIn",
		requestBody: loginForm,
		requestQueryStrings: undefined,
		requestHeaders: undefined,
	});
	let response = await _apiRequestHandler.SendApiRequest<UserTokenResponse>(request);

	if (!response.isSuccess) {
		if (response.errors) {
			return response.errors;
		}
	}

	setCookie("access-token", response.data.accessToken, {
		sameSite: "lax",
		// domain: "barisclb.com"
	});
	setCookie("refresh-token", response.data.refreshToken, {
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
