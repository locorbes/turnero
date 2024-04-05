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

CREATE PROCEDURE spAdminList(
    @init INT = 0,
    @rows INT = 100,
    @order_row NVARCHAR(100) = 'id'
	)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @sql NVARCHAR(MAX);

		SET @sql = 'SELECT * FROM (
						SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_row + ') AS RowNum, *
							FROM admins
						) AS Sub
					WHERE RowNum BETWEEN ' + CAST(@init + 1 AS NVARCHAR(10)) + ' AND ' + CAST(@init + @rows AS NVARCHAR(10)) + ';';

		EXEC sp_executesql @sql;
	END
GO

--EXEC spAdminList @init = 0, @rows = 100, @order_row = 'id';

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
	@status BIT,
	@updated_by INT
	)
	AS
	BEGIN
		UPDATE admins SET [user] = @user, mail= @mail, [name] = @name, surname = @surname, user_config = @user_config, provider_config = @provider_config, branch_config = @branch_config, [status] = @status, updated_by = @updated_by, updated_at = GETDATE() WHERE id = @id
	END
GO

CREATE PROC spAdminDelete(
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
CREATE TABLE regions(
    id INT PRIMARY KEY IDENTITY(1, 1),
    [name] VARCHAR(100) NOT NULL
)
GO

INSERT INTO regions([name]) VALUES('ARGENTINA'), ('URUGUAY')
GO

CREATE PROCEDURE spRegionList(
    @init INT = 0,
    @rows INT = 100,
    @order_row NVARCHAR(100) = 'id'
	)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @sql NVARCHAR(MAX);

		SET @sql = 'SELECT * FROM (
						SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_row + ') AS RowNum, *
							FROM regions
						) AS Sub
					WHERE RowNum BETWEEN ' + CAST(@init + 1 AS NVARCHAR(10)) + ' AND ' + CAST(@init + @rows AS NVARCHAR(10)) + ';';

		EXEC sp_executesql @sql;
	END
GO

--EXEC spRegionList @init = 0, @rows = 1, @order_row = 'id';

CREATE PROC spRegionCreate(
	@name VARCHAR(100),
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT,
	@created_by INT = 1
	)
	AS
	BEGIN
		IF(NOT EXISTS(SELECT * FROM regions WHERE [name] = @name))
		BEGIN
			INSERT INTO admins([name], created_by, created_at) VALUES(@name, @created_by, GETDATE())
			SET @status = 1
			SET @message = 'La region ha sido registrada'
		END
		ELSE
		BEGIN
			SET @status = 0
			SET @message = 'El region ya se encuentra registrada'
		END
	END
GO

CREATE PROC spRegionRead(
	@id INT
	)
	AS
	BEGIN
		SELECT * FROM regions WHERE id = @id
	END
GO

CREATE PROC spRegionUpdate(
	@id INT,
	@name VARCHAR(100)
	)
	AS
	BEGIN
		UPDATE regions SET [name] = @name WHERE id = @id
	END
GO

CREATE PROC spRegionDelete(
	@id INT
	)
	AS
	BEGIN
		DELETE FROM regions WHERE id = @id
	END
GO

/*PROVIDERS*/
CREATE TABLE providers(
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

CREATE PROCEDURE spProviderList(
    @init INT = 0,
    @rows INT = 100,
    @order_row NVARCHAR(100) = 'id'
	)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @sql NVARCHAR(MAX);

		SET @sql = 'SELECT * FROM (
						SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_row + ') AS RowNum, *
							FROM providers
						) AS Sub
					WHERE RowNum BETWEEN ' + CAST(@init + 1 AS NVARCHAR(10)) + ' AND ' + CAST(@init + @rows AS NVARCHAR(10)) + ';';

		EXEC sp_executesql @sql;
	END
GO

--EXEC spProviderList @init = 0, @rows = 1, @order_row = 'id';

CREATE PROC spProviderCreate(
	@cuit VARCHAR(50),
	@commercial_mail VARCHAR(50),
	@pass VARCHAR(64),
	@region_id INT,
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT,
	@code INT = NULL,
	@business_name VARCHAR(50) = NULL,
	@name VARCHAR(50) = NULL,
	@it_mail VARCHAR(50) = NULL,
	@origin VARCHAR(50) = NULL,
    @address VARCHAR(50) = NULL,
	@observations VARCHAR(255) = NULL,
	@fc_required TINYINT = NULL,
	@created_by INT = 1
	)
	AS
	BEGIN
		IF(NOT EXISTS(SELECT * FROM providers WHERE cuit = @cuit))
		BEGIN
			INSERT INTO providers(cuit, commercial_mail, pass, [status], region_id, code, business_name, [name], it_mail, origin, [address], observations, fc_required, created_by, created_at) VALUES(@cuit, @commercial_mail, @pass, 1, @region_id, @code, @business_name, @name, @it_mail, @origin, @address, @observations, @fc_required, @created_by, GETDATE())
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

