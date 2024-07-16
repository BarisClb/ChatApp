export type UserTokenResponse = {
	AccessToken: string;
	RefreshToken: string;
};

export type UserSessionResponse = {
	UserId: string;
	UserStatus: number;
	FirstName: string;
	LastName: string;
	EmailAddress: string;
	Username: string;
	LanguageCode: string;
	UserDateCreated: Date;
	IsAdmin: boolean;
	AccessToken: string;
	RefreshToken: string;
};
