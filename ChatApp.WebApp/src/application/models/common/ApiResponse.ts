export interface ApiResponse<T> {
	data: T;
	statusCode: number;
	isSuccess: boolean;
	errors: string[];
	tokens: ApiResponseToken;
}

interface ApiResponseToken {
	accessToken: string;
	refreshToken: string;
}

export type NoContent = {};
