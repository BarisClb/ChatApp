import { persistReducer } from "redux-persist";
import { userPersistConfig } from "@/store/configs/userConfig";
import { combineReducers } from "@reduxjs/toolkit";
import { userReducer } from "./userReducer";
import { commonPersistConfig } from "@/store/configs/commonConfig";
import { commonReducer } from "./commonReducer";

const rootReducer = combineReducers({
	user: persistReducer(userPersistConfig, userReducer),
	common: persistReducer(commonPersistConfig, commonReducer),
});

export default rootReducer;
