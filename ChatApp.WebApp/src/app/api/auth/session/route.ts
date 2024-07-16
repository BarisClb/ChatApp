import { JwtPayload } from "@/application/models/common/Session";
import { compactDecrypt } from "jose";
import jwt from "jsonwebtoken";
import { cookies } from "next/headers";
import crypto from "crypto";
import { ApiResponse } from "@/application/models/common/ApiResponse";

const jwtEncryptionKey = process.env.JWT_ENCRYPTION_KEY ?? "";
const jwtIssuerKey = process.env.JWT_ISSUER_KEY ?? "";

export async function GET(request: Request, response: Response): Promise<Response> {
	const encryptedJwtToken = cookies().get("access-token")?.value;
	if (!encryptedJwtToken) {
		const failApiResponse = {
			Data: null,
			Errors: ["Internal server error"],
			IsSuccess: false,
			StatusCode: 500,
			Tokens: null,
		} as ApiResponse<boolean | null>;
		return new Response(JSON.stringify(failApiResponse), {
			status: 500,
			headers: { "Content-Type": "application/json" },
		});
	}

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

		return new Response(await response.text(), {
			status: response.status,
			headers: { "Content-Type": "application/json" },
		});
	} catch (error) {
		await fetch("/api/auth/logout", { cache: "no-cache", method: "POST" });
		console.error(error);
		const failApiResponse = {
			Data: null,
			Errors: ["Internal server error"],
			IsSuccess: false,
			StatusCode: 500,
			Tokens: null,
		} as ApiResponse<boolean | null>;
		return new Response(JSON.stringify(failApiResponse), {
			status: 500,
			headers: { "Content-Type": "application/json" },
		});
	}
}
