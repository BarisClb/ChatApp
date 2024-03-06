import storage from "redux-persist/lib/storage";

export const commonPersistConfig = {
	key: "common",
	storage: storage,
	whitelist: ["isLLoading"],
};
