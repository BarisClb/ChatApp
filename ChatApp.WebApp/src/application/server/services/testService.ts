"use server";

import { cookies } from "next/headers";
import jwt from "jsonwebtoken";
import { compactDecrypt, jwtDecrypt, jwtVerify } from "jose";
import crypto from "crypto";

const encryptedJwtToken =
	"eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwidHlwIjoiSldUIiwiY3R5IjoiSldUIn0..acOBXw81ySWVfwXKs8jvzg.rQLM-DGlhLrLN1T7b87R5xbCv6j9LlNkDNLpmLNxbuh5X3q0flLoY0RwXLMT69jyVh405CsBhWMZEaM9WS_YRhSyMmVX3tAvHvZr62AlKFNvJCgDAj2JOS7SN4AQWx_6fSg_YCNgbfzcLlzcak-90sL8Mu4dNCxl4Ko7YLAvO3goRuPRSQgjtFDdKkoHYGYf2zhCCZ80F7wl7R2lxRWEX3Iuat58ZqAI8Et25D5xJ7ujo2kxeA43IVo9onb5p-KZiGaRcTBwMpCPgGqUZ-VVdaurYHXkExLZ5BFxE98XxPI7p_uj7jw8PJBmbKsp4WH2kQvWRUAl23hcznJjtF0p9gPsmEeTHi8wvKLYzR5Fr6LIiCfBXAA6Y-bTaEjmrc1jKharSwUXgnuO-S4DiSpD5N6Sx3Co__C28ZY8gCuYY-iojtRe7fwJp1jl84fwHwGZVk2bgRKRam75K8Mg9KduJROmMKfDbjreboWPi8kUDa2q5-IAoOvpDDzyEKlMK_vga1CnvjWtVs_zLMO2943UrVT2tseSVL_TG3Iuu05npeEpZRf0Jg2mZ2naLgyMPhu9bzP_WoYRPa0HMHao0Pf75B9xQxSvAl4F3E5nt9NqaixHVzMZzjXgK7pT-LsqaII0L7lxeWeehuoQrvQRTmoYqBZeqtxN7KswOOnP21K33Fw8_tfMBn6Or2E0c8HxPJa3utAa0EyB-Aos4UM27lMNyEEPInY08KqZdLY52G_jHWTtUrvJV2qCSFrc_bmRfgrEjnJefO2bL0DtRw3lxQsPn9UjQ3fSK-K8WQ3QQ7fk3EGxnIewxHdKgXq8Aza6eqKF.VU9gVwZT2Zs4DAflSZkZGcP852OA79nLMwjJuxE0Uvc";
const encryptionKeyString = "jwt-supersecretencryptionkey-for-chatapp-users";
const signingKeyString = "jwt-supersecretissuerkey-for-chatapp-users";

async function test(retry: boolean = false) {
	try {
		await test5();
	} catch (error: any) {
		if (error instanceof jwt.TokenExpiredError) {
			// Handle the specific case where the token has expired
			console.error("Token expired at:", error.expiredAt);
			// You might want to redirect the user to login or refresh the token here
		} else {
			// Handle other possible errors
			console.error("Error decoding token:", error.message);
		}
	}
}

async function test5() {
	const secretBytes = Buffer.from(encryptionKeyString, "utf-8");
	const sha512 = crypto.createHash("sha512");
	sha512.update(secretBytes);
	const result = sha512.digest();
	var decryptedToken = await compactDecrypt(encryptedJwtToken, result);

	var decryptedTokenString = decryptedToken.plaintext.toString();

	const secretBytes2 = Buffer.from(signingKeyString, "utf-8");
	const sha256 = crypto.createHash("sha256");
	sha256.update(secretBytes2);
	const signingKey = sha256.digest();

	const jwtString = Buffer.from(decryptedToken.plaintext).toString("utf8");

	const decodedToken = jwt.verify(jwtString, signingKey);

	console.log(decodedToken);
}

async function test4() {
	// const textEncoder = new TextEncoder();
	// const encryptionKey = textEncoder.encode(encryptionKeyString);
	// const signingKey = textEncoder.encode(signingKeyString);
	// const sha256Hash = crypto.createHash("sha256").update(signingKeyString, "utf8").digest("hex");
	// const sha512Hash = crypto.createHash("sha512").update(encryptionKeyString, "utf8").digest("hex");
	// const sha256HashKey = textEncoder.encode(sha256Hash);
	// const sha512HashKey = textEncoder.encode(sha512Hash);
	// const decryptedToken = await jwtDecrypt(encryptedJwtToken, sha256HashKey, {
	// 	contentEncryptionAlgorithms: ["A256CBC-HS512"],
	// });
	// console.log(decryptedToken);
	// const { payload } = await jwtVerify(decryptedToken, sha512HashKey, {
	// 	keyManagementAlgorithms: ["HS256"],
	// });
	// console.log(payload);
}

async function test3() {
	// const textEncoder = new TextEncoder();
	// const encryptionKey = textEncoder.encode(encryptionKeyString);
	// const signingKey = textEncoder.encode(signingKeyString);
	// const secretBytes = Buffer.from(encryptionKeyString, "utf-8");
	// const sha512 = crypto.createHash("sha512");
	// sha512.update(secretBytes);
	// const result = sha512.digest();
	// const decryptedToken = await compactDecrypt(encryptedJwtToken, result, {
	// 	contentEncryptionAlgorithms: ["A256CBC-HS512"],
	// });
	// console.log(decryptedToken);
	// const { payload } = await jwtVerify(decryptedToken.plaintext.toString(), signingKey, {
	// 	algorithms: ["HS256"],
	// });
}

async function test2() {
	// const encryptionKeyBuffer = Buffer.from(encryptionKeyString, "utf-8");
	// const encryptionKey = crypto.createSecretKey(encryptionKeyBuffer);
	// try {
	// 	const { plaintext } = await compactDecrypt(encryptedJwtToken, encryptionKey);
	// 	const decryptedToken = plaintext.toString();
	// 	console.log(decryptedToken);
	// } catch (error) {
	// 	console.error("Error processing JWT: ", error);
	// }
	// const verifiedToken = await jwtVerify(decryptedToken, signingKeyString);
	// console.log(verifiedToken.payload);
}

async function test1() {
	// const decodedToken = jwt.decode(encryptedJwtToken, { complete: true });
	// console.log(decodedToken);
	// const secretKey = new TextEncoder().encode(encryptionKeyString);
	// function tryParseBoolean(value: string): boolean | null {
	// 	if (value.toLowerCase() === "true") {
	// 		return true;
	// 	} else if (value.toLowerCase() === "false") {
	// 		return false;
	// 	} else {
	// 		return null;
	// 	}
	// }
}

export const testServiceServer = {
	test,
};
