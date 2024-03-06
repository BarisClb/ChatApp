"use client";
import { RegisterFormProps } from "@/application/models/props/RegisterFormProps";
import { RegisterFormInput } from "@/application/models/RegisterFormTypes";
import React, { useState } from "react";

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

	const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const { name, value } = e.target;
		setFormData({ ...formData, [name]: value });
	};

	const handleSelectChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
		const { name, value } = e.target;
		setFormData({ ...formData, [name]: value });
	};

	const handleTogglePassword = () => {
		setShowPassword(!showPassword);
	};

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		const errors: string[] = [];

		// Validation
		if (formData.FirstName.trim() === "") {
			errors.push("First Name is required");
		}
		if (formData.LastName.trim() === "") {
			errors.push("Last Name is required");
		}
		if (formData.EmailAddress.trim() === "") {
			errors.push("Email Address is required");
		}
		if (formData.Username.trim() === "") {
			errors.push("Username is required");
		}
		if (formData.Password.trim() === "") {
			errors.push("Password is required");
		}
		if (formData.LanguageCode.trim() === "") {
			errors.push("Language Code is required");
		}

		if (errors.length > 0) {
			setErrors(errors);
			window.scrollTo(0, 0); // Scroll to top of the page
			return;
		}

		try {
			props.submitForm(formData);
			// Here you can send formData to your backend API for registration
			console.log("Form submitted:", formData);
		} catch (error) {
			console.error("Error submitting form:", error);
		}
	};

	return (
		<div className="flex justify-center items-center h-screen">
			<div className="w-96 bg-gray-100 p-8 rounded-lg">
				<h1 className="text-2xl font-semibold mb-4">Register</h1>
				{errors.length > 0 && (
					<div className="mb-4 bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-md">
						<ul>
							{errors.map((error, index) => (
								<li key={index}>{error}</li>
							))}
						</ul>
					</div>
				)}
				<form onSubmit={handleSubmit}>
					<div className="mb-4">
						<label className="block mb-2">First Name:</label>
						<input
							type="text"
							name="FirstName"
							value={formData.FirstName}
							onChange={handleInputChange}
							className="w-full px-3 py-2 border rounded-md"
						/>
					</div>
					<div className="mb-4">
						<label className="block mb-2">Last Name:</label>
						<input
							type="text"
							name="LastName"
							value={formData.LastName}
							onChange={handleInputChange}
							className="w-full px-3 py-2 border rounded-md"
						/>
					</div>
					<div className="mb-4">
						<label className="block mb-2">Email Address:</label>
						<input
							type="email"
							name="EmailAddress"
							value={formData.EmailAddress}
							onChange={handleInputChange}
							className="w-full px-3 py-2 border rounded-md"
						/>
					</div>
					<div className="mb-4">
						<label className="block mb-2">Username:</label>
						<input
							type="text"
							name="Username"
							value={formData.Username}
							onChange={handleInputChange}
							className="w-full px-3 py-2 border rounded-md"
						/>
					</div>
					<div className="mb-4">
						<label className="block mb-2">Password:</label>
						<div className="relative">
							<input
								type={showPassword ? "text" : "password"}
								name="Password"
								value={formData.Password}
								onChange={handleInputChange}
								className="w-full px-3 py-2 border rounded-md pr-10" // Increased padding to accommodate the button
							/>
							<button
								type="button"
								className="absolute inset-y-0 right-0 px-3 py-2 bg-gray-200 rounded-md"
								onClick={handleTogglePassword}
							>
								{showPassword ? "Hide" : "Show"}
							</button>
						</div>
					</div>
					<div className="mb-4">
						<label className="block mb-2">Language Code:</label>
						<select
							name="LanguageCode"
							value={formData.LanguageCode}
							onChange={handleSelectChange}
							className="w-full px-3 py-2 border rounded-md"
						>
							<option value="">Select Language</option>
							{props.languages.map((language) => (
								<option key={language.code} value={language.code}>
									{language.name}
								</option>
							))}
						</select>
					</div>
					<button
						type="submit"
						className="w-full bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600"
					>
						Register
					</button>
				</form>
			</div>
		</div>
	);
}

export default RegisterForm;