CREATE PROC spProviderRead(
	@id INT
	)
	AS
	BEGIN
		SELECT * FROM providers WHERE id = @id
	END
GO

CREATE PROC spProviderUpdate(
	@id INT,
	@cuit VARCHAR(50),
	@commercial_mail VARCHAR(50),
	@region_id INT,
	@code INT = NULL,
	@business_name VARCHAR(50) = NULL,
	@name VARCHAR(50) = NULL,
	@it_mail VARCHAR(50) = NULL,
	@origin VARCHAR(50) = NULL,
    @address VARCHAR(50) = NULL,
	@observations VARCHAR(255) = NULL,
	@fc_required TINYINT = NULL,
	@updated_by INT
	)
	AS
	BEGIN
		UPDATE providers SET cuit = @cuit, commercial_mail = @commercial_mail, region_id = @region_id, code = @code, business_name = @business_name, [name] = @name, it_mail = @it_mail, origin = @origin, [address] = @address, observations = @observations, fc_required = @fc_required, updated_by = @updated_by, updated_at = GETDATE() WHERE id = @id
	END
GO

CREATE PROC spProviderDelete(
	@id INT
	)
	AS
	BEGIN
		DELETE FROM providers WHERE id = @id
	END
GO

/*BRANCHS*/
CREATE TABLE branchs(
  id  INT PRIMARY KEY IDENTITY(1, 1),
  [name] VARCHAR(50) DEFAULT NULL,
  code VARCHAR(50) DEFAULT NULL,
  commercial_mail VARCHAR(50) DEFAULT NULL,
  it_mail VARCHAR(50) DEFAULT NULL,
  region_id INT DEFAULT NULL,
  [address] VARCHAR(50) DEFAULT NULL,
  longitude VARCHAR(50) DEFAULT NULL,
  latitude VARCHAR(50) DEFAULT NULL,
  company_id INT DEFAULT NULL,
  created_at DATETIME DEFAULT NULL,
  updated_at DATETIME DEFAULT NULL,
  created_by INT DEFAULT NULL,
  updated_by INT DEFAULT NULL,
  CONSTRAINT FK_branchs_regions FOREIGN KEY (region_id) REFERENCES regions(id)
)
GO

CREATE PROCEDURE spBranchList(
    @init INT = 0,
    @rows INT = 100,
    @order_row NVARCHAR(100) = 'id'
	)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @sql NVARCHAR(MAX);

		SET @sql = 'SELECT * FROM (
						SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_row + ') AS RowNum, *
							FROM branchs
						) AS Sub
					WHERE RowNum BETWEEN ' + CAST(@init + 1 AS NVARCHAR(10)) + ' AND ' + CAST(@init + @rows AS NVARCHAR(10)) + ';';

		EXEC sp_executesql @sql;
	END
GO

--EXEC spBranchList @init = 0, @rows = 1, @order_row = 'id';


CREATE PROC spBranchRead(
	@id INT
	)
	AS
	BEGIN
		SELECT * FROM branchs WHERE id = @id
	END
GO

CREATE PROC spBranchCreate(
	@name VARCHAR(50),
	@code VARCHAR(50),
	@commercial_mail VARCHAR(50),
	@it_mail VARCHAR(50),
	@region_id INT,
	@address VARCHAR(50),
	@longitude VARCHAR(50),
	@latitude VARCHAR(50),
	@company_id INT,
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT,
	@created_by INT = 1
	)
	AS
	BEGIN
		IF(NOT EXISTS(SELECT * FROM branchs WHERE [name] = @name))
		BEGIN
			INSERT INTO branchs([name], code, commercial_mail, it_mail, region_id, [address], longitude, latitude, company_id, created_by, created_at) VALUES( @name, @code, @commercial_mail, @it_mail, @region_id, @address, @longitude, @latitude, @company_id, @created_by, GETDATE())
			SET @status = 1
			SET @message = 'La sucursal ha sido registrada'
		END
		ELSE
		BEGIN
			SET @status = 0
			SET @message = 'La sucursal ya se encuentra registrada'
		END
	END
