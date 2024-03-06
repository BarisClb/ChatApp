import { getToken } from "next-auth/jwt";
import { NextRequest, NextResponse } from "next/server";
import createIntlMiddleware from "next-intl/middleware";
import { Constants } from "@/application/helpers/Constants";

export async function middleware(request: NextRequest) {
	console.log("middleware");

	const handleI18nRouting = createIntlMiddleware({
		locales: Constants.languages,
		defaultLocale: Constants.defaultLanguage,
		localePrefix: "always",
	});
	const response = handleI18nRouting(request);

	if (!(await checkIfUrlPathIsLocalized(request.nextUrl.pathname))) {
		return response;
	}
	const url = new URL(request.url);
	console.log(url?.pathname?.substring(1, 3))
	console.log(request.nextUrl.pathname?.split("/")[1])
	response.headers.set("x-url", request.url ?? "");
	response.headers.set("x-origin", url?.origin ?? "");
	response.headers.set("x-pathname", url?.pathname ?? "");
	response.headers.set("x-locale", url?.pathname?.substring(1, 3) ?? "");
	response.headers.set("x-pathname-without-locale", url?.pathname?.substring(3) ?? "");

	return response;

	// if (request.nextUrl.pathname.startsWith("/authTest")) {
	// 	var isAuthenticated = await checkAuthentication(request);
	// 	var isAuthorized = await checkAuthorization(request);
	// 	console.log(isAuthenticated);
	// 	console.log(isAuthorized);
	// 	return NextResponse.redirect(new URL("/", request.url));
	// }
}

async function checkIfUrlPathIsLocalized(urlPath: string) {
	const pathArray = urlPath.split("/");
	if (pathArray.length < 2 || !Constants.languages.includes(pathArray[1])) {
		return false;
	}
	return true;
}

// async function checkAuthentication(request: NextRequest): Promise<boolean> {
// 	const session = await getToken({
// 		req: request,
// 		secret: process.env.NEXTAUTH_SECRET,
// 	});
// 	console.log(session);
// 	return session != null;
// }

// async function checkAuthorization(request: NextRequest): Promise<boolean> {
// 	const session = await getToken({
// 		req: request,
// 		secret: process.env.NEXTAUTH_SECRET,
// 	});
// 	console.log(session);
// 	return session?.user == null || session.user.IsAdmin == false ? false : true;
// }

export const config = {
	matcher: ["/((?!api|_next|_vercel|.*\\..*).*)"],
};
