"use client";

import React, { useState } from "react";
import { RegisterFormProps } from "@/application/models/props/RegisterFormProps";
import { RegisterFormInput } from "@/application/models/RegisterFormTypes";
import { authServiceClient as _authService } from "@/application/client/services/authService";

function RegisterForm({ props }: { props: RegisterFormProps }) {
	const [formData, setFormData] = useState<RegisterFormInput>({
		FirstName: "",
		LastName: "",
		EmailAddress: "",
		Username: "",
		Password: "",
		LanguageCode: "",
	});
	const [errors, setErrors] = useState<string[]>([]);
	const [showPassword, setShowPassword] = useState<boolean>(false);

	const submitForm = async (form: RegisterFormInput): Promise<string[] | null> => {
		return await _authService.register(form);
	};

	const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const { name, value } = e.target;
		setFormData({ ...formData, [name]: value });
	};

	const handleSelectChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
		const { name, value } = e.target;
		setFormData({ ...formData, [name]: value });
	};

	const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
		console.log(formData);

		e.preventDefault();
		const errors: string[] = [];

		if (formData.FirstName.trim() === "") {
			errors.push(props.Localization.Errors.FirstNameNotEmpty);
		}
		if (formData.LastName.trim() === "") {
			errors.push(props.Localization.Errors.LastNameNotEmpty);
		}
		if (formData.EmailAddress.trim() === "") {
			errors.push(props.Localization.Errors.EmailAddressNotEmpty);
		}
		if (formData.Username.trim() === "") {
			errors.push(props.Localization.Errors.UsernameNotEmpty);
		}
		if (formData.Password.trim() === "") {
			errors.push(props.Localization.Errors.PasswordNotEmpty);
		}
		if (formData.LanguageCode.trim() === "") {
			errors.push(props.Localization.Errors.LanguageNotSelected);
		}

		if (errors.length > 0) {
			setErrors(errors);
			window.scrollTo(0, 0);
			return;
		}

		try {
			const response = await submitForm(formData);
			if (response) {
				if (response.length > 0) {
					setErrors(response);
				} else {
					window.location.href = `${props.RefererUrl ?? `/${props.Locale ?? ""}`}`;
				}
			}
		} catch (error) {
			console.error("Error submitting form:", error);
			setErrors([props.Localization.Errors.GenericError]);
		}
	};

	return (
		<div
			id="register-form-wrapper"
			className="tw-bg-white tw-p-8 tw-rounded-lg tw-shadow-lg tw-w-full tw-max-w-md dark:tw-bg-gray-800 dark:tw-text-white"
		>
			{errors.length > 0 && (
				<div className="tw-mb-4 tw-bg-red-100 tw-border tw-border-red-400 tw-text-red-700 tw-px-4 tw-py-3 tw-rounded-md">
					<ul>
						{errors.map((error, index) => (
							<li key={index}>{error}</li>
						))}
					</ul>
				</div>
			)}
			<form onSubmit={handleSubmit} className="tw-space-y-6">
				<div>
					<label
						htmlFor="FirstName"
						className="tw-block tw-text-sm tw-font-medium tw-text-gray-700 dark:tw-text-gray-300"
					>
						{props.Localization.Fields.FirstName}:
					</label>
					<input
						type="text"
						id="FirstName"
						name="FirstName"
						value={formData.FirstName}
						onChange={handleInputChange}
						required
						className="tw-mt-1 tw-block tw-w-full tw-px-3 tw-py-2 tw-border tw-border-gray-300 tw-rounded-md tw-shadow-sm tw-focus:outline-none tw-focus:ring tw-focus:ring-indigo-500 tw-focus:border-indigo-500 sm:tw-text-sm dark:tw-bg-gray-700 dark:tw-border-gray-600 dark:tw-placeholder-gray-400 dark:tw-text-white"
					/>
				</div>
				<div>
					<label
						htmlFor="LastName"
						className="tw-block tw-text-sm tw-font-medium tw-text-gray-700 dark:tw-text-gray-300"
					>
						{props.Localization.Fields.LastName}:
					</label>
					<input
						type="text"
						id="LastName"
						name="LastName"
						value={formData.LastName}
						onChange={handleInputChange}
						required
						className="tw-mt-1 tw-block tw-w-full tw-px-3 tw-py-2 tw-border tw-border-gray-300 tw-rounded-md tw-shadow-sm tw-focus:outline-none tw-focus:ring tw-focus:ring-indigo-500 tw-focus:border-indigo-500 sm:tw-text-sm dark:tw-bg-gray-700 dark:tw-border-gray-600 dark:tw-placeholder-gray-400 dark:tw-text-white"
					/>
				</div>
				<div>
					<label
						htmlFor="EmailAddress"
						className="tw-block tw-text-sm tw-font-medium tw-text-gray-700 dark:tw-text-gray-300"
					>
						{props.Localization.Fields.EmailAddress}:
					</label>
					<input
						type="email"
						id="EmailAddress"
						name="EmailAddress"
						value={formData.EmailAddress}
						onChange={handleInputChange}
						required
						className="tw-mt-1 tw-block tw-w-full tw-px-3 tw-py-2 tw-border tw-border-gray-300 tw-rounded-md tw-shadow-sm tw-focus:outline-none tw-focus:ring tw-focus:ring-indigo-500 tw-focus:border-indigo-500 sm:tw-text-sm dark:tw-bg-gray-700 dark:tw-border-gray-600 dark:tw-placeholder-gray-400 dark:tw-text-white"
					/>
				</div>
				<div>
					<label
						htmlFor="Username"
						className="tw-block tw-text-sm tw-font-medium tw-text-gray-700 dark:tw-text-gray-300"
					>
						{props.Localization.Fields.Username}:
					</label>
					<input
						type="text"
						id="Username"
						name="Username"
						value={formData.Username}
						onChange={handleInputChange}
						required
						className="tw-mt-1 tw-block tw-w-full tw-px-3 tw-py-2 tw-border tw-border-gray-300 tw-rounded-md tw-shadow-sm tw-focus:outline-none tw-focus:ring tw-focus:ring-indigo-500 tw-focus:border-indigo-500 sm:tw-text-sm dark:tw-bg-gray-700 dark:tw-border-gray-600 dark:tw-placeholder-gray-400 dark:tw-text-white"
					/>
				</div>
				<div>
					<label
						htmlFor="Password"
						className="tw-block tw-text-sm tw-font-medium tw-text-gray-700 dark:tw-text-gray-300"
					>
						{props.Localization.Fields.Password}:
					</label>
					<div className="tw-relative">
						<input
							type={showPassword ? "text" : "password"}
							id="Password"
							name="Password"
							value={formData.Password}
							onChange={handleInputChange}
							required
							className="tw-mt-1 tw-block tw-w-full tw-px-3 tw-py-2 tw-border tw-border-gray-300 tw-rounded-md tw-shadow-sm tw-focus:outline-none tw-focus:ring tw-focus:ring-indigo-500 tw-focus:border-indigo-500 sm:tw-text-sm dark:tw-bg-gray-700 dark:tw-border-gray-600 dark:tw-placeholder-gray-400 dark:tw-text-white"
						/>
						<button
							type="button"
							onClick={() => setShowPassword((prev) => !prev)}
							className="tw-absolute tw-inset-y-0 tw-right-0 tw-flex tw-items-center tw-justify-center tw-px-3 tw-py-1 tw-w-20 tw-text-sm tw-font-medium tw-text-gray-200 tw-border tw-border-gray-500 tw-rounded-md dark:tw-text-gray-300 hover:tw-text-gray-800 dark:hover:tw-text-gray-100 focus:tw-outline-none focus:tw-ring tw-focus:ring-indigo-600 dark:focus:tw-ring-indigo-600 tw-bg-indigo-700 hover:tw-bg-indigo-800"
						>
							{showPassword
								? props.Localization.Misc.HidePassword
								: props.Localization.Misc.ShowPassword}
						</button>
					</div>
				</div>
				<div>
					<label
						htmlFor="LanguageCode"
						className="tw-block tw-text-sm tw-font-medium tw-text-gray-700 dark:tw-text-gray-300"
					>
						{props.Localization.Fields.Language}:
					</label>
					<select
						id="LanguageCode"
						name="LanguageCode"
						value={formData.LanguageCode}
						onChange={handleSelectChange}
						required
						className="tw-mt-1 tw-block tw-w-full tw-px-3 tw-py-2 tw-border tw-border-gray-300 tw-rounded-md tw-shadow-sm tw-focus:outline-none tw-focus:ring tw-focus:ring-indigo-500 tw-focus:border-indigo-500 sm:tw-text-sm dark:tw-bg-gray-700 dark:tw-border-gray-600 dark:tw-placeholder-gray-400 dark:tw-text-white"
					>
						<option value="">Select Language</option>
						{props.Languages.map((language) => (
							<option key={language.LanguageCode} value={language.LanguageCode}>
								{language.LanguageName}
							</option>
						))}
					</select>
				</div>
				<button
					type="submit"
					className="tw-w-full tw-flex tw-justify-center tw-px-4 tw-py-2 tw-border tw-border-transparent tw-text-sm tw-font-medium tw-rounded-md tw-text-white tw-bg-indigo-600 hover:tw-bg-indigo-700 focus:tw-outline-none focus:tw-ring-2 focus:tw-ring-offset-2 focus:tw-ring-indigo-500 dark:focus:tw-ring-offset-gray-800"
				>
					{props.Localization.Misc.RegisterButton}
				</button>
			</form>
		</div>
	);
}

export default RegisterForm;
