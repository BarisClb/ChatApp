import { LoginFormInput } from "@/application/models/LoginFormTypes";
import { userServiceServer as _userService } from "@/application/services/server/userService";
import React from "react";

async function TestServerPage() {
	const loginForm = { userField: "admin", password: "VerySecretPassword123" } as LoginFormInput;
	var response = await _userService.userLogin(loginForm);
	console.log(response);
	return <div>TestServerPage</div>;
}

export default TestServerPage;
