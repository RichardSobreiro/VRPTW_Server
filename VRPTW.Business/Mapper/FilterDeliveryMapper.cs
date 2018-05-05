using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;

namespace VRPTW.Business.Mapper
{
	public static class FilterDeliveryMapper
	{
		public static FilterDelivery CreateEntity(this FilterDeliveryDto dto)
		{
			return new FilterDelivery()
			{
				DateDeliveryInitial = dto.desiredDateInitial,
				DateDeliveryFinal = dto.desiredDateFinal,
				ClientName = dto.clientName,
				ProductType = dto.productType,
				QuantityProductInitial = dto.quantityProductInitial,
				QuantityProductFinal = dto.quantityProductFinal		   
			};
		}

		public static FilterDeliveryDto CreateDto(this FilterDelivery entity)
		{
			return new FilterDeliveryDto()
			{
				desiredDateInitial = entity.DateDeliveryInitial,
				clientName = entity.ClientName,
				productType = entity.ProductType,
				quantityProductInitial = entity.QuantityProductInitial,
				quantityProductFinal = entity.QuantityProductFinal,
				valueStatus = entity.ValueStatus
			};
		}
	}
}
