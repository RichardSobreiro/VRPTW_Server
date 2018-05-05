using System.Transactions;
using VRPTW.Business.Mapper;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Business;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Business
{
	public class CreateDeliveryBusiness: ICreateDeliveryBusiness
	{
		public int CreateFractionedDelivery(DeliveryDto fractionedDeliveryDto)
		{
			var delivery = fractionedDeliveryDto.CreateEntity();
			return InsertDelivery(delivery);
		}

		private int InsertDelivery(Delivery delivery)
		{																					
			using (var transaction = new TransactionScope())
			{
				delivery.DeliveryId = _deliveryRepository.InsertDelivery(delivery);

				transaction.Complete();
			}
			return delivery.DeliveryId;
		}
	
		private IDeliveryRepository _deliveryRepository;
		private IAddressRepository _addressRepository;

		public CreateDeliveryBusiness(IDeliveryRepository deliveryRepository, IDeliveryTruckTripRepository deliveryTruckTripRepository,
			IAddressRepository addressRepository, IFractionedTripRepository fractionedTripRepository, IGoogleMapsRepository googleMapsRepository,
			IDepotRepository depotRepository, IVehicleRepository vehicleRepository, ICeplexRepository ceplexRepository)
		{
			_deliveryRepository = deliveryRepository;
			_addressRepository = addressRepository;
		}
	}
}
