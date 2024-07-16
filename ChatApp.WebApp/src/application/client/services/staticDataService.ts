"use client";

import errorMessagesEn from "@/localization/en/errors.json";

function getErrorMessage(key: string): string {
	return "";
}

function getValue<T, K extends keyof T>(obj: T, key: K): T[K] {
	return obj[key];
}
