import { NextRequest } from "next/server";

export async function GET(nextRequest: NextRequest) {
	const param = nextRequest.nextUrl.searchParams.get("param");
	console.log(param);
}

// request can be type?
export async function POST(request: NextRequest, context: any) {
	const { params } = context;
	const body = request.json();
	const userId = request.nextUrl.pathname.split("/").pop();
	console.log(request);
	console.log(body);
	console.log(userId);
	console.log(params.userId);
}
