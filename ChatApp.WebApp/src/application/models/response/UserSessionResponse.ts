export type UserTokenResponse = {
	accessToken: string;
	refreshToken: string;
};

export type UserSessionResponse = {
	userId: string;
	userStatus: number;
	firstName: string;
	lastName: string;
	emailAddress: string;
	username: string;
	languageCode: string;
	userDateCreated: Date;
	isAdmin: boolean;
	accessToken: string;
	refreshToken: string;
};
