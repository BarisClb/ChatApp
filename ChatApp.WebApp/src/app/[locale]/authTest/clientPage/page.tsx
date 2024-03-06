"use client";
import React from "react";
import { signIn, useSession } from "next-auth/react";

function ClientPage() {
	// signIn("credentials", {userField: "admin", password: "VerySecretPassword123"})
	const { data: session } = useSession();
	const data = useSession();
	console.log({ session });
	console.log(session);
	console.log(typeof(session));
	console.log(session === undefined);
	console.log(session === null);
	console.log(session?.user == undefined);
	console.log(session?.user == null);
	return session === undefined ? (
		<h1>Loading...</h1>
	) : session === null ? (
		<div>You need to LogIn.</div>
	) : (
		<div>{session?.user.firstName}</div>
	);
}

export default ClientPage;
