async function initializeHeaders(headers: [string, string][]): Promise<[string, string][]> {
	let requestHeaders: HeadersInit = [];
	requestHeaders.push(["Accept", "application/json"]);
	requestHeaders.push(["Content-type", "application/json"]);
	if (headers && headers.length > 0) {
      for (let i = 0; i < headers.length; i++) {
         requestHeaders.push([headers[i][0], headers[i][1]])
      }
	}
   return requestHeaders;
}

export const apiRequestHelper = {
   initializeHeaders
}