"use server";

import React from "react";

async function TestServerPage() {
	console.log(`server page session: ${"session"}`);
	return <div>TestServerPage</div>;
}
export default TestServerPage;
