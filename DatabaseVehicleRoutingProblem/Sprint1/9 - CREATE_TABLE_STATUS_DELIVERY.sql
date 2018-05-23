USE [VehicleRoutingProblem]
GO

IF OBJECT_ID('dbo.StatusDelivery') IS NOT NULL
	DROP TABLE dbo.StatusDelivery 
GO

CREATE TABLE dbo.StatusDelivery (
	StatusDeliveryId INT IDENTITY NOT NULL,
	ValueStatus CHAR(1) NOT NULL,
	DescriptionStatus CHAR(64) NOT NULL
	PRIMARY KEY (StatusDeliveryId) 
)
GO

INSERT INTO StatusDelivery(ValueStatus, DescriptionStatus)
VALUES('I', 'Issued')
GO

INSERT INTO StatusDelivery(ValueStatus, DescriptionStatus)
VALUES('S', 'Scheduled')
GO

INSERT INTO StatusDelivery(ValueStatus, DescriptionStatus)
VALUES('P', 'Pendent')
GO

INSERT INTO StatusDelivery(ValueStatus, DescriptionStatus)
VALUES('C', 'Canceled')
GO