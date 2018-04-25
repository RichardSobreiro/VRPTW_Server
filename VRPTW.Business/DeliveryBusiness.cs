using System.Collections.Generic;
using VRPTW.Business.Internal;
using VRPTW.Business.Mapper;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Business;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Business
{
	public class DeliveryBusiness : IDeliveryBusiness
    {
		public void ScheduleDeliveries(List<DeliveryDto> deliveriesToBeScheduled)
		{
			var deliveriesTobeScheduledEntity = deliveriesToBeScheduled.CreateEntity();
			_deliveryInternal.ClusterFractionedTrips(deliveriesTobeScheduledEntity);
		}		
		
		public List<DeliveryDto> GetDeliveriesByFilter(FilterDeliveryDto filterDeliveryDto)
		{
			var filterDelivery = filterDeliveryDto.CreateEntity();
			var deliveries = _deliveryInternal.GetDeliveriesByFilter(filterDelivery);
			var deliveriesDto = deliveries.CreateDto();
			return deliveriesDto;
		}

		public DeliveryBusiness(IFractionedTripRepository fractionedTripRepository, IGoogleMapsRepository googleMapsRepository,
			IDepotRepository depotRepository, IVehicleRepository vehicleRepository, IAddressRepository addressRepository,
			ICeplexRepository ceplexRepository)
		{ 
			_deliveryInternal = new DeliveryInternal(fractionedTripRepository, googleMapsRepository, depotRepository, vehicleRepository, 
				addressRepository, ceplexRepository);
		}  

		private DeliveryInternal _deliveryInternal;	
	}
}
