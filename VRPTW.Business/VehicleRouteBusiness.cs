using System;
using System.Collections.Generic;
using System.Linq;
using VRPTW.Business.Mapper;
using VRPTW.CrossCutting.Extensions;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Business
{
	public class VehicleRouteBusiness : IVehicleRouteBusiness
	{
		public List<VehicleRouteDto> GetVehicleRoutes(VehicleRouteFilterDto filter)
		{
			var vehicleRoutes = _vehicleRouteRepository.GetVehicleRoutes(filter.CreateEntity());
			foreach (var vehicleRoute in vehicleRoutes)
			{
				foreach (var subRoute in vehicleRoute.SubRoutes)
				{
					subRoute.AddressOrigin = _addressRepository.GetAddressByAddressId(subRoute.AddressOriginId);
					subRoute.AddressDestiny = _addressRepository.GetAddressByAddressId(subRoute.AddressDestinyId);
				}
			}
			return vehicleRoutes.CreateDto();
		}

		public void ScheduleDeliveries(List<DeliveryDto> deliveriesToBeScheduled)
		{
			if (ValidateSchedulingOfDeliveries(deliveriesToBeScheduled))
			{
				ClusterDeliveriesOfTheSameClient(deliveriesToBeScheduled);
				var deliveriesToBeScheduledEntity = deliveriesToBeScheduled.CreateEntity();
				var depot = GetBestDepotToAttend(deliveriesToBeScheduled);
				var routes = ClusterFractionedTrips(depot, deliveriesToBeScheduledEntity);
				InsertVehicleRoutes(routes, depot, deliveriesToBeScheduled[0].productType);
			}
		}

		public bool ValidateSchedulingOfDeliveries(List<DeliveryDto> deliveriesToBeScheduled)
		{
			return true;
		}

		public Depot GetBestDepotToAttend(List<DeliveryDto> deliveriesToBeScheduled)
		{
			var depots = _depotRepository.SelectDepots();
			depots[0].Adress = _addressRepository.GetAddressByDepotId(depots[0].DepotId);
			return depots[0];
		}

		public VehicleRouteBusiness(IAddressRepository addressRepository, IDepotRepository depotRepository,
			IGoogleMapsRepository googleMapsRepository, ICeplexRepository ceplexRepository,
			IVehicleRepository vehicleRepository, IVehicleRouteRepository vehicleRouteRepository)
		{
			_addressRepository = addressRepository;
			_depotRepository = depotRepository;
			_googleMapsRepository = googleMapsRepository;
			_ceplexRepository = ceplexRepository;
			_vehicleRepository = vehicleRepository;
			_vehicleRouteRepository = vehicleRouteRepository;
		}

		private List<VehicleRoute> ClusterFractionedTrips(Depot depot,
			List<Delivery> deliveriesTobeScheduled)
		{
			var fractionedScheduledTrips = new List<DeliveryTruckTrip>();
			foreach (var delivery in deliveriesTobeScheduled)
			{
				var deliveryTruckTrips = AllocateQuantityOfProductToTruckTrips(delivery);
				GetAddressByClientId(deliveryTruckTrips);
				fractionedScheduledTrips.AddRange(deliveryTruckTrips);
			}						 
			return FindRoutes(depot, fractionedScheduledTrips);
		}

		private List<DeliveryTruckTrip> AllocateQuantityOfProductToTruckTrips(Delivery delivery)
		{
			var deliveriesTruckTrips = new List<DeliveryTruckTrip>();

			int numberOfTrips = (int)Math.Ceiling(delivery.QuantityProduct / 10);
			double quantityOfProductToDelivery = delivery.QuantityProduct;

			for (int i = 0; i < numberOfTrips; i++)
			{
				var newTrip = new DeliveryTruckTrip()
				{
					DeliveryId = delivery.DeliveryId,
					ClientId = delivery.Client.ClientId,
					ProductType = delivery.ProductType,
					QuantityProduct = GetDeliveryTruckTripQuantityOfProductToDelivery(ref quantityOfProductToDelivery)
				};
				deliveriesTruckTrips.Add(newTrip);
			}

			return deliveriesTruckTrips;
		}

		private double GetDeliveryTruckTripQuantityOfProductToDelivery(ref double remainingQuantityOfProductToDelivery)
		{
			double quantityOfProductToDelivery = 0;

			if (remainingQuantityOfProductToDelivery <= 10)
			{
				quantityOfProductToDelivery = remainingQuantityOfProductToDelivery;
			}
			else
			{
				quantityOfProductToDelivery = 10;
				remainingQuantityOfProductToDelivery -= quantityOfProductToDelivery;
			}

			return quantityOfProductToDelivery;
		}

		private void GetAddressByClientId(List<DeliveryTruckTrip> deliveryTruckTrips)
		{
			foreach (var deliveryTruckTrip in deliveryTruckTrips)
			{
				deliveryTruckTrip.Address = _addressRepository.GetAddressByClientId(deliveryTruckTrip.ClientId);
			}
		}

		private List<Depot> GetDepots()
		{
			var depots = _depotRepository.SelectDepots();
			foreach (var depot in depots)
			{
				depot.Adress = _addressRepository.GetAddressByDepotId(depot.DepotId);
			}
			return depots;
		}   

		private List<VehicleRoute> FindRoutes(Depot depot, List<DeliveryTruckTrip> fractionedScheduledTrips)
		{
			var routes = new List<VehicleRoute>();
			var ceplexParameters = GetCeplexParameters(depot, fractionedScheduledTrips);
			int[][][] routeMatrix = _ceplexRepository.SolveFractionedTrips(ceplexParameters);
			routes = GetRoutesFinded(depot, fractionedScheduledTrips, ceplexParameters, routeMatrix);
			FindSequenceNumberOfRoutes(routes, ceplexParameters, depot);
			return routes;
		}

		private CeplexParameters GetCeplexParameters(Depot depot, List<DeliveryTruckTrip> fractionedScheduledTrips)
		{
			var ceplexParameters = new CeplexParameters();

			var availablesVehicles = _vehicleRepository.GetAvailableVehiclesByDepot(depot.DepotId);
			ceplexParameters.QuantityOfVehiclesAvailable = availablesVehicles.Count;

			ceplexParameters.QuantityOfClients = fractionedScheduledTrips.Count;

			ceplexParameters.VehiclesGreatestPossibleDemand = 11;

			ceplexParameters.GreatestPossibleDemand = (int)fractionedScheduledTrips.Max(trip => trip.QuantityProduct) + 1;

			var numberOfPoints = fractionedScheduledTrips.Count + 1;
			var distancesAndDurations = GetDistancesAndDurations(depot, fractionedScheduledTrips, ceplexParameters.QuantityOfClients);
			ceplexParameters.Distance = distancesAndDurations.Item1;
			ceplexParameters.Duration = distancesAndDurations.Item2;

			ceplexParameters.VehicleCapacity = new int[ceplexParameters.QuantityOfVehiclesAvailable];
			for (int i = 0; i < ceplexParameters.QuantityOfVehiclesAvailable; i++)
			{
				ceplexParameters.VehicleCapacity[i] = 10;
			}

			ceplexParameters.ClientsDemand = new double[ceplexParameters.QuantityOfClients];
			for (int i = 0; i < ceplexParameters.QuantityOfClients; i++)
			{
				ceplexParameters.ClientsDemand[i] = fractionedScheduledTrips[i].QuantityProduct;
			}

			return ceplexParameters;
		}

		private List<VehicleRoute> GetRoutesFinded(Depot depot, List<DeliveryTruckTrip> fractionedScheduledTrips,
			CeplexParameters ceplexParameters, int[][][] routeMatrix)
		{
			var routes = new List<VehicleRoute>();
			for (int k = 0; k < ceplexParameters.QuantityOfVehiclesAvailable; k++)
			{
				var route = new VehicleRoute();
				route.DepotId = depot.DepotId;
				route.DateCreation = DateTime.Now.Date;
				route.DateScheduled = DateTime.Now.Date;
				for(int j = 0; j <= ceplexParameters.QuantityOfClients; j++)
				{
					for (int i = 0; i <= ceplexParameters.QuantityOfClients; i++)
					{
						if(routeMatrix[k][j][i] == 1 && j != i && j == 0)
						{
							var subRoute = new SubRoute();	
							subRoute.AddressOriginId = depot.Adress.AddressId;
							var clientDestiny = fractionedScheduledTrips.FirstOrDefault(c => c.ColumnIndex == i);
							subRoute.AddressDestinyId = clientDestiny.Address.AddressId;
							subRoute.Distance = ceplexParameters.Distance[j][i];
							subRoute.Duration = ceplexParameters.Duration[j][i].ConvertMinutesToDateTime();
							route.SubRoutes.Add(subRoute);
						}
						else if(routeMatrix[k][j][i] == 1 && j != i && j > 0 && i != 0)
						{
							var subRoute = new SubRoute();
							var clientOrigin = fractionedScheduledTrips.FirstOrDefault(c => c.ColumnIndex == j);
							subRoute.AddressOriginId = clientOrigin.Address.AddressId;
							var clientDestiny = fractionedScheduledTrips.FirstOrDefault(c => c.ColumnIndex == i);
							subRoute.AddressDestinyId = clientDestiny.Address.AddressId;
							subRoute.Distance = ceplexParameters.Distance[j][i];
							subRoute.Duration = ceplexParameters.Duration[j][i].ConvertMinutesToDateTime();
							route.SubRoutes.Add(subRoute);
						}
						else if (routeMatrix[k][j][i] == 1 && j != i && j > 0 && i == 0)
						{
							var subRoute = new SubRoute();
							var clientOrigin = fractionedScheduledTrips.FirstOrDefault(c => c.ColumnIndex == j);
							subRoute.AddressOriginId = clientOrigin.Address.AddressId;
							subRoute.AddressDestinyId = depot.Adress.AddressId;
							subRoute.Distance = ceplexParameters.Distance[j][i];
							subRoute.Duration = ceplexParameters.Duration[j][i].ConvertMinutesToDateTime();
							route.SubRoutes.Add(subRoute);
						}
					}
				}  
				if(route.SubRoutes.Count > 0)
					routes.Add(route);
			}
			return routes;
		}

		private Tuple<double[][], long[][]> GetDistancesAndDurations(Depot depot, List<DeliveryTruckTrip> fractionedScheduledTrips,
			int numberOfPoints)
		{				
			numberOfPoints++;
			double[][] Distance = new double[numberOfPoints][];
			long[][] Duration = new long[numberOfPoints][];

			Distance[0] = new double[numberOfPoints];
			Duration[0] = new long[numberOfPoints];
			for (int j = 0; j < numberOfPoints; j++)
			{
				if ((j == 0))
				{
					Distance[0][0] = 0;
					Duration[0][0] = 0;
				}
				else
				{
					long duration = 0;
					var distance = _googleMapsRepository.GetDistanceBetweenTwoAddresses(depot.Adress, 
						fractionedScheduledTrips[j - 1].Address, out duration);
					if (distance.HasValue)
					{
						Distance[0][j] = distance.Value;
						Duration[0][j] = duration;
					}
				}
			}

			for (int j = 1; j < numberOfPoints; j++)
			{
				Distance[j] = new double[numberOfPoints];
				Duration[j] = new long[numberOfPoints];
				fractionedScheduledTrips[j - 1].ColumnIndex = j;
				for (int i = 0; i < numberOfPoints; i++)
				{
					if ((j == i))
					{
						Distance[j][i] = 0;
						Duration[j][i] = 0;
					}
					else if (j < i)
					{
						long duration = 0;
						var distance = _googleMapsRepository.GetDistanceBetweenTwoAddresses(
							fractionedScheduledTrips[j - 1].Address, fractionedScheduledTrips[i - 1].Address, out duration);
						if (distance.HasValue)
						{
							Distance[j][i] = distance.Value;
							Duration[j][i] = duration;
						}
					}
					else if (j > i)
					{
						Distance[j][i] = Distance[i][j];
						Duration[j][i] = Duration[i][j];
					}
				}
			}

			Tuple<double[][],long[][]> DistancesAndDurations = Tuple.Create<double[][],long[][]>(Distance, Duration);
			return DistancesAndDurations;
		}  

		private void FindSequenceNumberOfRoutes(List<VehicleRoute> routes, CeplexParameters ceplexParameters, Depot depot)
		{
			for(var k = 0; k < routes.Count && routes[k].SubRoutes.Count != 0; k++)
			{	
				int indexOfDepot = routes[k].SubRoutes.FindIndex(s => s.AddressOriginId == depot.Adress.AddressId);
				routes[k].SubRoutes[indexOfDepot].SequenceNumber = 1;
				int addressIdPreviousDestiny = routes[k].SubRoutes[indexOfDepot].AddressDestinyId;
				int nextSequenceNumber = 2;
				while(nextSequenceNumber <= routes[k].SubRoutes.Count)
				{
					var nextIndex = routes[k].SubRoutes.FindIndex(s => s.AddressOriginId == addressIdPreviousDestiny);
					routes[k].SubRoutes[nextIndex].SequenceNumber = nextSequenceNumber;
					addressIdPreviousDestiny = routes[k].SubRoutes[nextIndex].AddressDestinyId;
					nextSequenceNumber++;
				}
			}
		}

		private void InsertVehicleRoutes(List<VehicleRoute> vehicleRoutes, Depot depot, int productType)
		{
			var availableVehicles = _vehicleRepository.GetAvailableVehiclesByDepot(depot.DepotId);
			foreach (var vehicleRoute in vehicleRoutes)
			{
				vehicleRoute.productType = productType;
				vehicleRoute.VehicleId = availableVehicles[0].VehicleId;
				vehicleRoute.VehicleRouteId = _vehicleRouteRepository.InsertVehicleRoute(vehicleRoute);
				foreach (var subRoute in vehicleRoute.SubRoutes)
				{
					subRoute.VehicleRouteId = vehicleRoute.VehicleRouteId;
					_vehicleRouteRepository.InsertSubRoute(subRoute);
				}
			}
		}

		private void ClusterDeliveriesOfTheSameClient(List<DeliveryDto> deliveriesToBeScheduled)
		{
			for(var i = 0; i < deliveriesToBeScheduled.Count; i++)
			{
				if(deliveriesToBeScheduled.Count(d => d.client.clientId == deliveriesToBeScheduled[i].client.clientId) > 1)
				{
					deliveriesToBeScheduled[i].quantityProduct = deliveriesToBeScheduled
						.FindAll(d => d.client.clientId == deliveriesToBeScheduled[i].client.clientId)
						.Sum(dSameClient => dSameClient.quantityProduct);
					deliveriesToBeScheduled.RemoveAll(d => 
						d.client.clientId == deliveriesToBeScheduled[i].client.clientId && 
						d.deliveryId != deliveriesToBeScheduled[i].deliveryId);
				}
			}
		}

		private readonly IAddressRepository _addressRepository;
		private readonly IDepotRepository _depotRepository;
		private readonly ICeplexRepository _ceplexRepository;
		private readonly IVehicleRepository _vehicleRepository;
		private readonly IGoogleMapsRepository _googleMapsRepository;
		private readonly IVehicleRouteRepository _vehicleRouteRepository;
	}
}
