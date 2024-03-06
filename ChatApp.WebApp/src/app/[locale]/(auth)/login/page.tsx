"use client";
import { useSession } from "next-auth/react";
import React from "react";
import Loading from "./loading";

function Login() {
	const { data: session } = useSession();
	return session === undefined ? (
		<Loading />
	) : session === null ? (
		<div>You need to LogIn.</div>
	) : (
		<div>You are already logged in, {session?.user.username}</div>
	);
}

export default Login;
