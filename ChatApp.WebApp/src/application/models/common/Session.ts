export interface Session {
	user: SessionUser;
	tokens: SessionTokens;
}

export interface SessionUser {
	userId: string;
	firstName: string;
	lastName: string;
	username: string;
	emailAddress: string;
	languageCode: string;
	userStatus: number;
	isAdmin: boolean;
	userDateCreated: Date;
}

export interface SessionTokens {
	accessToken: string;
	refreshToken: string;
}

export interface JwtPayload {
	iss: string;
	iat: number;
	aud: string;
	nbf: number;
	exp: number;
	accessTokenId: string;
	userId: string;
	firstName: string;
	lastName: string;
	username: string;
	emailAddress: string;
	languageCode: string;
	userStatus: number;
	isAdmin: boolean;
	userDateCreated: Date;
}
