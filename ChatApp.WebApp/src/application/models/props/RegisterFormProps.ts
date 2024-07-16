import {
	RegisterLocalizationLanguageModel,
	RegisterLocalizationModel,
} from "@/localization/models/registerLocalizationModels";

export type RegisterFormProps = {
	Languages: RegisterLocalizationLanguageModel[];
	Localization: RegisterLocalizationModel;
	Locale: string;
	RefererUrl: string;
};
