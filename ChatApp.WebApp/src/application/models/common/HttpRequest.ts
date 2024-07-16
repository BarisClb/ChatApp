export class HttpRequest {
	public constructor(init?: Partial<HttpRequest>) {
		Object.assign(this, init);
	}

	RequestMethod!: string;
	RequestUrl!: string;
	RequestBody!: object;
	RequestQueryStrings!: string[];
	RequestHeaders!: StringTuple[];
	RetryRequest?: boolean;
}

export type StringTuple = [string, string];
