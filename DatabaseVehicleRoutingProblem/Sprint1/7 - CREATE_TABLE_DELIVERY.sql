USE [VehicleRoutingProblem]
GO

IF OBJECT_ID ('dbo.Delivery') IS NOT NULL
	DROP TABLE dbo.Delivery
GO

CREATE TABLE [dbo].[Delivery] (
	DeliveryId INT IDENTITY NOT NULL,
	DateDelivery DATETIME NOT NULL,
	ClientId INT NULL,
	ProductType INT NOT NULL,
	QuantityProduct FLOAT NOT NULL,
	TypeDelivery CHAR(1) NOT NULL DEFAULT('S'),
	StatusDelivery CHAR(1) NOT NULL DEFAULT('S'),
	PRIMARY KEY (DeliveryId),
	FOREIGN KEY (ClientId) REFERENCES dbo.Client (ClientId),
	FOREIGN KEY (ProductType) REFERENCES dbo.Product (ProductType)
)
GO


