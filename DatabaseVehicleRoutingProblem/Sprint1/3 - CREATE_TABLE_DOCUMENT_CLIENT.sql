USE [VehicleRoutingProblem]
GO

IF OBJECT_ID('dbo.DocumentsClient') IS NOT NULL
	DROP TABLE dbo.DocumentsClient 
GO

CREATE TABLE [dbo.DocumentsClient](
	[DocumentType] [INT] IDENTITY NOT NULL,
	[DescriptionDocument] VARCHAR(256) NOT NULL,
	PRIMARY KEY (DocumentType)
)
GO

INSERT INTO dbo.[dbo.DocumentsClient] (DescriptionDocument) VALUES ('CPF')
INSERT INTO dbo.[dbo.DocumentsClient] (DescriptionDocument) VALUES ('CNPJ')

IF OBJECT_ID ('dbo.Client') IS NOT NULL
	DROP TABLE dbo.Client
GO

CREATE TABLE [dbo].[Client](
	[ClientId] [INT] IDENTITY NOT NULL,
	[DateCreation] [DATE] NOT NULL,
	[Name] VARCHAR(512) NULL,
	[DocumentNumber] [INT] UNIQUE NOT NULL,
	[DocumentType] [INT] NOT NULL
	PRIMARY KEY (ClientId),
	FOREIGN KEY (DocumentType) REFERENCES dbo.[dbo.DocumentsClient] (DocumentType)
) 
GO