GO
CREATE PROC spBranchUpdate(
	@id INT,
	@name VARCHAR(50),
	@code VARCHAR(50),
	@commercial_mail VARCHAR(50),
	@it_mail VARCHAR(50),
	@region_id INT,
	@address VARCHAR(50),
	@longitude VARCHAR(50),
	@latitude VARCHAR(50),
	@company_id INT,
	@updated_by INT
	)
	AS
	BEGIN
		UPDATE branchs SET [name] = @name, code = @code, commercial_mail = @commercial_mail, it_mail = @it_mail,region_id = @region_id, [address] = @address, longitude = @longitude, latitude = @latitude, company_id = @company_id, updated_by = @updated_by, updated_at = GETDATE() WHERE id = @id
	END
GO

CREATE PROC spBranchDelete(
	@id INT
	)
	AS
	BEGIN
		DELETE FROM branchs WHERE id = @id
	END
GO

/*TURNS*/
CREATE TABLE turns(
  id  INT PRIMARY KEY IDENTITY(1, 1),
  provider_id INT,
  branch_id INT,
  [time] DATETIME,
  entry_time DATETIME,
  [absent] BIT,
  [status] INT,
  observations VARCHAR(255) NULL,
  created_at DATETIME DEFAULT NULL,
  updated_at DATETIME DEFAULT NULL,
  created_by INT DEFAULT NULL,
  updated_by INT DEFAULT NULL,
  CONSTRAINT FK_turns_provider FOREIGN KEY (provider_id) REFERENCES providers(id),
  CONSTRAINT FK_turns_branch FOREIGN KEY (branch_id) REFERENCES branchs(id)
)
GO

CREATE PROCEDURE spTurnList(
    @init INT = 0,
    @rows INT = 100,
    @order_row NVARCHAR(100) = 'id'
	)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @sql NVARCHAR(MAX);

		SET @sql = 'SELECT * FROM (
						SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_row + ') AS RowNum, *
							FROM turns
						) AS Sub
					WHERE RowNum BETWEEN ' + CAST(@init + 1 AS NVARCHAR(10)) + ' AND ' + CAST(@init + @rows AS NVARCHAR(10)) + ';';

		EXEC sp_executesql @sql;
	END
GO

--EXEC spTurnList @init = 0, @rows = 1, @order_row = 'id';


CREATE PROC spTurnRead(
	@id INT
	)
	AS
	BEGIN
		SELECT * FROM turns WHERE id = @id
	END
GO

CREATE PROC spTurnCreate(
	@provider_id INT,
	@branch_id INT,
	@time DATETIME,
	@entry_time DATETIME,
	@absent BIT,
	@observations VARCHAR(255) = NULL,
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT,
	@created_by INT = 1
	)
	AS
	BEGIN
		IF(NOT EXISTS(SELECT * FROM turns WHERE branch_id = @branch_id AND [time] = @time))
		BEGIN
			INSERT INTO turns(provider_id, branch_id, [time], entry_time, [absent], [status], observations, created_by, created_at) VALUES(@provider_id, @branch_id, @time, @entry_time, @absent, 1, @observations, @created_by, GETDATE())
			SET @status = 1
			SET @message = 'El turno ha sido registrado'
		END
		ELSE
		BEGIN
			SET @status = 0
			SET @message = 'El turno ya se encuentra registrado'
		END
	END
GO

CREATE PROC spTurnUpdate(
	@id INT,
	@provider_id INT,
	@branch_id INT,
	@time DATETIME,
	@entry_time DATETIME,
	@absent BIT,
	@status BIT,
	@observations VARCHAR(255) = NULL,
	@updated_by INT
	)
	AS
	BEGIN
		UPDATE turns SET provider_id = @provider_id, branch_id = @branch_id, [time] = @time, entry_time = @entry_time, [absent] = @absent, [status] = @status, observations = @observations, updated_by = @updated_by, updated_at = GETDATE() WHERE id = @id
	END
GO

CREATE PROC spTurnDelete(
	@id INT
	)
	AS
	BEGIN
		DELETE FROM turns WHERE id = @id
	END
GO

/*FILES*/
CREATE TABLE files(
  id  INT PRIMARY KEY IDENTITY(1, 1),
  turn_id INT,
  code VARCHAR(50),
  CONSTRAINT FK_files_turn FOREIGN KEY (turn_id) REFERENCES turns(id)
)
GO

