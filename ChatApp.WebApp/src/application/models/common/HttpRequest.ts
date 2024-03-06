export class HttpRequest {
    public constructor(init?:Partial<HttpRequest>) {
        Object.assign(this, init);
    }

    requestMethod!: string 
    requestUrl!: string 
    requestBody!: object 
    requestQueryStrings!: string[]
    requestHeaders!: StringTuple[]
    retryRequest?: boolean
}

type StringTuple = [string, string];
