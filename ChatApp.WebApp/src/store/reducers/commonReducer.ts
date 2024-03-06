import { PayloadAction, createSlice } from "@reduxjs/toolkit";

export interface ICommonState {
	isLoading: boolean;
}

const initialState: ICommonState = {
	isLoading: false,
};

export const commonSlice = createSlice({
	name: "common",
	initialState,
	reducers: {
		setLoading: (state, action: PayloadAction<boolean>) => {
			return {
				...state,
				isLoading: action.payload,
			};
		},
	},
});

export const { setLoading } = commonSlice.actions;
export const commonReducer = commonSlice.reducer;
