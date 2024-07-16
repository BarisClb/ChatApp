"use client";

import React, { useState } from "react";
import { authServiceClient as _authService } from "@/application/client/services/authService";
import { LoginFormInput } from "@/application/models/LoginFormTypes";
import { LoginFormProps } from "@/application/models/props/LoginFormProps";

function LoginForm({ props }: { props: LoginFormProps }) {
	const [formData, setFormData] = useState<LoginFormInput>({ UserField: "", Password: "" });
	const [errors, setErrors] = useState<string[]>([]);
	const [showPassword, setShowPassword] = useState<boolean>(false);

	const submitForm = async (form: LoginFormInput): Promise<string[]> => {
		return await _authService.login(form);
	};

	const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const { name, value } = e.target;
		setFormData({ ...formData, [name]: value });
	};

	const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
		e.preventDefault();
		if (formData) {
			console.log(formData);

			if (!formData.UserField || formData.UserField == "") {
				setErrors([`${props.Localization.Errors.LoginFieldNotEmpty}`]);
				return;
			}
			if (!formData.Password || formData.Password == "") {
				setErrors([`${props.Localization.Errors.PasswordNotEmpty}`]);
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
		}
	};

	return (
		<div
			id="login-form-wrapper"
			className="tw-bg-white tw-p-8 tw-rounded-lg tw-shadow-lg tw-w-full tw-max-w-md dark:tw-bg-gray-800 dark:tw-text-white"
		>
			{errors.length > 0 && (
				<div className="tw-login-form-error tw-text-red-600 dark:tw-text-red-400 tw-mb-4">
					{errors.map((error, index) => (
						<div key={index}>{error}</div>
					))}
				</div>
			)}

			<form onSubmit={handleSubmit} className="tw-space-y-6">
				<div>
					<label
						htmlFor="UserField"
						className="tw-block tw-text-sm tw-font-medium tw-text-gray-700 dark:tw-text-gray-300"
					>
						{props.Localization.Fields.UserField}:
					</label>
					<input
						type="text"
						id="UserField"
						name="UserField"
						value={formData.UserField}
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
							className="tw-mt-1 tw-block tw-w-full tw-px-3 tw-py-2 tw-border tw-border-gray-500 tw-rounded-md tw-shadow-sm tw-focus:outline-none tw-focus:ring tw-focus:ring-indigo-500 tw-focus:border-indigo-500 sm:tw-text-sm dark:tw-bg-gray-700 dark:tw-border-gray-600 dark:tw-placeholder-gray-400 dark:tw-text-white"
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
				<button
					type="submit"
					className="tw-w-full tw-flex tw-justify-center tw-px-4 tw-py-2 tw-border tw-border-transparent tw-text-sm tw-font-medium tw-rounded-md tw-text-white tw-bg-indigo-600 hover:tw-bg-indigo-700 focus:tw-outline-none focus:tw-ring-2 focus:tw-ring-offset-2 focus:tw-ring-indigo-500 dark:focus:tw-ring-offset-gray-800"
				>
					{props.Localization.Misc.LoginButton}
				</button>
			</form>
		</div>
	);
}

export default LoginForm;
