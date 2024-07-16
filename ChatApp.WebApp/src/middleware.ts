import { NextRequest, NextResponse } from "next/server";
import createIntlMiddleware from "next-intl/middleware";
import { Constants } from "@/application/helpers/Constants";
import { SessionUser } from "./application/models/common/Session";
import { verifySession } from "./application/server/services/authService";

export async function middleware(request: NextRequest) {
	const handleI18nRouting = createIntlMiddleware({
		locales: Constants.Languages,
		defaultLocale: Constants.DefaultLanguage,
		localePrefix: "always",
		//localeDetection: false
	});
	const response = handleI18nRouting(request);

	if (!(await checkIfUrlPathIsLocalized(request.nextUrl.pathname))) {
		return response;
	}
	const url = new URL(request.url);
	const locale = url?.pathname?.substring(1, 3);
	response.headers.set("x-url", request.url ?? "/");
	response.headers.set("x-origin", url?.origin ?? "/");
	response.headers.set("x-pathname", url?.pathname ?? "/");
	response.headers.set("x-locale", locale ?? "/");
	response.headers.set("x-pathname-without-locale", url?.pathname?.substring(3) ?? "/");

	const session = await verifySession(request, response, true);
	if (!isAuthenticated(url?.pathname?.substring(3) ?? "/", session)) {
		return NextResponse.redirect(new URL(`${locale}/unauthenticated`, request.url));
	}

	if (!isAuthorized(url?.pathname?.substring(3) ?? "/", session)) {
		return NextResponse.redirect(new URL(`${locale}/unauthorized`, request.url));
	}

	return response;
}

async function checkIfUrlPathIsLocalized(urlPath: string) {
	const pathArray = urlPath.split("/");
	if (pathArray.length < 2 || !Constants.Languages.includes(pathArray[1])) {
		return false;
	}
	return true;
}

async function isAuthenticated(path: string, session: SessionUser | null): Promise<boolean> {
	return !(Constants.AuthenticatedPaths.some((x) => path.startsWith(x)) && session == null);
}

async function isAuthorized(path: string, session: SessionUser | null): Promise<boolean> {
	return !(Constants.AuthorizedPaths.some((x) => path.startsWith(x)) && session?.IsAdmin !== true);
}

export const config = {
	matcher: ["/((?!api|_next|_vercel|.*\\..*).*)"],
};
