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

		public DeliveryBusiness(IFractionedTripRepository _fractionedTripRepository, IGoogleMapsRepository _googleMapsRepository)
		{
			this._fractionedTripRepository = _fractionedTripRepository;
			this._googleMapsRepository = _googleMapsRepository;
		}

		private List<FractionedTrip> ClusterFractionedTrips(Delivery newfractionedDelivery)
		{
			var fractionedTrips = new List<FractionedTrip>();

			var fractionedScheduledTrips = _fractionedTripRepository.GetFractionedScheduledDeliveriesByProductType(newfractionedDelivery.ProductType);

			var distancesBetweenAddresses = FillDistanceBetweenEachAddress(fractionedScheduledTrips);
			

			
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
														 
		private IFractionedTripRepository _fractionedTripRepository;
		private IGoogleMapsRepository _googleMapsRepository;
	}
}
