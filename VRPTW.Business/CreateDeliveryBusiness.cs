using System;
using System.Collections.Generic;
using System.Transactions;
using VRPTW.Business.Internal;
using VRPTW.Business.Mapper;
using VRPTW.CrossCutting.Enumerations;
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

				delivery.DeliveriesTruckTips = AllocateQuantityOfProductToTruckTrips(delivery);

				_deliveryInternal.ClusterFractionedTrips(delivery);

				InsertDeliveriesTruckTrips(delivery);

				transaction.Complete();
			}
			return delivery.DeliveryId;
		}

		private void InsertDeliveriesTruckTrips(Delivery delivery)
		{
			delivery.DeliveriesTruckTips.ForEach(deliveryTruckTrip =>
			{
				deliveryTruckTrip.DeliveryId = delivery.DeliveryId;
				_deliveryTruckTripRepository.InsertDeliveryTruckTrip(deliveryTruckTrip);
			});
		} 

		private List<DeliveryTruckTrip> AllocateQuantityOfProductToTruckTrips(Delivery delivery)
		{
			var deliveriesTruckTrips = new List<DeliveryTruckTrip>();

			int numberOfTrips = (int)Math.Ceiling(delivery.QuantityProduct / 10);
			double quantityOfProductToDelivery = delivery.QuantityProduct;

			for(int i = 0; i < numberOfTrips; i++)
			{
				var newTrip = new DeliveryTruckTrip()
				{	
					DeliveryId = delivery.DeliveryId,
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

			if(remainingQuantityOfProductToDelivery <= (int)VehicleCapacity.STANDART)
			{
				quantityOfProductToDelivery = remainingQuantityOfProductToDelivery;
			}
			else
			{
				quantityOfProductToDelivery = (int)VehicleCapacity.STANDART;
				remainingQuantityOfProductToDelivery -= quantityOfProductToDelivery;
			}

			return quantityOfProductToDelivery;
		}
	
		private IDeliveryRepository _deliveryRepository;
		private IDeliveryTruckTripRepository _deliveryTruckTripRepository;
		private IAddressRepository _addressRepository;
		private DeliveryInternal _deliveryInternal;

		public CreateDeliveryBusiness(IDeliveryRepository deliveryRepository, IDeliveryTruckTripRepository deliveryTruckTripRepository,
			IAddressRepository addressRepository, IFractionedTripRepository fractionedTripRepository, IGoogleMapsRepository googleMapsRepository,
			IDepotRepository depotRepository, IVehicleRepository vehicleRepository, ICeplexRepository ceplexRepository)
		{
			_deliveryRepository = deliveryRepository;
			_deliveryTruckTripRepository = deliveryTruckTripRepository;
			_addressRepository = addressRepository;

			_deliveryInternal = new DeliveryInternal(fractionedTripRepository, googleMapsRepository, depotRepository, vehicleRepository, 
				addressRepository, ceplexRepository);
		}
	}
}
