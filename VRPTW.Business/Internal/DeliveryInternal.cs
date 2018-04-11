using System;
using System.Collections.Generic;
using System.Linq;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Business.Internal
{
	internal class DeliveryInternal
	{
		internal void ClusterFractionedTrips(Delivery newfractionedDelivery)
		{
			var fractionedTrips = new List<FractionedTrip>();

			var fractionedScheduledTrips = GetDeliveryTruckTripsByProductType(newfractionedDelivery);

			var depots = GetDepots();	   

			FindRoutes(depots, fractionedScheduledTrips);
		}

		private void FindRoutes(List<Depot> depots, List<DeliveryTruckTrip> fractionedScheduledTrips)
		{
			foreach (var depot in depots)
			{
				var ceplexParameters = GetCeplexParameters(depot, fractionedScheduledTrips);
				int[][] routeMatrix = _ceplexRepository.SolveFractionedTrips(ceplexParameters);
				//GetRouteFinded(depot, fractionedScheduledTrips, ceplexParameters, routeMatrix);
			}
		}

		private void GetRouteFinded(Depot depot, List<DeliveryTruckTrip> fractionedScheduledTrips, 
			CeplexParameters ceplexParameters, int[][] routeMatrix)
		{
			var route = new Route();
			for(int i = 0; i < ceplexParameters.QuantityOfClients; i++)
			{
				var subRoute = new SubRoute();
				double distance = 0;
				if(i == 0)
				{										 
					subRoute.AddressIdOrigin = depot.Adress.AddressId;
					subRoute.AddressIdDestiny = FindAddessIdOfTripInRouteMatrix(i, ceplexParameters.QuantityOfClients + 1, 
						routeMatrix, fractionedScheduledTrips, ceplexParameters, out distance);
					subRoute.Distance = distance;
				}
				else
				{
					var fractionedScheduledTrip = fractionedScheduledTrips.FirstOrDefault(trip => trip.ColumnIndex == i);
					subRoute.AddressIdOrigin = fractionedScheduledTrip.Address.AddressId;
					subRoute.AddressIdDestiny = FindAddessIdOfTripInRouteMatrix(i, ceplexParameters.QuantityOfClients + 1,
						routeMatrix, fractionedScheduledTrips, ceplexParameters, out distance);
					subRoute.Distance = distance;
				}
				route.SubRoutes.Add(subRoute);
			}
		}

		private int FindAddessIdOfTripInRouteMatrix(int originIndex, int numberOfNodes, int[][] routeMatrix, 
			List<DeliveryTruckTrip> fractionedScheduledTrips, CeplexParameters ceplexParameters, out double distance)
		{
			int indexTrip = 0;
			
			for(int i = 0; i < numberOfNodes; i++)
			{
				if(routeMatrix[originIndex][i] == 1)
				{
					indexTrip =  i;
					break;
				}	
			}
			distance = ceplexParameters.Time[originIndex][indexTrip];
			var fractionedScheduledTrip = fractionedScheduledTrips.FirstOrDefault(trip => trip.ColumnIndex == indexTrip);
			return fractionedScheduledTrip.Address.AddressId;
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
					var distance = _googleMapsRepository.GetDistanceBetweenTwoAddresses(depot.Adress, fractionedScheduledTrips[j-1].Address);
					if (distance.HasValue)
					{
						Time[0][j] = distance.Value;
					}
				}
			}

			for (int j = 1; j < numberOfPoints; j++)
			{
				Time[j] = new double[numberOfPoints];
				fractionedScheduledTrips[j-1].ColumnIndex = j;
				for (int i = 0; i < numberOfPoints; i++)
				{
					if ((j == i))
					{			
						Time[j][i] = 0;
					} 
					else if(j < i)
					{
						var distance = _googleMapsRepository.GetDistanceBetweenTwoAddresses(fractionedScheduledTrips[j-1].Address, fractionedScheduledTrips[i-1].Address);
						if (distance.HasValue)
						{
							Time[j][i] = distance.Value;
						}
					}
					else if(j > i)
					{
						Time[j][i] = Time[i][j];
					}
				}
			}

			return Time;
		}

		private List<Depot> GetDepots()
		{
			var depots = _depotRepository.SelectDepots();
			foreach(var depot in depots)
			{
				depot.Adress = _addressRepository.GetAddressByDepotId(depot.DepotId);
			}
			return depots;
		}

		private List<DeliveryTruckTrip> GetDeliveryTruckTripsByProductType(Delivery newfractionedDelivery)
		{
			var fractionedScheduledTrips = _fractionedTripRepository.GetFractionedScheduledDeliveriesByProductType(newfractionedDelivery.ProductType);
			foreach(var newDeliveryTruckTrip in newfractionedDelivery.DeliveriesTruckTips)
			{
				newDeliveryTruckTrip.ClientId = newfractionedDelivery.ClientId;
			}
			fractionedScheduledTrips.AddRange(newfractionedDelivery.DeliveriesTruckTips);
			foreach(var fractionedScheduledTrip in fractionedScheduledTrips)
			{
				fractionedScheduledTrip.Address = _addressRepository.GetAddressByClientId(fractionedScheduledTrip.ClientId);
			}
			return fractionedScheduledTrips;
		}

		internal DeliveryInternal(IFractionedTripRepository fractionedTripRepository, IGoogleMapsRepository googleMapsRepository,
			IDepotRepository depotRepository, IVehicleRepository vehicleRepository, IAddressRepository addressRepository,
			ICeplexRepository ceplexRepository)
		{
			_fractionedTripRepository = fractionedTripRepository;
			_googleMapsRepository = googleMapsRepository;
			_depotRepository = depotRepository;
			_vehicleRepository = vehicleRepository;
			_addressRepository = addressRepository;
			_ceplexRepository = ceplexRepository;
		}

		private IFractionedTripRepository _fractionedTripRepository;
		private IGoogleMapsRepository _googleMapsRepository;
		private IDepotRepository _depotRepository;
		private IVehicleRepository _vehicleRepository;
		private IAddressRepository _addressRepository;
		private ICeplexRepository _ceplexRepository;
	}
}
