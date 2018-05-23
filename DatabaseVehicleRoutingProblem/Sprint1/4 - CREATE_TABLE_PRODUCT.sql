USE [VehicleRoutingProblem]
GO

IF OBJECT_ID('dbo.Product') IS NOT NULL
	DROP TABLE dbo.Product
GO

CREATE TABLE dbo.Product (
	ProductType INT IDENTITY NOT NULL,
	DescriptionProduct VARCHAR(256) NOT NULL,
	UnityMeasurementId INT NOT NULL,
	Density NUMERIC(5,3) NOT NULL,
	ProductProviderId INT NOT NULL,
	PRIMARY KEY (ProductType),
	FOREIGN KEY (ProductProviderId) REFERENCES dbo.[ProductProvider] (ProductProviderId),
	FOREIGN KEY (UnityMeasurementId) REFERENCES dbo.[UnityMeasurement] (UnityMeasurementId)
) 
GO