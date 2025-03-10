DECLARE @dbName nvarchar(128);
SET @dbName = N'GIGALICIOUS_API';

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE [name] = @dbName)
BEGIN
	CREATE DATABASE GIGALICIOUS_API;
END
GO

USE GIGALICIOUS_API;
GO

DROP TABLE IF EXISTS GIGALICIOUS.dbo.Access;
DROP TABLE IF EXISTS GIGALICIOUS.dbo.Scopes;
DROP TABLE IF EXISTS GIGALICIOUS.dbo.Auth;
GO

CREATE TABLE GIGALICIOUS_API.dbo.Auth (
Id uniqueidentifier not null
	CONSTRAINT PK_Auth PRIMARY KEY
	CONSTRAINT DF_Auth_Id DEFAULT NEWSEQUENTIALID(),
[Key] nvarchar(59) not null,
CreatedAt datetime2 not null
	CONSTRAINT DF_Auth_CreatedAt DEFAULT SYSDATETIME()
);
GO

CREATE TABLE GIGALICIOUS_API.dbo.Scopes (
Id int not null IDENTITY (1,1)
	CONSTRAINT PK_Scopes PRIMARY KEY,
ResourceType nvarchar(50) not null,
AccessLevel nvarchar(50) not null
);
GO

CREATE TABLE GIGALICIOUS_API.dbo.Access (
Auth_Id uniqueidentifier not null
	CONSTRAINT FK_Auth_Access FOREIGN KEY (Auth_Id) REFERENCES Auth (Id),
Scopes_Id int not null
	CONSTRAINT FK_Scopes_Id FOREIGN KEY (Scopes_Id) REFERENCES Scopes (Id),
GrantedAt datetime2 not null
	CONSTRAINT DF_Access_GrantedAt DEFAULT SYSDATETIME()
);
GO

INSERT INTO GIGALICIOUS_API.dbo.Scopes (ResourceType, AccessLevel)
VALUES ('Sample', 'Read');

INSERT INTO GIGALICIOUS_API.dbo.Auth ([Key])
VALUES ('00000000-0000-0000-0000-000000000000');

DECLARE @sampleId uniqueidentifier;
SET @sampleId = (SELECT TOP (1) Id FROM Auth);

INSERT INTO GIGALICIOUS_API.dbo.Access (Auth_Id, Scopes_Id)
VALUES (@sampleId, 1)

