USE [VehicleRoutingProblem]
GO

IF OBJECT_ID('dbo.Depot') IS NOT NULL
	DROP TABLE dbo.Depot
GO

CREATE TABLE dbo.Depot (
	DepotId INT IDENTITY NOT NULL,
	Capacity INT NOT NULL,
	DepotDescription VARCHAR(256) NOT NULL
	PRIMARY KEY (DepotId)
)
GO