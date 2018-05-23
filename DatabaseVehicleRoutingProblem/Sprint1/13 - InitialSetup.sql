INSERT INTO Depot (Capacity, DepotDescription) VALUES (10000, 'Depot 1')

INSERT INTO [dbo].[Address]
           ([Street]
           ,[StreetNumber]
           ,[Neighborhood]
           ,[City]
           ,[CountryState]
           ,[Country]
           ,[ProductProviderId]
           ,[ClientId]
           ,[DepotId]
           ,[Latitude]
           ,[Longitude])
     VALUES
           ('Rua Aníbal Benévolo'
           ,190
           ,'Santa Efigência'
           ,'Belo Horizonte'
           ,'MG'
           ,'Brazil'
           ,NULL
           ,NULL
           ,1
           ,-19.925248
           ,-43.914746)
GO

INSERT INTO  Vehicle (Available, DepotId) VALUES (1, 1)
INSERT INTO  Vehicle (Available, DepotId) VALUES (1, 1)
INSERT INTO  Vehicle (Available, DepotId) VALUES (1, 1)
INSERT INTO  Vehicle (Available, DepotId) VALUES (1, 1)
INSERT INTO  Vehicle (Available, DepotId) VALUES (1, 1)
INSERT INTO  Vehicle (Available, DepotId) VALUES (1, 1)
INSERT INTO  Vehicle (Available, DepotId) VALUES (1, 1)
INSERT INTO  Vehicle (Available, DepotId) VALUES (1, 1)
INSERT INTO  Vehicle (Available, DepotId) VALUES (1, 1)
INSERT INTO  Vehicle (Available, DepotId) VALUES (1, 1)

INSERT INTO UnityMeasurement (DescriptionUnityMeasurement)
VALUES ('kg')
INSERT INTO UnityMeasurement (DescriptionUnityMeasurement)
VALUES ('m³')

INSERT INTO ProductProvider (ProviderName, DataCreation) VALUES ('Provider 1', GETDATE())
INSERT INTO ProductProvider (ProviderName, DataCreation) VALUES ('Provider 2', GETDATE())

INSERT INTO Product (DescriptionProduct, UnityMeasurementId, Density, ProductProviderId)
VALUES ('Product 1', 1, 1.0, 1)

INSERT INTO Product (DescriptionProduct, UnityMeasurementId, Density, ProductProviderId)
VALUES ('Product 2', 1, 2.0, 1)

INSERT INTO Product (DescriptionProduct, UnityMeasurementId, Density, ProductProviderId)
VALUES ('Product 3', 2, 1.0, 2)

INSERT INTO Product (DescriptionProduct, UnityMeasurementId, Density, ProductProviderId)
VALUES ('Product 4', 2, 2.0, 2)