CREATE PROCEDURE spFileList(
    @init INT = 0,
    @rows INT = 100,
    @order_row NVARCHAR(100) = 'id'
	)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @sql NVARCHAR(MAX);

		SET @sql = 'SELECT * FROM (
						SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_row + ') AS RowNum, *
							FROM files
						) AS Sub
					WHERE RowNum BETWEEN ' + CAST(@init + 1 AS NVARCHAR(10)) + ' AND ' + CAST(@init + @rows AS NVARCHAR(10)) + ';';

		EXEC sp_executesql @sql;
	END
GO

--EXEC spFileList @init = 0, @rows = 1, @order_row = 'id';

CREATE PROC spFileRead(
	@id INT
	)
	AS
	BEGIN
		SELECT * FROM files WHERE id = @id
	END
GO

CREATE PROC spFileCreate(
	@turn_id INT,
	@code VARCHAR(50) = NULL,
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT
	)
	AS
	BEGIN
		INSERT INTO files(turn_id, code) VALUES(@turn_id, @code)
		SET @status = 1
		SET @message = 'El archivo ha sido registrado'
	END
GO

CREATE PROC spFileUpdate(
	@id INT,
	@turn_id INT,
	@code VARCHAR(50) = NULL
	)
	AS
	BEGIN
		UPDATE files SET turn_id = @turn_id, code = @code WHERE id = @id
	END
GO

CREATE PROC spFileDelete(
	@id INT
	)
	AS
	BEGIN
		DELETE FROM files WHERE id = @id
	END
GO

/*ADMIN_BRANCH*/
CREATE TABLE admin_branch(
  id  INT PRIMARY KEY IDENTITY(1, 1),
  admin_id INT,
  branch_id INT,
  profile_id INT,
  confirm BIT,
  created_at DATETIME,
  updated_at DATETIME,
  created_by INT,
  updated_by INT,
  CONSTRAINT FK_admin_branch FOREIGN KEY (admin_id) REFERENCES admins(id),
  CONSTRAINT FK_branch_admin FOREIGN KEY (branch_id) REFERENCES branchs(id)
) 
GO

CREATE PROCEDURE spAdminBranchList(
    @init INT = 0,
    @rows INT = 100,
    @order_row NVARCHAR(100) = 'id'
	)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @sql NVARCHAR(MAX);

		SET @sql = 'SELECT * FROM (
						SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_row + ') AS RowNum, *
							FROM admin_branch
						) AS Sub
					WHERE RowNum BETWEEN ' + CAST(@init + 1 AS NVARCHAR(10)) + ' AND ' + CAST(@init + @rows AS NVARCHAR(10)) + ';';

		EXEC sp_executesql @sql;
	END
GO

--EXEC spAdminBranchList @init = 0, @rows = 1, @order_row = 'id';


CREATE PROC spAdminBranchRead(
	@id INT
	)
	AS
	BEGIN
		SELECT * FROM admin_branch WHERE id = @id
	END
GO

CREATE PROC spAdminBranchCreate(
	@admin_id INT,
	@branch_id INT,
	@profile_id INT,
	@confirm BIT,
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT,
	@created_by INT = 1
	)
	AS
	BEGIN
		IF(NOT EXISTS(SELECT * FROM admin_branch WHERE admin_id = @admin_id AND branch_id = @branch_id))
		BEGIN
			INSERT INTO admin_branch(admin_id, branch_id, profile_id, confirm, created_by, created_at) VALUES(@admin_id, @branch_id, @profile_id, @confirm, @created_by, GETDATE())
			SET @status = 1
			SET @message = 'El admin y la sucursal ha sido registrado'
		END
		ELSE
		BEGIN
			SET @status = 0
			SET @message = 'El admin y la sucursal ya se encuentran registrado'
		END
	END
GO

CREATE PROC spAdminBranchUpdate(
	@id INT,
	@admin_id INT,
	@branch_id INT,
	@profile_id INT,
	@confirm BIT,
	@updated_by INT
	)
	AS
	BEGIN
		UPDATE admin_branch SET admin_id = @admin_id, branch_id = @branch_id, profile_id = @profile_id, confirm = @confirm, updated_by = @updated_by, updated_at = GETDATE() WHERE id = @id
	END
GO

CREATE PROC spAdminBranchDelete(
	@id INT
	)
	AS
	BEGIN
		DELETE FROM admin_branch WHERE id = @id
	END
GO

