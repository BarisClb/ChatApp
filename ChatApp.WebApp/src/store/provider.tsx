"use client";
import React from "react";
import { store } from "./index";
import { Provider } from "react-redux";

function ReduxProvider({ children }: { children: React.ReactNode }) {
	return <Provider store={store}>{children}</Provider>;
}

export default ReduxProvider;
