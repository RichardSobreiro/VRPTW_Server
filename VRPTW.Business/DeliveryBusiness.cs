using System;
using System.Collections.Generic;
using System.Linq;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Business;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Business
{
	public class DeliveryBusiness : IDeliveryBusiness
    {
		public DeliveryDto ScheduleFractionedTrip(DeliveryDto deliveryTobeScheduled)
		{
			var deliveryScheduled = new DeliveryDto();

			return deliveryScheduled;
		}			 

		public DeliveryBusiness(IFractionedTripRepository fractionedTripRepository, IGoogleMapsRepository googleMapsRepository,
			IDepotRepository depotRepository, IVehicleRepository vehicleRepository)
		{
			_fractionedTripRepository = fractionedTripRepository;
			_googleMapsRepository = googleMapsRepository;
			_depotRepository = depotRepository;
			_vehicleRepository = vehicleRepository;
		}

		private List<FractionedTrip> ClusterFractionedTrips(Delivery newfractionedDelivery)
		{
			var fractionedTrips = new List<FractionedTrip>();

			var fractionedScheduledTrips = _fractionedTripRepository.GetFractionedScheduledDeliveriesByProductType(newfractionedDelivery.ProductType);

			var depots = _depotRepository.SelectDepots();

			var distancesBetweenClients = FillDistanceBetweenEachAddress(fractionedScheduledTrips);

			FindRoutes(depots, distancesBetweenClients, fractionedScheduledTrips);

			return fractionedTrips;
		}

		private Dictionary<Tuple<int, int>, double> FillDistanceBetweenEachAddress(List<DeliveryTruckTrip> fractionedScheduledTrips)
		{
			var distancesBetweenAddresses = new Dictionary<Tuple<int, int>, double>();
			for(int i = 0; i < (fractionedScheduledTrips.Count - 1); i++)
			{
				for(int j = (i + 1); i < fractionedScheduledTrips.Count; j++)
				{
					var newDistance = 
						_googleMapsRepository.GetDistanceBetweenTwoAddresses(fractionedScheduledTrips[i].Address, fractionedScheduledTrips[j].Address);

					distancesBetweenAddresses.Add(
						Tuple.Create<int, int>(fractionedScheduledTrips[i].DeliveryId, fractionedScheduledTrips[j].DeliveryId), newDistance.Value);
				}
			}
			return distancesBetweenAddresses;
		}

		private void FindRoutes(List<Depot> depots, Dictionary<Tuple<int, int>, double> distancesBetweenClients,
			List<DeliveryTruckTrip> fractionedScheduledTrips)
		{	
			foreach(var depot in depots)
			{
				var ceplexParameters = GetCeplexParameters(depot, distancesBetweenClients, fractionedScheduledTrips);

			}							  
		}

		private CeplexParameters GetCeplexParameters(Depot depot, Dictionary<Tuple<int, int>, double> distancesBetweenClients,
			List<DeliveryTruckTrip> fractionedScheduledTrips)
		{
			var ceplexParameters = new CeplexParameters();

			var availablesVehicles = _vehicleRepository.GetAvailableVehiclesByDepot(depot.DepotId);
			ceplexParameters.QuantityOfVehiclesAvailable = availablesVehicles.Count;

			ceplexParameters.QuantityOfClients = fractionedScheduledTrips.Count;

			ceplexParameters.VehiclesGreatestPossibleDemand = 11;

			ceplexParameters.GreatestPossibleDemand = (int)fractionedScheduledTrips.Max(trip => trip.QuantityProduct) + 1;

			var numberOfPoints = fractionedScheduledTrips.Count + 1;
			ceplexParameters.Time = GetDistanceMatrix(depot, distancesBetweenClients, fractionedScheduledTrips, numberOfPoints);

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

		private double[][] GetDistanceMatrix(Depot depot, Dictionary<Tuple<int, int>, double> distancesBetweenClients, 
			List<DeliveryTruckTrip> fractionedScheduledTrips, int numberOfPoints)
		{
			double[][] Time = new double[numberOfPoints][];

			for (int j = 0; j < numberOfPoints; j++)
			{
				for (int i = 0; i < numberOfPoints; i++)
				{
					if ((j == i))
					{
						Time[j][i] = 0;
					}
					else if (j == 0)
					{
						var distance = _googleMapsRepository.GetDistanceBetweenTwoAddresses(depot.Adress, fractionedScheduledTrips[i].Address);
						if (distance.HasValue)
						{
							Time[j][i] = distance.Value;
						}
					}
					else
					{
						double distance = 0;
						if (distancesBetweenClients.TryGetValue(Tuple.Create(fractionedScheduledTrips[j].DeliveryTruckTripId, 
							fractionedScheduledTrips[i].DeliveryTruckTripId), out distance))
						{
							Time[j][i] = distance;
						}
						else if (distancesBetweenClients.TryGetValue(Tuple.Create(fractionedScheduledTrips[i].DeliveryTruckTripId,
							fractionedScheduledTrips[j].DeliveryTruckTripId), out distance))
						{
							Time[j][i] = distance;
						}	
						else
						{
							// TODO: Handle expetion
							throw new Exception();
						}
					}
				}
			}

			return Time;
		}

		private IFractionedTripRepository _fractionedTripRepository;
		private IGoogleMapsRepository _googleMapsRepository;
		private IDepotRepository _depotRepository;
		private IVehicleRepository _vehicleRepository;
	}
}
