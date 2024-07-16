export interface Session {
	User: SessionUser;
	Tokens: SessionTokens;
}

export interface SessionUser {
	UserId: string;
	FirstName: string;
	LastName: string;
	Username: string;
	EmailAddress: string;
	LanguageCode: string;
	UserStatus: number;
	IsAdmin: boolean;
	UserDateCreated: Date;
}

export interface SessionTokens {
	AccessToken: string;
	RefreshToken: string;
}

export interface JwtPayload {
	Iss: string;
	Iat: number;
	Aud: string;
	Nbf: number;
	Exp: number;
	AccessTokenId: string;
	UserId: string;
	FirstName: string;
	LastName: string;
	Username: string;
	EmailAddress: string;
	LanguageCode: string;
	UserStatus: number;
	IsAdmin: boolean;
	UserDateCreated: Date;
}
