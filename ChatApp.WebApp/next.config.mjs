// /** @type {import('next').NextConfig} */
// const nextConfig = {
// 	env: {
// 		API_URL: process.env.API_URL,
// 	},
// 	publicRuntimeConfig: {
// 		API_URL: process.env.API_URL,
// 	},
// 	reactStrictMode: false,
// };

// export default nextConfig;

import createNextIntlPlugin from "next-intl/plugin";
const withNextIntl = createNextIntlPlugin();

/** @type {import('next').NextConfig} */
const nextConfig = {
	env: {
		API_URL: process.env.API_URL,
	},
	publicRuntimeConfig: {
		API_URL: process.env.API_URL,
	},
	reactStrictMode: false,
};

export default withNextIntl(nextConfig);
