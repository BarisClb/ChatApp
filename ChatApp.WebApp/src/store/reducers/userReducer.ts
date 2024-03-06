import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit";
import { SetUserType } from "../models/userModels";

export interface IUserState {
	isUser: boolean;
	isAdmin: boolean;
	username: string;
}

const initialState: IUserState = {
	isUser: false,
	isAdmin: false,
	username: "",
};

export const userSlice = createSlice({
	name: "user",
	initialState,
	reducers: {
		setUser: (state, action: PayloadAction<SetUserType>) => {
			return {
				isUser: action.payload.isUser,
				isAdmin: action.payload.isAdmin,
				username: action.payload.username,
			};
		},
		setusername: (state, action: PayloadAction<string>) => {
			return {
				...state,
				username: action.payload,
			};
		},
		clearUser: () => {
			return {
				...initialState,
			};
		},
	}
});

export const { setUser, setusername } = userSlice.actions;
export const userReducer = userSlice.reducer;