/*PROVIDER_BRANCH*/
CREATE TABLE provider_branch(
  id  INT PRIMARY KEY IDENTITY(1, 1),
  provider_id INT,
  branch_id INT,
  profile_id INT,
  created_at DATETIME,
  updated_at DATETIME,
  created_by INT,
  updated_by INT,
  CONSTRAINT FK_provider_branch FOREIGN KEY (provider_id) REFERENCES providers(id),
  CONSTRAINT FK_branch_provider FOREIGN KEY (branch_id) REFERENCES branchs(id)
) 
GO

CREATE PROCEDURE spProviderBranchList(
    @init INT = 0,
    @rows INT = 100,
    @order_row NVARCHAR(100) = 'id'
	)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @sql NVARCHAR(MAX);

		SET @sql = 'SELECT * FROM (
						SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_row + ') AS RowNum, *
							FROM provider_branch
						) AS Sub
					WHERE RowNum BETWEEN ' + CAST(@init + 1 AS NVARCHAR(10)) + ' AND ' + CAST(@init + @rows AS NVARCHAR(10)) + ';';

		EXEC sp_executesql @sql;
	END
GO

--EXEC spProviderBranchList @init = 0, @rows = 1, @order_row = 'id';


CREATE PROC spProviderBranchRead(
	@id INT
	)
	AS
	BEGIN
		SELECT * FROM provider_branch WHERE id = @id
	END
GO

CREATE PROC spProviderBranchCreate(
	@provider_id INT,
	@branch_id INT,
	@profile_id INT,
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT,
	@created_by INT = 1
	)
	AS
	BEGIN
		IF(NOT EXISTS(SELECT * FROM provider_branch WHERE provider_id = @provider_id AND branch_id = @branch_id))
		BEGIN
			INSERT INTO provider_branch(provider_id, branch_id, profile_id, created_by, created_at) VALUES(@provider_id, @branch_id, @profile_id, @created_by, GETDATE())
			SET @status = 1
			SET @message = 'El proveedor y la sucursal ha sido registrado'
		END
		ELSE
		BEGIN
			SET @status = 0
			SET @message = 'El proveedor y la sucursal ya se encuentran registrado'
		END
	END
GO

CREATE PROC spProviderBranchUpdate(
	@id INT,
	@provider_id INT,
	@branch_id INT,
	@profile_id INT,
	@updated_by INT
	)
	AS
	BEGIN
		UPDATE provider_branch SET provider_id = @provider_id, branch_id = @branch_id, profile_id = @profile_id, updated_by = @updated_by, updated_at = GETDATE() WHERE id = @id
	END
GO

CREATE PROC spProviderBranchDelete(
	@id INT
	)
	AS
	BEGIN
		DELETE FROM provider_branch WHERE id = @id
	END
GO

/*SCHEDULE_CONFIG*/
CREATE TABLE schedule_config(
  id  INT PRIMARY KEY IDENTITY(1, 1),
  branch_id INT,
  [day] INT,
  since TIME(6),
  until TIME(6),
  turn_minutes INT,
  turn_maximum INT,
  created_at DATETIME,
  updated_at DATETIME,
  created_by INT,
  updated_by INT,
  CONSTRAINT FK_schedule_config_branch FOREIGN KEY (branch_id) REFERENCES branchs(id)
) 
GO

CREATE PROCEDURE spScheduleConfigList(
    @init INT = 0,
    @rows INT = 100,
    @order_row NVARCHAR(100) = 'id'
	)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @sql NVARCHAR(MAX);

		SET @sql = 'SELECT * FROM (
						SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_row + ') AS RowNum, *
							FROM schedule_config
						) AS Sub
					WHERE RowNum BETWEEN ' + CAST(@init + 1 AS NVARCHAR(10)) + ' AND ' + CAST(@init + @rows AS NVARCHAR(10)) + ';';

		EXEC sp_executesql @sql;
	END
GO

--EXEC spScheduleConfigList @init = 0, @rows = 1, @order_row = 'id';


CREATE PROC spScheduleConfigRead(
	@id INT
	)
	AS
	BEGIN
		SELECT * FROM schedule_config WHERE id = @id
	END
GO

CREATE PROC spScheduleConfigCreate(
	@branch_id INT,
	@day INT,
	@since TIME(6),
	@until TIME(6),
	@turn_minutes INT,
	@turn_maximum INT,
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT,
	@created_by INT = 1
	)
	AS
	BEGIN
		IF(NOT EXISTS(SELECT * FROM schedule_config WHERE branch_id = @branch_id AND [day] = @day AND @since = since AND @until = until AND @turn_minutes = turn_minutes AND @turn_maximum = turn_maximum))
		BEGIN
			INSERT INTO schedule_config(branch_id, [day], since, until, turn_minutes, turn_maximum, created_by, created_at) VALUES(@branch_id, @day, @since, @until, @turn_minutes, @turn_maximum, @created_by, GETDATE())
			SET @status = 1
			SET @message = 'La configuración ha sido registrada'
		END
		ELSE
		BEGIN
			SET @status = 0
			SET @message = 'La configuración ya se encuentra registrada'
		END
	END
