USE [VehicleRoutingProblem]
GO

IF OBJECT_ID('dbo.UnityMeasurement') IS NOT NULL
	DROP TABLE dbo.UnityMeasurement
GO

CREATE TABLE dbo.UnityMeasurement (
	UnityMeasurementId INT IDENTITY NOT NULL,
	DescriptionUnityMeasurement VARCHAR(256) NOT NULL
	PRIMARY KEY (UnityMeasurementId) 
) 
GO