import { ApiResponse, NoContent } from "@/application/models/common/ApiResponse";
import { HttpRequest, StringTuple } from "@/application/models/common/HttpRequest";
import { SendHttpRequest } from "@/application/server/common/apiRequestHandler";

export async function POST(req: Request) {
	try {
		const headersArray: StringTuple[] = [];
		req.headers.forEach((value, key) => {
			headersArray.push([key, value]);
		});

		const httpRequest = new HttpRequest({
			RequestMethod: "POST",
			RequestUrl: "/api/v1/user/logOut",
			RequestBody: undefined,
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
		} as ApiResponse<NoContent | null>;
		return new Response(JSON.stringify(failApiResponse), {
			status: 500,
			headers: { "Content-Type": "application/json" },
		});
	}
}
