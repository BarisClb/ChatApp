import { LoginLocalizationModel } from "@/localization/models/loginLocalizationModels";

export interface LoginFormProps {
	Localization: LoginLocalizationModel;
	RefererUrl?: string | null;
	Locale: string;
}
