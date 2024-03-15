CREATE DATABASE TURNERO
GO

USE TURNERO
GO

/*ADMINS*/
CREATE TABLE admins(
  id INT PRIMARY KEY IDENTITY(1, 1),
  [user]  VARCHAR(50) DEFAULT NULL,
  pass VARCHAR(64) DEFAULT NULL,
  [name] VARCHAR(50) DEFAULT NULL,
  surname VARCHAR(50) DEFAULT NULL,
  mail VARCHAR(320) DEFAULT NULL UNIQUE,
  [status] INT DEFAULT NULL,
  change_pass TINYINT DEFAULT NULL,
  token VARCHAR(100) DEFAULT NULL,
  user_config BIT DEFAULT NULL,
  provider_config BIT DEFAULT NULL,
  branch_config BIT DEFAULT NULL,
  created_at DATETIME2(6) DEFAULT NULL,
  updated_at DATETIME2(6) DEFAULT NULL,
  created_by INT DEFAULT NULL,
  updated_by INT DEFAULT NULL
)
GO

CREATE PROC spAdminValidate(
	@user VARCHAR(50),
	@pass VARCHAR(64)
	)
	AS
	BEGIN
		IF(EXISTS(SELECT * FROM admins WHERE [user] = @user AND pass = @pass AND [status] = 1))
		BEGIN
			SELECT id FROM admins WHERE [user] = @user AND pass = @pass AND [status] = 1
		END
		ELSE
		BEGIN
			SELECT '0'
		END
	END
GO	

CREATE PROC spAdminTokenUpdate(
	@mail VARCHAR(320),
	@token VARCHAR(100)
	)
	AS
	BEGIN
		UPDATE admins SET token = @token WHERE mail = @mail
	END
GO

CREATE PROC spAdminPassUpdate(
	@id int,
	@pass VARCHAR(64)
	)
	AS
	BEGIN
		UPDATE admins SET pass = @pass WHERE id = @id
	END
GO

CREATE PROC spAdminCreate(
	@user VARCHAR(50),
	@mail VARCHAR(320),
	@pass VARCHAR(64),
	@name VARCHAR(50),
	@surname VARCHAR(50),
	@user_config BIT,
	@provider_config BIT,
	@branch_config BIT,
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT,
	@created_by INT = 1
	)
	AS
	BEGIN
		IF(NOT EXISTS(SELECT * FROM admins WHERE mail = @mail))
		BEGIN
			INSERT INTO admins([user], mail, pass, [name], surname, user_config, provider_config, branch_config, [status], created_by, created_at) VALUES(@user, @mail, @pass, @name, @surname,	@user_config, @provider_config,	@branch_config, 1, @created_by, GETDATE())
			SET @status = 1
			SET @message = 'El usuario ha sido registrado'
		END
		ELSE
		BEGIN
			SET @status = 0
			SET @message = 'El correo ya se encuentra registrado'
		END
	END
GO

CREATE PROC spAdminRead(
	@id INT
	)
	AS
	BEGIN
		SELECT * FROM admins WHERE id = @id
	END
GO

CREATE PROC spAdminUpdate(
	@id INT,
	@user VARCHAR(50),
	@mail VARCHAR(320),
	@name VARCHAR(50),
	@surname VARCHAR(50),
	@user_config BIT,
	@provider_config BIT,
	@branch_config BIT,
	@status BIT
	)
	AS
	BEGIN
		UPDATE admins SET [user] = @user, mail= @mail, [name] = @name, surname = @surname, user_config = @user_config, provider_config = @provider_config, branch_config = @branch_config, [status] = @status WHERE id = @id
	END
GO

CREATE PROC [spAdminDelete](
	@id INT
	)
	AS
	BEGIN
		DELETE FROM admins WHERE id = @id
	END
GO

DECLARE @msg VARCHAR(100), @status BIT
EXEC spAdminCreate 'test', 'locorbes@hotmail.com', '9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08','test', 'test', 0, 0, 0, @status OUTPUT, @msg OUTPUT
SELECT @msg
SELECT @status
GO

/*REGIONS*/
CREATE TABLE regions (
    id INT PRIMARY KEY IDENTITY(1, 1),
    [name] VARCHAR(100) NOT NULL
)
GO

INSERT INTO regions([name]) VALUES('ARGENTINA'), ('URUGUAY')
GO

/*PROVIDERS*/
CREATE TABLE providers (
  id  INT PRIMARY KEY IDENTITY(1, 1),
  code INT DEFAULT NULL,
  cuit VARCHAR(50) DEFAULT NULL UNIQUE,
  business_name VARCHAR(50) DEFAULT NULL,
  [name] VARCHAR(50) DEFAULT NULL,
  commercial_mail VARCHAR(50) DEFAULT NULL UNIQUE,
  it_mail VARCHAR(50) DEFAULT NULL,
  region_id INT DEFAULT NULL,
  origin VARCHAR(50) DEFAULT NULL,
  [address] VARCHAR(50) DEFAULT NULL,
  observations VARCHAR(255) DEFAULT NULL,
  fc_required TINYINT DEFAULT NULL,
  pass VARCHAR(64) DEFAULT NULL,
  change_pass TINYINT DEFAULT NULL,
  [status] INT DEFAULT NULL,
  token VARCHAR(100) DEFAULT NULL,
  created_at DATETIME2(6) DEFAULT NULL,
  updated_at DATETIME2(6) DEFAULT NULL,
  created_by INT DEFAULT NULL,
  updated_by INT DEFAULT NULL,
  CONSTRAINT FK_providers_regions FOREIGN KEY (region_id) REFERENCES regions(id)
)
GO

CREATE PROC spProviderValidate(
	@user VARCHAR(50),
	@pass VARCHAR(64)
	)
	AS
	BEGIN
		IF(EXISTS(SELECT * FROM providers WHERE cuit = @user AND pass = @pass AND [status] = 1))
		BEGIN
			SELECT id FROM providers WHERE cuit = @user AND pass = @pass AND [status] = 1
		END
		ELSE
		BEGIN
			SELECT '0'
		END
	END
GO	

CREATE PROC spProviderTokenUpdate(
	@mail VARCHAR(320),
	@token VARCHAR(100)
	)
	AS
	BEGIN
		UPDATE providers SET token = @token WHERE commercial_mail = @mail
	END
GO

CREATE PROC spProviderPassUpdate(
	@id int,
	@pass VARCHAR(64)
	)
	AS
	BEGIN
		UPDATE providers SET pass = @pass WHERE id = @id
	END
GO

CREATE PROC spProviderCreate(
	@cuit VARCHAR(50),
	@mail VARCHAR(320),
	@pass VARCHAR(64),
	@region_id INT,
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT,
	@created_by INT = 1
	)
	AS
	BEGIN
		IF(NOT EXISTS(SELECT * FROM providers WHERE cuit = @cuit))
		BEGIN
			INSERT INTO providers(cuit, commercial_mail, pass, [status], region_id, created_by, created_at) VALUES(@cuit, @mail, @pass, 1, @region_id, @created_by, GETDATE())
			SET @status = 1
			SET @message = 'El proveedor ha sido registrado'
		END
		ELSE
		BEGIN
			SET @status = 0
			SET @message = 'El cuit ya se encuentra registrado'
		END
	END
GO

DECLARE @msg VARCHAR(100), @status BIT
EXEC spProviderCreate '123456789', 'locorbes@hotmail.com', '9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08', 1, @status OUTPUT, @msg OUTPUT
SELECT @msg
SELECT @status
GO