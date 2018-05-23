USE [VehicleRoutingProblem]
GO

IF OBJECT_ID ('dbo.SubRoute') IS NOT NULL
	DROP TABLE dbo.SubRoute
GO

CREATE TABLE dbo.SubRoute (
	SubRouteId INT IDENTITY NOT NULL,
	VehicleRouteId INT NOT NULL,
	AddressOriginId INT NOT NULL,
	AddressDestinyId INT NOT NULL,
	Distance FLOAT NOT NULL,
	Duration DATETIME NOT NULL,
	SequenceNumber INT NOT NULL,
	PRIMARY KEY (SubRouteId),
	FOREIGN KEY (VehicleRouteId) REFERENCES dbo.VehicleRoute (VehicleRouteId),
	FOREIGN KEY (AddressOriginId) REFERENCES dbo.Address (AddressId),
	FOREIGN KEY (AddressDestinyId) REFERENCES dbo.Address (AddressId)
) 
GO