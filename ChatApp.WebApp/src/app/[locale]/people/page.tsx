import { userServiceServer as _userService } from "@/application/services/server/userService";
import React from "react";

async function People() {
	await _userService.test();
	return <div>People</div>;
}

export default People;
