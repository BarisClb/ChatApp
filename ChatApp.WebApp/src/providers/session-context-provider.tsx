// "use session";

// import React from "react";
// import { headers } from "next/headers";
// import { Constants } from "@/application/helpers/Constants";
// import { SessionUser } from "@/application/models/common/Session";
// import { getSession } from "@/application/server/services/authService";
// import NavbarWrapper from "@/components/navbar/navbar-wrapper";
// import { SessionContext } from "./session-context";

// export async function SessionContextProvider({ children }: { children: React.ReactNode }) {
// 	const session = await getSession();
// 	const headersList = headers();
// 	const pathnameWithoutLocale = headersList.get("x-pathname-without-locale") || "";
// 	const authenticatedPath = Constants.authenticatedPaths.some((x) =>
// 		pathnameWithoutLocale.startsWith(x)
// 	);
// 	const authorizedPath = Constants.authorizedPaths.some((x) =>
// 		pathnameWithoutLocale.startsWith(x)
// 	);

// 	const shouldRenderChildren =
// 		(!authorizedPath && !authenticatedPath) ||
// 		(authenticatedPath && session) ||
// 		(authorizedPath && (session?.isAdmin ?? false));

// 	return (
// 		<>
// 			<NavbarWrapper />
// 			{shouldRenderChildren ? (
// 				<SessionContext.Provider value={{ session }}>{children}</SessionContext.Provider>
// 			) : (
// 				<div>Unauthorized Page</div>
// 			)}
// 		</>
// 	);
// }

// export default SessionContextProvider;
