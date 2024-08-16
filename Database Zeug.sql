CREATE TABLE application (
	apl_id int NOT NULL IDENTITY(1,1),
	apl_description nvarchar(100),
	apl_first_edit_date datetime,
	apl_last_edit_date datetime,
	PRIMARY KEY(apl_id)
)

CREATE TABLE applicant (
	app_id int NOT NULL IDENTITY(1,1),
	app_firstname nvarchar(50),
	app_lastname nvarchar(50) NOT NULL,
	app_birthdate date NOT NULL,
	app_first_edit_date datetime,
	app_last_edit_date datetime,
	PRIMARY KEY(app_id),
)

CREATE TABLE applicantapplication (
	apa_id int NOT NULL IDENTITY(1,1),
	apa_apl_id int,
	apa_app_id int,
	apa_state nvarchar(50),
	apa_first_edit_date datetime,
	apa_last_edit_date datetime,
	FOREIGN KEY (apa_apl_id) REFERENCES application (apl_id),
	FOREIGN KEY (apa_app_id) REFERENCES applicant (app_id)
)

CREATE TABLE skill (
	skl_id int NOT NULL IDENTITY(1,1),
	skl_description nvarchar(100),
	skl_first_edit_date datetime,
	skl_last_edit_date datetime,
	PRIMARY KEY(skl_id),
)

CREATE TABLE applicantskill (
	ask_id int NOT NULL IDENTITY(1,1),
	ask_app_id int NOT NULL,
	ask_skl_id int NOT NULL,
	ask_first_edit_date datetime,
	ask_last_edit_date datetime,
	PRIMARY KEY(ask_id),
	FOREIGN KEY (ask_app_id) REFERENCES applicant (app_id),
	FOREIGN KEY (ask_skl_id) REFERENCES skill (skl_id)
)