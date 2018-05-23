USE [VehicleRoutingProblem]
GO

IF OBJECT_ID('dbo.VehicleRoute') IS NOT NULL
	DROP TABLE dbo.VehicleRoute
GO

CREATE TABLE dbo.VehicleRoute (
	VehicleRouteId INT IDENTITY NOT NULL,
	DateCreation DATETIME NOT NULL,
	DateScheduled DATETIME NOT NULL,
	DepartureTime DATETIME NULL,
	EstimatedTimeReturn DATETIME NULL,
	VehicleId INT NOT NULL,
	DepotId INT NOT NULL,
	ProductType INT NOT NULL,
	PRIMARY KEY (VehicleRouteId),
	FOREIGN KEY (VehicleId) REFERENCES dbo.Vehicle(VehicleId),
	FOREIGN KEY (DepotId) REFERENCES dbo.Depot (DepotId),
	FOREIGN KEY (ProductType) REFERENCES dbo.Product (ProductType)
	)
GO

