USE [VehicleRoutingProblem]
GO

IF OBJECT_ID('dbo.Address') IS NOT NULL
	DROP TABLE dbo.Address
GO

CREATE TABLE dbo.Address (
	AddressId INT IDENTITY NOT NULL,
	Street VARCHAR(256) NOT NULL,
	StreetNumber INT NOT NULL,
	Neighborhood VARCHAR(256) NOT NULL,
	City VARCHAR(128) NOT NULL,
	CountryState VARCHAR(128) NOT NULL,
	ProductProviderId INT NULL,
	ClientId INT NULL,
	DepotId INT NULL,
	Latitude FLOAT NOT NULL,
	Longitude FLOAT NOT NULL,
	PRIMARY KEY (AddressId),
	FOREIGN KEY (ProductProviderId) REFERENCES dbo.ProductProvider (ProductProviderId),
	FOREIGN KEY (ClientId) REFERENCES dbo.Client (ClientId),
	FOREIGN KEY (DepotId) REFERENCES dbo.Depot (DepotId)
)
GO