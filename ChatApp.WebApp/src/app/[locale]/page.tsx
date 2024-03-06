// "use client";
import { cookies } from "next/headers";
import jwt from "jsonwebtoken";
import { compactDecrypt, jwtDecrypt, jwtVerify } from "jose";
import crypto from "crypto";
import { testService } from "@/application/services/server/testService";

export default async function Home() {
	// const sessionCookie = cookies().get("session")?.value;

	// await testService.test();

	// const useEncryption = tryParseBoolean(process.env.JWT_USE_ENCRYPTION ?? "false");

	// console.log(process.env.JWT_USE_ENCRYPTION);
	// console.log(useEncryption);
	// console.log("end");

	return (
		<main className="flex text-green">
			<h1 className="text-lg tw-text-green-900 dark:tw-text-yellow-500">Hello World!</h1>
		</main>
	);
}
