"use client";
import ClientLoading from "@/components/common/client-loading";
import { useAppSelector } from "@/store";
import React from "react";

function LoadingContextProvider({ children }: { children: React.ReactNode }) {
	var isLoading = useAppSelector((state) => state?.common?.isLoading);
	return (
		<>
			{isLoading && <ClientLoading />}
			{children}
		</>
	);
}

export default LoadingContextProvider;
