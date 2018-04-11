using System;
using System.Collections.Generic;
using System.Linq;
using VRPTW.Business.Internal;
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
			//_deliveryInternal.ClusterFractionedTrips(deliveryTobeScheduled)
			return deliveryScheduled;
		}			 

		public DeliveryBusiness(IFractionedTripRepository fractionedTripRepository, IGoogleMapsRepository googleMapsRepository,
			IDepotRepository depotRepository, IVehicleRepository vehicleRepository, IAddressRepository addressRepository,
			ICeplexRepository ceplexRepository)
		{
			_fractionedTripRepository = fractionedTripRepository;
			_googleMapsRepository = googleMapsRepository;
			_depotRepository = depotRepository;
			_vehicleRepository = vehicleRepository;
			_deliveryInternal = new DeliveryInternal(fractionedTripRepository, googleMapsRepository, depotRepository, vehicleRepository, 
				addressRepository, ceplexRepository);
		}  

		private DeliveryInternal _deliveryInternal;
		private IFractionedTripRepository _fractionedTripRepository;
		private IGoogleMapsRepository _googleMapsRepository;
		private IDepotRepository _depotRepository;
		private IVehicleRepository _vehicleRepository;
	}
}
