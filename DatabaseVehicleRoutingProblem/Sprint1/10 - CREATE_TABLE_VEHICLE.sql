USE [VehicleRoutingProblem]
GO

IF OBJECT_ID('dbo.Vehicle') IS NOT NULL
	DROP TABLE dbo.Vehicle
GO

CREATE TABLE dbo.Vehicle (
	VehicleId INT IDENTITY NOT NULL,
	Available BIT NOT NULL,
	DepotId INT NOT NULL,
	PRIMARY KEY (VehicleId),
	FOREIGN KEY (DepotId) REFERENCES dbo.Depot (DepotId)
)
GO