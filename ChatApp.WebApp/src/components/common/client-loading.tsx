"use client";
import React from "react";
import "./styles/styles.css"

function ClientLoading() {
	return (
		<>
			<div id="loader-background"></div>
			<div className="loader-wrapper">
				<div className="loader-status">
					<div className="loader-status-bar"></div>
					<div className="loader-status-info">LOADING</div>
				</div>
			</div>
		</>
	);
}

export default ClientLoading;
