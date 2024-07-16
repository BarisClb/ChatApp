export interface ApiResponse<T> {
	Data: T;
	StatusCode: number;
	IsSuccess: boolean;
	Errors: string[];
	Tokens?: ApiResponseToken | null;
}

interface ApiResponseToken {
	AccessToken: string;
	RefreshToken: string;
}

export type NoContent = {};
