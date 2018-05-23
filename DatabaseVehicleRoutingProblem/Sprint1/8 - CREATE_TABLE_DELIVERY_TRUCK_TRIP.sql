USE [VehicleRoutingProblem]
GO

IF OBJECT_ID('dbo.DeliveryTruckTrip') IS NOT NULL
	DROP TABLE dbo.DeliveryTruckTrip
GO

CREATE TABLE dbo.DeliveryTruckTrip (
	DeliveryTruckTripId INT IDENTITY NOT NULL,
	DeliveryId INT NOT NULL,
	ProductType INT NOT NULL,
	QuantityProduct FLOAT NOT NULL,
	TimeTrip DATETIME NOT NULL,
	TimeArrivalClient DATETIME NOT NULL,
	StatusTrip CHAR(1) NOT NULL DEFAULT('S'),
	PRIMARY KEY (DeliveryTruckTripId),
	FOREIGN KEY (DeliveryId) REFERENCES dbo.Delivery (DeliveryId),
	FOREIGN KEY (ProductType) REFERENCES dbo.Product (ProductType)
)
GO
