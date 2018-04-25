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
				DateDelivery = dto.dateDelivery,
				ClientName = dto.clientName,
				ProductType = dto.productType,
				QuantityProduct = dto.quantityProduct,
				StatusDelivery = dto.statusDelivery
			};
		}

		public static FilterDeliveryDto CreateDto(this FilterDelivery entity)
		{
			return new FilterDeliveryDto()
			{
				dateDelivery = entity.DateDelivery,
				clientName = entity.ClientName,
				productType = entity.ProductType,
				quantityProduct = entity.QuantityProduct,
				statusDelivery = entity.StatusDelivery
			};
		}
	}
}
