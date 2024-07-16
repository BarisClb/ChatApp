"use client"

import { SessionUser } from "@/application/models/common/Session";
import { createContext, useContext } from "react";

interface SessionContextProps {
	session: SessionUser | null;
}

export const SessionContext = createContext<SessionContextProps>({ session: null });

export const useSession = () => useContext(SessionContext);
