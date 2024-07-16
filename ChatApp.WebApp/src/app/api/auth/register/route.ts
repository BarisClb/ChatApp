import { ApiResponse } from "@/application/models/common/ApiResponse";
import { HttpRequest, StringTuple } from "@/application/models/common/HttpRequest";
import { SendHttpRequest } from "@/application/server/common/apiRequestHandler";

export async function POST(req: Request): Promise<Response> {
	try {
		const headersArray: StringTuple[] = [];
		req.headers.forEach((value, key) => {
			headersArray.push([key, value]);
		});
		const httpRequest = new HttpRequest({
			RequestMethod: "POST",
			RequestUrl: "/api/v1/user/register",
			RequestBody: await req.json(),
			RequestHeaders: headersArray,
		});

		const response = await SendHttpRequest(httpRequest);

		return new Response(await response.text(), {
			status: response.status,
			headers: { "Content-Type": "application/json" },
		});
	} catch (error) {
		console.error(error);
		const failApiResponse = {
			Data: null,
			Errors: ["Internal server error"],
			IsSuccess: false,
			StatusCode: 500,
			Tokens: null,
		} as ApiResponse<boolean | null>;
		return new Response(JSON.stringify(failApiResponse), {
			status: 500,
			headers: { "Content-Type": "application/json" },
		});
	}
}
