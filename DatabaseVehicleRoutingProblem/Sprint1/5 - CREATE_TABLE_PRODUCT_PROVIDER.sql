USE [VehicleRoutingProblem]
GO

IF OBJECT_ID('dbo.ProductProvider') IS NOT NULL
	DROP TABLE dbo.ProductProvider
GO

CREATE TABLE dbo.ProductProvider (
	ProductProviderId INT IDENTITY NOT NULL,
	ProviderName VARCHAR(512) NOT NULL,
	DataCreation Date NOT NULL,
	PRIMARY KEY (ProductProviderId)
) 
GO
