"use client";

import { LoginFormInput } from "@/application/models/LoginFormTypes";
import { userServiceClient as _userClient } from "@/application/services/client/userService";
import React, { useEffect } from "react";

function TestClientPage() {
	const loginFunc = async () => {
		const loginForm = { userField: "admin", password: "VerySecretPassword123" } as LoginFormInput;
		const response = await _userClient.userLogin(loginForm);
		console.log(response)
	};

	useEffect(() => {
		loginFunc();
	}, []);

	return <div>TestClientPage</div>;
}

export default TestClientPage;