GO

CREATE PROC spScheduleConfigUpdate(
	@id INT,
	@branch_id INT,
	@day INT,
	@since TIME(6),
	@until TIME(6),
	@turn_minutes INT,
	@turn_maximum INT,
	@updated_by INT
	)
	AS
	BEGIN
		UPDATE schedule_config SET branch_id = @branch_id, [day] = @day, since = @since, until = @until, turn_minutes = @turn_minutes, turn_maximum = @turn_maximum, updated_by = @updated_by, updated_at = GETDATE() WHERE id = @id
	END
GO

CREATE PROC spScheduleConfigDelete(
	@id INT
	)
	AS
	BEGIN
		DELETE FROM schedule_config WHERE id = @id
	END
GO

/*SCHEDULE_EXCEPTION*/
CREATE TABLE schedule_exception(
  id  INT PRIMARY KEY IDENTITY(1, 1),
  branch_id INT,
  [day] INT,
  since TIME(6),
  until TIME(6),
  turn_minutes INT,
  turn_maximum INT,
  created_at DATETIME,
  updated_at DATETIME,
  created_by INT,
  updated_by INT,
  CONSTRAINT FK_schedule_exception_branch FOREIGN KEY (branch_id) REFERENCES branchs(id)
) 
GO

CREATE PROCEDURE spScheduleExceptionList(
    @init INT = 0,
    @rows INT = 100,
    @order_row NVARCHAR(100) = 'id'
	)
AS
	BEGIN
		SET NOCOUNT ON;

		DECLARE @sql NVARCHAR(MAX);

		SET @sql = 'SELECT * FROM (
						SELECT ROW_NUMBER() OVER (ORDER BY ' + @order_row + ') AS RowNum, *
							FROM schedule_exception
						) AS Sub
					WHERE RowNum BETWEEN ' + CAST(@init + 1 AS NVARCHAR(10)) + ' AND ' + CAST(@init + @rows AS NVARCHAR(10)) + ';';

		EXEC sp_executesql @sql;
	END
GO

--EXEC spScheduleExceptionList @init = 0, @rows = 1, @order_row = 'id';


CREATE PROC spScheduleExceptionRead(
	@id INT
	)
	AS
	BEGIN
		SELECT * FROM schedule_exception WHERE id = @id
	END
GO

CREATE PROC spScheduleExceptionCreate(
	@branch_id INT,
	@day INT,
	@since TIME(6),
	@until TIME(6),
	@turn_minutes INT,
	@turn_maximum INT,
	@status BIT OUTPUT,
	@message VARCHAR(100) OUTPUT,
	@created_by INT = 1
	)
	AS
	BEGIN
		IF(NOT EXISTS(SELECT * FROM schedule_exception WHERE branch_id = @branch_id AND [day] = @day AND @since = since AND @until = until AND @turn_minutes = turn_minutes AND @turn_maximum = turn_maximum))
		BEGIN
			INSERT INTO schedule_exception(branch_id, [day], since, until, turn_minutes, turn_maximum, created_by, created_at) VALUES(@branch_id, @day, @since, @until, @turn_minutes, @turn_maximum, @created_by, GETDATE())
			SET @status = 1
			SET @message = 'La excepción ha sido registrada'
		END
		ELSE
		BEGIN
			SET @status = 0
			SET @message = 'La excepción ya se encuentra registrada'
		END
	END
GO

CREATE PROC spScheduleExceptionUpdate(
	@id INT,
	@branch_id INT,
	@day INT,
	@since TIME(6),
	@until TIME(6),
	@turn_minutes INT,
	@turn_maximum INT,
	@updated_by INT
	)
	AS
	BEGIN
		UPDATE schedule_exception SET branch_id = @branch_id, [day] = @day, since = @since, until = @until, turn_minutes = @turn_minutes, turn_maximum = @turn_maximum, updated_by = @updated_by, updated_at = GETDATE() WHERE id = @id
	END
GO

CREATE PROC spScheduleExceptionDelete(
	@id INT
	)
	AS
	BEGIN
		DELETE FROM schedule_exception WHERE id = @id
	END
GO