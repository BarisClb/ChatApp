import React from "react";
import type { GetServerSidePropsContext, InferGetServerSidePropsType } from "next";
import { getServerSession } from "next-auth";
import { authOptions } from "@/app/api/auth/[...nextauth]/route";
import { getCsrfToken, signIn } from "next-auth/react";

async function ServerPage({ csrfToken }: InferGetServerSidePropsType<typeof getServerSideProps>) {
	const session = await getServerSession(authOptions);
	console.log(session);
	await signIn("credentials", { username: "jsmith", password: "1234" });
	return (
		<>
			<div>{session?.user && session.user.firstName}</div>
			<form method="post" action="/api/auth/callback/credentials">
				<input name="csrfToken" type="hidden" defaultValue={csrfToken} />
				<label>
					Username
					<input name="username" type="text" />
				</label>
				<label>
					Password
					<input name="password" type="password" />
				</label>
				<button type="submit">Sign in</button>
			</form>
		</>
	);
}
export async function getServerSideProps(context: GetServerSidePropsContext) {
	return {
		props: {
			csrfToken: await getCsrfToken(context),
		},
	};
}

export default ServerPage;
