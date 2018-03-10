using System;
using System.Collections.Generic;
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
			IDepotRepository depotRepository)
		{
			_fractionedTripRepository = fractionedTripRepository;
			_googleMapsRepository = googleMapsRepository;
			_depotRepository = depotRepository;
		}

		private List<FractionedTrip> ClusterFractionedTrips(Delivery newfractionedDelivery)
		{
			var fractionedTrips = new List<FractionedTrip>();

			var fractionedScheduledTrips = _fractionedTripRepository.GetFractionedScheduledDeliveriesByProductType(newfractionedDelivery.ProductType);

			var depots = _depotRepository.SelectDepots();

			var distancesBetweenClients = FillDistanceBetweenEachAddress(fractionedScheduledTrips);

			var ceplexParameters = GetCeplexParameters(depots, distancesBetweenClients, fractionedScheduledTrips);


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

		private CeplexParameters GetCeplexParameters(List<Depot> depots, Dictionary<Tuple<int, int>, double> distancesBetweenClients,
			List<DeliveryTruckTrip> fractionedScheduledTrips)
		{	
			foreach(var depot in depots)
			{
				var ceplexParameters = new CeplexParameters();

				var numberOfPoints = fractionedScheduledTrips.Count + 1;
				ceplexParameters.Time = new double[numberOfPoints][];
				for(int j = 0; j < numberOfPoints; j++)
				{
					for (int i = 0; i < numberOfPoints; i++)
					{
						if((j == 0 && i == 0) || (j == (numberOfPoints - 1) && i == (numberOfPoints - 1)))
						{
							var distance = _googleMapsRepository.GetDistanceBetweenTwoAddresses(depot.Adress, fractionedScheduledTrips[i].Address);
							if(distance.HasValue)
							{
								ceplexParameters.Time[j][i] = distance.Value;
							}
						}
					}
				}

				ceplexParameters.QuantityOfVehiclesAvailable
			}
			return new CeplexParameters();
		}
														 
		private IFractionedTripRepository _fractionedTripRepository;
		private IGoogleMapsRepository _googleMapsRepository;
		private IDepotRepository _depotRepository;
	}
}
