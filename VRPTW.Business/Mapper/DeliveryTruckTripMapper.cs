using System.Collections.Generic;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;

namespace VRPTW.Business.Mapper
{
	public static class DeliveryTruckTripMapper
	{
		public static DeliveryTruckTrip CreateEntity(this DeliveryTruckTripDto deliveryTruckTripDto)
		{
			return new DeliveryTruckTrip()
			{
				DeliveryTruckTripId = deliveryTruckTripDto.deliveryTruckTripId,
				DeliveryId = deliveryTruckTripDto.deliveryId,
				ProductType = deliveryTruckTripDto.productType,
				QuantityProduct = deliveryTruckTripDto.quantityProduct,
				TimeTrip = deliveryTruckTripDto.timeTrip,
				TimeArrivalClient = deliveryTruckTripDto.timeArrivalClient
			};
		}

		public static DeliveryTruckTripDto CreateDto(this DeliveryTruckTrip deliveryTruckTrip)
		{
			return new DeliveryTruckTripDto()
			{
				deliveryTruckTripId = deliveryTruckTrip.DeliveryTruckTripId,
				deliveryId = deliveryTruckTrip.DeliveryId,
				productType = deliveryTruckTrip.ProductType,
				quantityProduct = deliveryTruckTrip.QuantityProduct,
				timeTrip = deliveryTruckTrip.TimeTrip,
				timeArrivalClient = deliveryTruckTrip.TimeArrivalClient
			};
		}

		public static List<DeliveryTruckTripDto> CreateDto(this List<DeliveryTruckTrip> entities)
		{
			return entities.ConvertAll(entity => entity.CreateDto());
		}

		public static List<DeliveryTruckTrip> CreateEntity(this List<DeliveryTruckTripDto> dtos)
		{
			return dtos.ConvertAll(dto => dto.CreateEntity());
		}
	}
}
