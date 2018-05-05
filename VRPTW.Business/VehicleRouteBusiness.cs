using System;
using System.Collections.Generic;
using System.Linq;
using VRPTW.Business.Mapper;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Business
{
	public class VehicleRouteBusiness : IVehicleRouteBusiness
	{
		public void ScheduleDeliveries(List<DeliveryDto> deliveriesToBeScheduled)
		{			  
			var deliveriesToBeScheduledEntity = deliveriesToBeScheduled.CreateEntity();
			var routes = ClusterFractionedTrips(deliveriesToBeScheduledEntity);
			InsertVehicleRoutes(routes);
		}

		public void ValidateSchedulingOfDeliveries()
		{
			
		}

		public VehicleRouteBusiness(IAddressRepository addressRepository, IDepotRepository depotRepository, 
			IGoogleMapsRepository googleMapsRepository, ICeplexRepository ceplexRepository,
			IVehicleRepository vehicleRepository)
		{
			_addressRepository = addressRepository;
			_depotRepository = depotRepository;
			_googleMapsRepository = googleMapsRepository;
			_ceplexRepository = ceplexRepository;
			_vehicleRepository = vehicleRepository;
		}

		private List<VehicleRoute> ClusterFractionedTrips(List<Delivery> deliveriesTobeScheduled)
		{
			var fractionedScheduledTrips = new List<DeliveryTruckTrip>();
			foreach (var delivery in deliveriesTobeScheduled)
			{
				var deliveryTruckTrips = AllocateQuantityOfProductToTruckTrips(delivery);
				GetAddressByClientId(deliveryTruckTrips);
				fractionedScheduledTrips.AddRange(deliveryTruckTrips);
			}
			var depots = GetDepots();
			return FindRoutes(depots, fractionedScheduledTrips);
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

		private List<VehicleRoute> FindRoutes(List<Depot> depots, List<DeliveryTruckTrip> fractionedScheduledTrips)
		{
			var routes = new List<VehicleRoute>();
			foreach (var depot in depots)
			{
				var ceplexParameters = GetCeplexParameters(depot, fractionedScheduledTrips);
				int[][][] routeMatrix = _ceplexRepository.SolveFractionedTrips(ceplexParameters);
				routes = GetRoutesFinded(depot, fractionedScheduledTrips, ceplexParameters, routeMatrix);
			}
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
			ceplexParameters.Time = GetDistanceMatrix(depot, fractionedScheduledTrips, ceplexParameters.QuantityOfClients);

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
				for (int i = 0; i <= ceplexParameters.QuantityOfClients; i++)
				{
					var subRoute = new SubRoute();
					double distance = 0;
					if (i == 0)
					{
						subRoute.AddressOriginId = depot.Adress.AddressId;
						subRoute.AddressDestinyId = FindAddressIdOfTripInRouteMatrix(k, i,
							ceplexParameters.QuantityOfClients + 1, routeMatrix, fractionedScheduledTrips,
							depot, ceplexParameters, out distance);
						subRoute.Distance = distance;
					}
					else
					{
						var fractionedScheduledTrip = fractionedScheduledTrips.FirstOrDefault(trip =>
							trip.ColumnIndex == i);
						subRoute.AddressOriginId = fractionedScheduledTrip.Address.AddressId;
						subRoute.AddressDestinyId = FindAddressIdOfTripInRouteMatrix(k, i,
							ceplexParameters.QuantityOfClients + 1, routeMatrix, fractionedScheduledTrips,
							depot, ceplexParameters, out distance);
						subRoute.Distance = distance;
					}
					route.SubRoutes.Add(subRoute);
				}
				routes.Add(route);
			}
			return routes;
		}

		private double[][] GetDistanceMatrix(Depot depot, List<DeliveryTruckTrip> fractionedScheduledTrips,
			int numberOfPoints)
		{
			numberOfPoints++;
			double[][] Time = new double[numberOfPoints][];

			Time[0] = new double[numberOfPoints];
			for (int j = 0; j < numberOfPoints; j++)
			{
				if ((j == 0))
				{
					Time[0][0] = 0;
				}
				else
				{
					var distance = _googleMapsRepository.GetDistanceBetweenTwoAddresses(depot.Adress, fractionedScheduledTrips[j - 1].Address);
					if (distance.HasValue)
					{
						Time[0][j] = distance.Value;
					}
				}
			}

			for (int j = 1; j < numberOfPoints; j++)
			{
				Time[j] = new double[numberOfPoints];
				fractionedScheduledTrips[j - 1].ColumnIndex = j;
				for (int i = 0; i < numberOfPoints; i++)
				{
					if ((j == i))
					{
						Time[j][i] = 0;
					}
					else if (j < i)
					{
						var distance = _googleMapsRepository.GetDistanceBetweenTwoAddresses(
							fractionedScheduledTrips[j - 1].Address, fractionedScheduledTrips[i - 1].Address);
						if (distance.HasValue)
						{
							Time[j][i] = distance.Value;
						}
					}
					else if (j > i)
					{
						Time[j][i] = Time[i][j];
					}
				}
			}

			return Time;
		}

		private int FindAddressIdOfTripInRouteMatrix(int vehicleNumber, int originIndex, int numberOfNodes,
			int[][][] routeMatrix, List<DeliveryTruckTrip> fractionedScheduledTrips, Depot depot, CeplexParameters ceplexParameters,
			out double distance)
		{
			int indexTrip = 0;
			int i;

			for (i = 0; i < numberOfNodes; i++)
			{
				if (routeMatrix[vehicleNumber][originIndex][i] == 1)
				{
					indexTrip = i;
					break;
				}
			}
			distance = ceplexParameters.Time[originIndex][(i)];
			if (i != 0)
			{
				var fractionedScheduledTrip = fractionedScheduledTrips.FirstOrDefault(trip => trip.ColumnIndex == indexTrip);
				return fractionedScheduledTrip.Address.AddressId;
			}
			else
			{
				return depot.Adress.AddressId;
			}
		}

		private void InsertVehicleRoutes(List<VehicleRoute> vehicleRoutes)
		{
			foreach (var vehicleRoute in vehicleRoutes)
			{
				vehicleRoute.VehicleRouteId = _vehicleRouteRepository.InsertVehicleRoute(vehicleRoute);
				foreach (var subRoute in vehicleRoute.SubRoutes)
				{
					subRoute.VehicleRouteId = vehicleRoute.VehicleRouteId;
					_vehicleRouteRepository.InsertSubRoute(subRoute);
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
