export namespace Constants {
	export const Languages: string[] = ["en", "tr"];
	export const DefaultLanguage: string = "en";
	export const AuthenticatedPaths: string[] = ["/friends", "/chats", "/test/authentication"];
	export const AuthorizedPaths: string[] = ["/test/authorization"];
	export const CookieOptions: any = {
		sameSite: "lax",
		domain:
			process.env.NEXT_PUBLIC_ENVIRONMENT === "PROD" && process.env.NEXT_PUBLIC_COOKIE_DOMAIN
				? process.env.NEXT_PUBLIC_COOKIE_DOMAIN
				: undefined,
	};
}
