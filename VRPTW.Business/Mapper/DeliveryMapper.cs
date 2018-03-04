using System.Collections.Generic;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;

namespace VRPTW.Business.Mapper
{
	public static class DeliveryMapper
	{
		public static Delivery CreateEntity(this DeliveryDto deliveryDto)
		{
			return new Delivery()
			{
				DeliveryId = deliveryDto.deliveryId,
				DateDelivery = deliveryDto.dateDelivery,
				ClientId = deliveryDto.clientId,
				ProductType = deliveryDto.productType,
				QuantityProduct = deliveryDto.quantityProduct,
				DeliveriesTruckTips = deliveryDto.deliveriesTruckTips?.CreateEntity(),
				Address = deliveryDto.address?.CreateEntity()
			};
		}

		public static DeliveryDto CreateDto(this Delivery delivery)
		{
			return new DeliveryDto()
			{
				deliveryId = delivery.DeliveryId,
				dateDelivery = delivery.DateDelivery,
				clientId = delivery.ClientId,
				productType = delivery.ProductType,
				quantityProduct = delivery.QuantityProduct,
				deliveriesTruckTips = delivery.DeliveriesTruckTips?.CreateDto(),
				address = delivery.Address?.CreateDto()
			};
		}

		public static List<DeliveryDto> CreateDto(this List<Delivery> entities)
		{
			return entities.ConvertAll(entity => entity.CreateDto());
		}
	}
}
