import { LoginFormInput, LoginFormSubmitFunction } from "@/application/models/LoginFormTypes";
import React, { useState } from "react";

interface RegisterFormProps {
	submitForm: LoginFormSubmitFunction;
}

function LoginForm({ submitForm }: RegisterFormProps) {
	const [formData, setFormData] = useState<LoginFormInput>({ userField: "", password: "" });
	const [errors, setErrors] = useState<string[]>([]);
	const [showPassword, setShowPassword] = useState<boolean>(false);

	const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const { name, value } = e.target;
		setFormData({ ...formData, [name]: value });
	};

	const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
		e.preventDefault();
		submitForm(formData);
	};

	return (
		<form onSubmit={handleSubmit}>
			<div>
				<label htmlFor="username">Username or Email:</label>
				<input
					type="text"
					id="username"
					name="username"
					value={formData.userField}
					onChange={handleInputChange}
					required
				/>
			</div>
			<div>
				<label htmlFor="password">Password:</label>
				<div className="relative">
					<input
						type={showPassword ? "text" : "password"}
						id="password"
						name="password"
						value={formData.password}
						onChange={handleInputChange}
						required
					/>
					<button
						type="button"
						onClick={() => setShowPassword((prev) => !prev)}
						className="absolute inset-y-0 right-0 px-3 py-1"
					>
						{showPassword ? "Hide" : "Show"}
					</button>
				</div>
			</div>
			<button type="submit">Login</button>
		</form>
	);
}

export default LoginForm;
